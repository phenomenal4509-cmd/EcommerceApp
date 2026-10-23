import "./OrdersPage.css";
import { Header } from "../../components/Header.jsx";
import { OrdersGrid } from "./OrdersGrid.jsx";

export function OrdersPage({ cart, orders, getCart, getOrders }) {
  return (
    <>
      <link rel="icon" type="image/png" href="/images/orders-favicon.png" />
      <title>Orders - Syed&apos;s Store</title>
      <Header cart={cart} />

      <div className="orders-page">
        <div className="page-title">Your Orders</div>
        <OrdersGrid orders={orders} getCart={getCart} getOrders={getOrders} />
      </div>
    </>
  );
}
