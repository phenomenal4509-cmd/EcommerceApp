import "./ProductFilters.css";
import { useEffect, useRef, useState } from "react";
import { useSearchParams } from "react-router";

const SORT_OPTIONS = [
  { value: "id:asc", label: "Featured" },
  { value: "id:desc", label: "Newest arrivals" },
  { value: "name:asc", label: "Name: A to Z" },
  { value: "name:desc", label: "Name: Z to A" },
  { value: "price:asc", label: "Price: low to high" },
  { value: "price:desc", label: "Price: high to low" },
];

const parsePrice = (value) => {
  const trimmed = value.trim();
  if (trimmed === "") return null;
  const parsed = Number(trimmed);
  return Number.isFinite(parsed) && parsed >= 0 ? String(parsed) : null;
};

const validatePrices = (minPrice, maxPrice) => {
  const hasMin = minPrice.trim() !== "";
  const hasMax = maxPrice.trim() !== "";

  if (!hasMin && !hasMax) return "";

  const min = Number(minPrice);
  const max = Number(maxPrice);

  if ((hasMin && !Number.isFinite(min)) || (hasMax && !Number.isFinite(max)) || min < 0 || max < 0) {
    return "Prices must be zero or a positive number.";
  }

  if (hasMin && hasMax && min > max) {
    return "Minimum price cannot be greater than maximum price.";
  }

  return "";
};

export function ProductFilters({ onApply, isOpen, onClose, id }) {
  const [searchParams, setSearchParams] = useSearchParams();

  const nameFilter = searchParams.get("name") ?? "";
  const minPrice = searchParams.get("minPrice") ?? "";
  const maxPrice = searchParams.get("maxPrice") ?? "";
  const sortValue = `${searchParams.get("sortBy") ?? "id"}:${searchParams.get("sortDirection") ?? "asc"}`;

  const [draftName, setDraftName] = useState(nameFilter);
  const [draftMinPrice, setDraftMinPrice] = useState(minPrice);
  const [draftMaxPrice, setDraftMaxPrice] = useState(maxPrice);

  useEffect(() => {
    setDraftName(nameFilter);
  }, [nameFilter]);

  useEffect(() => {
    setDraftMinPrice(minPrice);
  }, [minPrice]);

  useEffect(() => {
    setDraftMaxPrice(maxPrice);
  }, [maxPrice]);

  const priceError = validatePrices(draftMinPrice, draftMaxPrice);
  const hasActiveFilters = Boolean(nameFilter || minPrice || maxPrice);

  const asideRef = useRef(null);

  useEffect(() => {
    if (isOpen && asideRef.current) {
      const focusTarget =
        asideRef.current.querySelector("input, select") ?? asideRef.current.querySelector("button");
      focusTarget?.focus();
    }
  }, [isOpen]);

  useEffect(() => {
    if (!isOpen) return undefined;

    const handleKeyDown = (event) => {
      if (event.key === "Escape") onClose?.();
    };

    window.addEventListener("keydown", handleKeyDown);
    return () => window.removeEventListener("keydown", handleKeyDown);
  }, [isOpen, onClose]);

  const apply = (updates) => {
    const next = new URLSearchParams(searchParams);

    for (const [key, value] of Object.entries(updates)) {
      if (value) {
        next.set(key, value);
      } else {
        next.delete(key);
      }
    }

    setSearchParams(next);
    onApply();
    onClose?.();
  };

  const handleSubmit = (event) => {
    event.preventDefault();
    if (priceError) return;

    apply({
      name: draftName.trim(),
      minPrice: parsePrice(draftMinPrice) ?? "",
      maxPrice: parsePrice(draftMaxPrice) ?? "",
    });
  };

  const handleSortChange = (event) => {
    const [sortBy, sortDirection] = event.target.value.split(":");
    apply({ sortBy, sortDirection });
  };

  const handleClear = () => {
    setDraftName("");
    setDraftMinPrice("");
    setDraftMaxPrice("");
    apply({ name: "", minPrice: "", maxPrice: "" });
  };

  return (
    <>
      <div
        className={`filters-overlay ${isOpen ? "visible" : ""}`}
        onClick={onClose}
        aria-hidden="true"
      />

      <aside ref={asideRef} id={id} className={`product-filters ${isOpen ? "open" : ""}`} aria-hidden={!isOpen} inert={!isOpen}>
        <div className="product-filters-header">
          <h2>Filters</h2>
          <button type="button" className="product-filters-close" onClick={onClose} aria-label="Close filters">
            ✕
          </button>
        </div>

        <div className="product-filters-section">
          <p className="product-filters-section-title">Search</p>
          <form className="product-filters-form" onSubmit={handleSubmit}>
            <div className="product-filters-field">
              <label htmlFor="filter-name">Name contains</label>
              <input
                id="filter-name"
                type="text"
                placeholder="e.g. shirt"
                value={draftName}
                onChange={(event) => setDraftName(event.target.value)}
              />
            </div>

            <div className="product-filters-price-row">
              <div className="product-filters-field">
                <label htmlFor="filter-min-price">Min price</label>
                <input
                  id="filter-min-price"
                  type="number"
                  min="0"
                  step="0.01"
                  placeholder="0.00"
                  value={draftMinPrice}
                  onChange={(event) => setDraftMinPrice(event.target.value)}
                />
              </div>

              <div className="product-filters-field">
                <label htmlFor="filter-max-price">Max price</label>
                <input
                  id="filter-max-price"
                  type="number"
                  min="0"
                  step="0.01"
                  placeholder="No limit"
                  value={draftMaxPrice}
                  onChange={(event) => setDraftMaxPrice(event.target.value)}
                />
              </div>
            </div>

            {priceError && <p className="product-filters-error">{priceError}</p>}

            <div className="product-filters-actions">
              <button type="submit" className="button-primary product-filters-apply" disabled={Boolean(priceError)}>
                Apply
              </button>

              {hasActiveFilters && (
                <button type="button" className="button-secondary product-filters-clear" onClick={handleClear}>
                  Clear
                </button>
              )}
            </div>
          </form>
        </div>

        <div className="product-filters-section">
          <p className="product-filters-section-title">Sort by</p>
          <div className="product-filters-sort">
            <select id="filter-sort" aria-label="Sort by" value={sortValue} onChange={handleSortChange}>
              {SORT_OPTIONS.map((option) => (
                <option key={option.value} value={option.value}>
                  {option.label}
                </option>
              ))}
            </select>
          </div>
        </div>
      </aside>
    </>
  );
}