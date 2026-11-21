# ADR-0012. Adopt the generally available WSL 3.0.1 container runtime

Status: Accepted

## Context

WSL 3.0.1 shipped on 2026-09-29. WSL containers (wslc) and the WSL Containers API reached general availability. The announcement removed preview labels (microsoft/WSL#41704) and documented `wsl --update` as the installation path. Previous 2.9.x releases in the repository were marked as prereleases.

Changes since the 2.9.13 baseline:

1. `wsl --update` installs or upgrades the stable release. `container.exe` is a built-in alias for `wslc.exe` with the same commands.
2. New commands include `wslc system info` (alias of `info`, showing client/server versions and active sessions), `wslc container restart` (with `-s/--signal` and `-t/--timeout`), and `wslc settings reset`.
3. Container create/run parameters include `--mount`, `--env-file`, `--domainname`, `--network-alias`, `--ip`, `-P/--publish-all`, `--no-healthcheck`, `--dns-option`, `--dns-search`, `--cidfile`, `-i/--interactive`, and `-t/--tty`. Most already appeared in 2.9.13's `ArgumentDefinitions.h`, but had not been exposed in the manager.
4. Network creation accepts `-o/--opt` driver options and `-l/--label`. The manager already supported them in v1.2.0.
5. Intune adds controls to allow WSL containers and allowlist image registries. The Defender for Endpoint plugin now covers container activity.
6. Access to Windows files from Linux is up to twice as fast. The release adds the `consomme` network mode.

## Decision

- Upgrade `Microsoft.WSL.Containers` from 2.9.13 to 3.0.1 and update the SDK fetch script's default.
- Change `WslcInstallService.RunInstallOrUpgradeAsync` from `wsl --update --pre-release` to `wsl --update`. Explain in the prompt that this updates the stable container components.
- Update the README, Settings guidance, Events unavailable message, and `WslcRunner` missing-executable message to refer to the stable version and `wsl --update`. Keep ADR-0009 and ADR-0011 as historical records and update the review section in ADR-0003.
- Extend the creation form and `ContainerCreateOptions` with `--mount`, `--env-file`, `--domainname`, `--network-alias`, `-P/--publish-all`, and `--no-healthcheck`. Reuse the Expander with TextBox/CheckBox controls for advanced options.
- `WslcInstallService.GetStatusAsync` already selects the highest version across release tags, so it detects the stable version (3.0.1 > 2.9.x).
- `WslcCapabilities` keeps 2.9.13 as the minimum for existing features. Version 3.0.1 meets that requirement.

## Consequences

Distribution and updates use the official stable channel. The creation form covers the main parameters, and users are directed to the stable upgrade command.

Intune, MDE, registry allowlists, and `consomme` are platform or administration concerns. The manager can pass through `network create -o` as needed. It continues to call `wslc.exe` for compatibility with earlier versions.

Upstream plans native `wslc compose`. The manager currently parses YAML and runs `wslc run` for each container. A separate ADR will evaluate whether to adopt the native command once available.
