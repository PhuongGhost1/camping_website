import { JSX, useEffect } from "react";
import Footer from "../../../components/Footer/Footer";
import Header from "../../../components/Header/Header";
import "./Home.css";
import Hero from "./Hero/Hero";
import Products from "./Products/Products";
import Backpack from "./Backpack/Backpack";
import Selling from "./Selling/Selling";
import Brand from "./Brand/Brand";
import Tips from "./Tips/Tips";
import ScrollReveal from "scrollreveal";
import { SellingProductsProps } from "../../../App";

interface CommonProps {
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
  onUpdateCartQuantity: (title: string, quantity: number) => void;
}

function Home({
  carts,
  quantity,
  totalPrice,
  onRemoveFromCart,
  isOpenCartWhenAdd,
  onAddToCart,
  products,
  productBoxes,
  onUpdateCartQuantity,
}: CommonProps): JSX.Element {
  useEffect(() => {
    const animate = ScrollReveal({
      origin: "top",
      distance: "60px",
      duration: 1500,
      delay: 200,
      reset: false,
    });

    const revealTimeout = setTimeout(() => {
      animate.reveal(
        ".nav, .hero-content h2, .hero-content h2 span, .heading, .tips"
      );
      animate.reveal(".backpack-content", { origin: "left" });
      animate.reveal(".backpack-img, .single-post", { origin: "right" });
      animate.reveal(".hero-img img, .btn, .btn img", { origin: "bottom" });

      animate.reveal(
        ".category-box, .product-box, .brand-box, .blog-box, .footer-box h3, .footer-box a, .footer-box p",
        { interval: 100 }
      );
      animate.reveal(".links", { origin: "right", interval: 100 });
    }, 300);

    return () => clearTimeout(revealTimeout);
  }, []);

  return (
    <>
      <Header
        carts={carts}
        quanity={quantity}
        totalPriceOnCart={totalPrice}
        onRemoveFromCart={onRemoveFromCart}
        isOpenCartWhenAdd={isOpenCartWhenAdd}
        onUpdateCartQuantity={onUpdateCartQuantity}
      />

      <Hero />

      <Products productBoxes={productBoxes} onAddToCart={onAddToCart} />

      <Backpack />

      <Selling sellingProducts={products} onAddToCart={onAddToCart} />

      <Brand />

      <Tips />

      <Footer />
    </>
  );
}

export default Home;
