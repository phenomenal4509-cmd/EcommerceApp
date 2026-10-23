import "./header.css";
import { getQuantity } from "../utlis/quantity";
import { NavLink, useSearchParams, useNavigate } from "react-router";
import { useState } from "react";
import { useAuth } from "../context/AuthContext";

export function Header(props) {
  const cart = props.cart;
  const { isAuthenticated, logout } = useAuth();

  const navigate = useNavigate();
  const totalQuantity = getQuantity(cart);

  const [searchParams] = useSearchParams();
  const search = searchParams.get("search");
  const [searchText, setSearchText] = useState(search || "");

  const getInputText = (event) => {
    setSearchText(event.target.value);
  };

  const handleSearchClick = () => {
    navigate(`/?search=${searchText}`);
  };

  const handleKeyDown = (e) => {
    if (e.key === "Enter") handleSearchClick();
  };

  const handleLogout = () => {
    logout();
    navigate("/");
  };

  return (
    <>
      <div className="header">
        <div className="left-section">
          <NavLink to="/" className="header-link">
            <span className="logo-text">
              Syed<span className="logo-accent">'s</span> Store
            </span>
            <span className="mobile-logo-text">
              S<span className="logo-accent">'s</span>
            </span>
          </NavLink>
        </div>

        <div className="middle-section">
          <input
            className="search-bar"
            type="text"
            placeholder="Search"
            value={searchText}
            onChange={getInputText}
            onKeyDown={handleKeyDown}
          />
          <button className="search-button" onClick={handleSearchClick}>
            <img className="search-icon" src="images/icons/search-icon.png" alt="Search" />
          </button>
        </div>

        <div className="right-section">
          <NavLink className="orders-link header-link" to="/order">
            <span className="orders-text">Orders</span>
          </NavLink>

          {isAuthenticated ? (
            <button
              className="header-link header-auth-btn button-secondary"
              onClick={handleLogout}
              id="logout-btn"
            >
              Sign out
            </button>
          ) : (
            <NavLink className="header-link header-auth-btn" to="/login">
              Sign in
            </NavLink>
          )}

          <NavLink className="cart-link header-link" to="/checkout">
            <img className="cart-icon" src="images/icons/cart-icon.png" alt="Cart" />
            <div className="cart-quantity">{totalQuantity}</div>
            <div className="cart-text">Cart</div>
          </NavLink>
        </div>
      </div>
    </>
  );
}
