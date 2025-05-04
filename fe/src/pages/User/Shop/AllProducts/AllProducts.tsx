import { SellingProductsProps } from "../../../../App";
import Box from "../../Home/Products/Box/Box";
import "./AllProducts.css";

interface AllProductsProps {
  products: SellingProductsProps[];
  onAddToCart: (
    product: SellingProductsProps,
    numberOfQuantity: number
  ) => void;
  cartIconRef: React.RefObject<HTMLDivElement>;
}

const AllProducts = ({
  products,
  onAddToCart,
  cartIconRef,
}: AllProductsProps) => {
  return (
    <>
      <div className="all-products-container">
        <h1>All Products</h1>
        <div className="products-list grid-fit">
          {products.map((product, index) => (
            <Box
              key={index}
              product={product}
              handleAddToCart={onAddToCart}
              cartIconRef={cartIconRef}
            />
          ))}
        </div>
      </div>
    </>
  );
};

export default AllProducts;
