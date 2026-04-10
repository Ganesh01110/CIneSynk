# CineSynk - Cloud Infrastructure Documentation

This document explains the "Infrastructure as Code" (IaC) setup for CineSynk. We use **Terraform** to define the entire AWS cloud environment.

---

## 🧱 1. Terraform Files (.tf)

| File | Purpose | Why it's here |
| :--- | :--- | :--- |
| `main.tf` | **Foundation** | Defines the AWS provider and region. It also handles global variables like your Project Name. |
| `vpc.tf` | **The Network** | Creates a private "Virtual Private Cloud." It splits the network into Public zones (for the Load Balancer) and Private zones (for your Database) to keep things secure. |
| `rds.tf` | **Database** | Sets up the **AWS RDS (MariaDB)** instance. This is a "Managed" database, meaning AWS handles the backups and OS updates for you. |
| `ecs.tf` | **Compute (Fargate)** | Defines the "Cluster" and the "Services." This is where your code actually runs. We use **Fargate** so you don't have to manage physical servers. |
| `ecr.tf` | **Registry** | Secure "parking spots" for your Docker images. Your CI/CD pipeline pushes images here, and ECS pulls them to run the app. |
| `alb.tf` | **Load Balancer** | The "Traffic Controller." It takes one URL and routes traffic to either the Frontend or the `/api` backend based on the path. |
| `outputs.tf` | **Results** | After deployment, this prints out your **Website URL** and Database addresses so you know where to go. |

---

## 📄 2. ECS Task Definitions (.json)
Found in `infrastructure/ecs/`.

These files are "Blueprints" for your containers:
- **`api-task-def.json`**: Tells AWS how much RAM (512MB) and CPU (256 units) the Backend needs.
- **`frontend-task-def.json`**: Similarly, configures the Frontend Nginx container.
- These files are used by **GitHub Actions** to automatically update your cloud app whenever you push new changes to the code.

---

## ⚠️ 💰 3. IMPORTANT: COST & SAFETY (Read Carefully)

As you are using this for **experimentation**, please follow these rules to minimize or avoid costs:

### Is it Free?
- **RDS & ALB:** Most new AWS accounts have a **12-month Free Tier**. As long as you stay under 750 hours/month, these are $0.
- **ECS Fargate:** Fargate is **NOT perpetually free**. It may cost a few cents per hour.
- **VPC:** The network is free, but NAT Gateways (often used for private subnets) cost money. I have designed your VPC to use an **Internet Gateway** to keep it minimal.

### How to avoid a bill:
1.  **Deploy for Testing:** Run `terraform apply` to see your work in the cloud.
2.  **Experiment:** Verify the URL, check the dashboard, and see the API in action.
3.  **DESTROY:** When you are done for the day, **ALWAYS** run:
    ```bash
    terraform destroy
    ```
    This command wipes everything clean and stops the AWS billing clock immediately. **Do not leave it running overnight if you want to ensure it stays $0.**

### Safety note:
- Since you are not sharing the link, you are safe from external traffic eating your "Load Balancer" bandwidth (which is also part of the Free Tier limits).
