import { ProductFromApi } from "../../../App";
import Footer from "../../../components/Footer/Footer";
import Header from "../../../components/Header/Header";
import AllProducts from "./AllProducts/AllProducts";
import "./Shop.css";

interface ShopProps {
  carts: ProductFromApi[];
  quanity: number;
  totalPriceOnCart: number;
  onRemoveFromCart: (product: ProductFromApi) => void;
  isOpenCartWhenAdd: boolean;
  onUpdateCartQuantity: (title: string, quantity: number) => void;
  products: ProductFromApi[];
  productBoxes: ProductFromApi[];
  onAddToCart: (product: ProductFromApi, numberOfQuantity: number) => void;
  cartIconRef: React.RefObject<HTMLDivElement>;
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
  cartIconRef,
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
        cartIconRef={cartIconRef}
        sellingProducts={products}
      />

      <AllProducts
        products={products}
        onAddToCart={onAddToCart}
        cartIconRef={cartIconRef}
      />

      <Footer />
    </>
  );
};

export default Shop;
