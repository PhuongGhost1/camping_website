import React, { useState } from "react";
import { OrderItemProps, ProductFromApi } from "../../../App";
import "./ProfilePage.css";
import Header from "../../../components/Header/Header";
import Footer from "../../../components/Footer/Footer";
import UserSettings from "./UserSettings/UserSettings";
import OrderHistory from "./OrderHistory/OrderHistory";

interface ProfilePageProps {
  carts: OrderItemProps[];
  quantity: number;
  totalPrice: number;
  onRemoveFromCart: (product: ProductFromApi) => void;
  isOpenCartWhenAdd: boolean;
  cartIconRef: React.RefObject<HTMLDivElement>;
  onUpdateCartQuantity: (title: string, quantity: number) => void;
  products: ProductFromApi[];
}

const ProfilePage: React.FC<ProfilePageProps> = ({
  carts,
  quantity,
  totalPrice,
  onRemoveFromCart,
  isOpenCartWhenAdd,
  cartIconRef,
  onUpdateCartQuantity,
  products,
}) => {
  const [activeTab, setActiveTab] = useState("profile");

  const handleTabClick = (tab: string) => {
    setActiveTab(tab);
  };

  return (
    <>
      <Header
        carts={carts}
        quanity={quantity}
        totalPriceOnCart={totalPrice}
        onRemoveFromCart={onRemoveFromCart}
        isOpenCartWhenAdd={isOpenCartWhenAdd}
        cartIconRef={cartIconRef}
        onUpdateCartQuantity={onUpdateCartQuantity}
        sellingProducts={products}
      />

      {
        <div className="profile-page-container">
          <div className="profile-page-flex">
            <div className="profile-tabs">
              <button
                className={`profile-tab ${
                  activeTab === "profile" ? "active" : ""
                }`}
                onClick={() => handleTabClick("profile")}
              >
                Profile
              </button>
              <button
                className={`profile-tab ${
                  activeTab === "orders" ? "active" : ""
                }`}
                onClick={() => handleTabClick("orders")}
              >
                Orders
              </button>
            </div>

            <div className="profile-content">
              {activeTab === "profile" && <UserSettings />}
              {activeTab === "orders" && <OrderHistory />}
            </div>
          </div>
        </div>
      }

      <Footer />
    </>
  );
};

export default ProfilePage;
