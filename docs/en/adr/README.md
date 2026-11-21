# Architecture decision records

This directory records the main architecture decisions for WSLCC, the WSLC container manager. Each record contains its status, context, decision, and consequences.

| ADR | Topic | Status |
|---|---|---|
| 0001 | [Build the Windows interface with WinUI 3 and the Windows App SDK](0001-use-winui3-windows-app-sdk.md) | Accepted |
| 0002 | [Implement MVVM with CommunityToolkit.Mvvm and partial properties](0002-mvvm-communitytoolkit-partial-property.md) | Accepted |
| 0003 | [Use the wslc CLI as the primary backend and retain the native API](0003-dual-channel-backend.md) | Accepted |
| 0004 | [Store application data locally in SQLite with Microsoft.Data.Sqlite](0004-sqlite-local-storage.md) | Accepted |
| 0005 | [Manage one session per process with explicit start and stop](0005-session-lifecycle.md) | Accepted |
| 0006 | [Accept alternative field names when parsing CLI JSON output](0006-cli-json-parsing-strategy.md) | Accepted |
| 0007 | [Build with the dotnet CLI and avoid the XamlCompiler bug on Chinese Windows](0007-msbuild-build-strategy.md) | Accepted |
| 0008 | [Deploy with WindowsAppSDKSelfContained and WindowsPackageType=None](0008-self-contained-deployment.md) | Accepted |
| 0009 | [Support wslc features introduced in 2.9.8 through 2.9.12](0009-sync-official-wslc-2.9.8-2.9.12.md) | Accepted |
| 0010 | [Add a mini container panel next to the system tray](0010-mini-container-panel.md) | Accepted |
| 0011 | [Support digests, all-tags, follow-link, and events in wslc 2.9.13](0011-sync-official-wslc-2.9.13.md) | Accepted |
| 0012 | [Adopt the generally available WSL 3.0.1 container runtime](0012-sync-official-wslc-3.0.1-ga.md) | Accepted |
| 0013 | [Ship self-contained and framework-dependent editions](0013-dual-release-editions.md) | Accepted |
| 0014 | [Diagnose 0x800701c0 when the wslc data directory is a reparse point](0014-wslc-reparse-point-redirection-guard.md) | Accepted |
