# ADR-0010. Add a mini container panel next to the system tray

Status: Accepted

## Context

Starting and stopping containers is frequent. Switching back to the large main window just to check a state or control a container takes extra steps. A lightweight resident panel near the tray can display status and offer start, stop, and restart actions.

## Decision

Add a separate `MiniWindow` (380 by 540 DIP) in the lower-right corner of the work area, inset by 12 DIP. The tray icon controls its visibility.

- A separate window has its own simple state machine. Its frequent, lightweight interactions differ from those in the full main window. Animation affects only the panel, which can stay initialized between uses.
- Only tray actions toggle visibility. Hiding on focus loss creates a race because clicking a taskbar icon deactivates the window before the click toggles it. Disabling auto-hide removes the race and makes behavior predictable.
- Put the window in the topmost group immediately below the taskbar with `SetWindowPos(hwnd, Shell_TrayWnd, ...)`. It slides out from behind the taskbar and other windows cannot obscure it. Call `PlaceBelowTaskbar` after `BringToForeground`.
- `Window` lacks built-in show/hide animation. Use per-frame `SetWindowPos` calls on one resident worker thread. A generation snapshot lets a new request invalidate an animation in progress. Each frame uses `SWP_NOZORDER` to keep a stable stacking order without flicker. Expanding omits fade-in, which would hide movement and make the panel appear suddenly.
- A custom `SystemBackdrop` keeps `IsInputActive` true for `DesktopAcrylicController`. The built-in `DesktopAcrylicBackdrop` becomes solid when the window loses the foreground. Use a solid background when Mica is disabled.
- Reuse `IWslcContainerService` for container lists and `stats`. Poll resource consumption every three seconds. Each row offers start, stop, restart, and an Open action if ports are mapped. The resident panel is independent of main-window navigation.
- By default, left-clicking the tray icon toggles the panel. Users can disable this in Settings to restore the main-window action. The tray context menu can also show or hide the panel. Persist the preference under `app.trayMiniPanel`, enabled by default.

## Consequences

One tray click gives access to status and controls. The panel's animation, stacking order, and materials can evolve independently. The extra resident window consumes resources. Positioning depends on the long-standing `Shell_TrayWnd` implementation detail. If the taskbar cannot be found, the panel falls back to ordinary topmost behavior. Secondary taskbars use `Shell_SecondaryTrayWnd`, so the current positioning uses only the primary display's work area.

## Related records

- [ADR-0001. WinUI 3 and the Windows App SDK](0001-use-winui3-windows-app-sdk.md)
- [ADR-0002. CommunityToolkit.Mvvm](0002-mvvm-communitytoolkit-partial-property.md)
