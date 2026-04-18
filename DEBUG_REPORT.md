# Docker Debug Report

## Summary

The URL shortener failure was not caused by ASP.NET CORS being disabled.

The backend was correctly configured to return CORS headers for `http://localhost:4200`, but the shorten-URL request was failing because the database was not being initialized correctly in Docker. That backend failure surfaced in the browser as a misleading CORS-style error.

## What Actually Happened

### 1. Docker Compose could not start the database cleanly

The `db` service in [`compose.yml`](./compose.yml) used a fixed container name:

- `sql_server2022`

That name conflicted with an older existing container on the machine, so Compose could not start the SQL Server service reliably until the stale container was removed.

### 2. The SQL initialization path was outdated

The SQL container startup command expected this executable to exist:

- `/opt/mssql-tools/bin/sqlcmd`

In the current `mcr.microsoft.com/mssql/server:2022-latest` image, that path was not available. As a result:

- the startup script did not run `setup.sql`
- the `urlshortener` database was never created
- the backend could start, but database-dependent API requests failed

### 3. The browser error looked like CORS, but the backend was actually failing

Direct testing showed that the backend did return:

- `Access-Control-Allow-Origin: http://localhost:4200`

So CORS was active.

The actual failure on the shorten endpoint was a backend `500` caused by SQL errors, including:

- `Cannot open database "urlshortener" requested by the login`
- `Login failed for user 'sa'`

This is why the frontend appeared to be hitting a CORS issue even though the root cause was backend/database startup.

### 4. After removing the fixed container name, the backend hostname also had to be updated

Once the fixed container name was removed, the backend could no longer use:

- `sql_server2022`

as the SQL hostname. In Docker Compose, the correct internal hostname is the service name:

- `db`

The backend connection string still pointed at `sql_server2022`, which caused temporary DNS/connection failures until it was corrected.

## Fixes Implemented

### 1. Simplified the SQL service in Compose

Updated [`compose.yml`](./compose.yml):

- removed the fixed `container_name`
- removed the custom SQL bootstrap command
- removed the dependency on the outdated `sqlcmd` path inside the SQL image

This makes the SQL container behave like a normal Compose-managed service.

### 2. Moved schema creation responsibility into the backend

Updated [`urlgoatbackend/Program.cs`](./urlgoatbackend/Program.cs):

- enabled SQL retry behavior with `EnableRetryOnFailure()`
- added startup migration logic using `context.Database.MigrateAsync()`
- added retry logic around migrations so the backend can wait for SQL Server to become reachable

This is more robust than relying on a shell script inside the database container.

### 3. Corrected the backend SQL hostname

Updated [`urlgoatbackend/appsettings.json`](./urlgoatbackend/appsettings.json):

- changed `Data Source=sql_server2022,1433`
- to `Data Source=db,1433`

That aligns the backend with Docker Compose service discovery.

## Verification

After the fixes:

- `docker compose up -d --build` succeeded
- the backend health endpoint returned `200 OK` at `http://localhost:5150/health`
- EF Core migrations were applied successfully on backend startup
- POST to `http://localhost:5150/api/UrlMapping/CreateShortUrl` succeeded

Successful response observed:

```json
{"shortenedUrl":"http://localhost:5150/api/UrlMapping/585592C8","sKey":"585592C8","newurl":true}
```

## Why This Looked Like a 2023 vs 2026 Compatibility Problem

There was compatibility drift, but not in the way the browser error suggested.

The main compatibility problems were:

- old Docker/SQL assumptions in `compose.yml`
- reliance on an outdated SQL tooling path inside the SQL Server image
- hard-coded service naming assumptions

The backend CORS middleware itself was still functioning.

## Remaining Follow-Up

There are still backend compatibility warnings worth cleaning up next, especially:

- `Microsoft.AspNet.WebApi.Cors 5.2.9` being referenced from a `net6.0` application
- `NU1701` restore warnings for older .NET Framework-oriented packages

Those warnings did not cause this Docker failure, but they are stale dependencies and should be removed or replaced.
