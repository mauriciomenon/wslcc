# ADR-0004. Store application data locally in SQLite with Microsoft.Data.Sqlite

Status: Accepted

## Context

The application needs to retain user settings (session name, storage path, and quotas), audit logs, image pull history, and container state snapshots. The candidates were SQLite, JSON files, and EF Core with SQLite.

## Decision

Store the database at `%LOCALAPPDATA%\WSLCC\wslcc.db`. Use Microsoft's ADO.NET provider, Microsoft.Data.Sqlite, behind repositories. Initialize the schema idempotently, enable WAL mode, and serialize writes at database level.

- A local application with little data and low concurrency benefits from SQLite's zero-deployment model.
- SQLite supports queries, aggregates such as counts, transactions, and future extensions beyond what plain JSON files provide.
- Using the provider directly avoids EF Core source-generator and migration overhead in WinUI/AOT and keeps dependencies small.
- WSLCC.Data is a separate project with one-way dependencies from App to Core to Data.

## Consequences

Data is durable, queryable, easy to back up as a single file, and readable by other tools. The team maintains its own schema and repository code, which is manageable at this scale.
