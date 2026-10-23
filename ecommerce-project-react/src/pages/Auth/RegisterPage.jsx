import { useState } from 'react';
import { Link } from 'react-router';
import { useAuth } from '../../context/AuthContext';
import './AuthPages.css';

export function RegisterPage() {
  const { register } = useAuth();

  const [email, setEmail] = useState('');
  const [password, setPassword] = useState('');
  const [confirmPassword, setConfirmPassword] = useState('');
  const [errors, setErrors] = useState([]);
  const [success, setSuccess] = useState(false);
  const [loading, setLoading] = useState(false);

  const handleSubmit = async (e) => {
    e.preventDefault();
    setErrors([]);

    if (password !== confirmPassword) {
      setErrors(['Passwords do not match.']);
      return;
    }

    setLoading(true);
    try {
      await register(email, password);
      setSuccess(true);
    } catch (err) {
      // ASP.NET Identity returns errors as an object of arrays, e.g.
      // { "errors": { "DuplicateUserName": ["Email already taken."] } }
      // or as ProblemDetails { detail: "..." }
      const data = err.response?.data;
      const collected = [];

      if (data?.errors) {
        for (const key of Object.keys(data.errors)) {
          for (const msg of data.errors[key]) {
            collected.push(msg);
          }
        }
      } else if (data?.detail) {
        collected.push(data.detail);
      } else if (data?.title) {
        collected.push(data.title);
      } else {
        collected.push('Registration failed. Please try again.');
      }

      setErrors(collected);
    } finally {
      setLoading(false);
    }
  };

  if (success) {
    return (
      <div className="auth-page">
        <div className="auth-card">
          <div className="auth-logo">
            <Link to="/" className="auth-logo-link">
              Syed<span className="logo-accent">'s</span> Store
            </Link>
          </div>
          <div className="auth-success">
            <div className="auth-success-icon">✓</div>
            <h2 className="auth-success-title">Account created!</h2>
            <p className="auth-success-message">
              We&apos;ve sent a confirmation email to <strong>{email}</strong>.
              Please check your inbox and click the confirmation link before signing in.
            </p>
            <Link to="/login" className="auth-submit button-primary" style={{ display: 'block', textAlign: 'center', textDecoration: 'none', marginTop: '20px' }}>
              Go to Sign in
            </Link>
          </div>
        </div>
      </div>
    );
  }

  return (
    <div className="auth-page">
      <div className="auth-card">
        <div className="auth-logo">
          <Link to="/" className="auth-logo-link">
            Syed<span className="logo-accent">'s</span> Store
          </Link>
        </div>

        <h1 className="auth-title">Create account</h1>

        {errors.length > 0 && (
          <ul className="auth-error-list">
            {errors.map((e, i) => <li key={i}>{e}</li>)}
          </ul>
        )}

        <form onSubmit={handleSubmit} className="auth-form">
          <div className="auth-field">
            <label htmlFor="reg-email" className="auth-label">Email address</label>
            <input
              id="reg-email"
              type="email"
              className="auth-input"
              value={email}
              onChange={(e) => setEmail(e.target.value)}
              required
              autoFocus
              placeholder="you@example.com"
            />
          </div>

          <div className="auth-field">
            <label htmlFor="reg-password" className="auth-label">Password</label>
            <input
              id="reg-password"
              type="password"
              className="auth-input"
              value={password}
              onChange={(e) => setPassword(e.target.value)}
              required
              placeholder="Min. 8 characters"
            />
          </div>

          <div className="auth-field">
            <label htmlFor="reg-confirm" className="auth-label">Confirm password</label>
            <input
              id="reg-confirm"
              type="password"
              className="auth-input"
              value={confirmPassword}
              onChange={(e) => setConfirmPassword(e.target.value)}
              required
              placeholder="••••••••"
            />
          </div>

          <button
            id="register-submit"
            type="submit"
            className="auth-submit button-primary"
            disabled={loading}
          >
            {loading ? 'Creating account…' : 'Create account'}
          </button>
        </form>

        <p className="auth-switch">
          Already have an account?{' '}
          <Link to="/login" className="link-primary">Sign in</Link>
        </p>
      </div>
    </div>
  );
}
