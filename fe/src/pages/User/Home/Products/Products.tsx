import { JSX } from "react";
import "./Products.css";
import img_product1 from "../../../../assets/product-1.png";
import img_product2 from "../../../../assets/product-2.png";
import img_product3 from "../../../../assets/product-3.png";
import Box from "./Box/Box";

interface ProductsProps {
  discount: string;
  image: string;
  title: string;
  rating: string;
  price: string;
  salePrice: string;
}

const products: ProductsProps[] = [
  {
    discount: "30%",
    image: img_product1,
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

function Products(): JSX.Element {
  return (
    <section className="products container" id="products">
      <p className="product-desc">
        Professional camping equipment for your trip. We have everything you
        need for a great camping experience. and we are here to help you find
        the right gear for your next adventure. make your camping experience
        more enjoyable and comfortable with our high-quality products. we have
        everything you need to make your camping trip a success. Our products
        are designed to be durable, lightweight, and easy to use, so you can
        focus on enjoying the great outdoors.
      </p>
      <div className="product-content">
        {products.map((product: ProductsProps, index: number) => (
          <Box key={index} product={product} />
        ))}
      </div>
    </section>
  );
}

export default Products;
