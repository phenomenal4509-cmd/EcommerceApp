import api from "../../api/axiosInstance";
import { Fragment, useState } from "react";
import dayjs from "dayjs";
import { Link } from "react-router";

// OrderDetailsDto:
//   id (int), totalCost (decimal dollars), orderDate (DateOnly string),
//   currentStatus (string), items: OrderItemDetailsDto[], trackingHistory[]
//
// OrderItemDetailsDto:
//   productId, quantity, unitPrice (decimal), productName,
//   deliveryOptionId, estimatedDeliveryAt (ISO DateTimeOffset)

export function OrdersGrid({ orders, getCart, getOrders }) {
  return (
    <div className="orders-grid">
      {orders.length === 0 && (
        <div className="orders-empty">
          You have no orders yet. <Link to="/" className="link-primary">Start shopping!</Link>
        </div>
      )}

      {orders.map((order) => (
        <div key={order.id} className="order-container">
          <div className="order-header">
            <div className="order-header-left-section">
              <div className="order-date">
                <div className="order-header-label">Order Placed:</div>
                {/* orderDate is a DateOnly string like "2024-05-15" */}
                <div>{dayjs(order.orderDate).format("MMMM D, YYYY")}</div>
              </div>

              <div className="order-total">
                <div className="order-header-label">Total:</div>
                {/* totalCost is already in dollars (decimal) */}
                <div>${Number(order.totalCost).toFixed(2)}</div>
              </div>

              <div className="order-status">
                <div className="order-header-label">Status:</div>
                <div className={`status-badge status-${order.currentStatus?.toLowerCase()}`}>
                  {order.currentStatus}
                </div>
              </div>
            </div>

            <div className="order-header-right-section">
              <div className="order-header-label">Order ID:</div>
              <div>#{order.id}</div>
            </div>
          </div>

          <div className="order-details-grid">
            {order.items.map((item) => (
              <OrderItemRow
                key={item.productId}
                item={item}
                orderId={order.id}
                cart={[]}        // passed through but not needed here
                getCart={getCart}
                getOrders={getOrders}
              />
            ))}
          </div>
        </div>
      ))}
    </div>
  );
}

function OrderItemRow({ item, orderId, getCart }) {
  const [loading, setLoading] = useState(false);

  const addToCart = async () => {
    setLoading(true);
    try {
      // Try POST first; if 409, fall back to PUT (merge)
      try {
        await api.post("/cart", {
          items: [{ productId: item.productId, quantity: 1, deliveryOptionId: item.deliveryOptionId }],
        });
      } catch (postErr) {
        if (postErr.response?.status === 409) {
          const cartRes = await api.get("/cart");
          const existing = cartRes.data.products ?? [];
          const existingEntry = existing.find((p) => p.id === item.productId);
          let newItems;
          if (existingEntry) {
            newItems = existing.map((p) => ({
              productId: p.id,
              quantity: p.id === item.productId ? p.quantity + 1 : p.quantity,
              deliveryOptionId: p.deliveryOptionId,
            }));
          } else {
            newItems = [
              ...existing.map((p) => ({ productId: p.id, quantity: p.quantity, deliveryOptionId: p.deliveryOptionId })),
              { productId: item.productId, quantity: 1, deliveryOptionId: item.deliveryOptionId },
            ];
          }
          await api.put("/cart", { items: newItems });
        } else {
          throw postErr;
        }
      }
      await getCart();
    } finally {
      setLoading(false);
    }
  };

  const deliveryDate = item.estimatedDeliveryAt
    ? dayjs(item.estimatedDeliveryAt).format("MMMM D")
    : "—";

  return (
    <Fragment key={item.productId}>
      <div className="product-image-container">
        <img className="product-image" src={`/${item.productImage}`} alt={item.productName} />
      </div>

      <div className="product-details">
        <div className="product-name">{item.productName}</div>
        <div className="product-delivery-date">
          Arriving on: {deliveryDate}
        </div>
        <div className="product-quantity">Quantity: {item.quantity}</div>
        <div className="product-unit-price">${Number(item.unitPrice).toFixed(2)} each</div>

        <button
          className="buy-again-button button-primary"
          onClick={addToCart}
          disabled={loading}
        >
          <img className="buy-again-icon" src="images/icons/buy-again.png" alt="" />
          <span className="buy-again-message">
            {loading ? "Adding…" : "Buy Again"}
          </span>
        </button>
      </div>

      <div>
        <Link to={`/tracking/${orderId}`}>
          <button className="track-package-button button-secondary">
            Track Package
          </button>
        </Link>
      </div>
    </Fragment>
  );
}
