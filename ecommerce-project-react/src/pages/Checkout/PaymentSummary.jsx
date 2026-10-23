import { formatMoney } from "../../utlis/money";
import { useNavigate } from "react-router";
import api from "../../api/axiosInstance";
import { useState } from "react";

// paymentSummary: PaymentSummaryDto
//   { totalItems, productCostCents, shippingCostCents,
//     totalCostBeforeTaxCents, taxCents, totalCostCents }  — all in cents

export function PaymentSummary({ paymentSummary, loading, getCart }) {
  const navigate = useNavigate();
  const [placing, setPlacing] = useState(false);
  const [error, setError] = useState("");

  const createOrder = async () => {
    setPlacing(true);
    setError("");
    try {
      // POST /checkout — backend converts cart → order and clears the cart
      await api.post("/checkout");
      await getCart(); // clear local cart state
      navigate("/order");
    } catch (err) {
      const msg =
        err.response?.data?.detail ||
        err.response?.data ||
        "Could not place order. Please try again.";
      setError(typeof msg === "string" ? msg : "Could not place order.");
    } finally {
      setPlacing(false);
    }
  };

  return (
    <div className="payment-summary">
      <div className="payment-summary-title">Payment Summary</div>

      {loading && (
        <div className="payment-summary-loading">Calculating…</div>
      )}

      {!loading && paymentSummary && (
        <>
          <div className="payment-summary-row">
            <div>Items ({paymentSummary.totalItems}):</div>
            <div className="payment-summary-money">
              ${formatMoney(paymentSummary.productCostCents)}
            </div>
          </div>

          <div className="payment-summary-row">
            <div>Shipping &amp; handling:</div>
            <div className="payment-summary-money">
              ${formatMoney(paymentSummary.shippingCostCents)}
            </div>
          </div>

          <div className="payment-summary-row subtotal-row">
            <div>Total before tax:</div>
            <div className="payment-summary-money">
              ${formatMoney(paymentSummary.totalCostBeforeTaxCents)}
            </div>
          </div>

          <div className="payment-summary-row">
            <div>Estimated tax (10%):</div>
            <div className="payment-summary-money">
              ${formatMoney(paymentSummary.taxCents)}
            </div>
          </div>

          <div className="payment-summary-row total-row">
            <div>Order total:</div>
            <div className="payment-summary-money">
              ${formatMoney(paymentSummary.totalCostCents)}
            </div>
          </div>

          {error && (
            <div className="payment-summary-error">{error}</div>
          )}

          <button
            className="place-order-button button-primary"
            onClick={createOrder}
            disabled={placing}
          >
            {placing ? "Placing order…" : "Place your order"}
          </button>
        </>
      )}

      {!loading && !paymentSummary && (
        <div className="payment-summary-empty">
          Add items to your cart to see the total.
        </div>
      )}
    </div>
  );
}
