import { CheckoutHeader } from "./CheckoutHeader";
import api from "../../api/axiosInstance";
import { OrderSummary } from "./OrderSummary";
import { useState, useEffect } from "react";
import "../Checkout/CheckoutPage.css";
import { PaymentSummary } from "./PaymentSummary";

// Backend endpoints used:
//   GET  /delivery-options  → DeliveryOptionDto[]  { id, deliveryDays, price, estimatedDeliveryTimeMs }
//   GET  /payment-summary   → PaymentSummaryDto    { totalItems, productCostCents, shippingCostCents,
//                                                     totalCostBeforeTaxCents, taxCents, totalCostCents }

export function CheckoutPage({ cart, getCart }) {
  const [deliveryOptions, setDeliveryOptions] = useState([]);
  const [paymentSummary, setPaymentSummary] = useState(null);
  const [loadingDelivery, setLoadingDelivery] = useState(true);
  const [loadingPayment, setLoadingPayment] = useState(true);
  const [deliveryError, setDeliveryError] = useState("");
  const [paymentError, setPaymentError] = useState("");

  // Fetch delivery options (no auth required)
  useEffect(() => {
    const getDeliveryData = async () => {
      setLoadingDelivery(true);
      setDeliveryError("");
      try {
        const res = await api.get("/delivery-options");
        setDeliveryOptions(res.data ?? []);
      } catch {
        setDeliveryError("Could not load delivery options.");
      } finally {
        setLoadingDelivery(false);
      }
    };
    getDeliveryData();
  }, []);

  // Fetch payment summary whenever cart changes
  useEffect(() => {
    const getPaymentData = async () => {
      setLoadingPayment(true);
      setPaymentError("");
      try {
        const res = await api.get("/payment-summary");
        setPaymentSummary(res.data);
      } catch (err) {
        if (err.response?.status === 401) {
          // Not logged in — clear summary silently
          setPaymentSummary(null);
        } else {
          setPaymentError("Could not load payment summary.");
        }
      } finally {
        setLoadingPayment(false);
      }
    };
    getPaymentData();
  }, [cart]);

  return (
    <>
      <link rel="icon" type="image/png" href="/images/cart-favicon.png" />
      <title>Checkout - Syed&apos;s Store</title>
      <CheckoutHeader cart={cart} />
      <div className="checkout-page">
        <div className="page-title">Review your order</div>

        {deliveryError && <div className="checkout-error">{deliveryError}</div>}
        {paymentError  && <div className="checkout-error">{paymentError}</div>}

        <div className="checkout-grid">
          <OrderSummary
            cart={cart}
            deliveryOptions={deliveryOptions}
            loadingDelivery={loadingDelivery}
            getCart={getCart}
          />
          <PaymentSummary
            paymentSummary={paymentSummary}
            loading={loadingPayment}
            getCart={getCart}
          />
        </div>
      </div>
    </>
  );
}
