// import { cart } from "../App.jsx";
export function getQuantity(cart = []) {
  if (!cart || !Array.isArray(cart)) return 0;
  let totalQuantity = 0;
  cart.forEach((item) => {
    totalQuantity += item.quantity;
  });
  return totalQuantity;
}
