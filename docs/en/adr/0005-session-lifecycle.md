# ADR-0005. Manage one session per process with explicit start and stop

Status: Accepted

## Context

Tests showed that only one session with a given name can be active. Calling `new Session(name).Start()` a second time throws COMException 0x800700B7 (file already exists). After Terminate, the same name can be started again, allowing reuse across application restarts. The native API cannot enumerate or reuse sessions. Incorrect handling can cause conflicts or leak resources.

## Decision

- `WslcApiHost` creates a single Session per process. SemaphoreSlim serializes access.
- `EnsureSessionAsync` starts the session idempotently. `TerminateSessionAsync` explicitly terminates it and releases the VM.
- Exiting the app leaves the session running to preserve container and VM state. The user can manage sessions from the interface or with `wslc system session terminate`. The dashboard lists active sessions for manual cleanup.
- The database's AppSettings persists the session name, storage path, CPU, and memory settings.

## Consequences

Session names do not collide, containers survive application restarts, and users can see and control VM resource consumption. Long-lived sessions consume memory and require idleTimeout or manual management.
