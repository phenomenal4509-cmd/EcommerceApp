import { createContext, useContext, useState, useEffect, useCallback } from 'react';
import api from '../api/axiosInstance';

const AuthContext = createContext(null);

export function AuthProvider({ children }) {
  // ── Initialise from localStorage (survive page refresh) ──────────────────
  const [accessToken, setAccessToken] = useState(() => localStorage.getItem('accessToken'));
  const [user, setUser] = useState(null);
  const [authLoading, setAuthLoading] = useState(!!localStorage.getItem('accessToken'));

  // ── Logout (defined before fetchCurrentUser so it can be referenced) ──────
  const logout = useCallback(() => {
    localStorage.removeItem('accessToken');
    localStorage.removeItem('tokenExpiry');
    setAccessToken(null);
    setUser(null);
  }, []);

  // ── Load /users/me whenever a token is available ──────────────────────────
  const fetchCurrentUser = useCallback(async () => {
    try {
      const res = await api.get('/users/me');
      setUser(res.data);
    } catch {
      // Token may be invalid/expired – clear everything
      logout();
    } finally {
      setAuthLoading(false);
    }
  }, [logout]);

  useEffect(() => {
    if (accessToken) {
      fetchCurrentUser();
    } else {
      setAuthLoading(false);
    }
  }, [accessToken, fetchCurrentUser]);

  // ── Listen for the 401 event fired by the Axios interceptor ──────────────
  useEffect(() => {
    const handle = () => logout();
    window.addEventListener('auth:logout', handle);
    return () => window.removeEventListener('auth:logout', handle);
  }, [logout]);

  // ── Login ─────────────────────────────────────────────────────────────────
  // ASP.NET Identity API endpoint:  POST /login  { email, password }
  // Response:  { tokenType, accessToken, expiresIn, refreshToken }
  const login = async (email, password) => {
    const res = await api.post('/login', { email, password });
    const token = res.data.accessToken;
    localStorage.setItem('accessToken', token);
    setAccessToken(token);
    // /users/me will be called by the useEffect above
  };

  // ── Register ──────────────────────────────────────────────────────────────
  // ASP.NET Identity API endpoint:  POST /register  { email, password }
  // The backend also requires email confirmation; we tell the caller so it can
  // show the appropriate message.
  const register = async (email, password) => {
    await api.post('/register', { email, password });
    // Registration triggers a confirmation email; the user cannot log in yet.
  };

  const isAuthenticated = !!accessToken;

  return (
    <AuthContext.Provider value={{ isAuthenticated, user, authLoading, login, logout, register }}>
      {children}
    </AuthContext.Provider>
  );
}

// eslint-disable-next-line react-refresh/only-export-components
export function useAuth() {
  return useContext(AuthContext);
}
