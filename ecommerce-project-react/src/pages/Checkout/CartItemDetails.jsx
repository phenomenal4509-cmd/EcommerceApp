import { useState } from "react";
import api from "../../api/axiosInstance";

// cartItem: CartItemDetailsDto { id, name, price (dollars), description, quantity, deliveryOptionId }
// cart: CartItemDetailsDto[]  (full cart, needed to rebuild PUT body)

export function CartItemDetails({ cartItem, cart, deleteCartItem, getCart }) {
  const [showInput, setShowInput] = useState(false);
  const [quantity, setQuantity] = useState(cartItem.quantity);
  const [loading, setLoading] = useState(false);

  const updateCartItem = async () => {
    if (!showInput) {
      setShowInput(true);
      return;
    }
    // Build full items list with updated quantity for this item
    const newQuantity = Number(quantity);
    if (newQuantity < 1 || newQuantity > 50) return;

    const updatedItems = cart.map((ci) => ({
      productId: ci.id,
      quantity: ci.id === cartItem.id ? newQuantity : ci.quantity,
      deliveryOptionId: ci.deliveryOptionId,
    }));

    setLoading(true);
    try {
      await api.put("/cart", { items: updatedItems });
      await getCart();
      setShowInput(false);
    } finally {
      setLoading(false);
    }
  };

  const checkKeyPressed = (e) => {
    if (e.key === "Enter") updateCartItem();
    else if (e.key === "Escape") setShowInput(false);
  };

  return (
    <div className="cart-item-details">
      <div className="product-name">{cartItem.name}</div>
      <div className="product-price">${cartItem.price.toFixed(2)}</div>

      <div className="product-quantity">
        <span>
          Quantity:&nbsp;{cartItem.quantity}
          <input
            type="number"
            className="input-qunatity-button"
            style={{ opacity: showInput ? 1 : 0, pointerEvents: showInput ? "auto" : "none" }}
            value={quantity}
            min={1}
            max={50}
            onChange={(e) => setQuantity(e.target.value)}
            onKeyDown={checkKeyPressed}
          />
        </span>
        <span
          className="update-quantity-link link-primary"
          onClick={updateCartItem}
          style={{ opacity: loading ? 0.5 : 1, cursor: loading ? "not-allowed" : "pointer" }}
        >
          {showInput ? "Save" : "Update"}
        </span>
        <span
          className="delete-quantity-link link-primary"
          onClick={deleteCartItem}
        >
          Delete
        </span>
      </div>
    </div>
  );
}
