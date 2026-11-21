# ADR-0013. Ship self-contained and framework-dependent editions

Status: Accepted

## Context

[ADR-0008](0008-self-contained-deployment.md) selected `WindowsAppSDKSelfContained=true` and `WindowsPackageType=None` for a ready-to-run, single-directory distribution. The result is about 100 MB, mostly bundled .NET and Windows App SDK runtimes. Users who already have the runtimes want a smaller download. Users without the runtimes still need a ready-to-run default.

## Decision

Build both editions for the same version and distinguish them by filename.

| Edition | Requirements | ZIP | Installer |
|---|---|---|---|
| Self-contained | Includes the .NET runtime and runs out of the box | `WSLCC-win-x64.zip` | `WSLCC-Setup.exe` |
| Framework-dependent | Requires installed .NET 10 Desktop Runtime and Windows App Runtime 2.4 | `WSLCC-win-x64-framework.zip` | `WSLCC-Setup-framework.exe` |

- The self-contained edition retains the original unsuffixed filenames so existing download links and automatic updates keep working. The framework-dependent edition uses `-framework`.
- The `WSLCCFrameworkDependent` project property defaults to `false`. When true, set `WindowsAppSDKSelfContained=false` and define `WSLCC_FRAMEWORK_DEPENDENT`.
- Automatic updates use the compile-time constant to select the installer matching the installed edition.
- The framework-dependent installer checks for both required runtimes before installation. If one is missing, it prompts the user and opens the download page.
- `publish.ps1` adds a `-Framework` switch. CI builds both editions and uploads four files.

## Consequences

Users choose between download size and bundled runtimes. The self-contained edition remains the default. CI build and upload time doubles, and both sets of filenames and update mappings require maintenance. The framework-dependent application cannot start without its runtimes, so the installer checks and guides the user.
