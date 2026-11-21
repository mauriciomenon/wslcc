# ADR-0009. Support wslc features introduced in 2.9.8 through 2.9.12

Status: Accepted

## Context

The official 2.9.8 through 2.9.12 WSL prereleases added wslc features and improved Docker compatibility. The WSL Containers API was still in preview, with general availability planned for fall 2026. The project had two problems:

1. `WslcInstallService` checked `/releases/latest`, which returns the latest stable release (then 2.7). Releases containing wslc were 2.9.x prereleases, so users could never see the updates to 2.9.10 or later.
2. The one-click upgrade ran `winget install --id Microsoft.Windows.WSL`, which installs the stable version. The official command to reach the 2.9.x prerelease channel was `wsl --update --pre-release`.

The manager also lacked the global `--session` option, session fields in `settings.yaml`, network management, prune operations, and new container creation options.

## Decision

- Fetch the releases list from `microsoft/WSL` with `per_page=40` and select the highest version across all tags, including prereleases.
- Launch `wsl --update --pre-release` with administrator privileges for upgrades.
- Upgrade `Microsoft.WSL.Containers` from 2.9.9 to 2.9.12. The package ships only with GitHub Releases and is unavailable on nuget.org. Build, publish, and CI use `scripts/fetch-wslc-sdk.ps1` to populate the local `nuget-packages/` source.
- Add `WslcSettingsFileService` to read and write the official `%LOCALAPPDATA%\wslc\settings.yaml`, preserving comments and formatting. It handles `cpuCount`, `memorySize`, `maxStorageSize`, `storagePath`, `defaultBindingAddress`, `hostLoopback`, `idleTimeout`, and `credentialStore`. Allow `WslcRunner` to pass the global `--session` option.
- Validate the session name on startup and when saving settings with `system session list`. Tests showed that `wslc --session <missing-name>` makes every operation fail with `WSLC_E_SESSION_NOT_FOUND`. Log the problem and fall back to the default wslc session if the name does not exist.
- Add network list/create/rm/connect/disconnect/prune/inspect, prune for containers/images/volumes/networks, `container cp`, 19 new creation parameters, `inspect -s`, `list --size`, `image ls --all`, and the logs options `-n`, `--timestamps`, `--details`, `--since`, and `--until`.

## Consequences

Detection and upgrades follow the official prerelease channel. Session settings take effect through the official file. Network management, cleanup, and the main creation parameters are available in the interface.

The YAML writer uses official comments to locate the keys. If the wording changes, it appends the key and value to the end of the relevant section, preserving valid YAML and behavior. Prune displays raw CLI output because the formats differ between resource types and do not provide uniform counts.
