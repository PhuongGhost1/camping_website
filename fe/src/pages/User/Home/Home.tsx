import { JSX, useEffect, useState } from "react";
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
import img_selling_1 from "../../../assets/selling-1.png";
import img_selling_2 from "../../../assets/selling-2.png";
import img_selling_3 from "../../../assets/selling-3.png";
import img_selling_4 from "../../../assets/selling-4.png";
import img_product_1 from "../../../assets/product-1.png";
import img_product2 from "../../../assets/product-2.png";
import img_product3 from "../../../assets/product-3.png";

interface SellingProductsProps {
  discount: string;
  image: string;
  title: string;
  rating: string;
  price: string;
  salePrice: string;
  quantity?: number;
  removing?: boolean;
}

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

function Home(): JSX.Element {
  const [products, setProducts] = useState<SellingProductsProps[]>(
    sellingProducts || []
  );
  const [productBoxes, setProductBoxes] = useState<SellingProductsProps[]>(
    productsBox || []
  );
  const [carts, setCarts] = useState<SellingProductsProps[]>([]);
  const [quantity, setQuantity] = useState(0);
  const [totalPrice, setTotalPrice] = useState(0);
  const [isOpenCartWhenAdd, setIsOpenCartWhenAdd] = useState(false);

  const handleAddToCart = (product: SellingProductsProps) => {
    const existing = carts.find((item) => item.title === product.title);
    if (existing) {
      setCarts(
        carts.map((item) =>
          item.title === product.title
            ? { ...item, quantity: (item.quantity || 1) + 1 }
            : item
        )
      );
    } else {
      setCarts([...carts, { ...product, quantity: 1 }]);
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

  useEffect(() => {
    const animate = ScrollReveal({
      origin: "top",
      distance: "60px",
      duration: 2200,
      delay: 350,
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
        { interval: 150 }
      );
      animate.reveal(".links", { origin: "right", interval: 150 });
    }, 300);

    return () => clearTimeout(revealTimeout);
  }, []);

  return (
    <>
      <Header
        carts={carts}
        quanity={quantity}
        totalPriceOnCart={totalPrice}
        onRemoveFromCart={handleRemoveFromCart}
        isOpenCartWhenAdd={isOpenCartWhenAdd}
      />

      <Hero />

      <Products productBoxes={productBoxes} onAddToCart={handleAddToCart} />

      <Backpack />

      <Selling sellingProducts={products} onAddToCart={handleAddToCart} />

      <Brand />

      <Tips />

      <Footer />
    </>
  );
}

export default Home;
