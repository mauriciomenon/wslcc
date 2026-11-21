# ADR-0014. Diagnose 0x800701c0 when the wslc data directory is a reparse point

Status: Accepted

## Context

Some users move `%LOCALAPPDATA%` or a subdirectory to another drive, leaving `%LOCALAPPDATA%\wslc` as a junction. wslc 2.9.13 and later use RedirectionGuard. In enforcement mode it refuses to traverse untrusted mount points, so wslc fails to read `settings.yaml` or session storage. The reported error translates as follows:

```text
The path cannot be traversed because it contains an untrusted mount point.
Error code: 0x800701c0
```

A minimal test confirmed that reading a file through a junction with enforcement enabled consistently throws `IOException` (0x800701c0). Audit mode and disabling the policy did not produce the error. `wslc.exe` imports `SetProcessMitigationPolicy` and uses this path. Child processes inherit the parent's RedirectionGuard. Testing confirmed that `CreateProcess` mitigation attributes cannot relax an enforced parent policy.

## Decision

Run diagnostics on startup and give targeted guidance:

- `WslcPathDiagnostics.DetectUntrustedReparsePoints` checks `%LOCALAPPDATA%\wslc` and the default storage path for junctions or symbolic links.
- On the home page, add guidance when an error contains `800701c0` or mentions an untrusted mount point, or when the startup check finds a reparse point.
- Show a path policy warning InfoBar on the Containers page listing the affected paths.
- Recognize `0x800701c0` separately. Explain how to select a real storage directory or restore `%LOCALAPPDATA%\wslc` as an ordinary directory.
- Avoid flooding the log with the same stats polling error by recording it once.

## Consequences

Users see the problem and its remedy directly, without having to search through official WSL issues. The application preserves their directory layout. Users must adjust the path themselves. The app cannot automatically work around a parent's enforced RedirectionGuard policy through `CreateProcess`.
