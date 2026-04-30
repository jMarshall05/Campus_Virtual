# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project Overview

Campus Virtual is an educational management platform for Colegio Santa Ana ("Santa Ana a Un Click"). It manages users, courses, groups, tasks, grades, documents, and authentication.

## Commands

### Backend (.NET 8)
```bash
# Run the API (from repo root)
dotnet run --project Api

# Build entire solution
dotnet build Campus_Virtual.sln

# Apply EF Core migrations
dotnet ef database update --project DA --startup-project Api

# Add a new migration
dotnet ef migrations add <MigrationName> --project DA --startup-project Api
```

Backend runs on `http://localhost:5099` (HTTP) or `https://localhost:7034` (HTTPS).  
API docs available at `http://localhost:5099/scalar/v1` (Scalar UI, dev only).

### Frontend (React + Vite)
```bash
# From UI/ directory
cd UI
npm install
npm run dev      # Dev server on http://localhost:5173
npm run build    # Production build to UI/dist
npm run lint     # ESLint
npm run preview  # Preview production build
```

The frontend base API URL is hardcoded in `UI/src/api/apiClient.js` (`http://localhost:5099/api`). Change it there when targeting a different environment.

## Architecture

### Solution Projects

| Project | Role |
|---------|------|
| `Api/` | ASP.NET Core Web API — controllers, middleware, DI wiring, file uploads |
| `Abstracciones/` | Shared contracts — service interfaces, DTOs (`Modelos/ModelosDto/`), request/response models (`Modelos/Requests/`, `Modelos/Responses/`), and `BusinessException` |
| `Reglas/` | Static validation classes that throw `BusinessException` on rule violations — called by services |
| `Servicios/` | Business logic services + Mapster mapping registrations (`Servicios/Mapster/`) + helpers |
| `DA/` | Data Access — EF Core `ApplicationDbContext`, entity models (`Entidades/`), repository interfaces (`Interfaces/`) and implementations (`Implementaciones/`) |
| `UI/` | React 19 + TypeScript frontend |

### Request Flow

```
Controller → Service → [Reglas validation] → Repository (DA) → SQL Server
```

- Controllers are thin; they delegate immediately to services via injected interfaces.
- Services call `Reglas` static methods to validate business rules before any write.
- Mapping between entities ↔ DTOs uses **Mapster**; mapping configs live in `Servicios/Mapster/` and implement `IRegister`. They are auto-discovered via `config.Scan(AppDomain.CurrentDomain.GetAssemblies())` in `Program.cs`.
- `BusinessException` is the only domain error type. `Program.cs` catches it globally and returns HTTP 400 `{ mensaje: "..." }`.

### Database

- **SQL Server**, database `Proyecto_SantaAna`, connection string in `Api/appsettings.json` under `"BD"`.
- `ApplicationDbContext` extends `IdentityDbContext`. ASP.NET Identity tables are renamed with `_Core` suffix (e.g., `AspNetUsers_Core`).
- Application entities follow the `*AD` naming convention (e.g., `UsuariosAD`, `GruposAD`).

### Authentication

- **JWT Bearer** tokens, 45-minute expiry, `ClockSkew = Zero`.
- **Roles:** `Administradores`, `Profesores`, `Estudiantes` — applied via `[Authorize(Roles = "...")]`.
- **2FA:** TOTP (Google Authenticator compatible); enabling/disabling issues a new JWT.
- Frontend stores the JWT in `localStorage`. Token is read/decoded via helpers in `UI/src/utils/auth.js`. Role is extracted from the claim `http://schemas.microsoft.com/ws/2008/06/identity/claims/role`.
- `ProtectedRoute` in `UI/src/components/auth/ProtectedRoute.jsx` enforces role-based access on the client.

### Frontend Structure

```
UI/src/
├── api/           # One file per domain (authService, userService, groupService, …)
│                  # All calls go through apiFetchJson / apiFetchBlob in apiClient.js
├── pages/         # Route-level components grouped by domain (auth/, users/, groups/, courses/, docs/)
├── components/    # Reusable pieces (auth/ProtectedRoute, users/UserDetails, …)
├── layouts/       # Shell with sidebar/navbar (adminLayout.jsx wraps protected routes)
├── utils/         # auth.js — token helpers (GuardarToken, validarToken, getUserRole, …)
└── App.jsx        # Route definitions (BrowserRouter + React Router v7)
```

Adding a new page: create the component under `pages/<domain>/`, add a service file under `api/`, wire the route in `App.jsx`, and wrap it with `<ProtectedRoute requiredRole="...">` inside the `<Layout />` route.

### File Storage

Uploaded files are stored under `Api/Uploads/Docs/`. The `IFileStorageService` / `FileStorageService` handle disk I/O; the root path is injected from `IWebHostEnvironment.ContentRootPath`.

## Key Conventions

- **New backend feature:** add interface to `Abstracciones/Servicios/`, implement in `Servicios/Servicios/`, add repository interface to `DA/Interfaces/` + implementation in `DA/Implementaciones/`, register both in `Program.cs`, add DTOs/Requests to `Abstracciones/Modelos/`.
- **Validation errors** must use `BusinessException` (never throw raw exceptions from services/rules). The global handler converts them to 400 responses.
- **Mapster configs** for a new entity go in a new `*Map.cs` file inside `Servicios/Mapster/` implementing `IRegister`; they are picked up automatically.
- Frontend API calls use `apiFetchJson` for JSON endpoints and `apiFetchBlob` for binary downloads — no raw `fetch` calls in components.
