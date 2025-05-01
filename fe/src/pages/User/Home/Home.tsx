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

function Home(): JSX.Element {
  useEffect(() => {
    const animate = ScrollReveal({
      origin: "top",
      distance: "60px",
      duration: 2200,
      delay: 350,
    });

    animate.reveal(
      ".nav, .hero-content h2, .hero-content h2 span, .heading, .tips"
    );
    animate.reveal(".backpack-content", { origin: "left" });
    animate.reveal(".backpack-img, .single-post", { origin: "right" });
    animate.reveal(".hero-img img, .btn, .btn img", { origin: "bottom" });
    animate.reveal(
      ".category-box, .product-box, .brand-box, .blog-box, .footer-box h3, .footer-box a, .footer-box p",
      {
        interval: 150,
      }
    );
    animate.reveal(".links", { origin: "right", interval: 150 });
  }, []);

  return (
    <>
      <Header />

      <Hero />

      <Products />

      <Backpack />

      <Selling />

      <Brand />

      <Tips />

      <Footer />
    </>
  );
}

export default Home;
