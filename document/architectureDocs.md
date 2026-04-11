# CineSynk - System Architecture & Data Flow

This document provides a visual and technical breakdown of how CineSynk works, from user interaction to cloud deployment.

---

## 🏗️ 1. High-Level System Architecture
This diagram shows how the user interacts with the system through the Load Balancer and how the services communicate.

```mermaid
graph TD
    User((User / Browser)) -->|HTTP/HTTPS| ALB[AWS Application Load Balancer]
    
    subgraph "Public Subnet"
        ALB -->|Port 80| FE[Frontend: Nginx / React]
    end

    subgraph "Private Subnet"
        ALB -->|Port 5000 /api| BE[Backend: .NET 10 API]
        BE -->|Port 3306| DB[(MariaDB Database)]
    end

    subgraph "Observability Layer (Local)"
        PROM[Prometheus] -->|Scrapes| BE
        GRAF[Grafana] -->|Visualizes| PROM
    end
```

---

## 🔄 2. Core Booking Flow (Sequence)
This diagram illustrates the logic lifecycle when a user attempts to reserve a seat. It highlights the security and concurrency checks.

```mermaid
sequenceDiagram
    participant U as User
    participant A as Auth Middleware
    participant B as Booking Service
    participant D as Database

    U->>A: POST /api/bookings/reserve
    A->>A: Validate JWT Token
    alt Token Invalid
        A-->>U: 401 Unauthorized
    else Token Valid
        A->>B: Process Reservation
        B->>D: Check Seat Availability & Version
        D-->>B: Current Seat Status
        alt Seat Already Taken
            B-->>U: 400 Conflict (Concurrency Error)
        else Seat Available
            B->>D: Update Seat (Optimistic Locking)
            D-->>B: Commit SUCCESS
            B-->>U: 200 OK (Reservation Confirmed)
        end
    end
```

---

## 🤖 3. DevSecOps Pipeline Flow
How your code travels from a `git push` to a live production environment.

```mermaid
graph LR
    Push(Git Push) --> Build[Build: .NET & React]
    Build --> Test[Tests: Unit & Arch]
    Test --> Security[Security: Snyk Scan]
    Security --> Audit[Performance: Lighthouse]
    Audit --> Docker[Container: Docker Build]
    Docker --> ECR[Registry: AWS ECR]
    ECR --> ECS[Deploy: AWS ECS Fargate]
```

---

## 📉 4. Observability Data Flow
How performance metrics are collected and displayed in the local dashboard.

```mermaid
graph LR
    API[CineSynk API] -->|Exposes /metrics| Prom[Prometheus]
    Prom -->|Stores Time-Series Data| Prom
    Graf[Grafana] -->|Queries| Prom
    Graf -->|Renders| Dashboard[Health Dashboard]
```

---

## 🛠️ 5. Clean Architecture Layers
The internal structure of the Backend code ensuring maintainability.

- **Presentation:** Controllers & Middlewares (API endpoints).
- **Application:** Services (Business logic & Orchestration).
- **Domain:** Models & DTOs (The "Heart" of the system).
- **Infrastructure:** Data Access (EF Core & Migrations).
