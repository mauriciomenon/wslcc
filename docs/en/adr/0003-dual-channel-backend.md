# ADR-0003. Use the wslc CLI as the primary backend and retain the native API

Status: Accepted

## Context

wslc exposes a Docker-style CLI (`wslc.exe` with `--format json`) and a native API (the C# projection in the Microsoft.WSL.Containers NuGet package). Local tests in September 2026 successfully started the API Session, but operations such as GetImages consistently threw COMException E_UNEXPECTED (0x8000FFFF). The CLI's info and image ls commands worked, while container and volume commands intermittently returned E_UNEXPECTED. The preview runtime was unstable in that environment, with interference from the VPN or network (microsoft/WSL#41082).

## Decision

- Use `wslc.exe` subprocesses and JSON parsing by default for reads and writes from the interface.
- Retain `WslcApiHost` as the alternative. It wraps environment detection, sessions, images, and progress events for future use in suitable environments.
- The environment service reports `ApiAvailable` and the dashboard shows the current channel.

The official CLI provides a stable programmatic interface with Docker-style syntax and machine-readable output. It also covers operations such as list, logs, and stats that the API lacked. Using the preview API by default would compromise availability. Keeping the two interfaces separate allows a future change without spreading preview-specific behavior throughout the application.

## Consequences

The application works reliably where the CLI is available and can switch when the API stabilizes. CLI parsing depends on field names, so the parser accepts several variants. Starting a subprocess adds a small overhead that is negligible for desktop interaction.

## Review on 2026-09-29

WSL 3.0.1 reached general availability on 2026-09-29, including containers and the API. The official announcement removed all preview labels. After upgrading the local WSL and wslc installation to 3.0.1, the network-related E_UNEXPECTED error no longer reproduced. The CLI remains the primary interface for reads and writes.

The SDK was updated to 3.0.1 in [ADR-0012](0012-sync-official-wslc-3.0.1-ga.md). No application workflow currently calls `WslcApiHost`. The GA API can be evaluated when type safety or event-driven progress reporting warrants it.
