# ------------------------------------------------------------------
# STAGE 1: Dependencies (Leveraging caching)
# ------------------------------------------------------------------
FROM node:20-alpine AS deps
RUN apk add --no-cache libc6-compat
WORKDIR /app

# Only copy files for dependency resolution
COPY package.json package-lock.json ./
RUN npm ci

# ------------------------------------------------------------------
# STAGE 2: Builder (Compile Source)
# ------------------------------------------------------------------
FROM node:20-alpine AS builder
WORKDIR /app
COPY --from=deps /app/node_modules ./node_modules
COPY . .

# Run build to generate static assets in /dist
RUN npm run build

# ------------------------------------------------------------------
# STAGE 3: Runner (Ultra-lightweight Nginx)
# ------------------------------------------------------------------
FROM nginx:stable-alpine AS runner
WORKDIR /usr/share/nginx/html

# Security: Create non-root user (Standard Nginx image has 'nginx' user)
# We will ensure Nginx uses this user and has permissions
RUN touch /var/run/nginx.pid && \
    chown -R nginx:nginx /var/run/nginx.pid /var/cache/nginx /var/log/nginx /etc/nginx/conf.d

# Copy static assets from builder
COPY --from=builder /app/dist .
COPY nginx.conf /etc/nginx/conf.d/default.conf

# Switch to non-root user
USER nginx

EXPOSE 80

# Health check to ensure the UI is serving
HEALTHCHECK --interval=30s --timeout=10s --start-period=5s --retries=3 \
    CMD wget --quiet --tries=1 --spider http://localhost:80 || exit 1

CMD ["nginx", "-g", "daemon off;"]
