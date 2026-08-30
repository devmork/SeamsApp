# Getting started

## 1. Prerequisites

- .NET 8 SDK
- SQL Server (local or remote) reachable with a connection string
- Optional: EF Core tools (`dotnet tool install --global dotnet-ef`)

## 2. Clone and restore

```bash
dotnet restore
```

## 3. Configuration

Connection string and JWT settings are read from configuration (`appsettings.json` / environment-specific files / user secrets).

**Required keys:**

| Key | Purpose |
| --- | ------- |
| `ConnectionStrings:DefaultConnection` | SQL Server connection for `SeamsDbContext` |
| `Jwt:Issuer` | Token issuer |
| `Jwt:Audience` | Token audience |
| `Jwt:Key` | Symmetric signing key (long random string) |

Development secrets can be stored with:

```bash
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Server=...;Database=...;..."
dotnet user-secrets set "Jwt:Issuer" "https://localhost"
dotnet user-secrets set "Jwt:Audience" "https://localhost"
dotnet user-secrets set "Jwt:Key" "<long-random-key>"
```

Do **not** commit real production connection strings or JWT keys into source control.

## 4. Database

EF Core migrations live under `Migrations/`. Apply them:

```bash
dotnet ef database update
```

On first run, `AdminSeeder.SeedAdmin` (called from `Program.cs`) creates an Admin user if none exists:

- Email: `admin@dmc.edu.ph`
- Role: `Admin`
- Password: set in seeder code (change for any non-local environment)

## 5. Run

```bash
dotnet run
```

Launch profiles (`Properties/launchSettings.json`):

| Profile | URLs |
| ------- | ---- |
| `http` | `http://localhost:5035` |
| `https` | `https://localhost:7122` and `http://localhost:5035` |

Swagger UI is enabled and available at `/swagger`.

## 6. CORS

The policy `FrontendPolicy` allows:

- `https://seams-web.vercel.app`
- `http://localhost:5173`

## 7. Verify

1. Open Swagger.
2. Call `POST /api/auth/login` with the seeded admin credentials.
3. Authorize subsequent requests with the returned JWT (`Bearer <token>`).

## 8. Project file notes

- Target framework: `net8.0`
- Notable packages: EF Core SQL Server, JWT Bearer, AutoMapper, Scrutor, QRCoder, SkiaSharp.QrCode, Swashbuckle
- XML documentation is generated for Swagger comments.
