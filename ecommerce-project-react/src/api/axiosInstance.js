import axios from 'axios';

// All requests go through Vite's proxy which forwards to http://localhost:5296
const api = axios.create({
  headers: {
    'Content-Type': 'application/json',
  },
});

// ─── Request interceptor ───────────────────────────────────────────────────────
// Attach the Bearer token to every outgoing request when one is stored.
api.interceptors.request.use((config) => {
  const token = localStorage.getItem('accessToken');
  if (token) {
    config.headers.Authorization = `Bearer ${token}`;
  }
  return config;
});

// ─── Response interceptor ─────────────────────────────────────────────────────
// Clear stale credentials on 401 so the user is redirected to /login.
api.interceptors.response.use(
  (response) => response,
  (error) => {
    if (error.response?.status === 401) {
      localStorage.removeItem('accessToken');
      localStorage.removeItem('tokenExpiry');
      // We dispatch a custom event so the AuthContext can react without a hard reload.
      window.dispatchEvent(new Event('auth:logout'));
    }
    return Promise.reject(error);
  }
);

export default api;
