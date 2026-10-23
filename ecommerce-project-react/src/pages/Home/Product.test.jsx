import { describe, it, expect, vi, beforeEach } from "vitest";
import { Product } from "./Product.jsx";
import { render, screen } from "@testing-library/react";
import userEvent from "@testing-library/user-event";
import api from "../../api/axiosInstance";

// Mock the shared axios instance (not the axios package) so Product.jsx never
// touches the real interceptors / localStorage token logic in unit tests.
vi.mock("../../api/axiosInstance", () => ({
  default: { post: vi.fn(), get: vi.fn(), put: vi.fn() },
}));

// Product.jsx reads auth state and navigates on unauthenticated add-to-cart.
vi.mock("../../context/AuthContext", () => ({
  useAuth: () => ({ isAuthenticated: true }),
}));

vi.mock("react-router", () => ({
  useNavigate: () => vi.fn(),
}));

let product;
let getCart;

beforeEach(() => {
  vi.clearAllMocks();
  api.post.mockResolvedValue({});
  product = {
    id: "e43638ce-6aa0-4b85-b27f-e1d07eb678c6",
    name: "Black and Gray Athletic Cotton Socks - 6 Pairs",
    price: 10.9,
    description: "Socks, sports, apparel",
    image: "images/products/athletic-cotton-socks-6-pairs.jpg",
  };
  getCart = vi.fn();
});

describe("Product component", () => {
  it("renders the product details correctly", () => {
    render(<Product product={product} getCart={getCart} />);

    expect(
      screen.getByText("Black and Gray Athletic Cotton Socks - 6 Pairs"),
    ).toBeInTheDocument();
    expect(screen.getByText("$10.90")).toBeInTheDocument();
    expect(screen.getByTestId("product-image")).toHaveAttribute(
      "src",
      "/images/products/athletic-cotton-socks-6-pairs.jpg",
    );
    // Rating row was replaced by the category/description text.
    expect(screen.getByText("Socks, sports, apparel")).toBeInTheDocument();
    expect(screen.getByTestId("add-to-cart-button")).toBeInTheDocument();
  });

  it("adds the product to the cart", async () => {
    const user = userEvent.setup();
    render(<Product product={product} getCart={getCart} />);

    await user.click(screen.getByTestId("add-to-cart-button"));

    expect(api.post).toHaveBeenCalledWith("/cart", {
      items: [
        { productId: product.id, quantity: 1, deliveryOptionId: 1 },
      ],
    });
    expect(getCart).toHaveBeenCalled();
  });
});
