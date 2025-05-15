import React, { useState } from "react";
import { OrderItemProps, ProductFromApi } from "../../../../App";
import "./Content.css";

interface ContentCheckOutProps {
  carts: OrderItemProps[];
  onRemoveFromCart: (product: ProductFromApi) => void;
  onUpdateCartQuantity: (product: ProductFromApi, quantity: number) => void;
}

const Content: React.FC<ContentCheckOutProps> = ({
  carts,
  onRemoveFromCart,
  onUpdateCartQuantity,
}) => {
  const [selectedOption, setSelectedOption] = useState<"pickup" | "shipping">(
    "pickup"
  );

  const totalPriceOfItem = (product: ProductFromApi) => {
    const currentProduct = carts.find(
      (item) => item.product.name === product.name
    );
    if (currentProduct) {
      return Number(product.price) * (currentProduct.quantity || 1);
    }
    return 0;
  };

  const handleQuantityChange = (
    product: ProductFromApi,
    orderId: string,
    productId: string,
    accumator: string
  ) => {
    const currentProduct = carts.find(
      (item) => item.orderId === orderId && item.productId === productId
    );
    if (currentProduct) {
      const newQuantity =
        accumator === "+"
          ? (currentProduct.quantity || 1) + 1
          : (currentProduct.quantity || 1) - 1;
      onUpdateCartQuantity(product, newQuantity);
    }
  };

  const handleOption = (option: "pickup" | "shipping") => {
    setSelectedOption(option);
  };

  return (
    <div className="checkout-content">
      <div className="checkout-title">
        <p>Items</p>
        <p>Quantity</p>
        <p>Item Price</p>
        <p>Total</p>
      </div>
      {carts.length > 0 ? (
        carts.map((orderItem, index) => (
          <div key={index} className="checkout-product">
            <div className="checkout-product-info">
              <img
                src={orderItem.product.imageUrl}
                alt={orderItem.product.name}
              />
              <div className="product-info-item">
                <h2>{orderItem.product.name}</h2>
                <p>Color: Seafoam</p>
                <p>Size: 36 fl oz</p>
                <p>Item: 2484770001</p>
                <div className="button">
                  <button className="btn-save">Save for later</button>
                  <button onClick={() => onRemoveFromCart(orderItem.product)}>
                    Remove
                  </button>
                </div>
              </div>
            </div>
            <div className="check-price">
              <div className="price">
                <div className="change-quantity">
                  <button
                    className="btn-minus"
                    onClick={() =>
                      handleQuantityChange(
                        orderItem.product,
                        orderItem.orderId,
                        orderItem.productId,
                        "-"
                      )
                    }
                  >
                    -
                  </button>
                  <input
                    type="number"
                    value={orderItem.quantity}
                    min={1}
                    readOnly
                  />
                  <button
                    className="btn-plus"
                    onClick={() =>
                      handleQuantityChange(
                        orderItem.product,
                        orderItem.orderId,
                        orderItem.productId,
                        "+"
                      )
                    }
                  >
                    +
                  </button>
                </div>
                <h3>${orderItem.price.toFixed(2)}</h3>
                <span>${totalPriceOfItem(orderItem.product).toFixed(2)} </span>
              </div>
              <div className="kind-transport">
                <div>
                  <div
                    className={`transport-box ${
                      selectedOption === "pickup" ? "active" : ""
                    }`}
                    onClick={() => handleOption("pickup")}
                  >
                    <p>Pick up</p>
                    <p>at Seattle Flagship</p>
                    <p>Free & Fast</p>
                  </div>
                  <a href="#">Change store</a>
                </div>
                <div>
                  <div
                    className={`transport-box ${
                      selectedOption === "shipping" ? "active" : ""
                    }`}
                    onClick={() => handleOption("shipping")}
                  >
                    <p>Ship to address</p>
                    <p>to 98109</p>
                    <p>Free</p>
                  </div>
                  <a href="#">Change ZIP code</a>
                </div>
              </div>
            </div>
          </div>
        ))
      ) : (
        <p>Your cart is empty.</p>
      )}
    </div>
  );
};

export default Content;
