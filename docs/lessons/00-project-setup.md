# Lesson 0: Project Setup

**Date:** 2026-02-15
**Topics:** ASP.NET Core 9.0, Docker, OpenTelemetry, Project Structure

## Overview

This lesson covers the initial setup of the Developer Habit Tracker API, including the project structure, build configuration, and observability setup.

## What We Built

### 1. Solution Structure

Created a modern ASP.NET Core 9.0 solution using:
- **New .slnx format** for solution files
- **Directory.Build.props** for centralized build settings
- **Directory.Packages.props** for centralized package management
- **.editorconfig** for strict code style enforcement

### 2. API Project (DevHabit.Api)

A minimal API project with:
- Controllers for future endpoints
- OpenTelemetry instrumentation
- Swagger/OpenAPI documentation
- Docker containerization

### 3. Build Configuration

Enabled strict quality standards:
- All warnings treated as errors (`TreatWarningsAsErrors: true`)
- Nullable reference types enabled
- C# 13 language features
- File-scoped namespaces required

### 4. Observability with OpenTelemetry

Configured comprehensive telemetry:
- **Tracing**: ASP.NET Core and HTTP client instrumentation
- **Metrics**: ASP.NET Core, HTTP client, and runtime metrics
- **Logging**: Console and OTLP exporters
- **Aspire Dashboard** for local visualization

### 5. Docker Setup

Multi-stage Dockerfile:
- SDK image for building
- Runtime image for deployment
- Optimized for production
- Docker Compose with Aspire Dashboard

## Key Files

- `DevHabit.slnx` - Solution file
- `Directory.Build.props` - Global build settings
- `DevHabit.Api/Program.cs` - Application entry point
- `DevHabit.Api/Dockerfile` - Container configuration
- `docker-compose.yml` - Local development orchestration

## Running the Project

```bash
# Build
dotnet build DevHabit.slnx --configuration Release

# Run locally (HTTP)
dotnet run --project DevHabit/DevHabit.Api/DevHabit.Api.csproj --launch-profile http

# Run with Docker (includes Aspire Dashboard)
docker-compose -f DevHabit/docker-compose.yml up
```

**Local URLs:**
- API (HTTP): http://localhost:5280
- API (HTTPS): https://localhost:7217
- Aspire Dashboard: http://localhost:18888

## What's Next

In the next lessons, we'll:
- Add database integration
- Implement domain models
- Create repositories and services
- Build API endpoints
- Add authentication and authorization

## Key Takeaways

1. **Modern project structure** uses centralized configuration for consistency
2. **Strict code quality** is enforced at build time, not review time
3. **Observability is built-in** from day one, not added later
4. **Docker containerization** ensures consistent environments
5. **OpenTelemetry** provides vendor-neutral observability
