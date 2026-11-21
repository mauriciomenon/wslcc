# ADR-0001. Build the Windows interface with WinUI 3 and the Windows App SDK

Status: Accepted

## Context

WSLC needs a native Windows desktop controller. The candidates were WinUI 3, WPF, and Tauri/Electron. The application must follow Microsoft's design guidelines, use standard controls without custom drawing, and integrate closely with native wslc features.

## Decision

Use WinUI 3 (Windows App SDK 2.4.0) and .NET 10.

- Microsoft promotes WinUI 3 for native Windows interfaces. It follows Fluent Design and includes Mica, rounded corners, and theme resources.
- Standard controls such as NavigationView, ListView, InfoBar, and ContentDialog meet the requirement to avoid hand-drawn components.
- WinUI and Microsoft.WSL.Containers (the WinRT API) share the Microsoft ecosystem and support straightforward type interoperability.
- Self-contained deployment removes the need to install the runtime separately.

## Consequences

The interface follows Windows 11 visual and interaction conventions. Developers can work efficiently with a framework Microsoft maintains. The framework supports only Windows, which fits the product's purpose. On Chinese Windows, a known XAML compiler bug can affect local builds using `dotnet build`. See [ADR-0007](0007-msbuild-build-strategy.md) for the workaround.
