import { SellingProductsProps } from "../../../App";
import Footer from "../../../components/Footer/Footer";
import Header from "../../../components/Header/Header";
import AllProducts from "./AllProducts/AllProducts";
import "./Shop.css";

interface ShopProps {
  carts: SellingProductsProps[];
  quanity: number;
  totalPriceOnCart: number;
  onRemoveFromCart: (product: SellingProductsProps) => void;
  isOpenCartWhenAdd: boolean;
  onUpdateCartQuantity: (title: string, quantity: number) => void;
  products: SellingProductsProps[];
  productBoxes: SellingProductsProps[];
  onAddToCart: (
    product: SellingProductsProps,
    numberOfQuantity: number
  ) => void;
}

const Shop = ({
  carts,
  quanity,
  totalPriceOnCart,
  onRemoveFromCart,
  isOpenCartWhenAdd,
  onUpdateCartQuantity,
  products,
  onAddToCart,
}: ShopProps) => {
  return (
    <>
      <Header
        carts={carts}
        quanity={quanity}
        totalPriceOnCart={totalPriceOnCart}
        onRemoveFromCart={onRemoveFromCart}
        isOpenCartWhenAdd={isOpenCartWhenAdd}
        onUpdateCartQuantity={onUpdateCartQuantity}
      />

      <AllProducts products={products} onAddToCart={onAddToCart} />

      <Footer />
    </>
  );
};

export default Shop;
