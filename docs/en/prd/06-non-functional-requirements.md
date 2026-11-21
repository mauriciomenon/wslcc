# Nonfunctional requirements

## 1. Performance

- With local data, the main window must become interactive within 3 seconds of process startup.
- Refresh the image or container list within 2 seconds. The wslc command accounts for most of this time, and the UI must remain responsive.
- Execute long-running operations (pull and run) asynchronously and show progress while the UI remains responsive.
- Database operations should take milliseconds and must never block the UI thread.

## 2. Reliability

- Handle CLI and API exceptions consistently and display readable messages. These exceptions must never crash the app.
- SQLite must preserve settings and history in full across app restarts.
- When wslc is unavailable because the process is missing, its version is too old or a VM session is faulty, users can still browse database content.
- Support operation without a network connection. If the image registry is unreachable, only the affected operation fails.

## 3. Security

- Do not store credentials. Sensitive operations such as registry login are outside the scope of v1.
- Keep sensitive arguments out of audit logs. For port mappings and environment variables, keys can be recorded without their values.
- Store the database in the user's local configuration directory. Keep it outside the installation directory.

## 4. Maintainability

- Keep dependencies one-way from App (UI) to Core (services) to Data (storage).
- Provide service interfaces (IWslc*Service) so implementations can be replaced and unit tested.
- Eliminate all compiler warnings. Use partial properties to address MVVMTK0045.
- Provide a repeatable, scriptable build command as described in [ADR-0007](../adr/0007-msbuild-build-strategy.md).

## 5. Observability

- Write unhandled global exceptions to `%LOCALAPPDATA%\WSLCC\app-crash.log`.
- Audit all writes, including category, action, details, result and duration.
- Keep a traceable image pull history with reference, status, start and finish times and error.
- Show the data channel (native API availability) on the dashboard so users can diagnose differences in functionality.

## 6. Compatibility

- Target Windows 11 for runtime. The minimum target platform is 10.0.17763.
- Accept alternative JSON field names when wslc output changes so a single missing field does not invalidate the entire result.
- The WSL preview API showed instability in local tests. Use the CLI as the primary channel to avoid E_UNEXPECTED in the native session API.

## 7. Localization

- The original specification calls for Simplified Chinese UI text.
- Preserve original wslc error codes such as E_UNEXPECTED so users can look up corresponding issues in the official repository.
