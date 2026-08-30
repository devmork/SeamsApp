# Conventions

## 1. Naming

| Kind | Convention | Example |
| ---- | ---------- | ------- |
| Controllers | `*Controller`, route often `api/<kebab-or-singular>` | `StudentController` → `api/student` |
| Services | Interface `I*Service`, class `*Service` | `IEventService` / `EventService` |
| DTOs | `*Request` / `*Response` under `DTOs/<Domain>/` | `EventRequest`, `AttendanceRecordResponse` |
| Entities | Singular PascalCase | `Student`, `AttendanceRecord` |
| Primary keys | `*Id` or domain-specific (`RecordID`, `ApplicationId`) | |

Scrutor registration depends on the class name ending with `Service`.

## 2. JSON

- Property names serialized as camelCase (`Program.cs` Json options).
- Prefer nullable reference types where the model allows.

## 3. Status / soft delete

Prefer status flags over physical deletes for domain entities:

- Active / visible: `Status == 1`
- Soft-deleted or inactive: `Status == 0` (attendance, students, officers, events)
- Applications: `1` pending, `2` approved, `3` rejected

Attendance record “history” status codes used for UI marking: `0` absent, `1` present, `2` still open.

## 4. Auth attributes

- Class-level `[Authorize]` is common; open specific actions with `[AllowAnonymous]`.
- Restrict writes with `[Authorize(Roles = "Admin")]` or `"Admin, Officer"`.
- Student-only endpoints use `Roles = "Student"` and resolve identity via `ClaimsUtility.GetUserIdFromClaims`.

## 5. Mapping

AutoMapper profile: `Utilities/AutoMapperUtility.cs` → class `AutoMapperProfiles`.

Maps exist for Student, User, Attendance, StudentApplication, Event, Officer. Some list methods project manually with `Select` instead of relying only on AutoMapper.

## 6. Claims and current user

Always use `ClaimsUtility` rather than parsing claims ad hoc. Services that need the acting user inject `IHttpContextAccessor`.

## 7. Passwords

Use `IPasswordHasher<User>` (ASP.NET Identity hasher) for hash and verify. Never store plain passwords.

## 8. QR codes

- Generation helpers: `QRCodeUtility` (SkiaSharp) and QRCoder usage inside student flows.
- Stored as `byte[]` on `Student.QRCode` in some paths; content typically includes name + school student id.

## 9. Git / secrets

- Do not commit real connection strings or JWT keys.
- Production settings belong in environment configuration or secure secret stores, not in the repository docs.

## 10. Documentation discipline

When code changes:

| Change | Update |
| ------ | ------ |
| New/changed domain under services/controllers | FEATURES.md |
| Structure, DI, DbContext relationships | ARCHITECTURE.md |
| Login, roles, claims | AUTHENTICATION.md |
| Setup, env keys, run scripts | GETTING-STARTED.md |
| Naming / status conventions | CONVENTIONS.md |

Keep docs grounded in the source that exists. Prefer small section edits over full rewrites.
