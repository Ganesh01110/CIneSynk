# ------------------------------------------------------------------
# STAGE 1: Dependencies (Restore)
# ------------------------------------------------------------------
FROM mcr.microsoft.com/dotnet/sdk:10.0-preview AS deps
WORKDIR /app

COPY *.csproj ./
RUN dotnet restore -r linux-musl-x64

# ------------------------------------------------------------------
# STAGE 2: Builder (Publish)
# ------------------------------------------------------------------
FROM mcr.microsoft.com/dotnet/sdk:10.0-preview AS builder
WORKDIR /app
COPY --from=deps /root/.nuget /root/.nuget
COPY . ./
RUN dotnet publish -c Release -o out -r linux-musl-x64 --no-restore --self-contained false

# ------------------------------------------------------------------
# STAGE 3: Runner (Alpine Runtime)
# ------------------------------------------------------------------
FROM mcr.microsoft.com/dotnet/aspnet:10.0-preview-alpine AS runner
WORKDIR /app

# Security: Best practice for .NET containers
ENV ASPNETCORE_HTTP_PORTS=8080
ENV DOTNET_RUNNING_IN_CONTAINER=true

COPY --from=builder /app/out .

# Use a non-root user for security (The image has a 'app' user by default in newer .NET images)
USER $APP_UID

EXPOSE 8080

# Health check
HEALTHCHECK --interval=30s --timeout=10s --start-period=5s --retries=3 \
    CMD wget --quiet --tries=1 --spider http://localhost:8080/health || exit 1

ENTRYPOINT ["dotnet", "TicketBooking.Api.dll"]
