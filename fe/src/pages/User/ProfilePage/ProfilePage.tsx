import React, { useState } from "react";
import {
  OrderItemProps,
  ProductFromApi,
  RawOrderItem,
  UserProps,
} from "../../../App";
import "./ProfilePage.css";
import Header from "../../../components/Header/Header";
import Footer from "../../../components/Footer/Footer";
import UserSettings from "./UserSettings/UserSettings";
import OrderHistory from "./OrderHistory/OrderHistory";
import { useAuthenContext } from "../../../hooks/AuthenContext";
import { ApiGateway } from "../../../services/api/ApiService";

interface ProfilePageProps {
  carts: OrderItemProps[];
  quantity: number;
  totalPrice: number;
  onRemoveFromCart: (product: ProductFromApi) => void;
  isOpenCartWhenAdd: boolean;
  cartIconRef: React.RefObject<HTMLDivElement>;
  onUpdateCartQuantity: (product: ProductFromApi, quantity: number) => void;
  products: ProductFromApi[];
  user: UserProps | null;
}

export interface PaymentOrderProps {
  id: string;
  paymentMethod: string;
  status: string;
  amount: number;
  paidAt: string;
  orderId: string;
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
  user,
}) => {
  const [activeTab, setActiveTab] = useState("profile");
  const { fetchUser } = useAuthenContext();
  const [getPaymentOrders, setGetPaymentOrders] = useState<PaymentOrderProps[]>(
    []
  );
  const [paymentDetails, setPaymentDetails] = useState<OrderItemProps[]>([]);
  const [selectedOrderId, setSelectedOrderId] = useState<string | null>(null);
  const [isOpenPaymentDetails, setIsOpenPaymentDetails] = useState(false);

  const handleTabClick = (tab: string) => {
    setActiveTab(tab);
  };

  const handleChoosePaymentDetails = async (orderId: string) => {
    const orderItems: RawOrderItem[] =
      (await ApiGateway.getAllOrderProducts<RawOrderItem[]>(orderId)) ?? [];

    const orderItemsWithProduct = (await Promise.all(
      orderItems.map(async (item) => {
        const product = await ApiGateway.getProductById<ProductFromApi>(
          item.productId
        );
        if (!product) return null;

        return { ...item, product };
      })
    )) as OrderItemProps[];

    setPaymentDetails(orderItemsWithProduct);
    setSelectedOrderId(orderId);
    setIsOpenPaymentDetails(true);
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
        user={user}
      />

      {
        <div className="profile-page-container">
          <div className="profile-page-flex">
            <div className="profile-tabs">
              <button
                className={`profile-tab ${
                  activeTab === "profile" ? "active" : ""
                }`}
                onClick={() => {
                  handleTabClick("profile");
                  setSelectedOrderId(null);
                  setIsOpenPaymentDetails(false);
                }}
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
              {activeTab === "profile" && (
                <UserSettings user={user} loadUser={fetchUser} />
              )}
              {activeTab === "orders" && (
                <OrderHistory
                  handleChoosePaymentDetails={handleChoosePaymentDetails}
                  getPaymentOrders={getPaymentOrders}
                  selectedOrderId={selectedOrderId}
                  setGetPaymentOrders={setGetPaymentOrders}
                />
              )}
            </div>
          </div>
          {isOpenPaymentDetails && (
            <div className="profile-page-background">
              <div className="profile-page-payment-details">
                <h2>Payment Details</h2>
                <div className="profile-page-payment-details-list">
                  {paymentDetails.map((item) => (
                    <div key={item.id} className="profile-page-payment-item">
                      <img
                        src={item.product.imageUrl}
                        alt={item.product.name}
                        className="profile-page-product-image"
                      />
                      <div className="profile-page-product-info">
                        <h3>{item.product.name}</h3>
                        <p>Quantity: {item.quantity}</p>
                        <p>Price: ${item.product.price}</p>
                      </div>
                    </div>
                  ))}
                </div>
              </div>
            </div>
          )}
        </div>
      }

      <Footer />
    </>
  );
};

export default ProfilePage;
