import api from "../../api/axiosInstance";
import dayjs from "dayjs";

// cartItem: CartItemDetailsDto { id, name, price, quantity, deliveryOptionId }
// cart: CartItemDetailsDto[]
// deliveryOptions: DeliveryOptionDto[] { id, deliveryDays, price (dollars), estimatedDeliveryTimeMs }

export function DeliveryOptions({ cartItem, cart, deliveryOptions, getCart }) {
  const updateDeliveryOption = async (newOptionId) => {
    // Build full items list with updated deliveryOptionId for this item
    const updatedItems = cart.map((ci) => ({
      productId: ci.id,
      quantity: ci.quantity,
      deliveryOptionId: newOptionId,
    }));

    try {
      await api.put("/cart", { items: updatedItems });
      await getCart();
    } catch (err) {
      console.error("Failed to update delivery option:", err);
    }
  };

  return (
    <div className="delivery-options">
      <div className="delivery-options-title">Choose a delivery option:</div>
      {deliveryOptions.map((option) => {
        const priceString =
          option.price === 0 ? "FREE Shipping" : `$${option.price.toFixed(2)} Shipping`;

        const deliveryDate = dayjs().add(option.deliveryDays, "day").format("dddd, MMMM D");
        const isSelected = option.id === cartItem.deliveryOptionId;

        return (
          <div
            key={option.id}
            className="delivery-option"
            onClick={() => updateDeliveryOption(option.id)}
            style={{ cursor: "pointer" }}
          >
            <input
              type="radio"
              className="delivery-option-input"
              checked={isSelected}
              name={`delivery-options-${cartItem.id}`}
              onChange={() => {}}
              readOnly
            />
            <div>
              <div className="delivery-option-date">{deliveryDate}</div>
              <div className="delivery-option-price">{priceString}</div>
            </div>
          </div>
        );
      })}
    </div>
  );
}
