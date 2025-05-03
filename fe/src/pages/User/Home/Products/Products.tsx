import "./Products.css";
import Box from "./Box/Box";

interface ProductsProps {
  discount: string;
  image: string;
  title: string;
  rating: string;
  price: string;
  salePrice: string;
  quantity?: number;
  removing?: boolean;
}

interface ProductsBoxProps {
  productBoxes: ProductsProps[];
  onAddToCart: (item: ProductsProps) => void;
}

const Products = ({ productBoxes, onAddToCart }: ProductsBoxProps) => {
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
        {productBoxes.map((product: ProductsProps, index: number) => (
          <Box key={index} product={product} handleAddToCart={onAddToCart} />
        ))}
      </div>
    </section>
  );
};

export default Products;
