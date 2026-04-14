# 🎬 CineSynk: Enterprise Ticket Booking Ecosystem

![CineSynk License](https://img.shields.io/badge/License-MIT-green.svg)
![Build Status](https://img.shields.io/badge/CI%2FCD-Passing-brightgreen.svg)
![Security Scan](https://img.shields.io/badge/Security-Snyk%20Shield-blueviolet.svg)
![Coverage](https://img.shields.io/badge/Testing-5--Tiered-orange.svg)

**CineSynk** is a production-grade, real-time ticket booking application designed for high-concurrency environments. Built with a focus on Clean Architecture, security, and automated cloud scalability, it serves as a blueprint for modern full-stack enterprise development.

---

## 🚀 Key Features

- **High-Concurrency Booking:** Implements **Optimistic Concurrency Control** (EF Core RowVersion) to prevent double-booking.
- **Stateless Security:** Secure JWT-based authentication with BCrypt password hashing.
- **Enterprise Resilience:** Background workers for automatic cleanup of expired reservations (Idempotency assured).
- **Premium UX:** High-performance React UI with glassmorphic aesthetics.
- **Cloud Native:** Fully automated deployment to **AWS ECS Fargate**.
- **Full Observability:** Integrated **Prometheus** & **Grafana** stack for real-time monitoring.
- **Interactive API:** Full **Swagger UI** for endpoint discovery and testing.

---

## 🛠️ Technical Stack

### **Backend (.NET 10)**
- **Framework:** ASP.NET Core 10 (Web API)
- **Persistence:** Entity Framework Core 9 (MariaDB)
- **Testing:** xUnit, Moq, NetArchTest (Architecture enforcement)

### **Frontend (Vite / React 18)**
- **Framework:** React 18 with TypeScript
- **State:** Redux Toolkit & RTK Query
- **Styles:** Vanilla CSS with Modern Glassmorphism
- **E2E:** Playwright Browser Testing

### **DevSecOps & Infrastructure**
- **Containerization:** Optimized Multi-stage Alpine Dockerfiles
- **CI/CD:** 5-Stage GitHub Actions (Lint -> Snyk -> Lighthouse -> Docker -> AWS)
- **Cloud:** AWS (ECS, RDS, ALB, ECR)
- **IaC:** Terraform

---

## 📚 Documentation Hub

Explore the detailed technical documentation for each layer of CineSynk:

| Category | Documentation Link |
| :--- | :--- |
| **Backend** | [Clean Architecture & Services](./document/backendDocs.md) |
| **Frontend** | [State Management & UI Library](./document/frontendDocs.md) |
| **Quality Gates** | [5-Tiered Testing Strategy](./document/testDocs.md) |
| **DevOps** | [CI/CD & Pipeline Workflow](./document/devopsDocs.md) |
| **Cloud Infra** | [Terraform & AWS Deployment](./document/infraDocs.md) |
| **Workflows** | [Step-by-Step Command Guide](./document/workflowDocs.md) |
| **Architecture** | [Visual Flow & Data Diagrams](./document/architectureDocs.md) |

---

## 🔧 Getting Started (Local Development)

### **Prerequisites**
- Docker & Docker Compose
- .NET 10 SDK (for local development)
- Node.js 20+

### **Run with Docker Compose**
Spin up the entire ecosystem (API, Frontend, MariaDB, and Monitoring) with one command:
```bash
docker-compose up -d --build
```

### **Entry Points**
| Service | Local URL |
| :--- | :--- |
| **Frontend UI** | `http://localhost:5173` |
| **API Swagger UI** | `http://localhost:5000/swagger` |
| **Grafana Dashboard** | `http://localhost:3001` (user: `admin`, pass: `admin`) |
| **Prometheus** | `http://localhost:9090` |

---

## 🧪 Advanced Testing Strategy

CineSynk implements a "Defense-in-Depth" testing philosophy:
1.  **Unit Tests:** Business logic verification.
2.  **Architecture Tests:** Automated enforcement of layering rules.
3.  **Integration Tests:** Verifying the full API-to-DB chain.
4.  **Load Testing (k6):** Simulating 100+ concurrent booking requests.
5.  **E2E (Playwright):** Browser-based user journey verification.

---

## 🏛️ Architecture Highlights
- **Layered Design:** Strict separation of concerns between Controllers, Services, and Data.
- **Scalability:** Stateless API design ready horizontal scaling via AWS ECS.
- **Maintainability:** Fully documented codebase with TypeScript strict-mode and C# Nullable reference types.

**Developed with ❤️ for the DevSecOps & Cloud Engineering community.**
