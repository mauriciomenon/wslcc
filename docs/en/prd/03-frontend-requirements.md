# Frontend functional requirements (WinUI 3)

## 1. Design guidelines

- Follow Fluent Design and the Windows 11 design language. Use Mica backgrounds, system text styles (Title/Subtitle/BodyStrong/CaptionTextBlockStyle), rounded cards (Border with CardBackgroundFillColorDefaultBrush), InfoBar, ProgressRing/ProgressBar, ContentDialog and NavigationView.
- Use standard controls and built-in theme resources for all visuals. Custom drawing with Path, Canvas, Polyline or hand-drawn control templates is prohibited.
- Use standard WinUI controls for all interactions, including NavigationView, Frame, ListView, TextBox, CheckBox, Button, InfoBar, ProgressBar, ContentDialog and TextBlock.
- Follow MVVM. Bind each page's DataContext to its ViewModel. Use RelayCommand/AsyncRelayCommand for commands and ObservableObject/[ObservableProperty] for data changes. Use partial properties in WinUI for CsWinRT AOT compatibility.
- Load page data on Loaded. Drive feedback for asynchronous operations through ObservableProperty state, such as IsLoading, IsPulling and IsRunning.

## 2. Navigation

| Item | Page | Icon | Contents |
|---|---|---|---|
| Dashboard | HomePage | E80F (Home) | Environment, data channel and session list |
| Containers | ContainersPage | E7F4 | List, create, start, stop and delete containers |
| Images | ImagesPage | EB9F | List, pull and delete images |
| Logs | LogsPage | E7C3 (Page/Script) | Live logs for the selected container |
| Activity | ActivityPage | E823 (History) | Audit log and pull history |
| About | AboutPage | EA3A | Version and description |
| Settings (built-in entry, IsSettingsSelected) | SettingsPage | E713 | Session settings and database details |

## 3. Page requirements

### 3.1 HomePage (Dashboard)

- The header shows the page title, Refresh button and loading indicator.
- The environment card shows the wslc, Linux kernel, Windows and session manager versions, active session count, missing components, native API availability and settings file path.
- The session card lists active sessions from `wslc info` with names and IDs.
- Show a readable error in the InfoBar if loading the environment fails.

### 3.2 ContainersPage (Containers)

- The header contains the title, Refresh and New (ContentDialog).
- The selected container's toolbar shows its name and buttons to start, stop, restart and remove it. Each command's CanExecute follows the container state.
- The ListView shows a state icon (play for running, square for stopped), name, image, Status and ports.
- The creation dialog uses standard controls for name, image, comma-separated `host:container` port mappings and comma-separated `KEY=VALUE` environment variables. Provide CheckBox controls for automatic removal and background execution.
- Refresh the list after row or selection actions. Record failures in the audit log and display an InfoBar.

### 3.3 ImagesPage (Images)

- The header contains the title, Refresh and Delete selected.
- The pull row has a TextBox with a hint such as "Image name, e.g. nginx:latest" and a Pull button.
- The progress area has an indeterminate ProgressBar and status text displaying live wslc pull output line by line.
- The ListView shows the image name, truncated ID, size and creation time.
- After successful deletion, remove the entry and refresh the list.

### 3.4 LogsPage (Logs)

- The header contains the title, a container ComboBox and Refresh.
- Display `wslc logs` output in a read-only TextBox. Disable TextWrapping, set VerticalScrollBarVisibility=Auto, use a monospaced font and set the standard IsReadOnly property to true.
- Show a readable InfoBar error if the container is stopped or its logs are unavailable.

### 3.5 ActivityPage (Activity)

- The header contains the title and Refresh.
- The audit ListView shows time, category, action, result (success or failure) and duration.
- The pull history ListView shows image reference, total amount, status, start time and finish time.
- Both lists show local SQLite data from `WslcAuditService` and `WslcHistoryService`.

### 3.6 SettingsPage (Settings)

- The session card shows the session name, storage path and CPU and memory quotas. Load the form from AppSettings, write changes to the database and confirm that the save succeeded.
- The data channel card shows native API availability and the current channel.
- The database card shows the database path, audit entry count and pull history count.
- The button for clearing the audit log opens a confirmation ContentDialog.

## 4. Visual constraints

- Get colors, corner radii and spacing from system theme resources. Do not hardcode color values in pages.
- Use FontIcon with standard Segoe Fluent Icons code points for every icon. Do not use images or custom vectors.
