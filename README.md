# Developer Habit Tracker

A RESTful API for tracking and analyzing developer habits, helping developers build consistent routines and improve productivity.

## Tech Stack

- **Framework:** ASP.NET Core 9.0
- **Language:** C# 13
- **Observability:** OpenTelemetry (tracing, metrics, logging)
- **Containerization:** Docker with multi-stage builds
- **Dashboard:** ASP.NET Aspire Dashboard for observability

## Getting Started

### Prerequisites

- [.NET 9 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
- [Docker](https://www.docker.com/products/docker-desktop/) (optional)

### Running Locally

```bash
# Clone the repository
git clone https://github.com/njabuloskosana/developer-habit-tracker.git
cd developer-habit-tracker

# Run the API
dotnet run --project DevHabit/DevHabit.Api/DevHabit.Api.csproj
```

The API will be available at `http://localhost:5280`

### Running with Docker

```bash
cd DevHabit
docker-compose up
```

This starts the API along with the Aspire Dashboard for observability at `http://localhost:18888`

## Project Structure

```
DevHabit/
├── DevHabit.Api/           # Web API project
├── Directory.Build.props   # Shared build configuration
├── Directory.Packages.props # Centralized package management
└── docker-compose.yml      # Container orchestration
```

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.
