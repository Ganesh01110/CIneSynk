# CineSynk: Enterprise Test Strategy

CineSynk adopts a **Defense-in-Depth** testing strategy. We believe that quality is not a phase, but a continuous gate. Our "Shift-Left" approach ensures that security and logic are verified at every stage of the lifecycle.

---

## 🏛️ 1. The Testing Pyramid

Our strategy is built on a pyramid of automated checks, ensuring stability from the base code to the user interface.

| Layer | Tooling | Focus | Frequency |
| :--- | :--- | :--- | :--- |
| **SecOps** | Snyk & OSV | Vulnerability & Dependency Scanning | Every Push |
| **Architecture** | NetArchTest | Decoupling & Layering Enforcement | Every Push |
| **Unit** | xUnit & Moq | Business Logic & Method-level Isolation | Every Push |
| **Integration** | WebAppFactory | Multi-component API Chain verification | Every Push |
| **E2E** | Playwright | Full Browser User-Journey validation | Pre-Deployment |
| **Load** | k6 | High-Concurrency & Stress Resiliency | Periodic / Manual |
| **Observability** | Prometheus/Grafana | Real-time Health & Performance Monitoring | Continuous |

---

## 🛡️ Layer 1: Security Scanning (SecOps)
**What:** Automated auditing of third-party libraries.
**Why:** To prevent vulnerabilities like SQL injection or compromised dependencies from reaching production.
**How:** Integrated into CI/CD using **Snyk CLI**. It blocks the build if "High" severity issues are detected.

---

## 🏗️ Layer 2: Architectural Enforcement
**What:** Verification of the "Clean Architecture" boundaries.
**Why:** Maintains long-term maintainability. It prevents the Presentation layer from directly accessing Infrastructure, ensuring strict decoupling.
**How:** Automated via `NetArchTest.eNet`.

---

## 🧪 Layer 3: Unit & Functional Tests
**What:** Granular testing of services (Auth, Booking, Shows).
**Why:** Provides near-instant feedback to developers on core logic changes.
**How:** xUnit with Moq for database abstraction.

---

## 🛰️ Layer 4: API Integration Tests
**What:** Verifying HTTP requests against a real (In-Memory) application host.
**Why:** Ensures that middlewares, filters, and services coordinate correctly.
**How:** `Microsoft.AspNetCore.Mvc.Testing`.

---

## 🎭 Layer 5: End-to-End (E2E) Browser Tests
**What:** Simulating a real user journey in Chromium/Firefox/WebKit.
**Why:** Catches visual bugs, broken links, and JavaScript runtime errors that unit tests cannot detect.
**How:** Playwright.

---

## 📉 Layer 6: Load & Stress Testing
**What:** High-pressure simulation of peak traffic.
**Why:** Ensures the **Optimistic Concurrency** mechanism and MariaDB locking handle 100+ simultaneous bookings without data corruption.
**How:** k6.

---

## 📊 Layer 7: Continuous Observability
**What:** Real-time metrics and logging.
**Why:** Provides visibility into live production health and user behavior.
**How:** Prometheus (collection) and Grafana (visualization).
