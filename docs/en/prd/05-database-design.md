# Database design (WSLCC.Data / SQLite)

## 1. Technology choice

- This single-machine desktop app stores small amounts of configuration, audit and history data and has low concurrency.
- SQLite requires no separate deployment and uses a single file that can be read across platforms, simplifying production maintenance.
- Microsoft.Data.Sqlite is the official ADO.NET provider. It avoids the AOT and migration overhead of EF Core in WinUI.

## 2. Database location and connection

- File: `%LOCALAPPDATA%\WSLCC\wslcc.db`.
- Each operation opens a short-lived `SqliteConnection`, executes and closes it. Serialize database writes with SemaphoreSlim. Enable WAL mode for concurrent reads.
- On first open, initialize the schema with idempotent CREATE TABLE IF NOT EXISTS statements.

## 3. Database schema

### 3.1 app_settings (application settings)

| Column | Type | Description |
|---|---|---|
| key | TEXT PK | Setting key |
| value | TEXT NOT NULL | Setting value (JSON or plain text) |
| updated_at | TEXT NOT NULL | Update time (ISO8601) |

Standard keys are `session.name`, `session.storagePath`, `session.cpuCount`, `session.memoryMb` and `ui.defaultImage`.

### 3.2 audit_log (operation audit)

| Column | Type | Description |
|---|---|---|
| id | INTEGER PK AUTOINCREMENT | Primary key |
| ts | TEXT NOT NULL | Event time in ISO8601 format |
| category | TEXT NOT NULL | Category (image/container/session/settings/system) |
| action | TEXT NOT NULL | Action (pull/run/start/stop/restart/remove/delete/...) |
| detail | TEXT | Details such as object name or image reference |
| result | TEXT NOT NULL | Outcome (success/failure) |
| message | TEXT | Error message on failure |
| duration_ms | INTEGER | Duration in milliseconds |

### 3.3 pull_history (image pull history)

| Column | Type | Description |
|---|---|---|
| id | INTEGER PK AUTOINCREMENT | Primary key |
| image_ref | TEXT NOT NULL | Image reference |
| status | TEXT NOT NULL | Outcome (success/failure) |
| error | TEXT | Reason for failure |
| started_at | TEXT NOT NULL | Start time |
| finished_at | TEXT | Finish time |

### 3.4 container_snapshot (container state snapshots)

| Column | Type | Description |
|---|---|---|
| id | INTEGER PK AUTOINCREMENT | Primary key |
| name | TEXT NOT NULL | Container name |
| image | TEXT | Image |
| state | TEXT | State (running/exited/...) |
| status | TEXT | State description |
| ports | TEXT | Port mappings as text |
| observed_at | TEXT NOT NULL | Observation time |

Index: `(name, observed_at)`.

### 3.5 session_state (session state)

| Column | Type | Description |
|---|---|---|
| name | TEXT PK | Session name |
| storage_path | TEXT NOT NULL | Storage path |
| cpu_count | INTEGER | CPU quota |
| memory_mb | INTEGER | Memory quota |
| created_at | TEXT | Initial creation time |
| last_started_at | TEXT | Most recent start time |
| last_terminated_at | TEXT | Most recent termination time |

## 4. Repository interfaces

| Repository | Operations |
|---|---|
| SettingsRepository | Get(key)/Set(key,value)/GetAll() |
| AuditLogRepository | Add(entry)/Query(limit, category?)/Clear()/Count() |
| PullHistoryRepository | Add(entry)/Query(limit)/Count() |
| ContainerSnapshotRepository | Add(entry)/QueryLatest(limit)/ClearOlderThan(days) |
| SessionStateRepository | Upsert(state)/Get(name)/GetAll() |

## 5. Data flow

- Reads start at the page's Loaded event. The ViewModel calls a service (CLI/API) and binds the returned DTOs to the list. Audits, history and settings are read from the database.
- For writes, the user initiates an operation, a service executes it, WslcAuditService.Record records success or failure, and the UI refreshes.
- After a successful container list refresh, WslcSnapshotService.RecordContainer writes snapshots in batches. Retain the most recent N days.
