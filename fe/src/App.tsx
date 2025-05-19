import { BrowserRouter, Route, Routes, useLocation } from "react-router-dom";
import "./App.css";
import Home from "./pages/User/Home/Home";
import Product from "./pages/User/Product/Product";
import { useEffect, useRef, useState } from "react";
import Shop from "./pages/User/Shop/Shop";
import Category from "./pages/User/Category/Category";
import Checkout from "./pages/User/Checkout/Checkout";
import ProfilePage from "./pages/User/ProfilePage/ProfilePage";
import { ApiGateway } from "./services/api/ApiService";
import { useAuthenContext } from "./hooks/AuthenContext";
import "react-toastify/dist/ReactToastify.css";
import { useCart } from "./hooks/useCart";
import { toast } from "react-toastify";

export interface OrderProps {
  id: string;
  userId: string;
  totalPrice: number;
}

export interface RawOrderItem {
  id: string;
  orderId: string;
  productId: string;
  quantity: number;
  price: number;
  status: string;
  order?: OrderProps;
}

export interface OrderItemProps extends RawOrderItem {
  product: ProductFromApi;
  removing?: boolean;
}

export interface UserProps {
  id: string;
  name: string;
  imageUrl: string;
  email: string;
  passwordHash: string;
}

export interface CategoryProductProps {
  id: string;
  name: string;
  description: string;
  createdAt: string;
}

export interface ProductFromApi {
  id: string;
  name: string;
  description: string;
  price: number;
  stock: number;
  imageUrl: string;
  categoryId: string;
  category: CategoryProductProps;
  reviews: [];
  createdAt: string;
  removing?: boolean;
}

export interface IActionResult {
  status: number;
  message: string;
}

function App() {
  const [isOpenCartWhenAdd, setIsOpenCartWhenAdd] = useState(false);
  const [products, setProducts] = useState<ProductFromApi[]>([]);
  const [productBoxes, setProductBoxes] = useState<ProductFromApi[]>([]);
  const cartIconRef = useRef<HTMLDivElement>(null);
  const { user } = useAuthenContext();
  const { carts, setCarts, totalPrice, quantity, uniqueOrderId, fetchCarts } =
    useCart(user as UserProps);
  const [isLoading, setIsLoading] = useState<boolean>(false);

  const handleAddToCartSuccess = () => {
    toast.success("Product added to cart successfully!");
  };

  const handleRemoveFromCartSuccess = () => {
    toast.success("Product removed from cart successfully!");
  };

  const handleUpdateCartQuantity = async (
    product: ProductFromApi,
    quantity: number
  ) => {
    if (!user) {
      alert("You need to login to update the product quantity");
      return;
    }

    try {
      await ApiGateway.updateProductInCart(
        product.id,
        uniqueOrderId!,
        quantity,
        product.price
      );
      await fetchCarts();
    } catch (error) {
      console.error("Error updating quantity:", error);
    }
  };

  const handleAddToCart = async (
    product: ProductFromApi,
    numberOfQuantity: number
  ) => {
    if (!user) {
      alert("You need to login to add the product to cart");
      return;
    }

    await ApiGateway.addProductToCart(
      uniqueOrderId!,
      product.id,
      numberOfQuantity,
      product.price
    );

    await fetchCarts();

    setIsOpenCartWhenAdd(true);

    handleAddToCartSuccess();
  };

  const handleRemoveFromCart = (product: ProductFromApi) => {
    if (!user) {
      alert("You need to login to remove the product from cart");
      return;
    }

    const existing = carts.find((item) => item.product.id === product.id);
    if (!existing) return;

    if (existing.quantity && existing.quantity > 0) {
      setCarts((prev) =>
        prev.map((item) =>
          item.product.id === product.id ? { ...item, removing: true } : item
        )
      );

      setTimeout(() => {
        setCarts((prev) =>
          prev.filter((item) => item.product.id !== product.id)
        );

        ApiGateway.removeProductFromCart(uniqueOrderId!, product.id);
      }, 1000);
    }

    handleRemoveFromCartSuccess();
  };

  useEffect(() => {
    fetchProducts();
  }, []);

  const fetchProducts = async () => {
    setIsLoading(true);

    try {
      const data = await ApiGateway.getAllProducts<{
        products: ProductFromApi[];
        page: number;
        pageSize: number;
        total: number;
      }>("", 1, 7);

      const data2 = await ApiGateway.getAllProducts<{
        products: ProductFromApi[];
        page: number;
        pageSize: number;
        total: number;
      }>("", 1, 3);

      setProducts(data.products || []);
      setProductBoxes(data2.products || []);
    } catch (error) {
      console.error("Error fetching products:", error);
      setProducts([]);
      setProductBoxes([]);
    } finally {
      setIsLoading(false);
    }
  };

  return (
    <BrowserRouter>
      <MainRoutes
        carts={carts}
        quantity={quantity}
        totalPrice={totalPrice}
        onRemoveFromCart={handleRemoveFromCart}
        isOpenCartWhenAdd={isOpenCartWhenAdd}
        onAddToCart={handleAddToCart}
        products={products}
        productBoxes={productBoxes}
        handleUpdateCartQuantity={handleUpdateCartQuantity}
        cartIconRef={cartIconRef as React.RefObject<HTMLDivElement>}
        user={user}
        loading={isLoading}
      />
    </BrowserRouter>
  );
}

