# WSLC Container Manager product overview

## 1. Product purpose

WSLC Container Manager (WSLCC) gives Windows developers a desktop interface for Microsoft WSL Containers (wslc). Developers can browse containers and images, create and run containers, stop or delete them, and pull images without memorizing the wslc command syntax.

- WSL includes wslc. There is no Docker Desktop license cost, separate daemon or Docker Engine dependency.
- Lists, forms, progress indicators and status displays provide access to command-line operations.
- The database records operations for audit, history and troubleshooting.
- Separate UI and backend layers, persistent data and observable errors support production maintenance.

## 2. Target users

| Role | Environment | Main needs |
|---|---|---|
| Individual developer | Uses WSL for daily development | Pull images, start and stop containers, check status quickly |
| Enterprise developer | Works under Intune or GPO policies | Control and audit operations and retain basic functionality on restricted networks |
| Operations or DevOps engineer | Manages containers in batches | Refresh lists, start or stop containers in one click, understand errors |

## 3. Scope

### 3.1 Included from MVP to v1.0

- Environment checks cover wslc, kernel and Windows versions, missing components and active sessions.
- List, pull (with live progress), delete and refresh images.
- List containers with names, images, status and ports. Create containers with a name, image, port mappings, environment variables, automatic removal and background execution. Start, stop, restart and delete them.
- Configure and save the session name, storage path and CPU and memory quotas.
- Store application settings, audit logs, image pull history and container state snapshots in local SQLite storage.
- Show readable CLI and API errors consistently in an InfoBar.

### 3.2 Deferred to later v1.x releases

- Built-in terminal connected to container stdin/stdout streams.
- Docker Compose orchestration.
- Remote host management.
- Concurrent management of multiple sessions.
- Image build wizard using a Dockerfile.

## 4. Constraints and prerequisites

- Target Windows 10 version 1809 or later (minimum platform 10.0.17763). Windows 11 is the development and runtime target.
- WSL 2.9.3 or later is required (preview with wslc.exe). The app checks for it and displays the upgrade command if it is missing.
- Image pulls require access to a container registry. If network restrictions prevent a pull, the app must display a readable error and keep running.
- The stack comprises WinUI 3 (Windows App SDK 2.4.0), .NET 10, CommunityToolkit.Mvvm, Microsoft.WSL.Containers (native API), the wslc CLI (fallback) and SQLite (Microsoft.Data.Sqlite).

## 5. Success criteria

- A self-contained build runs on a machine without Windows App Runtime installed.
- Environment checks and image and container lists work in a real wslc environment.
- Writes (pull, start, stop, delete and create) succeed when wslc is available. When it is unavailable, failures explain the cause.
- SQLite retains settings, audit logs, pull history and container snapshots across application restarts.
- The build succeeds without compiler warnings, including MVVMTK0045.
