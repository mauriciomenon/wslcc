# Backend functional requirements (WSLCC.Core / WSLCC.Data)

## 1. Layers

```
WSLCC.Core            Domain and infrastructure services, independent of the UI
  Models/             Immutable DTOs for environment, images, containers, progress, settings and auditing
  Cli/                wslc.exe process runner and error mapping
  Services/           Environment, image, container, system, log, settings, audit and history services
  WslcHost.cs         Singleton service facade

WSLCC.Data            Data access layer
  WslcDatabase.cs     SQLite connection and schema initialization
  Repositories/       Settings, audit, pull history, container snapshots and session state
```

WSLCC.Core depends on WSLCC.Data. WSLCC.App depends on WSLCC.Core and has no direct dependency on Data.

## 2. Data channels

- The primary channel is the `wslc` CLI (`wslc.exe` with `--format json`). It was the only stable option in the test environment, where the native API returned E_UNEXPECTED. It also offers the broadest command coverage, including list, logs, stats, save and load.
- The alternative is the Microsoft.WSL.Containers native API, exposed through `WslcApiHost`. Where supported, it allows event-driven progress and type-safe calls.
- At runtime the environment service reports `ApiAvailable`. Image and container services use the CLI. `WslcApiHost` exposes native operations for a future switch.

## 3. Services and responsibilities

| Service | Main operations | Behavior |
|---|---|---|
| WslcEnvironmentService | GetEnvironmentAsync | Combines `wslc info --format json` and API GetMissingComponents/GetVersion |
| WslcImageService | List / Pull / Delete | `image ls --format json`, streaming `pull`, `rmi` |
| WslcContainerService | List / Start / Stop / Kill / Restart / Remove / Run | `container list --format json`, `start/stop/kill/restart/rm`, `run -d --rm --name -p -e -v` |
| WslcLogService | GetLogsAsync | Monospaced text from `logs <name>` |
| WslcInspectService | InspectContainerAsync | Structured JSON string from `inspect <name> --format json` |
| WslcSystemService | GetInfoAsync / ListSessionsAsync / TerminateSessionsAsync | `info --format json`, `system session list`, `system session terminate` |
| WslcSettingsService | Get/Set session settings | Reads and writes AppSettings for the session name, storage path, CPU and memory |
| WslcAuditService | Record / Query / Clear | Audits category, action, details, result and duration |
| WslcHistoryService | RecordPull / QueryPulls | Stores and queries image pull history |
| WslcSnapshotService | RecordContainer / QueryLatest | Caches observations of container state |
| WslcApiHost | Native API access | Session singleton, PullImageAsync progress, GetImagesAsync, DeleteImageAsync, TerminateSessionAsync |

## 4. CLI execution and error contract

- Route all CLI calls through `WslcRunner`.
  - Launch `wslc.exe <args>` without a window and redirect UTF-8 output.
  - On success, return stdout. Parse `--format json` as one JSON object per line (Docker style) and accept alternative names for fields.
  - On failure, combine stdout and stderr. Extract the code from `错误代码： X` or `Error code: X` and throw `WslcCliException` with WslcErrorCode.
  - For long-running commands such as pull and run, report each output line through `IProgress<string>`.
- Display exception messages in the UI InfoBar. Include the error code, such as `(E_UNEXPECTED)`, so users can look it up in the wslc documentation and GitHub issues.

## 5. Additional capabilities

These features extend the wslc CLI. If a command is unavailable, report a readable error and keep the app running.

1. Read live and historical container logs with `logs <name>`.
2. Display structured container details with `inspect <name> --format json`.
3. Manage leftover VM sessions with `system session list` and `terminate`. In testing, stale sessions caused E_UNEXPECTED in later operations. Session management provides a way to recover.
4. Reserve save/load commands for future image export and import.
5. Use WslcAuditService to record writes in the database and retain image pull history.
6. Read and write session settings from the Settings page. Use them as defaults when creating containers or sessions.
7. Save container state to SQLite whenever the list refreshes. The app can display the last snapshot while offline or after a quick restart.

## 6. Concurrency and threading

- Services are stateless singletons scoped to the process (WslcHost.Default).
- Await asynchronous methods on the UI context. Leave the choice of ConfigureAwait(false) to the caller. Long-running service operations must keep the UI responsive.
- WslcRunner starts a separate process for each call, executing each call's commands sequentially. Serialize database writes with SemaphoreSlim on the shared SQLite connection.
