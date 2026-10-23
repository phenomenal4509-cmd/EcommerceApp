import "./PageNotFound.css";
import { Header } from "../components/Header.jsx";
import { Link } from "react-router";

export function PageNotFound({ cart }) {
  return (
    <>
      <link rel="icon" type="image/jpg" href="/images/PageNotFound.jpg" />
      <title>Page Not Found - Syed's Store</title>
      <Header cart={cart} />
      <div className="not-found-container">
        <div className="not-found-content">
          <h1 className="not-found-code">404</h1>
          <h2 className="not-found-title">Page Not Found</h2>
          <p className="not-found-text">
            Oops! The page you are looking for has drifted away or does not exist.
          </p>
          <Link to="/" className="back-home-button">
            Go Back Home
          </Link>
        </div>
      </div>
    </>
  );
}
