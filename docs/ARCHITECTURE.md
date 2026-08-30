# Architecture

## 1. High-level layout

```
SeamsApp/
├── Controllers/          # HTTP endpoints (thin)
├── Data/
│   └── SeamsDbContext.cs
├── DTOs/                 # Request/response contracts per domain
├── Interfaces/
│   └── Services/Commands/  # Service interfaces
├── Models/               # EF entities (+ Models/Base for some)
├── Migrations/
├── Seeders/
│   └── AdminSeeder.cs
├── Services/
│   └── Commands/         # Service implementations
├── Utilities/            # Claims, AutoMapper profile, QR helpers
├── Program.cs
├── appsettings*.json
└── SeamsApp.csproj
```

There is no separate repository layer. Controllers depend on service interfaces; services use `SeamsDbContext` (and sometimes AutoMapper / `IHttpContextAccessor`) directly.

## 2. Request flow

1. Client → JWT Bearer (except login)
2. Controller action (role attributes + model binding)
3. Service method (business logic + EF)
4. Optional AutoMapper mapping to/from DTOs
5. JSON response (camelCase via `JsonNamingPolicy.CamelCase`)

## 3. DbContext and entities

`SeamsDbContext` exposes:

| DbSet | Entity |
| ----- | ------ |
| `Admins` | `Admin` |
| `Attendances` | `Attendance` (`Models.Base`) |
| `AttendanceRecords` | `AttendanceRecord` |
| `Events` | `Event` |
| `Officers` | `Officer` |
| `Students` | `Student` (`Models.Base`) |
| `StudentApplications` | `StudentApplication` |
| `Users` | `User` |

Relationships (from model snapshot / navigation properties):

- `User` 1–1 `Admin` / `Officer` / `Student` (FK on the role tables)
- `Event` 1–many `Attendance`
- `Attendance` 1–many `AttendanceRecord`

`AttendanceRecord` joins to students by `SchoolStudentID` string (no formal FK to `Student`).

## 4. Dependency injection (`Program.cs`)

- Controllers + JSON camelCase
- `IPasswordHasher<User>` → `PasswordHasher<User>`
- Scrutor scans the assembly and registers every class whose name ends with `Service` as its implemented interfaces (scoped)
- Output caching (default 15s)
- CORS policy `FrontendPolicy`
- JWT Bearer authentication
- Swagger + Bearer security definition
- AutoMapper with `AutoMapperProfiles`
- `SeamsDbContext` → SQL Server
- `IHttpContextAccessor`
- Admin seeding on startup inside a scope

## 5. Controllers and routes

| Controller | Route prefix | Notes |
| ---------- | ------------ | ----- |
| `AuthController` | `api/auth` | Login is `[AllowAnonymous]` |
| `StudentController` | `api/student` | Profile, list, QR lookup |
| `OfficerController` | `api/officer` | Promote / demote officers |
| `EventController` | `api/event` | Event CRUD |
| `AttendanceController` | (see controller) | Attendance sessions under events |
| `AttendanceRecordController` | `api/attendance-record` | Logging + student history |
| `StudentApplicationController` | (see controller) | Apply / approve / reject |
| `UserController` | `api/[controller]` | Mostly stubs |

Exact route templates and role requirements are documented in [FEATURES.md](FEATURES.md).

## 6. Soft status flags

Several entities use integer `Status` (and related fields) instead of hard deletes:

- Students, Officers, Events, Attendances, Applications, AttendanceRecords

Convention values observed in services:

| Context | Values used in code |
| ------- | ------------------- |
| Student / Officer / Attendance | `1` = active, `0` = inactive/deleted |
| StudentApplication | `1` = pending, `2` = approved, `3` = rejected |
| Attendance session mark (history) | `0` = absent, `1` = present, `2` = upcoming |

## 7. Cross-cutting utilities

- `ClaimsUtility` — read `NameIdentifier`, roles, email from `HttpContext`
- `AutoMapperProfiles` — maps between entities and DTOs
- `QRCodeUtility` / QRCoder usage — generate QR content from student identity
- `AdminSeeder` — bootstrap first Admin

## 8. Output caching

Selected GET endpoints use `[OutputCache]` / `[OutputCache(Duration = 30)]`. Global default expiration is 15 seconds.
