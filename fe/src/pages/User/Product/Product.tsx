import { useParams } from "react-router-dom";
import { OrderItemProps, ProductFromApi, UserProps } from "../../../App";
import Footer from "../../../components/Footer/Footer";
import Header from "../../../components/Header/Header";
import "./Product.css";
import ProductDetail from "./ProductDetail/ProductDetail";

interface CommonProps {
  carts: OrderItemProps[];
  quantity: number;
  totalPrice: number;
  onRemoveFromCart: (product: ProductFromApi) => void;
  isOpenCartWhenAdd: boolean;
  onAddToCart: (product: ProductFromApi, numberOfQuantity: number) => void;
  onUpdateCartQuantity: (product: ProductFromApi, quantity: number) => void;
  cartIconRef: React.RefObject<HTMLDivElement>;
  user: UserProps | null;
}

interface ProductPageProps extends CommonProps {
  products: ProductFromApi[];
  productBoxes: ProductFromApi[];
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
  user,
}: ProductPageProps) => {
  const { title } = useParams<{ title: string }>();
  const normalizedTitle = title?.replace(/-/g, " ").toLowerCase().trim();
  const product = products.find(
    (item) => item.name.toLowerCase().trim() === normalizedTitle
  );
  const productBox = productBoxes.find(
    (item) => item.name.toLowerCase().trim() === normalizedTitle
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
        sellingProducts={products}
        user={user}
      />

      <ProductDetail
        currentProduct={currentProduct}
        onAddToCart={onAddToCart}
        cartIconRef={cartIconRef}
      />

      <Footer />
    </>
  );
};

export default Product;
