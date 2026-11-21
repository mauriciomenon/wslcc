# Acceptance criteria

## 1. Build

- AC-01. Build with Visual Studio MSBuild (amd64) using `MSBuild.exe WSLCC.App.csproj -p:Platform=x64`. The build finishes with zero errors and zero warnings.
- AC-02. Build with WindowsAppSDKSelfContained and WindowsPackageType=None. The executable runs directly on a machine without Windows App Runtime installed.

## 2. Runtime

- AC-03. On startup the main window displays its Mica background, NavigationView and default Dashboard page. The process continues to run without crashing.
- AC-04. The dashboard displays wslc, kernel and Windows versions, missing components, active sessions and API availability.
- AC-05. Images lists local images when `wslc image ls` is available. The pull field and button are usable and a failed pull shows a readable error.
- AC-06. Containers lists containers when wslc is available. The creation dialog opens with fields for name, image, ports, environment variables, automatic removal and background execution. Start, stop and delete button states follow the selected item.
- AC-07. In Logs, select a container to inspect `wslc logs` output. A missing or unavailable container produces a readable error.
- AC-08. Activity displays audit logs and pull history from SQLite.

## 3. Persistence

- AC-09. Change the session name, storage path and quotas in Settings and save. Verify that the values persist after restarting the app.
- AC-10. Each pull, create, start, stop or delete operation produces an audit record with category, action, result and duration.
- AC-11. When a pull completes, its image reference, status and time appear in pull history.
- AC-12. `%LOCALAPPDATA%\WSLCC\wslcc.db` exists and is readable.

## 4. Robustness

- AC-13. In an environment where wslc commands return E_UNEXPECTED, the app continues running and shows readable errors for the affected operations.
- AC-14. When direct Docker Hub access is unavailable because TLS is blocked, a failed pull only affects that operation. Other pages remain usable.
- AC-15. The app writes global exceptions to `%LOCALAPPDATA%\WSLCC\app-crash.log`.

## 5. Design

- AC-16. The UI uses standard WinUI controls throughout, without custom-drawn components such as Path or Canvas.
- AC-17. All ViewModels use the partial-property form of [ObservableProperty] and produce zero MVVMTK0045 warnings.
- AC-18. WSLCC.Data owns database access and WSLCC.Core owns services. App does not access the database directly.
