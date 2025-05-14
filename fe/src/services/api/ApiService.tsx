import axios from "axios";

interface LoginResponse {
  email: string;
  pwd: string;
  accessToken: string;
  refreshToken: string;
}

interface RefreshTokenResponse {
  accessToken: string;
  refreshToken: string;
}

export class ApiGateway {
  public static readonly API_Base: string = import.meta.env
    .BASE_GATEWAY_API_URL;

  private static axiosInstance = axios.create({
    baseURL: ApiGateway.API_Base,
    headers: {
      "Content-Type": "application/json",
    },
  });

  static {
    ApiGateway.axiosInstance.interceptors.response.use(
      (response) => response,
      async (error) => {
        const originalRequest = error.config;

        // If unauthorized and not yet retried, try refreshing
        if (
          error.response?.status === 401 &&
          !originalRequest._retry &&
          localStorage.getItem("refreshToken")
        ) {
          originalRequest._retry = true;

          const refreshToken = localStorage.getItem("refreshToken");

          try {
            const res =
              await ApiGateway.axiosInstance.post<RefreshTokenResponse>(
                `/auth/refresh-token`,
                { refreshToken }
              );

            const { accessToken, refreshToken: newRefreshToken } = res.data;

            localStorage.setItem("accessToken", accessToken);
            localStorage.setItem("refreshToken", newRefreshToken);

            ApiGateway.axiosInstance.defaults.headers.common[
              "Authorization"
            ] = `Bearer ${accessToken}`;
            originalRequest.headers["Authorization"] = `Bearer ${accessToken}`;

            return ApiGateway.axiosInstance(originalRequest);
          } catch (refreshError) {
            console.error("Failed to refresh token:", refreshError);
            await ApiGateway.LogOut();
            window.location.href = "/"; // or trigger login modal
            return Promise.reject(refreshError);
          }
        }

        return Promise.reject(error);
      }
    );
  }

  private static getToken(): string | null {
    return localStorage.getItem("accessToken");
  }

  private static setAuthHeader(): void {
    const token = this.getToken();
    if (token) {
      this.axiosInstance.defaults.headers.common[
        "Authorization"
      ] = `Bearer ${token}`;
    }
  }

  public static async LoginDefault<T extends LoginResponse>(
    email: string,
    pwd: string
  ): Promise<boolean> {
    try {
      const response = await this.axiosInstance.post<T>(`/auth/login`, {
        email,
        pwd,
      });

      const { accessToken, refreshToken } = response.data;

      localStorage.setItem("accessToken", accessToken);
      localStorage.setItem("refreshToken", refreshToken);

      return true;
    } catch (error) {
      console.error("Error logging in:", error);
      return false;
    }
  }

  public static async LogOut(): Promise<void> {
    try {
      this.setAuthHeader();
      await this.axiosInstance.delete(`/auth/logout`);

      localStorage.removeItem("accessToken");
      localStorage.removeItem("refreshToken");

      delete this.axiosInstance.defaults.headers.common["Authorization"];
    } catch (error) {
      console.error("Error logging out:", error);
      throw error;
    }
  }

  public static async getAllProducts<T>(
    searchKeyword: string,
    page: number,
    pageSize: number
  ): Promise<T> {
    try {
      const response = await this.axiosInstance.get<T>(
        `/products/all-products?searchKeyword=${searchKeyword}&Page=${page}&PageSize=${pageSize}`
      );
      return response.data;
    } catch (error) {
      console.error("Error fetching products:", error);
      throw error;
    }
  }

  public static async getAllCategories<T>(): Promise<T> {
    try {
      const response = await this.axiosInstance.get<T>(
        "/categories/all-categories"
      );
      return response.data;
    } catch (error) {
      console.error("Error fetching categories:", error);
      throw error;
    }
  }

  public static async getUser<T>(): Promise<T | null> {
    this.setAuthHeader();
    try {
      const reponse = await this.axiosInstance.get<T>(`/users/user-info`);
      return reponse.data;
    } catch (error) {
      console.error("Error fetching user:", error);
      throw error;
    }
  }

  public static async Register<T>(
    email: string,
    pwd: string,
    name: string
  ): Promise<T | null> {
    try {
      const response = await this.axiosInstance.post<T>(`/auth/register`, {
        name,
        email,
        pwd,
      });
      return response.data;
    } catch (error) {
      console.error("Error registering:", error);
      throw error;
    }
  }

  public static async getProductById<T>(id: string): Promise<T | null> {
    try {
      const response = await this.axiosInstance.get<T>(`/products/${id}`);
      return response.data;
    } catch (error) {
      console.error("Error fetching product by ID:", error);
      throw error;
    }
  }

  public static async getOrderByUserId<T>(): Promise<T | null> {
    this.setAuthHeader();
    try {
      const response = await this.axiosInstance.get<T>(`/orders/order`);
      return response.data;
    } catch (error) {
      console.error("Error fetching orders:", error);
      throw error;
    }
  }

  public static async getAllOrderProducts<T>(
    orderId: string
  ): Promise<T | null> {
    this.setAuthHeader();
    try {
      const response = await this.axiosInstance.get<T>(
        `/orders/all-order-products?orderId=${orderId}`
      );
      return response.data;
    } catch (error) {
      console.error("Error fetching all orders:", error);
      throw error;
    }
  }

  public static async addProductToCart<T>(
    orderId: string,
    productId: string,
    quantity: number,
    price: number
  ): Promise<T | null> {
    this.setAuthHeader();
    try {
      const response = await this.axiosInstance.post<T>(`/orders/add-to-cart`, {
        orderId,
        productId,
        quantity,
        price,
      });
      return response.data;
    } catch (error) {
      console.error("Error adding product to cart:", error);
      throw error;
    }
  }
}
