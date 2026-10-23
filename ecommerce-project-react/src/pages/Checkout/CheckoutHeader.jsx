import "../Checkout/CheckoutHeader.css";
import { getQuantity } from "../../utlis/quantity";
import { Link } from "react-router";
export function CheckoutHeader({ cart }) {
  let totalQuantity = getQuantity(cart);
  return (
    <>
      <div className="checkout-header">
        <div className="header-content">
          <div className="checkout-header-left-section">
            <Link to="/" className="checkout-logo-link">
              <span className="checkout-logo-text">
                Syed<span className="logo-accent">'s</span> Store
              </span>
              <span className="checkout-mobile-logo-text">
                S<span className="logo-accent">'s</span>
              </span>
            </Link>
          </div>

          <div className="checkout-header-middle-section">
            Checkout (
            <a className="return-to-home-link" href="/">
              {totalQuantity} items
            </a>
            )
          </div>

          <div className="checkout-header-right-section">
            <img src="images/icons/checkout-lock-icon.png" />
          </div>
        </div>
      </div>
    </>
  );
}
