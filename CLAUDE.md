# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Build & Development Commands

All commands should be run from the repository root.

```bash
# Restore dependencies
dotnet restore DevHabit.slnx

# Build
dotnet build DevHabit.slnx --configuration Release --no-restore

# Run tests
dotnet test DevHabit.slnx --configuration Release --no-restore --no-build

# Publish
dotnet publish DevHabit.slnx --configuration Release --no-restore --no-build

# Run API locally (HTTP)
dotnet run --project DevHabit/DevHabit.Api/DevHabit.Api.csproj --launch-profile http

# Run with Docker Compose (includes Aspire Dashboard for observability)
docker-compose -f DevHabit/docker-compose.yml up
```

**Local URLs:**
- HTTP: `http://localhost:5280`
- HTTPS: `https://localhost:7217`
- Aspire Dashboard (when using Docker): `http://localhost:18888`

## Architecture

**Tech Stack:** ASP.NET Core 9.0 Web API with C# 13, containerized with Docker, instrumented with OpenTelemetry.

**Project Structure:**
```
DevHabit/
├── DevHabit.slnx              # Solution file (new .slnx format)
├── Directory.Build.props       # Global build settings (warnings as errors, nullable enabled)
├── Directory.Packages.props    # Centralized NuGet package versions
├── .editorconfig              # Strict code style enforcement
├── docker-compose.yml         # Docker orchestration with Aspire Dashboard
└── DevHabit.Api/              # Main API project
    ├── Program.cs             # Entry point with OpenTelemetry configuration
    ├── Controllers/           # API controllers
    └── Dockerfile             # Multi-stage container build
```

**Observability:** OpenTelemetry is configured for tracing, metrics, and logging, exporting via OTLP to the Aspire Dashboard. Instrumentation covers ASP.NET Core, HTTP client, and runtime metrics.

## Code Style Requirements

The `.editorconfig` enforces strict rules (violations are errors, not warnings):
- File-scoped namespaces required
- Prefer `int` over `Int32` (predefined types)
- Null propagation (`?.`) required where applicable
- Collection initializers required
- Expression-bodied members for properties and accessors
- No `this.` qualifier
- No unused parameters
- Explicit accessibility modifiers required
- All warnings treated as errors (`TreatWarningsAsErrors: true`)
