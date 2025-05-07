import "./OrderHistory.css";

const OrderHistory = () => {
  return (
    <div className="order-history">
      <h1>Order History</h1>
      <table className="order-history-table">
        <thead>
          <tr>
            <th>Order ID</th>
            <th>Date</th>
            <th>Total Amount</th>
            <th>Status</th>
          </tr>
        </thead>
        <tbody>
          <tr>
            <td>12345</td>
            <td>2023-10-01</td>
            <td>$100.00</td>
            <td>Shipped</td>
          </tr>
          <tr>
            <td>12345</td>
            <td>2023-10-01</td>
            <td>$100.00</td>
            <td>Shipped</td>
          </tr>
          <tr>
            <td>12345</td>
            <td>2023-10-01</td>
            <td>$100.00</td>
            <td>Shipped</td>
          </tr>
        </tbody>
      </table>
    </div>
  );
};

export default OrderHistory;
