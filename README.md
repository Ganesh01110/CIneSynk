# CineSynk | Premium Ticket Booking System

**CineSynk** is a high-performance, enterprise-grade movie ticket booking application built with ASP.NET Core 10 and React.

## 🚀 Features
- **Real-time Seat Map:** Interactive grid with instant availability updates.
- **Enterprise Security:** JWT-based authentication with BCrypt hashing.
- **Optimistic Concurrency:** Guaranteed no double-bookings at the database level.
- **Auto-Cleanup:** Background workers to release abandoned seat reservations.
- **Scalable Architecture:** Clean code separation with a decoupled service layer.

## 🛠 Tech Stack
- **Backend:** .NET 10 (ASP.NET Core Web API), EF Core 9
- **Frontend:** React 18, TypeScript, Redux Toolkit (RTK Query), Vite
- **Database:** MariaDB 10.11
- **DevSecOps:** Docker, Docker Compose, GitHub Actions

## 📦 Getting Started

### Prerequisites
- Docker Desktop
- .NET 10 SDK (for local development)
- Node.js 20+

### Database Setup
1. Use a MariaDB instance (XAMPP or Docker).
2. Create a database named `bookingapplication`.
3. Run the setup scripts found in `document/backendDocs.md`.

### Professional Setup (Docker)
Run the entire stack with one command:
```bash
docker-compose up -d --build
```

## 📄 Documentation
Detailed technical documentation can be found in the `/document` folder.
