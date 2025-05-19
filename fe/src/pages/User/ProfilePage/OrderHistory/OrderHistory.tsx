import { useEffect, useState } from "react";
import { OrderProps } from "../../../../App";
import "./OrderHistory.css";
import { ApiGateway } from "../../../../services/api/ApiService";

interface PaymentOrderProps {
  id: string;
  paymentMethod: string;
  status: string;
  amount: number;
  paidAt: string;
}

const OrderHistory = () => {
  const [paymentOrders, setPaymentOrders] = useState<PaymentOrderProps[]>([]);

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

      setPaymentOrders(filteredOrders);
    };

    fetchPaymentOrders()
      .then(() => console.log("Payment orders fetched successfully"))
      .catch((error) => console.error("Error fetching payment orders:", error));
  }, []);

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
          </tr>
        </thead>
        <tbody>
          {paymentOrders.length === 0 ? (
            <tr>
              <td colSpan={4}>No orders found</td>
            </tr>
          ) : (
            paymentOrders.map((order) => (
              <tr key={order.id}>
                <td>{paymentOrders.indexOf(order) + 1}</td>
                <td>{order.paymentMethod}</td>
                <td>{new Date(order.paidAt).toLocaleDateString()}</td>
                <td>${order.amount.toFixed(2)}</td>
                <td>{order.status}</td>
              </tr>
            ))
          )}
        </tbody>
      </table>
    </div>
  );
};

export default OrderHistory;
