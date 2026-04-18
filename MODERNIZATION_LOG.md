# Modernization Log

## Summary

This document records modernization and stabilization changes made to the project.

So far, the work has focused on:

- removing outdated compatibility risks
- making Docker startup more reliable
- pinning frontend tooling to a stable Node version

## Compatibility Cleanup

### Removed the legacy Web API CORS package

Updated [`urlgoatbackend/urlgoatbackend.csproj`](./urlgoatbackend/urlgoatbackend.csproj)

Removed:

```xml
<PackageReference Include="Microsoft.AspNet.WebApi.Cors" Version="5.2.9" />
```

### Why this was safe

The backend already uses ASP.NET Core CORS configuration in [`urlgoatbackend/Program.cs`](./urlgoatbackend/Program.cs):

- `builder.Services.AddCors(...)`
- `app.UseCors("AllowMultipleOrigins")`

The removed package belonged to the older ASP.NET Web API stack and was not needed for the app's current runtime behavior.

### Removed redundant controller-level CORS attributes

Updated [`urlgoatbackend/Controllers/UrlMappingController.cs`](./urlgoatbackend/Controllers/UrlMappingController.cs)

Removed:

- `using Microsoft.AspNetCore.Cors;`
- `[EnableCors]` on `CreateShortUrl`
- `[EnableCors]` on `RedirectToOriginalUrl`

### Result

This removed the `NU1701` compatibility warnings caused by restoring an old .NET Framework-oriented package into a `net6.0` application.

## Docker and Database Stabilization

### Simplified the SQL service in Compose

Updated [`compose.yml`](./compose.yml)

Changes:

- removed the fixed `container_name`
- removed the custom SQL bootstrap command
- removed the dependency on the outdated `sqlcmd` path inside the SQL image

### Moved schema creation into the backend

Updated [`urlgoatbackend/Program.cs`](./urlgoatbackend/Program.cs)

Changes:

- enabled SQL retry behavior with `EnableRetryOnFailure()`
- added startup migration logic using `context.Database.MigrateAsync()`
- added retry logic around migrations so the backend can wait for SQL Server to become reachable

### Corrected the backend SQL hostname

Updated [`urlgoatbackend/appsettings.json`](./urlgoatbackend/appsettings.json)

Changed:

- `Data Source=sql_server2022,1433`

to:

- `Data Source=db,1433`

### Result

The backend can now create/apply its schema during startup without depending on brittle SQL container shell behavior, and the Compose stack starts cleanly.

## Frontend Node Pinning

### Pinned the frontend Docker image to Node 18

Updated [`urlgoatfrontend/Dockerfile`](./urlgoatfrontend/Dockerfile)

Changed:

```dockerfile
FROM node:latest as build
```

to:

```dockerfile
FROM node:18-bullseye as build
```

### Why this was done

The frontend is built with Angular 16. Using `node:latest` allows Docker builds to drift to much newer Node releases that Angular 16 was not designed around.

Pinning the image to Node 18 makes the environment more stable and predictable while keeping the app within the supported range for Angular 16.

### Result

The frontend container rebuilt successfully on Node 18 and the application continued working normally.

## Docker Health Checks

### Added health checks to Compose services

Updated [`compose.yml`](./compose.yml)

Changes:

- added a SQL Server health check using `/opt/mssql-tools18/bin/sqlcmd`
- added a backend health check that verifies the API container is accepting connections on port `80`
- changed `depends_on` so the backend waits for a healthy database
- changed `depends_on` so the frontend waits for a healthy backend

### Why this was done

Previously, containers were only starting in order, not waiting for the service behind each container to be ready.

That meant:

- the backend could still be applying migrations while another service or test tried to call it
- the frontend could start before the backend was actually ready
- early requests right after `docker compose up` could produce misleading transient failures

Health checks improve startup reliability by distinguishing:

- container started

from:

- service actually ready

### Result

The stack now has explicit readiness checks for the database and backend so startup sequencing is based on service health instead of simple container launch order.

## Backend Nullable Warning Cleanup

### Tightened nullable contracts in backend models and repository APIs

