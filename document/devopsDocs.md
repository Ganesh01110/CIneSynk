# CineSynk - DevSecOps Technical Documentation

This document provide a deep dive into the CI/CD, containerization, and security orchestration of the CineSynk project.

## 📦 Containerization (Docker)

CineSynk is fully "Dockerized" to ensure consistency across development, testing, and production environments.

### 🔌 Multi-Stage Backend Build (`backend/Dockerfile`)
The .NET backend uses two stages to maintain security and reduce image size:
1. **Build Stage:** Uses the `.NET 10 SDK` to restore dependencies and publish the Release binary.
2. **Runtime Stage:** Only includes the lightweight `.NET 10 ASP.NET` runtime. The source code and SDK are discarded, leaving only the compiled artifacts.

### 🌐 Multi-Stage Frontend Build (`frontend/Dockerfile`)
1. **Build Stage:** Uses `Node.js 20` to perform a production TypeScript build.
2. **Serve Stage:** Uses `Nginx:Alpine`. The resulting `dist/` artifacts are copied into the Nginx web root, and a custom `nginx.conf` handles Single Page App (SPA) routing and API proxying.

---

## 🎛 Orchestration (Docker Compose)

The entire PulsQueue ecosystem can be launched with **`docker-compose up -d`**.

- **`db` Service:** MariaDB 10.11 with persistent volume mapping (`db_data`).
- **`backend` Service:** Connects to the database using environment-variable-based connection strings.
- **`frontend` Service:** Serves the UI on port 3000 and proxies all `/api/*` traffic to the backend container.

---

## 🤖 Advanced CI/CD Pipeline (GitHub Actions)

The project uses a sophisticated 5-stage pipeline defined in `.github/workflows/ci.yml`.

### Stage 1: Parallel Build & Test
The system simultaneously builds both the **.NET solution** and the **React application**. If any linting error or build failure occurs, the pipeline stops immediately.

### Stage 2: Security Scanning (Snyk.io)
Integrated security auditing:
- Scans `package.json` for vulnerable JavaScript dependencies.
- Scans `.csproj` for vulnerable NuGet packages.
- Stops the build if "High" severity vulnerabilities are found.

### Stage 3: Performance Audit (Lighthouse)
Uses Lighthouse CI to verify that the frontend build meets professional standards for:
- Performance
- Accessibility
- Best Practices
- SEO

### Stage 4: Docker Verification
Automatically runs `docker-compose build` in the CI environment to ensure that any changes haven't broken the containerization logic.

### Stage 5: Notifications (Discord)
Sends a rich embed notification to a Discord Webhook showing:
- Build Status (Success/Failure)
- Commit SHA and Message
- Author and Push Event
- Direct Link to the CI Job logs

---

## 🛠 Future Roadmap: Phase 5 (Cloud)

The next step for DevSecOps is **Cloud Deployment**.
- **Terraform:** To provision Azure Web Apps or AWS ECS clusters.
- **Secrets Management:** Integrating Azure Key Vault or AWS Secrets Manager for production.
