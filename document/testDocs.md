# CineSynk - Testing Documentation

This document provide a deep dive into the specialized testing ecosystem of CineSynk. We use a **tiered testing strategy** to ensure every layer of the application is verified, from raw logic to high-concurrency stress.

---

## 🧪 1. Backend Unit & Functional Tests
**What:** Testing individual classes and methods in isolation.
**Why:** To ensure that our core business logic—like password hashing or seat availability checks—works perfectly before it ever touches a database.
**How:**
- **Framework:** `xUnit`
- **Mocking:** `Moq`
- **Execution:** `dotnet test`
- **Location:** `backend/TicketBooking.Tests/`

---

## 🏗️ 2. Architectural Tests (NetArchTest)
**What:** Automated enforcement of code boundaries and dependencies.
**Why:** To prevent "Spaghetti Code." It ensures that developers don't accidentally make a Controller talk directly to the Database, bypassing the Service layer.
**How:**
- **Tool:** `NetArchTest.eNet`
- **Benefit:** Fails the build if dependency rules are broken.
- **Location:** `backend/TicketBooking.Tests/ArchitectureTests.cs`

---

## 🛰️ 3. Full Integration Tests
**What:** Testing the entire Backend chain from HTTP request to Database.
**Why:** To verify that different parts of the system work together correctly (e.g., Auth middlewear correctly passing user ID to the Booking service).
**How:**
- **Tool:** `WebApplicationFactory`
- **Database:** `InMemoryDatabase` for rapid execution.
- **Location:** `backend/TicketBooking.Tests/BookingIntegrationTests.cs`

---

## 🎭 4. End-to-End (E2E) Browser Tests
**What:** Mimicking a real user inside a browser (clicking, typing).
**Why:** To ensure the **User Experience** is intact. It catches bugs that unit tests miss, like buttons being hidden or JS errors in the browser.
**How:**
- **Tool:** `Playwright`
- **Framework:** `@playwright/test`
- **Execution:** `npx playwright test`
- **Location:** `frontend/tests/e2e/`

---

## 📉 5. Load & Stress Testing
**What:** Simulating hundreds of users hitting the system at once.
**Why:** Ticket websites are famous for crashing during major releases. We need to ensure our **Optimistic Concurrency** logic prevents "double-booking" under extreme load.
**How:**
- **Tool:** `k6`
- **Test Metric:** 100+ Virtual Users (VUs) with a <500ms response time requirement.
- **Location:** `tests/load/k6-load-test.js`

---

## 🔄 🔍 6. Regression Testing
**What:** Re-running all the above tests after every change.
**Why:** To ensure that fixing one bug didn't accidentally create another one.
**How:** Automated via **GitHub Actions** (`ci.yml`) on every push and pull request.

---

## ⚙️ 7. Graceful Shutdown Testing
**What:** Testing how the app behaves when it is told to stop (SIGTERM).
**Why:** If the system shuts down in the middle of a booking, we need to ensure the database doesn't get corrupted and resources are released properly.
**How:** Mocking `IHostApplicationLifetime` to monitor the cancellation token flow in backend tests.
