# Ecommerce App

A full-stack ecommerce application with an **ASP.NET Core 10 minimal API** backend and a **React 19 + Vite** frontend. It supports product browsing, carts, checkout, orders, shipment tracking, and account management with role/claim-based authorization.

## Features

- Product catalog with filtering, sorting, pagination, and search
- Shopping cart and checkout flow with delivery options and payment summary
- Order creation, order history, and shipment tracking
- Authentication and authorization via ASP.NET Core Identity (bearer tokens)
- Role and claim management through admin endpoints
- Email confirmation and password reset via SMTP (MailKit)
- Automatic database migrations and seed data (products, delivery options, `Admin` role)

## Tech Stack

| Layer | Technologies |
| --- | --- |
| Backend | .NET 10, ASP.NET Core Minimal APIs, Entity Framework Core 10, Npgsql (PostgreSQL), ASP.NET Core Identity, MailKit |
| Frontend | React 19, Vite 6, React Router 7, Axios, Vitest, ESLint |
| Database | PostgreSQL (uses the `pg_trgm` extension for search indexes) |

## Repository Structure

```
.
├── MyApp.Api/                  # ASP.NET Core backend
│   ├── Data/                   # DbContext, Identity user, seeding, migrations helper
│   ├── Dtos/                   # Request/response contracts
│   ├── Endpoints/              # Minimal API endpoint groups
│   ├── Migrations/             # EF Core migrations
│   ├── Models/                 # Domain entities
│   └── Services/               # Email sender, cart pricing
├── ecommerce-project-react/    # React frontend (Vite)
│   ├── public/images/          # Product and UI images
│   └── src/                    # Pages, components, context, API client
└── .gitignore
```

## Prerequisites

- [.NET SDK 10](https://dotnet.microsoft.com/download)
- [Node.js 18+](https://nodejs.org/) with npm
- [PostgreSQL 14+](https://www.postgresql.org/download/) running locally
- Git

## Backend Setup

### 1. Clone the repository

```bash
git clone https://github.com/phenomenal4509-cmd/EcommerceApp.git
cd EcommerceApp
```

### 2. Create the database

```bash
createdb -U postgres EcommerceDb
```

Or create a database named `EcommerceDb` with pgAdmin/your preferred client. The app applies migrations and seed data automatically on startup.

> The migrations create the `pg_trgm` extension. If your database user lacks permission, run
> `CREATE EXTENSION IF NOT EXISTS pg_trgm;` as a superuser first.

### 3. Configure the connection string

`MyApp.Api/appsettings.json` ships with placeholders and **must not** contain real credentials. Store secrets with .NET User Secrets:

```bash
cd MyApp.Api
dotnet user-secrets set "ConnectionStrings:Ecommerce" "Host=localhost;Port=5432;Database=EcommerceDb;Username=postgres;Password=your_password"
```

Alternatively, set `ConnectionStrings:Ecommerce` in `appsettings.Development.json` (never commit real credentials).

### 4. Configure email (optional)

Email is used for account confirmation and password reset. Gmail requires an [App Password](https://myaccount.google.com/apppasswords) (2-Step Verification must be enabled).

```bash
cd MyApp.Api
dotnet user-secrets set "Email:Username" "your-email@gmail.com"
dotnet user-secrets set "Email:Password" "your-16-character-app-password"
```

The SMTP client connects to `smtp.gmail.com:587` using STARTTLS. Email confirmation is **not** enforced in Development; it is required in other environments.

### 5. Restore and run

```bash
cd MyApp.Api
dotnet restore
dotnet run --launch-profile http
```

The API is available at **http://localhost:5296**. On startup it will:

- Apply all pending EF Core migrations
- Seed products and delivery options (only when missing)
- Create the `Admin` role if it does not exist

For HTTPS, use the `https` profile (https://localhost:7260 and http://localhost:5296).

### 6. Working with EF Core migrations

Only needed after changing models or the `DbContext`:

```bash
dotnet tool install --global dotnet-ef
dotnet ef migrations add <MigrationName> --project MyApp.Api
dotnet ef database update --project MyApp.Api
```

## Frontend Setup

```bash
cd ecommerce-project-react
npm install
npm run dev
```

The app runs at **http://localhost:5173**. Vite proxies API routes (`/products`, `/cart`, `/orders`, `/login`, etc.) to `http://localhost:5296`, so **start the backend first**.

### Build for production

```bash
npm run build
npm run preview
```

### Run tests and lint

```bash
npx vitest run
npm run lint
```

## Configuration Reference

| Key | Where to set | Description |
| --- | --- | --- |
| `ConnectionStrings:Ecommerce` | User Secrets / `appsettings.Development.json` | PostgreSQL connection string |
| `Email:Username` | User Secrets | SMTP account (sender address) |
| `Email:Password` | User Secrets | SMTP app password |
| `ASPNETCORE_ENVIRONMENT` | Launch profile / environment | `Development` disables required email confirmation |

## API Overview

| Group | Base route |
| --- | --- |
| Products | `/products` |
| Cart | `/cart` |
| Orders | `/orders` |
| Checkout | `/checkout` |
| Delivery options | `/delivery-options` |
| Payment summary | `/payment-summary` |
| Users | `/users` |
| Admin (roles/claims) | `/admin` |
| Identity | `/register`, `/login`, `/refresh`, `/confirmEmail`, `/resendConfirmationEmail`, `/forgotPassword`, `/resetPassword`, `/manage` |

## Authentication

- ASP.NET Core Identity issues bearer tokens (valid for **2 hours**).
- Send the token on protected requests: `Authorization: Bearer <accessToken>`.
- The frontend stores the token in `localStorage`; an Axios request interceptor attaches it and a response interceptor clears it on `401`.
- Authorization policies:
  - `PremiumUser` — requires the `AccountType=Premium` claim
  - `PremiumAdmin` — requires the `AccountType=Premium` claim and the `Admin` role
- The `Admin` role is created automatically, but users must be assigned to it via the admin endpoints.

## Troubleshooting

| Problem | Fix |
| --- | --- |
| `Npgsql` connection refused | Ensure PostgreSQL is running and the host/port/credentials in the connection string are correct |
| `pg_trgm` extension error | Run `CREATE EXTENSION IF NOT EXISTS pg_trgm;` as a PostgreSQL superuser |
| CORS error from the frontend | Use the frontend at `http://localhost:5173` or `http://127.0.0.1:5173` (allowed origins), or update CORS in `Program.cs` |
| `Email service is not configured` | Set `Email:Username` and `Email:Password` in User Secrets |
| Requests return `401` | The token expired or is missing; sign in again |
| Changes not in the database | Restart the API so migrations apply, or run `dotnet ef database update` |
