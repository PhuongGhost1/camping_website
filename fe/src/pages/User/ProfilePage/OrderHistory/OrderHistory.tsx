import { useEffect } from "react";
import { OrderProps } from "../../../../App";
import "./OrderHistory.css";
import { ApiGateway } from "../../../../services/api/ApiService";
import { PaymentOrderProps } from "../ProfilePage";
interface OrderHistoryProps {
  handleChoosePaymentDetails: (orderId: string) => void;
  getPaymentOrders: PaymentOrderProps[];
  selectedOrderId: string | null;
  setGetPaymentOrders: React.Dispatch<
    React.SetStateAction<PaymentOrderProps[]>
  >;
}

const OrderHistory: React.FC<OrderHistoryProps> = ({
  handleChoosePaymentDetails,
  getPaymentOrders,
  selectedOrderId,
  setGetPaymentOrders,
}) => {
  useEffect(() => {
    const fetchPaymentOrders = async () => {
      const order = await ApiGateway.GetAllOrders<OrderProps[]>();
      if (!order) {
        console.error("No orders found for this user");
        return;
      }

      const paymentOrders = order.map((item) =>
        ApiGateway.GetAllPayments<PaymentOrderProps[]>(item.id)
      );

      const paymentOrdersData = await Promise.all(paymentOrders);

      const filteredOrders = paymentOrdersData
        .flat()
        .filter(
          (order): order is PaymentOrderProps =>
            order !== null && order !== undefined
        );

      setGetPaymentOrders(filteredOrders);
    };

    fetchPaymentOrders();
  }, [setGetPaymentOrders]);

  const flattenedOrders = getPaymentOrders
    .flatMap((item) => item.payment)
    .filter((p) => p !== undefined && p !== null);
  console.log(flattenedOrders);

  return (
    <div className="order-history">
      <h1>Order History</h1>
      <table className="order-history-table">
        <thead>
          <tr>
            <th>No</th>
            <th>Payment Method</th>
            <th>Date</th>
            <th>Total Amount</th>
            <th>Status</th>
            <th></th>
          </tr>
        </thead>
        <tbody>
          {flattenedOrders.length === 0 ? (
            <tr>
              <td colSpan={4}>No orders found</td>
            </tr>
          ) : (
            flattenedOrders.map((order) => (
              <tr
                key={order.id}
                className={`order-history-row ${
                  selectedOrderId === order.orderId ? "clicked" : ""
                }`}
              >
                <td>{flattenedOrders.indexOf(order) + 1}</td>
                <td>{order.paymentMethod}</td>
                <td>{new Date(order.paidAt).toLocaleDateString()}</td>
                <td>${order.amount}</td>
                <td>{order.status}</td>
                <td>
                  <button
                    className="order-history-button"
                    onClick={() => handleChoosePaymentDetails(order.orderId)}
                  >
                    View
                  </button>
                </td>
              </tr>
            ))
          )}
        </tbody>
      </table>
    </div>
  );
};

export default OrderHistory;
