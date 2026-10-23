import { defineConfig } from 'vite'
import react from '@vitejs/plugin-react'

// https://vite.dev/config/
export default defineConfig({
  plugins: [react()],
  server: {
    proxy: {
      // Proxy all backend routes to ASP.NET Core backend (http profile)
      '/products':        { target: 'http://localhost:5296', changeOrigin: true },
      '/cart':            { target: 'http://localhost:5296', changeOrigin: true },
      '/orders':          { target: 'http://localhost:5296', changeOrigin: true },
      '/checkout':        { target: 'http://localhost:5296', changeOrigin: true },
      '/delivery-options':{ target: 'http://localhost:5296', changeOrigin: true },
      '/payment-summary': { target: 'http://localhost:5296', changeOrigin: true },
      '/users':           { target: 'http://localhost:5296', changeOrigin: true },
      '/admin':           { target: 'http://localhost:5296', changeOrigin: true },
      // ASP.NET Identity API endpoints
      '/login':           { target: 'http://localhost:5296', changeOrigin: true },
      '/register':        { target: 'http://localhost:5296', changeOrigin: true },
      '/refresh':         { target: 'http://localhost:5296', changeOrigin: true },
      '/confirmEmail':    { target: 'http://localhost:5296', changeOrigin: true },
      '/resendConfirmationEmail': { target: 'http://localhost:5296', changeOrigin: true },
      '/forgotPassword':  { target: 'http://localhost:5296', changeOrigin: true },
      '/resetPassword':   { target: 'http://localhost:5296', changeOrigin: true },
      '/manage':          { target: 'http://localhost:5296', changeOrigin: true },
    }
  }
})