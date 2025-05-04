import { useParams } from "react-router-dom";
import { SellingProductsProps } from "../../../App";
import Footer from "../../../components/Footer/Footer";
import Header from "../../../components/Header/Header";
import "./Product.css";
import ProductDetail from "./ProductDetail/ProductDetail";

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
  onUpdateCartQuantity: (title: string, quantity: number) => void;
  cartIconRef: React.RefObject<HTMLDivElement>;
}

interface ProductPageProps extends CommonProps {
  products: SellingProductsProps[];
  productBoxes: SellingProductsProps[];
}

const Product = ({
  carts,
  quantity,
  totalPrice,
  onRemoveFromCart,
  isOpenCartWhenAdd,
  onAddToCart,
  products,
  productBoxes,
  onUpdateCartQuantity,
  cartIconRef,
}: ProductPageProps) => {
  const { title } = useParams<{ title: string }>();
  const product = products.find(
    (item) => item.title.toLowerCase() === title?.toLowerCase()
  );
  const productBox = productBoxes.find(
    (item) => item.title.toLowerCase() === title?.toLowerCase()
  );
  const currentProduct = product ?? productBox;
  if (!currentProduct) {
    return <div>Product not found</div>;
  }

  return (
    <>
      <Header
        carts={carts}
        quanity={quantity}
        totalPriceOnCart={totalPrice}
        onRemoveFromCart={onRemoveFromCart}
        isOpenCartWhenAdd={isOpenCartWhenAdd}
        onUpdateCartQuantity={onUpdateCartQuantity}
        cartIconRef={cartIconRef}
      />

      <ProductDetail
        title={title ?? ""}
        currentProduct={currentProduct}
        onAddToCart={onAddToCart}
        cartIconRef={cartIconRef}
      />

      <Footer />
    </>
  );
};

export default Product;
