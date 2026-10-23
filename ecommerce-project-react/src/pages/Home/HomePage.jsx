import "./HomePage.css";
import api from "../../api/axiosInstance";
import { useCallback, useEffect, useRef, useState } from "react";
import { Header } from "../../components/Header.jsx";
import { useSearchParams } from "react-router";
import { ProductsGrid } from "./ProductsGrid.jsx";
import { ProductFilters } from "./ProductFilters.jsx";

// Backend GET /products?search=&name=&minPrice=&maxPrice=&sortBy=&sortDirection=&page=&pageSize=
// Returns PagedResult<ProductDetailsDto>: { items, page, pageSize, totalItems, totalPages }
// ProductDetailsDto: { id, name, price, description, image, version }

export function HomePage({ cart, getCart }) {
  const [products, setProducts] = useState([]);
  const [page, setPage] = useState(1);
  const [totalPages, setTotalPages] = useState(1);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState("");
  const [filtersOpen, setFiltersOpen] = useState(false);
  const filtersToggleRef = useRef(null);

  const closeFilters = useCallback(() => {
    setFiltersOpen(false);
    filtersToggleRef.current?.focus();
  }, []);

  const [searchParams] = useSearchParams();
  const search = searchParams.get("search");
  const nameFilter = searchParams.get("name");
  const minPrice = searchParams.get("minPrice");
  const maxPrice = searchParams.get("maxPrice");
  const sortBy = searchParams.get("sortBy");
  const sortDirection = searchParams.get("sortDirection");

  useEffect(() => {
    setPage(1);
  }, [search]);

  const resetPage = useCallback(() => {
    setPage(1);
  }, []);

  useEffect(() => {
    const getHomePageData = async () => {
      setLoading(true);
      setError("");
      try {
        const params = new URLSearchParams();
        if (search) params.set("search", search);
        if (nameFilter) params.set("name", nameFilter);
        if (minPrice) params.set("minPrice", minPrice);
        if (maxPrice) params.set("maxPrice", maxPrice);
        if (sortBy) params.set("sortBy", sortBy);
        if (sortDirection) params.set("sortDirection", sortDirection);
        params.set("page", page);
        params.set("pageSize", 24);

        const response = await api.get(`/products?${params.toString()}`);
        setProducts(response.data.items ?? []);
        setTotalPages(response.data.totalPages ?? 1);
      } catch {
        setError("Could not load products. Please make sure the backend is running.");
      } finally {
        setLoading(false);
      }
    };

    getHomePageData();
  }, [search, nameFilter, minPrice, maxPrice, sortBy, sortDirection, page]);

  return (
    <>
      <link rel="icon" type="image/png" href="/images/home-favicon.png" />
      <title>Syed&apos;s Store</title>
      <Header cart={cart} />

      <div className="home-page">
        <button
          ref={filtersToggleRef}
          type="button"
          className="button-secondary filters-toggle"
          onClick={() => setFiltersOpen((open) => !open)}
          aria-expanded={filtersOpen}
          aria-controls="product-filters"
        >
          <span className="filters-toggle-icon" aria-hidden="true">☰</span>
          Filters
        </button>

        <ProductFilters
          id="product-filters"
          isOpen={filtersOpen}
          onClose={closeFilters}
          onApply={resetPage}
        />

        {loading && (
          <div className="home-loading">
            <div className="loading-spinner"></div>
            <p>Loading products…</p>
          </div>
        )}

        {!loading && error && (
          <div className="home-error">
            <p>{error}</p>
          </div>
        )}

        {!loading && !error && products.length === 0 && (
          <div className="home-empty">
            <p>No products found{search ? ` for "${search}"` : ""}.</p>
            {(nameFilter || minPrice || maxPrice) && <p>Try widening your price range or clearing the name filter.</p>}
          </div>
        )}

        {!loading && !error && products.length > 0 && (
          <>
            <ProductsGrid products={products} getCart={getCart} />
            <div className="pagination-controls" style={{ display: "flex", gap: "12px", justifyContent: "center", margin: "24px 0" }}>
              <button className="button-secondary" disabled={page <= 1} onClick={() => setPage((p) => p - 1)}>
                Previous
              </button>
              <span>Page {page} of {totalPages}</span>
              <button className="button-secondary" disabled={page >= totalPages} onClick={() => setPage((p) => p + 1)}>
                Next
              </button>
            </div>
          </>
        )}
      </div>
    </>
  );
}
