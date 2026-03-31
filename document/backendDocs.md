# CineSynk - Backend Technical Documentation

This document provide a deep dive into the backend architecture and features of the CineSynk Ticket Booking system.

## 🏗 Architecture Overview

The backend is built as a RESTful Web API using **ASP.NET Core (targeted at .NET 10)**. It follows a decoupled service-based architecture where business logic is separated from the API controllers.

### Core Tech Stack
- **Framework:** ASP.NET Core 10
- **Database:** MariaDB (via XAMPP)
- **ORM:** Entity Framework Core 9.0 (Stable version used for environment compatibility)
- **Database Provider:** Pomelo.EntityFrameworkCore.MySql
- **Security:** JWT (JSON Web Tokens) & BCrypt.Net-Next

---

## 🔐 Identity & Security

### Authentication
PulseQueue uses state-less **JWT Authentication**. 
- **Registration:** Users provide name, phone, age, gender, and password. Passwords are never stored in plain text; they are hashed using **BCrypt** (a modern, salted hashing algorithm).
- **Login:** On successful login, the server issues a JWT signed with a 256-bit secret key.
- **Claims:** The token contains the user's ID, Name, Role, and Phone Number, allowing the frontend to personalize the experience without extra API calls.

### Authorization
Endpoints like `Reserve` and `Confirm` are protected by the `[Authorize]` attribute, ensuring only authenticated users can modify the seat map.

---

## 🎟 Booking Engine Logic

The system uses a **two-step booking flow** to mimic real-world payment processing:

1.  **Reservation (Pending):** A user selects a seat. The system creates a `Booking` record with a `Pending` status. This "locks" the seat temporarily.
2.  **Confirmation (Confirmed):** Once payment is simulated, the status is updated to `Confirmed`.

### ⚡ Concurrency Management (The "Double-Booking" Problem)
To prevent two users from booking the exact same seat at the same time:
- We use **Optimistic Concurrency Control**.
- The `Bookings` table has a `RowVersion` (Timestamp) column.
- If two simultaneous requests try to modify the same seat's status, Entity Framework detects the version mismatch and throws a `DbUpdateConcurrencyException`. The API then gracefully informs the second user that the seat was "just taken."

---

## 🛠 "Under the Hood" Enterprise Features

### 1. Automatic Seat Cleanup (Background Service)
The `BookingCleanupService` is an `IHostedService` that runs in the background of the API.
- **Why:** To prevent "ghost" reservations from locking seats forever if a user abandons their checkout.
- **How:** Every 60 seconds, it scans the database for `Pending` bookings older than 10 minutes and automatically marks them as `Cancelled`.

### 2. Idempotency Middleware
To protect against duplicate bookings caused by network retries or double-clicking:
- Any booking request can include an **`X-Idempotency-Key`** header (a unique GUID).
- The `IdempotencyMiddleware` caches the response of successful requests for 1 hour.
- If the same key is sent again, the middleware intercepts the call and returns the *cached result* immediately without touching the database or booking logic again.

### 3. Global Exception Handling
A custom middleware wraps every request to ensure that even if an unhandled error occurs, the server responds with a clean, standardized JSON `ProblemDetails` response instead of a raw stack trace, improving both security and user experience.

---

## 🗄 Database Schema
Because the .NET 10 preview environment is currently unstable with auto-migration tools, we use **Manual Schema Management**.

### Key Entities
- **Users:** Stores profile data and secure password hashes.
- **Shows:** Cinema movie listings with start/end times.
- **Seats:** The physical grid of the theatre, mapped to specific Shows and Tiers (Balcony, Premium, etc.).
- **Bookings:** Tracks the relationship between a User, a Show, and a Seat.

---

## 🚀 API Endpoints Summary

| Method | Endpoint | Description | Auth Required |
| :--- | :--- | :--- | :--- |
| POST | `/api/auth/register` | Register a new user | No |
| POST | `/api/auth/login` | Login and receive JWT | No |
| GET | `/api/shows` | List all active shows | No |
| GET | `/api/shows/{id}/seats`| Get full theatre seat map | No |
| POST | `/api/bookings/reserve`| Temporary seat lock | **Yes** |
| POST | `/api/bookings/confirm`| Finalize booking (Payment) | **Yes** |
| GET | `/health` | Check API system health | No |