Updated:

- [`urlgoatbackend/Models/UrlMapping.cs`](./urlgoatbackend/Models/UrlMapping.cs)
- [`urlgoatbackend/Models/CreateShortUrl.cs`](./urlgoatbackend/Models/CreateShortUrl.cs)
- [`urlgoatbackend/Dto/CreateShortUrlDto.cs`](./urlgoatbackend/Dto/CreateShortUrlDto.cs)
- [`urlgoatbackend/Interfaces/IUrlMappingRepository.cs`](./urlgoatbackend/Interfaces/IUrlMappingRepository.cs)
- [`urlgoatbackend/Repository/UrlMappingRepository.cs`](./urlgoatbackend/Repository/UrlMappingRepository.cs)
- [`urlgoatbackend/Controllers/UrlMappingController.cs`](./urlgoatbackend/Controllers/UrlMappingController.cs)
- [`urlgoatbackend/Program.cs`](./urlgoatbackend/Program.cs)

Changes:

- initialized non-nullable string properties with safe defaults
- changed repository methods that can legitimately return no record from `Task<UrlMapping>` to `Task<UrlMapping?>`
- updated controller code to handle nullable repository results explicitly
- changed seed startup resolution to use `GetRequiredService(...)` instead of nullable service lookups

### Why this was done

The remaining compiler warnings were no longer package or Docker issues. They were nullable-reference warnings caused by:

- non-nullable model properties without default initialization
- methods claiming they always return an entity when they can actually return `null`
- dependency injection lookups using APIs that return nullable values

### Result

The backend nullability annotations now match the actual runtime behavior more accurately, reducing noise in the build and making the code contracts clearer.

## SQL Server Image Pinning

### Pinned the SQL Server container image to a specific tested release

Updated [`compose.yml`](./compose.yml)

Changed:

```yaml
image: "mcr.microsoft.com/mssql/server:2022-latest"
```

to:

```yaml
image: "mcr.microsoft.com/mssql/server:2022-CU24-GDR1-ubuntu-22.04"
```

### Why this was done

The floating `2022-latest` tag can change over time, which makes rebuilds less reproducible.

Pinning the image to a specific SQL Server 2022 release tag means future rebuilds will stay on that tested release unless the tag is intentionally changed.

### Verification basis

Registry inspection confirmed that at the time of pinning:

- `2022-latest`
- `2022-CU24-GDR1-ubuntu-22.04`

resolved to the same image manifest digest:

- `sha256:a3afbf7b43c3d59bcae7668245613afa0a432648b7ad4bafd16abf9f9d4ecc04`

An attempt to pin by digest directly caused a Docker Desktop unpack error on this machine, so the project was pinned to the exact release tag instead of the tag-plus-digest form.

### Result

The project now gets the stability benefits of staying on SQL Server 2022 while avoiding unplanned image drift from a floating tag.

### Local cleanup

After pinning the SQL Server image, the old local floating tag was removed:

- removed local tag: `mcr.microsoft.com/mssql/server:2022-latest`
- kept local pinned tag: `mcr.microsoft.com/mssql/server:2022-CU24-GDR1-ubuntu-22.04`

This cleanup was limited to the project-related SQL image reference so unrelated Docker images were not touched.

## Verification Performed

Verified during the modernization work:

- `docker compose up -d --build` completed successfully
- the Angular dev server started successfully on port `4200`
- the backend health endpoint returned `200 OK`
- the shorten endpoint returned successful responses after the changes

Example successful API responses observed:

```json
{"shortenedUrl":"http://localhost:5150/api/UrlMapping/9F5626EA","sKey":"9F5626EA","newurl":true}
```

```json
{"shortenedUrl":"http://localhost:5150/api/UrlMapping/30E4F5D5","sKey":"30E4F5D5","newurl":true}
```

## Remaining Follow-Up

The main remaining warnings are no longer package compatibility issues. They are standard nullable-reference warnings such as:

- `CS8618`
- `CS8602`
- `CS8603`

Those can be cleaned up separately as code-quality improvements.
