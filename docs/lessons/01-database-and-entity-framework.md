# Lesson 1: Database Integration with Entity Framework Core

**Date:** 2026-02-15
**Topics:** PostgreSQL, Entity Framework Core 9.0, Dependency Injection, Migrations, Value Objects

## Overview

This lesson covers the integration of PostgreSQL database using Entity Framework Core, including the first domain entity (Habit), automatic migrations, and database observability.

## What We Built

### 1. PostgreSQL Database Setup

Added PostgreSQL 17.2 to the Docker Compose stack:
- Container name: `devhabit.postgres`
- Database: `devhabit`
- Port: `5432`
- Data persistence via volume: `./.containers/postgres_data`
- Connection string in `appsettings.Development.json`

### 2. Entity Framework Core Integration

**NuGet Packages Added:**
- `Npgsql.EntityFrameworkCore.PostgreSQL` (9.0.4) - PostgreSQL provider
- `EFCore.NamingConventions` (9.0.0) - Snake case naming for database
- `Microsoft.EntityFrameworkCore.Tools` (9.0.13) - Migration tooling
- `Npgsql.OpenTelemetry` (9.0.4) - Database tracing

**DbContext Registration:**
```csharp
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options
        .UseNpgsql(connectionString,
            npgsql => npgsql.MigrationsHistoryTable(
                HistoryRepository.DefaultTableName,
                Schemas.Application))
        .UseSnakeCaseNamingConvention());
```

**Key Decisions:**
- **Dependency Injection**: DbContext registered as scoped service (default)
- **Snake Case**: Database uses `snake_case` naming (e.g., `created_at_utc`)
- **Custom Schema**: `dev_habit` schema instead of default `public`
- **Migration History**: Tracked in custom schema for organization

### 3. Habit Entity Design

Created the core `Habit` entity with:

**Properties:**
- `Id` (string) - Unique identifier
- `Name` (string, max 100) - Habit name
- `Description` (string, max 500) - Detailed description
- `Type` (enum) - Binary or Measurable
- `Status` (enum) - Ongoing or Completed
- `IsArchived` (bool) - Soft delete flag
- `EndDate` (DateOnly?) - Optional completion date
- Timestamps: `CreatedAtUtc`, `UpdatedAtUtc`, `LastCompletedAtUtc`

**Value Objects (Owned Entities):**
- `Frequency` - How often the habit should be done
  - `Type` (Daily/Weekly/Monthly)
  - `TimesPerPeriod` (int)
- `Target` - Goal for measurable habits
  - `Value` (int)
  - `Unit` (string, e.g., "minutes", "pages")
- `Milestone` - Progress tracking
  - `Target` (int)
  - `Current` (int)

**Configuration (HabitConfiguration.cs):**
```csharp
builder.HasKey(h => h.Id);
builder.Property(h => h.Id).HasMaxLength(500);
builder.Property(h => h.Name).HasMaxLength(100);
builder.Property(h => h.Description).HasMaxLength(500);
builder.OwnsOne(h => h.Frequency);
builder.OwnsOne(h => h.Target, ...);
builder.OwnsOne(h => h.Milestone);
```

### 4. Database Context

**ApplicationDbContext.cs:**
- Primary constructor with `DbContextOptions<ApplicationDbContext>`
- `DbSet<Habit> Habits` for querying habits
- Applies all entity configurations from assembly
- Uses custom `dev_habit` schema

### 5. Auto-Migration Extension

Created `DatabaseExtensions.cs` with `ApplyMigrationsAsync`:
- Runs on application startup in Development
- Creates service scope for DbContext
- Applies pending migrations automatically
- Logs success/failure with structured logging

### 6. Initial Migration

Generated migration: `20260215025457_Add_Habits`
- Creates `habits` table in `dev_habit` schema
- Creates owned entity tables for Frequency, Target, Milestone
- Migration history in `dev_habit.__EFMigrationsHistory`

### 7. Enhanced Observability

Added database tracing to OpenTelemetry:
```csharp
.WithTracing(tracing => tracing
    .AddHttpClientInstrumentation()
    .AddAspNetCoreInstrumentation()
    .AddNpgsql())  // ← New: PostgreSQL tracing
```

Now database queries are traced in the Aspire Dashboard!

## Key Files

**New Files:**
- `DevHabit.Api/Entities/Habit.cs` - Domain entity
- `DevHabit.Api/Database/ApplicationDbContext.cs` - EF Core context
- `DevHabit.Api/Database/Schemas.cs` - Schema constants
- `DevHabit.Api/Database/Configurations/HabitConfiguration.cs` - Entity configuration
- `DevHabit.Api/Extentions/DatabaseExtentions.cs` - Migration helper
- `DevHabit.Api/Migrations/Application/20260215025457_Add_Habits.cs` - Initial migration

**Modified Files:**
- `DevHabit.Api.csproj` - Added EF Core packages
- `Program.cs` - DbContext registration and migration call
- `Directory.Packages.props` - Package versions
- `docker-compose.yml` - PostgreSQL service
- `appsettings.Development.json` - Connection string

## Dependency Injection in Action

This lesson introduces our first real use of DI:

**Service Registration:**
```csharp
builder.Services.AddDbContext<ApplicationDbContext>(options => ...);
```
- Registers `ApplicationDbContext` as a **scoped** service
- Scoped = one instance per HTTP request
- Options pattern for configuration

**Service Consumption:**
```csharp
// In DatabaseExtensions.cs
using IServiceScope scope = app.Services.CreateScope();
ApplicationDbContext dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
```
- Creates a service scope
- Resolves DbContext from DI container
- Proper disposal via `using` statement

## Running the Updated Project

```bash
# Start PostgreSQL and API with Docker Compose
docker-compose -f DevHabit/docker-compose.yml up

# Or run locally (requires PostgreSQL running)
dotnet run --project DevHabit/DevHabit.Api/DevHabit.Api.csproj --launch-profile http
```

**What Happens:**
1. Application starts
2. DbContext is registered in DI container
3. In Development mode, `ApplyMigrationsAsync()` is called
4. EF Core checks for pending migrations
5. Migrations are applied to PostgreSQL
6. Application is ready with database initialized

**Verify:**
- Aspire Dashboard: http://localhost:18888
- Check traces for database operations
- PostgreSQL accessible at `localhost:5432`

## What's Next

In the next lessons, we'll:
- Create a repository pattern for data access
- Add CRUD API endpoints for habits
- Implement validation
- Add unit tests for the domain model
- Create integration tests for the database layer

## Key Takeaways

1. **EF Core + DI** - DbContext is registered as a scoped service, perfect for web requests
2. **Value Objects** - Use owned entities for complex properties that don't need separate tables
3. **Snake Case** - Database uses snake_case while C# uses PascalCase automatically
4. **Auto-Migration** - Development environments apply migrations automatically on startup
5. **Custom Schema** - Organize database objects with custom schemas
6. **Observability** - Database queries are traced alongside HTTP requests
7. **Configuration Pattern** - Separate entity configuration keeps DbContext clean
8. **Extension Methods** - Encapsulate cross-cutting concerns like migration application
