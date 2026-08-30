# Authentication

## 1. Overview

- Scheme: JWT Bearer (`Microsoft.AspNetCore.Authentication.JwtBearer`)
- Login endpoint is anonymous; almost all other endpoints require `[Authorize]`
- Role-based authorization uses `[Authorize(Roles = "...")]` with roles stored on `User.Role`

## 2. Login flow

**Endpoint:** `POST /api/auth/login`  
**Body:** `LoginRequest` (`Email`, `Password`)  
**Handler:** `AuthController` → `IAuthService.LoginAsync`

Steps in `AuthService.LoginAsync`:

1. Load `User` by email (or throw `UnauthorizedAccessException`).
2. Verify password with `IPasswordHasher<User>.VerifyHashedPassword`.
3. Generate JWT via `IJwtService.GenerateToken`.
4. Return `LoginResponse`:

| Field | Source |
| ----- | ------ |
| `Token` | JWT string |
| `UserId` | `user.UserId` |
| `Email` | `user.Email` |
| `Role` | `user.Role` |

Failed password or missing user → `401 Unauthorized` with `{ Error: "..." }`.

## 3. Token contents (`JwtService`)

Claims written into the token:

| Claim type | Value |
| ---------- | ----- |
| `ClaimTypes.NameIdentifier` | `UserId` (string) |
| `ClaimTypes.Email` | Email |
| `ClaimTypes.Role` | Role (if present) |

Token parameters:

- Issuer / Audience / Key from configuration (`Jwt:*`)
- Algorithm: HMAC-SHA256
- Lifetime: 30 minutes from issuance

## 4. Validation (`Program.cs`)

```text
ValidateIssuer = true
ValidateAudience = true
ValidateLifetime = true
ValidateIssuerSigningKey = true
```

## 5. Reading claims in code

Use `ClaimsUtility` (static helpers):

| Method | Behavior |
| ------ | -------- |
| `GetUserIdFromClaims(HttpContext)` | Parses `NameIdentifier`; throws if missing/invalid |
| `GetUserEmailFromClaims(HttpContext)` | Email claim or null |
| `GetUserRolesFromClaims(HttpContext)` | List of role claim values |
| `HasRole(HttpContext, role)` | `User.IsInRole(role)` |

Services that need the current user (e.g. setting `CreatedBy`) inject `IHttpContextAccessor` and call these helpers.

## 6. Roles in practice

| Role string | Typical authorization |
| ----------- | --------------------- |
| `Admin` | Create/update/delete events, students, officers; approve applications; delete attendance records |
| `Officer` | Read students/events; create/update attendance records; manage attendance sessions |
| `Student` | Own profile (`/api/student/me`), own attendance history (`/api/attendance-record/me`) |

Role is a plain string column on `User`. Promoting a user to Officer updates `User.Role` to `"Officer"` and inserts an `Officer` row; removal sets role back to `"Student"` and removes the officer row.

## 7. Seeding

`AdminSeeder.SeedAdmin` runs once at startup if no user with `Role == "Admin"` exists. It creates a `User` + linked `Admin` record with a hashed password.

Change the seeded password before deploying anywhere public.

## 8. Swagger

Swagger is configured with a Bearer security definition. Paste the token from login into the Authorize dialog to call protected endpoints from the UI.
