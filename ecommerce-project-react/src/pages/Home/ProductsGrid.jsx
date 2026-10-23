import { Product } from "./Product.jsx";
export function ProductsGrid({ products, getCart }) {
  return (
    <div className="products-grid">
      {products.map((product) => {
        return <Product key={product.id} product={product} getCart={getCart} />;
      })}
    </div>
  );
}
