# CineSynk - Frontend Technical Documentation

This document provide a deep dive into the frontend architecture and implementation of the CineSynk Cinema Booking UI.

## 🏗 Architecture Overview

The frontend is a modern **Single Page Application (SPA)** built with **React 18** and **TypeScript**, using **Vite** for optimized development and bundling.

### Core Tech Stack
- **Framework:** React 18
- **Language:** TypeScript
- **State Management:** Redux Toolkit (RTK)
- **Data Fetching:** RTK Query
- **Styling:** Vanilla CSS (Modern aesthetic with glassmorphism and theme variables)
- **Icons:** Lucide React

---

## 💎 Design System & UI

### Modern Cinematic Aesthetic
- **Colors:** Deep obsidian backgrounds (`#0d0d12`) with vibrant cyber-blue gradients for focus elements.
- **Glassmorphism:** Used extensively for cards and modals (`backdrop-filter: blur(12px)`), creating a premium, layered feel.
- **Micro-animations:** Subtle hover effects and CSS keyframe animations (`fadeIn`, `pulse`) for a dynamic, interactive experience.

### Key Components
- **Navbar:** Sticky navigation with user profile and auth status.
- **Show Cards:** Cinematic posters with metadata.
- **Seat Map Grid:** Interactive, color-coded grid representing the theatre layout.

---

## 🛰 State Management & API Integration

### Redux Store Config
The application uses a centralized **Redux Store** at `src/store/store.ts`.  It manages:
- **`authSlice`:** Persistent user credentials (synchronized with `localStorage`).
- **`apiSlice`:** All server-side data managed by **RTK Query**.

### RTK Query & Caching
Instead of manual `useEffect` fetches, the app uses RTK Query.
- **`useGetShowsQuery`:** Fetches movie listings and caches them for high-speed navigation.
- **`useGetShowSeatsQuery`:** Fetches the real-time seat map for a specific show ID.
- **Automated Re-fetching:** When a user reserves a seat, the system automatically invalidates the "Show" cache, triggering an instant refresh of the seat map for all users.

---

## 🔐 Auth Flow & Guards

- **JWT Integration:** The system automatically extracts the JWT from the Redux store and injects it into every relevant request header using a `prepareHeaders` middleware.
- **ProtectedRoute:** A Higher-Order Component (HOC) that intercepts navigation to the Dashboard and Seat Map, redirecting unauthorized users back to the Login portal.

---

## 🛠 Development & Build

### Running Locally
1. `cd frontend`
2. `npm install`
3. `npm run dev`

### Production Build
The project uses **Vite's build pipeline** to generate highly optimized, minified assets in the `dist/` folder.
```bash
npm run build
```
These assets are then served by **Nginx** in the production Docker container.
