import { useEffect, useState, useCallback } from "react";
import { Routes, Route, Navigate } from "react-router";
import "./App.css";

// Pages
import { HomePage }     from "./pages/Home/HomePage";
import { OrdersPage }   from "./pages/Orders/OrdersPage";
import { CheckoutPage } from "./pages/Checkout/CheckoutPage";
import { TrackingPage } from "./pages/TrackingPage";
import { PageNotFound } from "./pages/PageNotFound";
import { LoginPage }    from "./pages/Auth/LoginPage";
import { RegisterPage } from "./pages/Auth/RegisterPage";

// Auth
import { useAuth } from "./context/AuthContext";
import api from "./api/axiosInstance";

// ─── Protected route wrapper ─────────────────────────────────────────────────
function RequireAuth({ children }) {
  const { isAuthenticated, authLoading } = useAuth();
  if (authLoading) return null; // wait for token validation before rendering
  return isAuthenticated ? children : <Navigate to="/login" replace />;
}

// ─── Root App component ──────────────────────────────────────────────────────
function App() {
  const { isAuthenticated } = useAuth();

  // cart: array of CartItemDetailsDto from backend
  // { id, name, price, description, quantity, deliveryOptionId }
  const [cart, setCart] = useState([]);
  const [orders, setOrders] = useState([]);

  // ── Fetch cart from backend ────────────────────────────────────────────────
  // GET /cart → { cartId, totalCost, products: [...CartItemDetailsDto] }
  const getCart = useCallback(async () => {
    if (!isAuthenticated) {
      setCart([]);
      return;
    }
    try {
      const response = await api.get("/cart");
      // products is the array of CartItemDetailsDto
      setCart(response.data.products ?? []);
    } catch (err) {
      // 404 means user has no cart yet – that is fine
      if (err.response?.status === 404) {
        setCart([]);
      }
      // 401 handled by interceptor
    }
  }, [isAuthenticated]);

  // ── Fetch orders from backend ──────────────────────────────────────────────
  // GET /orders → OrderDetailsDto[]
  const getOrders = useCallback(async () => {
    if (!isAuthenticated) {
      setOrders([]);
      return;
    }
    try {
      const response = await api.get("/orders");
      setOrders(response.data ?? []);
    } catch {
      setOrders([]);
    }
  }, [isAuthenticated]);

  // Refresh both on auth change
  useEffect(() => {
    getCart();
    getOrders();
  }, [isAuthenticated, getCart, getOrders]);

  return (
    <Routes>
      {/* Public routes */}
      <Route path="/login"    element={<LoginPage />} />
      <Route path="/register" element={<RegisterPage />} />

      {/* Public-accessible main pages (cart/orders require auth – handled inside) */}
      <Route
        index
        element={<HomePage cart={cart} getCart={getCart} />}
      />
      <Route
        path="/checkout"
        element={
          <RequireAuth>
            <CheckoutPage cart={cart} getCart={getCart} />
          </RequireAuth>
        }
      />
      <Route
        path="/order"
        element={
          <RequireAuth>
            <OrdersPage cart={cart} orders={orders} getCart={getCart} getOrders={getOrders} />
          </RequireAuth>
        }
      />
      <Route
        path="/tracking/:orderId"
        element={
          <RequireAuth>
            <TrackingPage cart={cart} />
          </RequireAuth>
        }
      />

      {/* 404 */}
      <Route path="*" element={<PageNotFound cart={cart} />} />
    </Routes>
  );
}

export default App;
