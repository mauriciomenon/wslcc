# ADR-0002. Implement MVVM with CommunityToolkit.Mvvm and partial properties

Status: Accepted

## Context

WinUI 3 pages must bind state and commands to their views. Implementing INotifyPropertyChanged and ICommand by hand is verbose and error-prone. The project needs Microsoft's recommended MVVM toolkit and compatibility with CsWinRT ahead-of-time (AOT) compilation.

## Decision

Use CommunityToolkit.Mvvm 8.4 and declare each `[ObservableProperty]` as a partial property (`public partial string X { get; set; }`).

- Warning MVVMTK0045 explains that field-based observable properties in WinUI 3 generate code incompatible with AOT. Partial properties let the CsWinRT source generator produce the correct WinRT marshaling code.
- Use RelayCommand and AsyncRelayCommand for commands. Asynchronous commands support CanExecute updates and await.

## Consequences

The implementation builds without compiler warnings, retains type safety, and needs much less boilerplate. Partial properties require C# 13 or later, which net10.0 supports.
