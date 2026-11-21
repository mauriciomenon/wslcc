<div align="center">
  <img src="src/WSLCC.App/Assets/AppIcon.png" alt="WSLCC" width="120" />
  <h1>WSLCC</h1>
  <p>A Windows desktop manager for WSL containers (wslc)</p>
  <p>Manage images, containers, Compose projects, volumes and logs through Microsoft's WSL Container API and the wslc CLI.</p>
</div>

[Portugues (Brasil)](README.md) | [English](README.en.md) | [简体中文](README.zh-CN.md)

## Interface language

In this fork, open **Settings > Appearance > Language** to follow the system language or choose English, Portuguese (Brazil) or Simplified Chinese. Quit WSLCC completely, including the tray icon, then reopen it to apply the change.

Project documentation is available in [Chinese](docs), [English](docs/en) and [Portuguese (Brazil)](docs/pt-BR).

## Features

- **Dashboard.** Check the environment status, resource quotas and active sessions. Open a session terminal with one click.
- **Containers.** List containers with automatically retrieved website icons, inspect resource usage and details, and start, stop, restart or remove containers. Open exposed ports in a browser, launch a terminal and copy files with the option to follow symbolic links. View logs by tail length, timestamp or time range. Create containers with volumes, mounts, environment variables, health checks, network aliases and resource limits.
- **Compose.** Deploy, edit, rebuild, stop and remove projects. Follow each deployment step in the progress view and logs. Reuse local images or pull the latest versions.
- **Images.** Inspect images, digests and intermediate layers. View pull history, pull all tags, remove individual images or prune unused ones.
- **Volumes.** Manage mounted volumes and prune unused ones.
- **Networks.** List, inspect, create and remove networks. Connect or disconnect containers and prune unused networks.
- **Events.** Look up past container and network lifecycle events or watch them live.
- **Cleanup.** Prune containers, images, volumes and networks with one click.
- **Logs and auditing.** View container logs and the audit trail for operations.
- **Mini panel.** Left-click the tray icon to open a panel in the lower-right corner. Check container status, start, stop or restart containers, open exposed ports and start or stop the wslc session. Swipe down to dismiss it.
- **Error reporting.** Copy diagnostic information and open a new GitHub issue with one click.
- **Settings.** Configure the session in the official wslc `settings.yaml`, set an image registry mirror, choose the theme and background material, or start minimized to the tray. Settings are grouped in Windows 11-style cards.
- **Memory usage.** Hiding WSLCC in the tray releases the main window's pages and can reduce its working set to about 40 MB.

## Requirements

- Windows 10 or 11 (x64).
- Microsoft's WSL Containers CLI, now generally available. Update WSL with `wsl --update`. [WSL Containers documentation](https://github.com/MicrosoftDocs/wsl/blob/main/WSL/wsl-container.md)
- .NET 10 SDK for development builds.
- The framework-dependent edition also requires .NET 10 Desktop Runtime and Windows App Runtime 2.4. The self-contained edition includes its runtimes.

## Build

```powershell
powershell -ExecutionPolicy Bypass -File build.ps1
```

The first build downloads the `Microsoft.WSL.Containers` SDK package to `nuget-packages/`. Microsoft distributes this package exclusively through GitHub releases for microsoft/WSL. You can also run `scripts/fetch-wslc-sdk.ps1` manually.

## Packaging and releases

- **Self-contained.** This larger edition includes the .NET runtime and runs after extraction.
- **Framework-dependent.** This smaller edition uses the .NET 10 Desktop Runtime and Windows App Runtime 2.4 installed on the system.

```powershell
# Self-contained (default)
powershell -ExecutionPolicy Bypass -File publish.ps1

# Framework-dependent
powershell -ExecutionPolicy Bypass -File publish.ps1 -Framework
```

Each command produces a ZIP archive containing a single application directory. Extract it and run WSLCC. Add `-InnoSetup` to also build the installer for that edition.

Pushing a tag matching `v*` triggers the GitHub Actions workflow that builds and creates a release with these files.

| Edition | ZIP archive | Installer |
| --- | --- | --- |
| Self-contained | `WSLCC-win-x64.zip` | `WSLCC-Setup.exe` |
| Framework-dependent | `WSLCC-win-x64-framework.zip` | `WSLCC-Setup-framework.exe` |

The built-in updater detects new versions, downloads the package for the installed edition and replaces the current version.

## Contributors

- Application icon design by [lemonno2333](https://github.com/lemonno2333).
- Project development by [DSUK](https://github.com/iwvw).

## Community

- Technology discussions on [Linux.do](https://linux.do).

## License

WSLCC uses the [MIT License](LICENSE).
