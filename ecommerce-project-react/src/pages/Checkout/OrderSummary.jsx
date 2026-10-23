import api from "../../api/axiosInstance";
import { CartItemDetails } from "./CartItemDetails";
import { DeliveryOptions } from "./DeliveryOptions";
import dayjs from "dayjs";

// cart: CartItemDetailsDto[]  { id, name, price (dollars), description, quantity, deliveryOptionId }
// deliveryOptions: DeliveryOptionDto[]  { id, deliveryDays, price (dollars), estimatedDeliveryTimeMs }

export function OrderSummary({ cart, deliveryOptions, loadingDelivery, getCart }) {
  if (loadingDelivery) {
    return <div className="order-summary order-summary--loading">Loading delivery options…</div>;
  }

  // Build a helper to delete a single cart item by rebuilding the cart without it
  const buildDeleteFn = (itemId) => async () => {
    const remaining = cart
      .filter((ci) => ci.id !== itemId)
      .map((ci) => ({ productId: ci.id, quantity: ci.quantity, deliveryOptionId: ci.deliveryOptionId }));

    if (remaining.length === 0) {
      // Delete the whole cart
      await api.delete("/cart");
    } else {
      await api.put("/cart", { items: remaining });
    }
    await getCart();
  };

  return (
    <div className="order-summary">
      {cart.length === 0 && (
        <div className="cart-empty-msg">Your cart is empty.</div>
      )}

      {cart.map((cartItem) => {
        const selectedDeliveryOption =
          deliveryOptions.find((opt) => opt.id === cartItem.deliveryOptionId) ?? deliveryOptions[0];

        const deliveryDate = selectedDeliveryOption
          ? dayjs().add(selectedDeliveryOption.deliveryDays, "day").format("dddd, MMMM D")
          : "—";

        return (
          <div key={cartItem.id} className="cart-item-container">
            <div className="delivery-date">
              Estimated delivery: <strong>{deliveryDate}</strong>
            </div>

            <div className="cart-item-details-grid">
              <div className="product-image-container">
                <img className="product-image" src={`/${cartItem.image}`} alt={cartItem.name} />
              </div>

              <CartItemDetails
                cartItem={cartItem}
                cart={cart}
                deleteCartItem={buildDeleteFn(cartItem.id)}
                getCart={getCart}
              />

              <DeliveryOptions
                cartItem={cartItem}
                cart={cart}
                deliveryOptions={deliveryOptions}
                getCart={getCart}
              />
            </div>
          </div>
        );
      })}
    </div>
  );
}
