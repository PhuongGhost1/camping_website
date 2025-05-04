import { BrowserRouter, Route, Routes, useLocation } from "react-router-dom";
import "./App.css";
import Home from "./pages/User/Home/Home";
import Product from "./pages/User/Product/Product";
import { useEffect, useState } from "react";
import img_selling_1 from "./assets/selling-1.png";
import img_selling_2 from "./assets/selling-2.png";
import img_selling_3 from "./assets/selling-3.png";
import img_selling_4 from "./assets/selling-4.png";
import img_product_1 from "./assets/product-1.png";
import img_product2 from "./assets/product-2.png";
import img_product3 from "./assets/product-3.png";
import Shop from "./pages/User/Shop/Shop";

const sellingProducts: SellingProductsProps[] = [
  {
    discount: "30%",
    image: img_selling_1,
    title: "Small Sun Headlight",
    rating: "4.5",
    price: "300.00",
    salePrice: "200.00",
  },
  {
    discount: "30%",
    image: img_selling_2,
    title: "Sprint Sunglasses",
    rating: "4.5",
    price: "300.00",
    salePrice: "200.00",
  },
  {
    discount: "30%",
    image: img_selling_3,
    title: "Sleeping Bag",
    rating: "4.5",
    price: "300.00",
    salePrice: "200.00",
  },
  {
    discount: "30%",
    image: img_selling_4,
    title: "Eletric Water Bottle",
    rating: "4.5",
    price: "300.00",
    salePrice: "200.00",
  },
  {
    discount: "30%",
    image: img_product_1,
    title: "Camping Tent",
    rating: "4.5",
    price: "300.00",
    salePrice: "200.00",
  },
];

const productsBox: SellingProductsProps[] = [
  {
    discount: "30%",
    image: img_product_1,
    title: "Folding Camping Table",
    rating: "4.5",
    price: "150.00",
    salePrice: "110.00",
  },
  {
    discount: "30%",
    image: img_product2,
    title: "Sport Bottle",
    rating: "4.5",
    price: "300.00",
    salePrice: "200.00",
  },
  {
    discount: "30%",
    image: img_product3,
    title: "Camping Tent",
    rating: "4.5",
    price: "210.00",
    salePrice: "180.00",
  },
];

export interface SellingProductsProps {
  discount: string;
  image: string;
  title: string;
  rating: string;
  price: string;
  salePrice: string;
  quantity?: number;
  removing?: boolean;
}

function App() {
  const [carts, setCarts] = useState<SellingProductsProps[]>([]);
  const [quantity, setQuantity] = useState(0);
  const [totalPrice, setTotalPrice] = useState(0);
  const [isOpenCartWhenAdd, setIsOpenCartWhenAdd] = useState(false);
  const [products, setProducts] =
    useState<SellingProductsProps[]>(sellingProducts);
  const [productBoxes, setProductBoxes] =
    useState<SellingProductsProps[]>(productsBox);

  const handleUpdateCartQuantity = (title: string, quantity: number) => {
    setCarts((prev) =>
      prev.map((item) => (item.title === title ? { ...item, quantity } : item))
    );
  };

  const handleAddToCart = (
    product: SellingProductsProps,
    numberOfQuantity: number
  ) => {
    const existing = carts.find((item) => item.title === product.title);
    if (existing) {
      setCarts(
        carts.map((item) =>
          item.title === product.title
            ? { ...item, quantity: (item.quantity || 0) + numberOfQuantity }
            : item
        )
      );
    } else {
      setCarts([...carts, { ...product, quantity: numberOfQuantity }]);
    }
    setIsOpenCartWhenAdd(true);
  };

  const handleRemoveFromCart = (product: SellingProductsProps) => {
    const existing = carts.find((item) => item.title === product.title);
    if (existing) {
      if (existing.quantity && existing.quantity > 1) {
        setCarts(
          carts.map((item) =>
            item.title === product.title
              ? { ...item, quantity: (item.quantity || 1) - 1 }
              : item
          )
        );
      } else {
        setCarts(
          carts.map((item) =>
            item.title === product.title ? { ...item, removing: true } : item
          )
        );

        setTimeout(() => {
          setCarts((prev) =>
            prev.filter((item) => item.title !== product.title)
          );
        }, 1000);
      }
    }
  };

  useEffect(() => {
    const total = carts.reduce((acc, item) => {
      return acc + Number(item.salePrice) * (item.quantity || 1);
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
      />
    </BrowserRouter>
  );
}

export default App;

interface RouteProps {
  carts: SellingProductsProps[];
  quantity: number;
  totalPrice: number;
  onRemoveFromCart: (product: SellingProductsProps) => void;
  isOpenCartWhenAdd: boolean;
  onAddToCart: (
    product: SellingProductsProps,
    numberOfQuantity: number
  ) => void;
  products: SellingProductsProps[];
  productBoxes: SellingProductsProps[];
  handleUpdateCartQuantity: (title: string, quantity: number) => void;
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
          />
        }
      />
      <Route path="/category" element={<></>} />
    </Routes>
  );
}
