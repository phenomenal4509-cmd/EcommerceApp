import "./TrackingPage.css";
import dayjs from "dayjs";
import { useParams, Link } from "react-router";
import { Header } from "../components/Header.jsx";
import { useState, useEffect } from "react";
import api from "../api/axiosInstance";

// Route: /tracking/:orderId
// Backend GET /orders/:id → OrderDetailsDto
//   { id, totalCost, orderDate, currentStatus,
//     items: OrderItemDetailsDto[], trackingHistory: TrackingHistoryDto[] }
//
// TrackingHistoryDto: { status, statusChangedAt }

export function TrackingPage({ cart }) {
  const { orderId } = useParams();
  const [order, setOrder] = useState(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState("");

  useEffect(() => {
    const fetchOrder = async () => {
      setLoading(true);
      setError("");
      try {
        const res = await api.get(`/orders/${orderId}`);
        setOrder(res.data);
      } catch (err) {
        if (err.response?.status === 404) {
          setError("Order not found.");
        } else {
          setError("Could not load tracking information.");
        }
      } finally {
        setLoading(false);
      }
    };
    fetchOrder();
  }, [orderId]);

  // Determine delivery progress from currentStatus
  const statusProgress = {
    Pending:   0,
    Processing: 15,
    Shipped:   55,
    Delivered: 100,
    Cancelled: 0,
  };
  const deliveryProgress = order ? (statusProgress[order.currentStatus] ?? 0) : 0;

  return (
    <>
      <link rel="icon" type="image/png" href="/images/tracking-favicon.png" />
      <title>Tracking - Syed&apos;s Store</title>

      <Header cart={cart} />

      {loading && (
        <div className="tracking-page tracking-loading">
          <div className="loading-spinner"></div>
          <p>Loading tracking…</p>
        </div>
      )}

      {!loading && error && (
        <div className="tracking-page tracking-error">
          <p>{error}</p>
          <Link to="/order" className="link-primary">← Back to Orders</Link>
        </div>
      )}

      {!loading && !error && order && (
        <div className="tracking-page">
          <div className="order-tracking">
            <Link to="/order" className="back-to-orders-link link-primary">
              ← View all orders
            </Link>

            <div className="order-tracking-title">
              Order #{order.id} &mdash; <span className="tracking-status">{order.currentStatus}</span>
            </div>

            <div className="order-tracking-date">
              Placed on {dayjs(order.orderDate).format("MMMM D, YYYY")}
            </div>

            {/* Progress bar */}
            <div className="progress-labels-container">
              <div className={`progress-label ${deliveryProgress < 50 ? "current-status" : ""}`}>
                Preparing
              </div>
              <div className={`progress-label ${deliveryProgress >= 50 && deliveryProgress < 100 ? "current-status" : ""}`}>
                Shipped
              </div>
              <div className={`progress-label ${deliveryProgress >= 100 ? "current-status" : ""}`}>
                Delivered
              </div>
            </div>

            <div className="progress-bar-container">
              <div className="progress-bar" style={{ width: `${deliveryProgress}%` }} />
            </div>

            {/* Items in the order */}
            <div className="tracking-items">
              {order.items.map((item) => (
                <div key={item.productId} className="tracking-item">
                  <div className="tracking-item-placeholder">
                    <img className="product-image" src={`/${item.productImage}`} alt={item.productName} style={{ width: "100%", height: "100%", objectFit: "cover" }} />
                  </div>
                  <div className="tracking-item-info">
                    <div className="product-info">{item.productName}</div>
                    <div className="product-info tracking-info-sub">
                      Quantity: {item.quantity} &bull; ${Number(item.unitPrice).toFixed(2)} each
                    </div>
                    {item.estimatedDeliveryAt && (
                      <div className="delivery-date">
                        Estimated arrival: {dayjs(item.estimatedDeliveryAt).format("dddd, MMMM D")}
                      </div>
                    )}
                  </div>
                </div>
              ))}
            </div>

            {/* Tracking history log */}
            {order.trackingHistory?.length > 0 && (
              <div className="tracking-history">
                <div className="tracking-history-title">Tracking History</div>
                {[...order.trackingHistory]
                  .sort((a, b) => new Date(b.statusChangedAt) - new Date(a.statusChangedAt))
                  .map((entry, idx) => (
                    <div key={idx} className="tracking-history-entry">
                      <div className="tracking-history-status">{entry.status}</div>
                      <div className="tracking-history-date">
                        {dayjs(entry.statusChangedAt).format("MMM D, YYYY h:mm A")}
                      </div>
                    </div>
                  ))}
              </div>
            )}
          </div>
        </div>
      )}
    </>
  );
}
