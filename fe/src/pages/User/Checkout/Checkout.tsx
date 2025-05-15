import React from "react";
import { OrderItemProps, ProductFromApi } from "../../../App";
import "./Checkout.css";
import Footer from "../../../components/Footer/Footer";
import Content from "./Content/Content";
import Summary from "./Summary/Summary";

interface CheckoutProps {
  carts: OrderItemProps[];
  quantity: number;
  totalPrice: number;
  onRemoveFromCart: (product: ProductFromApi) => void;
  onUpdateCartQuantity: (product: ProductFromApi, quantity: number) => void;
}

const Checkout: React.FC<CheckoutProps> = ({
  carts,
  quantity,
  totalPrice,
  onRemoveFromCart,
  onUpdateCartQuantity,
}) => {
  return (
    <>
      <section className="checkout-container container">
        <div className="checkout-header">
          <h2>Your Shopping Cart</h2>
          <span>({`${quantity} ${quantity === 1 ? "item" : "items"}`})</span>
        </div>
        <div className="checkout-body">
          <Content
            carts={carts}
            onRemoveFromCart={onRemoveFromCart}
            onUpdateCartQuantity={onUpdateCartQuantity}
          />

          <Summary totalPrice={totalPrice} />
        </div>
      </section>

      <Footer />
    </>
  );
};

export default Checkout;
