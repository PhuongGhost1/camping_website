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
import { ReactNotifications } from "react-notifications-component";

export interface UserProps {
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
  const [carts, setCarts] = useState<ProductFromApi[]>([]);
  const [quantity, setQuantity] = useState(0);
  const [totalPrice, setTotalPrice] = useState(0);
  const [isOpenCartWhenAdd, setIsOpenCartWhenAdd] = useState(false);
  const [products, setProducts] = useState<ProductFromApi[]>([]);
  const [productBoxes, setProductBoxes] = useState<ProductFromApi[]>([]);
  const cartIconRef = useRef<HTMLDivElement>(null);

  const handleUpdateCartQuantity = (title: string, quantity: number) => {
    setCarts((prev) =>
      prev.map((item) => (item.name === title ? { ...item, quantity } : item))
    );
  };

  const handleAddToCart = (
    product: ProductFromApi,
    numberOfQuantity: number
  ) => {
    const existing = carts.find((item) => item.name === product.name);
    if (existing) {
      setCarts(
        carts.map((item) =>
          item.name === product.name
            ? { ...item, quantity: (item.stock || 0) + numberOfQuantity }
            : item
        )
      );
    } else {
      setCarts([...carts, { ...product, stock: numberOfQuantity }]);
    }
    setIsOpenCartWhenAdd(true);
  };

  const handleRemoveFromCart = (product: ProductFromApi) => {
    const existing = carts.find((item) => item.name === product.name);
    if (existing) {
      if (existing.stock && existing.stock > 1) {
        setCarts(
          carts.map((item) =>
            item.name === product.name
              ? { ...item, quantity: (item.stock || 1) - 1 }
              : item
          )
        );
      } else {
        setCarts(
          carts.map((item) =>
            item.name === product.name ? { ...item, removing: true } : item
          )
        );

        setTimeout(() => {
          setCarts((prev) => prev.filter((item) => item.name !== product.name));
        }, 1000);
      }
    }
  };

  useEffect(() => {
    fetchProducts();
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
      return acc + Number(item.price) * (item.stock || 1);
    }, 0);
    setTotalPrice(total);

    const totalQty = carts.reduce((acc, item) => acc + (item.stock || 1), 0);
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
  carts: ProductFromApi[];
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
      <ReactNotifications />
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
