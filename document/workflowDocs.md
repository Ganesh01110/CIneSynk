# CineSynk - Operational Workflow Guide

This document contains the exact commands required to run and test every part of the CineSynk project.

---

## 🏗️ 1. Local Orchestration (Recommended)
Run the entire ecosystem (Frontend, API, DB, Prometheus, Grafana) in one go:
```bash
# Build and Start all 5 services
docker-compose up -d --build

# Stop all services
docker-compose down
```

---

## 💻 2. Individual Development
If you want to run the code without Docker for faster development:

### **Backend (.NET API)**
```bash
cd backend/TicketBooking.Api
dotnet run
```
*Access:* `http://localhost:5000/swagger`

### **Frontend (Vite/React)**
```bash
cd frontend
npm install
npm run dev
```
*Access:* `http://localhost:5173`

---

## 🧪 3. Automated Testing Suite

### **Backend Unit & Architecture Tests**
```bash
dotnet test
```

### **E2E Browser Tests (Playwright)**
Make sure the app is running first!
```bash
cd frontend
npx playwright test
```

### **Load & Stress Testing (k6)**
Make sure the backend is running!
```bash
# You must have k6 installed locally
k6 run tests/load/k6-load-test.js
```

---

## ☁️ 4. Cloud Infrastructure (AWS)
Ensure you have the AWS CLI configured with your credentials.

```bash
cd infrastructure/terraform

# Initialize Terraform
terraform init

# Preview changes (Safest)
terraform plan

# Deploy to AWS (COURSES COSTS!)
terraform apply

# UN-DEPLOY / STOP BILLING (Critical for experiments)
terraform destroy
```

---

## 🛠️ 5. Database Management
Since we implemented **Automatic Migrations** in `Program.cs`, you do **not** need to run any scripts for the schema. The app will create and update the database automatically when it starts.

However, if you ever need to manually create a new migration:
```bash
cd backend/TicketBooking.Api
dotnet ef migrations add <MigrationName>
dotnet ef database update
```
