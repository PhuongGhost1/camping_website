import "./Products.css";
import Box from "./Box/Box";
import { ProductFromApi, UserProps } from "../../../../App";

interface ProductsBoxProps {
  productBoxes: ProductFromApi[];
  onAddToCart: (item: ProductFromApi, numberOfQuantity: number) => void;
  cartIconRef?: React.RefObject<HTMLDivElement>;
  user: UserProps | null;
}

const Products = ({
  productBoxes,
  onAddToCart,
  cartIconRef,
  user,
}: ProductsBoxProps) => {
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
        {productBoxes.map((product: ProductFromApi, index: number) => (
          <Box
            key={index}
            product={product}
            handleAddToCart={onAddToCart}
            cartIconRef={cartIconRef}
            user={user}
          />
        ))}
      </div>
    </section>
  );
};

export default Products;
