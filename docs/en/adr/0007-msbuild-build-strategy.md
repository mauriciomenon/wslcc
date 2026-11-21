# ADR-0007. Build with the dotnet CLI and avoid the XamlCompiler bug on Chinese Windows

Status: Accepted

## Context

On the local Chinese Windows system (.NET 10 SDK and Windows App SDK 2.4.0/WinUI 2.3.6), `dotnet build` invokes `XamlCompiler.exe` as a net472 subprocess (`UseXamlCompilerExecutable=true`). During Pass2, processing XamlTypeInfo raises WMC9999 (NullReference, "Object reference not set to an instance of an object") together with WMC1509 (No LocalAssembly). The issue is related to microsoft/microsoft-ui-xaml#11157: XamlCompiler resource names do not match on non-English systems. Running in-process with `UseXamlCompilerExecutable=false` instead produces MSB4027 because MetadataLoadContext has been disposed.

Only local builds on Chinese Windows exhibited the problem. The English GitHub Actions windows-latest runner completes XAML Pass2 with `dotnet build`. Builds now use the dotnet CLI without requiring the full Visual Studio MSBuild installation.

## Decision

- Use `dotnet build src/WSLCC.App/WSLCC.App.csproj -c <Configuration> -p:Platform=<Platform> -p:RuntimeIdentifier=win-<Platform>`.
- The supported build paths are `build.ps1` and the CI workflow at `.github/workflows/build.yml`. They use `dotnet build` and `dotnet publish`.
- Run CI on English windows-latest with the .NET 10 SDK to avoid WMC9999.

## Consequences

Builds need no Visual Studio installation. CI on the English runner passes, and local developers need only the .NET 10 SDK. If building directly on Chinese Windows triggers WMC9999, build in an English environment or wait for an upstream fix.
