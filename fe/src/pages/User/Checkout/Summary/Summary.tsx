import React, { useState } from "react";
import "./Summary.css";
import { ApiGateway } from "../../../../services/api/ApiService";
import { OrderProps, UserProps } from "../../../../App";

interface CheckOutProps {
  approvalUrl: string;
  paymentId: string;
}

interface SummaryProps {
  totalPrice: number;
  user: UserProps | null;
}

const Summary: React.FC<SummaryProps> = ({ totalPrice, user }) => {
  const [isCheckout, setIsCheckout] = useState(false);

  const handleCheckout = async () => {
    if (!user) {
      alert("Please login to proceed to checkout.");
      return;
    }

    setIsCheckout(true);

    const orderUser = await ApiGateway.getOrderByUserId<OrderProps>();
    if (!orderUser?.id) {
      console.error("No order found for the user.");
      return;
    }

    try {
      const data = await ApiGateway.processPayment<CheckOutProps>(
        orderUser.id,
        Number(totalPrice.toFixed(2))
      );

      if (!data || !data.approvalUrl) {
        console.error("Failed to process payment or missing approval URL.");
        return;
      }

      setTimeout(() => {
        setIsCheckout(false);
        window.location.href = data.approvalUrl;
      }, 2000);
    } catch (error) {
      console.error("Error during payment processing:", error);
      setIsCheckout(false);
    }
  };

  return (
    <div className="checkout-summary">
      <div className="checkout-header-summary">
        <div>
          <i className="ri-roadster-line"></i>
          <p>100%</p>
        </div>
        <h3>Satisfaction Guarantee</h3>
        <p>
          Pick up your items in store with free and easy returns or exchanges.
        </p>
      </div>
      <div className="checkout-body-summary">
        <p className="title">Order summary</p>
        <div className="order-total">
          <div className="details">
            <p>SubTotal</p>
            <p>${totalPrice.toFixed(2)}</p>
          </div>
          <div className="details">
            <p>Standard shipping</p>
            <p>FREE</p>
          </div>
          <div className="details">
            <p>REI store pick-up</p>
            <p>FREE</p>
          </div>
          <div className="details">
            <div className="tax-info">
              <p>Estimated taxes</p>
              <svg
                data-v-ce764d51=""
                xmlns="http://www.w3.org/2000/svg"
                viewBox="0 0 24 24"
                className="cdr-icon_15-2-0 cdr-icon--small_15-2-0 cdr-button__icon"
                aria-label="Tax information"
              >
                <path
                  role="presentation"
                  d="M12 2c5.523 0 10 4.477 10 10s-4.477 10-10 10S2 17.523 2 12 6.477 2 12 2zm0 13.5a1.25 1.25 0 100 2.5 1.25 1.25 0 000-2.5zM12 6a3.5 3.5 0 00-3.5 3.5 1 1 0 002 0 1.5 1.5 0 013 0c0 .816-.881 1.119-1.5 1.75s-1 1.296-1 2.257a1 1 0 102-.007 1.493 1.493 0 01.695-1.266c.233-.148 1.805-.938 1.805-2.734A3.5 3.5 0 0012 6z"
                ></path>
              </svg>
            </div>
            <p>_ _ _</p>
          </div>
          <div className="details">
            <p>Order total</p>
            <p>${totalPrice.toFixed(2)}</p>
          </div>
        </div>
      </div>
      <div className="read">
        <div>
          <svg
            data-v-e86ab2c9=""
            xmlns="http://www.w3.org/2000/svg"
            viewBox="0 0 24 24"
            aria-hidden="true"
            className="cdr-icon_15-2-0 alert__icon"
          >
            <path
              role="presentation"
              d="M19.998 3a1 1 0 011 1l-.011 7.587a1 1 0 01-.293.705l-8.404 8.414a1 1 0 01-1.411.004l-7.582-7.506a1 1 0 01-.007-1.415l8.402-8.492A1 1 0 0112.403 3h7.595zm-1.002 2h-6.175l-7.408 7.485 6.165 6.103 7.41-7.417L18.995 5zM16.75 6a1.25 1.25 0 110 2.5 1.25 1.25 0 010-2.5z"
            ></path>
          </svg>
        </div>
        <p>
          Apply coupons in the Review and Pay section at the end of checkout.
        </p>
      </div>
      <div className={`procced-button ${isCheckout ? "disabled" : ""}`}>
        <div>
          <button
            onClick={handleCheckout}
            disabled={isCheckout}
            className={`${isCheckout ? "disabled" : ""}`}
          >
            Procced to checkout
          </button>
          {isCheckout && <div className="loading"></div>}
        </div>

        <a href="/" className="cancel-btn">
          Cancel
        </a>
      </div>
    </div>
  );
};

export default Summary;
