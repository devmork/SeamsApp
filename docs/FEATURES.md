# Features

This document inventories the real domain modules and the service/controller patterns used in the codebase. Do not assume endpoints that are not listed here.

## 1. Service pattern

- Interface under `Interfaces/Services/Commands/I*Service.cs`
- Implementation under `Services/Commands/*Service.cs`
- Registered automatically by Scrutor (class name ends with `Service`)
- Controllers depend on the interface only

Typical feature skeleton for a domain:

```
DTOs/<Domain>/          # *Request, *Response
Interfaces/.../I*Service.cs
Services/Commands/*Service.cs
Controllers/*Controller.cs
Models/ (or Models/Base/)
```

## 2. Auth

| Item | Location |
| ---- | -------- |
| Service | `AuthService` / `IAuthService`, `JwtService` / `IJwtService` |
| Controller | `AuthController` → `api/auth` |

| Method | Route | Auth | Notes |
| ------ | ----- | ---- | ----- |
| Login | `POST /api/auth/login` | Anonymous | Returns JWT + user info |

## 3. Students

| Item | Location |
| ---- | -------- |
| Service | `StudentService` / `IStudentService` |
| Controller | `StudentController` → `api/student` |

| Method | Route | Roles | Notes |
| ------ | ----- | ----- | ----- |
| List active | `GET /api/student` | Admin, Officer | Cached 30s; `Status == 1` |
| Get by id | `GET /api/student/{studentId}` | Admin, Officer, Student | |
| QR lookup | `GET /api/student/qr/{schoolStudentId}` | Admin, Officer | |
| Soft delete | `PATCH /api/student/{studentId}` | Admin | |
| Update | `PUT /api/student/{studentId}` | Admin | Implementation currently incomplete in service |
| My profile | `GET /api/student/me` | Student | Uses claims user id |

Student entities store personal + school fields, optional `QRCode` bytes, and link to `User` for email.

## 4. Officers

| Item | Location |
| ---- | -------- |
| Service | `OfficerService` / `IOfficerService` |
| Controller | `OfficerController` → `api/officer` |

| Method | Route | Roles | Notes |
| ------ | ----- | ----- | ----- |
| Create | `POST /api/officer` | Admin | Body: `{ userId }`; sets role to Officer |
| Remove | `PATCH /api/officer/{userId}` | Admin | Sets role back to Student |
| List | `GET /api/officer` | Admin | Active officers with student profile fields when present |

## 5. Events

| Item | Location |
| ---- | -------- |
| Service | `EventService` / `IEventService` |
| Controller | `EventController` → `api/event` |

| Method | Route | Roles | Notes |
| ------ | ----- | ----- | ----- |
| List | `GET /api/event` | Admin, Officer, Student | Output cached |
| Get by id | `GET /api/event/{eventId}` | Admin, Officer | |
| Create | `POST /api/event` | Admin | |
| Update | (see controller) | Admin | |
| Delete | (see controller) | Admin | Soft status |

Request shape: `Title`, `StartDate`, `EndDate`.

## 6. Attendance sessions

| Item | Location |
| ---- | -------- |
| Service | `AttendanceService` / `IAttendanceService` |
| Controller | `AttendanceController` |

Sessions belong to an event (`EventId`). Fields include `Title`, `Date`, `Session`, `LogType`, `StartTime`, `EndTime`, `Status`, `CreatedBy`.

Create is tied to an event id; list/filter by event; soft-delete sets `Status = 0`.

## 7. Attendance records (logging)

| Item | Location |
| ---- | -------- |
| Service | `AttendanceRecordService` / `IAttendanceRecordService` |
| Controller | `AttendanceRecordController` → `api/attendance-record` |

| Method | Route | Roles | Notes |
| ------ | ----- | ----- | ----- |
| List all | `GET /api/attendance-record` | Admin, Officer | |
| Get by id | `GET /api/attendance-record/{recordId}` | Admin, Officer, Student | |
| By session | `GET /api/attendance-record/attendance/{attendanceId}` | Admin, Officer | |
| Create (log) | `POST /api/attendance-record` | Admin, Officer | Rejects duplicate student+session |
| Update | `PUT /api/attendance-record/{recordId}` | Admin, Officer | |
| Delete | `DELETE /api/attendance-record/{recordId}` | Admin | Hard delete |
| My history | `GET /api/attendance-record/me` | Student | Grouped by event/session |

Create request: `AttendanceID`, `SchoolStudentID`, optional `Status` (default 1).

History response groups sessions under events and marks present / absent / upcoming based on existing records and current time.

## 8. Student applications

| Item | Location |
| ---- | -------- |
| Service | `StudentApplicationService` / `IStudentApplicationService` |
| Controller | `StudentApplicationController` |

| Method | Notes |
| ------ | ----- |
| Create application | Accepts personal + school fields (`CreateStudentApplicationRequest`) |
| Approve | Creates `User` + `Student` (with QR), sets application status to approved (`2`) |
| Reject | Sets status to rejected (`3`) |
| List all / pending / approved / rejected | Filtered by status |

Pending applications use status `1` in the pending query.

## 9. Users

`UserController` and `UserService` exist but most actions are commented out or throw `NotImplementedException`. Prefer domain-specific endpoints above.

## 10. Adding a new feature (checklist)

When introducing a new domain:

1. Add entity (and migration if needed).
2. Add DTOs under `DTOs/<Domain>/`.
3. Add `I*Service` + `*Service` (name must end with `Service` for Scrutor).
4. Add controller with explicit routes and `[Authorize(Roles = ...)]`.
5. Register AutoMapper maps in `AutoMapperProfiles` if needed.
6. Update this FEATURES inventory and any related architecture/auth notes.
7. Do not introduce a separate repository layer unless the existing pattern changes in code first.
