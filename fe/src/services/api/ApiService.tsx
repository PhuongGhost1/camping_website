import axios from "axios";

export class ApiGateway {
  public static readonly API_Base: string = import.meta.env
    .BASE_GATEWAY_API_URL;

  private static axiosInstance = axios.create({
    baseURL: ApiGateway.API_Base,
    headers: {
      "Content-Type": "application/json",
    },
  });

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
}