export default App;

interface RouteProps {
  carts: OrderItemProps[];
  quantity: number;
  totalPrice: number;
  onRemoveFromCart: (product: ProductFromApi) => void;
  isOpenCartWhenAdd: boolean;
  onAddToCart: (product: ProductFromApi, numberOfQuantity: number) => void;
  products: ProductFromApi[];
  productBoxes: ProductFromApi[];
  handleUpdateCartQuantity: (product: ProductFromApi, quantity: number) => void;
  cartIconRef: React.RefObject<HTMLDivElement>;
  user: UserProps | null;
  loading: boolean;
}

function MainRoutes({
  carts,
  quantity,
  totalPrice,
  onRemoveFromCart,
  isOpenCartWhenAdd,
  onAddToCart,
  products,
  productBoxes,
  handleUpdateCartQuantity,
  cartIconRef,
  user,
  loading,
}: RouteProps) {
  const location = useLocation();
  const background = location.state?.background;

  return (
    <Routes location={background || location}>
      <Route
        path="/"
        element={
          <Home
            carts={carts}
            quantity={quantity}
            totalPrice={totalPrice}
            onRemoveFromCart={onRemoveFromCart}
            isOpenCartWhenAdd={isOpenCartWhenAdd}
            onAddToCart={onAddToCart}
            products={products}
            productBoxes={productBoxes}
            onUpdateCartQuantity={handleUpdateCartQuantity}
            cartIconRef={cartIconRef}
            user={user}
            loading={loading}
          />
        }
      />
      <Route
        path="/product/:title"
        element={
          <Product
            carts={carts}
            quantity={quantity}
            totalPrice={totalPrice}
            onRemoveFromCart={onRemoveFromCart}
            isOpenCartWhenAdd={isOpenCartWhenAdd}
            onAddToCart={onAddToCart}
            products={products}
            productBoxes={productBoxes}
            onUpdateCartQuantity={handleUpdateCartQuantity}
            cartIconRef={cartIconRef}
            user={user}
          />
        }
      />
      <Route
        path="/shop-all-products"
        element={
          <Shop
            carts={carts}
            quanity={quantity}
            totalPriceOnCart={totalPrice}
            onRemoveFromCart={onRemoveFromCart}
            isOpenCartWhenAdd={isOpenCartWhenAdd}
            onUpdateCartQuantity={handleUpdateCartQuantity}
            products={products}
            productBoxes={productBoxes}
            onAddToCart={onAddToCart}
            cartIconRef={cartIconRef}
            user={user}
          />
        }
      />
      <Route
        path="/category/:name"
        element={
          <Category
            carts={carts}
            quanity={quantity}
            totalPriceOnCart={totalPrice}
            onRemoveFromCart={onRemoveFromCart}
            isOpenCartWhenAdd={isOpenCartWhenAdd}
            onUpdateCartQuantity={handleUpdateCartQuantity}
            products={products}
            productBoxes={productBoxes}
            onAddToCart={onAddToCart}
            cartIconRef={cartIconRef}
            user={user}
          />
        }
      />
      <Route
        path="/checkout"
        element={
          <Checkout
            carts={carts}
            quantity={quantity}
            totalPrice={totalPrice}
            onRemoveFromCart={onRemoveFromCart}
            onUpdateCartQuantity={handleUpdateCartQuantity}
            user={user}
          />
        }
      />
      <Route
        path="/profile"
        element={
          <ProfilePage
            carts={carts}
            quantity={quantity}
            totalPrice={totalPrice}
            onRemoveFromCart={onRemoveFromCart}
            isOpenCartWhenAdd={isOpenCartWhenAdd}
            cartIconRef={cartIconRef}
            onUpdateCartQuantity={handleUpdateCartQuantity}
            products={products}
            user={user}
          />
        }
      />
    </Routes>
  );
}
