# ADR-0008. Deploy with WindowsAppSDKSelfContained and WindowsPackageType=None

Status: Accepted

## Context

The template defaults to a packaged MSIX application. On a machine without Windows App Runtime, launching the executable directly caused DeploymentManager initialization to fail. `DeploymentInitializeOptions..ctor()` threw COMException 0x80040154 (class not registered), crashing the app.

## Decision

- Set `<WindowsAppSDKSelfContained>true</WindowsAppSDKSelfContained>` in the project to distribute the Windows App SDK runtime with the application.
- Set `<WindowsPackageType>None</WindowsPackageType>` to declare an unpackaged application and skip MSIX deployment and automatic runtime installation.
- Launch the built executable directly from `bin\x64\Debug\net10.0-windows10.0.26100.0\win-x64\WSLCC.App.exe`.
- Retain the win-x64, arm64, and x86 PublishProfiles for a future MSIX release.

## Consequences

The application can launch directly, supports portable distribution, and is simple to verify in CI. Bundling the runtime increases the output size. In unpackaged mode, some system integrations, such as dark-mode notifications, depend on the Windows App SDK managing them.
