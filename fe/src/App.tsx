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
import AuthenProvider from "./hooks/AuthenContext";
import { ToastContainer } from "react-toastify";
import "react-toastify/dist/ReactToastify.css";

export interface OrderProps {
  id: string;
  userId: string;
  totalPrice: number;
}

export interface OrderItemProps {
  id: string;
  orderId: string;
  productId: string;
  quantity: number;
  price: number;
  product: ProductFromApi;
}

export interface UserProps {
  id: string;
  name: string;
  imageUrl: string;
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

function App() {
  const [carts, setCarts] = useState<OrderItemProps[]>([]);
  const [quantity, setQuantity] = useState(0);
  const [totalPrice, setTotalPrice] = useState(0);
  const [isOpenCartWhenAdd, setIsOpenCartWhenAdd] = useState(false);
  const [products, setProducts] = useState<ProductFromApi[]>([]);
  const [productBoxes, setProductBoxes] = useState<ProductFromApi[]>([]);
  const cartIconRef = useRef<HTMLDivElement>(null);

  const fetchCarts = async () => {
    try {
      const data = await ApiGateway.getOrderByUserId<OrderProps>();

      if (data) {
        const orderId = data.id;
        const orderProducts = await ApiGateway.getAllOrderProducts<{
          orderItems: OrderItemProps[];
        }>(orderId);

        if (orderProducts) {
          setCarts(orderProducts.orderItems || []);
        }
      }
    } catch (error) {
      console.error("Error fetching carts:", error);
      setCarts([]);
    }
  };

  const handleUpdateCartQuantity = (title: string, quantity: number) => {
    setCarts((prev) =>
      prev.map((item) =>
        item.product.name === title ? { ...item, quantity } : item
      )
    );
  };

  const handleAddToCart = (
    product: ProductFromApi,
    numberOfQuantity: number
  ) => {
    const existing = carts.find((item) => item.product.name === product.name);
    if (existing) {
      setCarts(
        carts.map((item) =>
          item.product.name === product.name
            ? { ...item, quantity: (item.quantity || 0) + numberOfQuantity }
            : item
        )
      );
    } else {
      const newCartItem: OrderItemProps = {
        id: crypto.randomUUID(), // Temporary ID until saved to backend
        orderId: "", // Or fetch current cart order ID if available
        productId: product.id,
        quantity: numberOfQuantity,
        price: product.price,
        product: product,
      };

      setCarts([...carts, newCartItem]);
    }
    setIsOpenCartWhenAdd(true);
  };

  const handleRemoveFromCart = (product: ProductFromApi) => {
    const existing = carts.find((item) => item.product.name === product.name);
    if (existing) {
      if (existing.quantity && existing.quantity > 1) {
        setCarts(
          carts.map((item) =>
            item.product.name === product.name
              ? { ...item, quantity: (item.quantity || 1) - 1 }
              : item
          )
        );
      } else {
        setCarts(
          carts.map((item) =>
            item.product.name === product.name
              ? { ...item, removing: true }
              : item
          )
        );

        setTimeout(() => {
          setCarts((prev) =>
            prev.filter((item) => item.product.name !== product.name)
          );
        }, 1000);
      }
    }
  };

  useEffect(() => {
    fetchProducts();
    fetchCarts();
  }, []);

  const fetchProducts = async () => {
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
    }
  };

  useEffect(() => {
    const total = carts.reduce((acc, item) => {
      return acc + Number(item.price) * (item.quantity || 1);
    }, 0);
    setTotalPrice(total);

    const totalQty = carts.reduce((acc, item) => acc + (item.quantity || 1), 0);
    setQuantity(totalQty);
  }, [carts]);

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
  handleUpdateCartQuantity: (title: string, quantity: number) => void;
  cartIconRef: React.RefObject<HTMLDivElement>;
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
}: RouteProps) {
  const location = useLocation();
  const background = location.state?.background;

  return (
    <AuthenProvider>
      <ToastContainer position="top-right" autoClose={5000} closeOnClick />
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
            />
          }
        />
      </Routes>
    </AuthenProvider>
  );
}
