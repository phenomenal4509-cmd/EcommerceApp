import api from "../../api/axiosInstance";
import { useState } from "react";
import { useAuth } from "../../context/AuthContext";
import { useNavigate } from "react-router";

// Backend ProductDetailsDto: { id, name, price, description, version }
// price is in dollars (decimal), no image/rating fields

export function Product({ product, getCart }) {
  const { isAuthenticated } = useAuth();
  const navigate = useNavigate();
  const [addedMessage, setAddedMessage] = useState(false);
  const [quantity, setQuantity] = useState(1);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState("");

  const addToCart = async () => {
    if (!isAuthenticated) {
      navigate("/login");
      return;
    }
    setLoading(true);
    setError("");
    try {
      // POST /cart  or  PUT /cart  (backend decides based on whether cart exists)
      // We always send the full desired cart state via PUT if it exists,
      // or create via POST. The simplest approach is to try POST first,
      // then fall back to PUT if a cart already exists.
      try {
        await api.post("/cart", {
          items: [{ productId: product.id, quantity, deliveryOptionId: 1 }],
        });
      } catch (postErr) {
        if (postErr.response?.status === 409) {
          // Cart already exists — fetch it and merge
          const cartRes = await api.get("/cart");
          const existingProducts = cartRes.data.products ?? [];

          // Build new items list: merge existing + new item
          const existingItem = existingProducts.find((p) => p.id === product.id);
          let newItems;
          if (existingItem) {
            newItems = existingProducts.map((p) =>
              p.id === product.id
                ? { productId: p.id, quantity: p.quantity + quantity, deliveryOptionId: p.deliveryOptionId }
                : { productId: p.id, quantity: p.quantity, deliveryOptionId: p.deliveryOptionId }
            );
          } else {
            newItems = [
              ...existingProducts.map((p) => ({
                productId: p.id,
                quantity: p.quantity,
                deliveryOptionId: p.deliveryOptionId,
              })),
              { productId: product.id, quantity, deliveryOptionId: 1 },
            ];
          }
          await api.put("/cart", { items: newItems });
        } else {
          throw postErr;
        }
      }

      await getCart();
      setAddedMessage(true);
      setTimeout(() => setAddedMessage(false), 2000);
    } catch (err) {
      const msg =
        err.response?.data?.detail ||
        err.response?.data ||
        "Could not add to cart.";
      setError(typeof msg === "string" ? msg : "Could not add to cart.");
    } finally {
      setLoading(false);
    }
  };

  const selectQuantity = (event) => {
    setQuantity(Number(event.target.value));
  };

  return (
    <div className="product-container">
      <div className="product-image-container">
        <img className="product-image" src={`/${product.image}`} alt={product.name} data-testid="product-image" />
      </div>

      <div className="product-name limit-text-to-2-lines">{product.name}</div>

      {/* No rating in backend – show description category instead */}
      <div className="product-rating-container">
        <div className="product-category-text">{product.description}</div>
      </div>

      {/* price is in dollars from backend */}
      <div className="product-price">${product.price.toFixed(2)}</div>

      <div className="product-quantity-container">
        <select value={quantity} onChange={selectQuantity}>
          {[1,2,3,4,5,6,7,8,9,10].map((n) => (
            <option key={n} value={n}>{n}</option>
          ))}
        </select>
      </div>

      <div className="product-spacer"></div>

      {error && <div className="product-error">{error}</div>}

      <div className="added-to-cart" style={{ opacity: addedMessage ? 1 : 0 }}>
        <img src="images/icons/checkmark.png" alt="Added" />
        Added
      </div>

      <button
        className="add-to-cart-button button-primary"
        onClick={addToCart}
        disabled={loading}
        data-testid="add-to-cart-button"
      >
        {loading ? "Adding…" : "Add to Cart"}
      </button>
    </div>
  );
}
