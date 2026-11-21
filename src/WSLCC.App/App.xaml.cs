using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Microsoft.Windows.AppLifecycle;
using WSLCC.Core.Services;
using Windows.UI;

namespace WSLCC_App;

public partial class App : Application
{
    private Window? _window;

    public static MainWindow? Main { get; private set; }

    private static readonly string LogPath = Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
        "WSLCC", "app-crash.log");

    public App()
    {
#if WSLCC_FRAMEWORK_DEPENDENT
        WslcUpdateService.InstallerAssetName = "WSLCC-Setup-framework.exe";
#else
        WslcUpdateService.InstallerAssetName = "WSLCC-Setup.exe";
#endif
        UnhandledException += (_, e) => WriteLog($"UnhandledException: {e.Exception}");
        AppDomain.CurrentDomain.UnhandledException += (_, e) => WriteLog($"AppDomain: {e.ExceptionObject}");
        TaskScheduler.UnobservedTaskException += (_, e) => WriteLog($"UnobservedTask: {e.Exception}");
    }

    public static void WriteLog(string message)
    {
        try
        {
            Directory.CreateDirectory(Path.GetDirectoryName(LogPath)!);
            File.AppendAllText(LogPath, $"[{DateTime.Now:O}] {message}{Environment.NewLine}");
        }
        catch
        {
        }
    }

    public static void ApplyTheme(string theme)
    {
        CurrentTheme = theme;

        var window = Main;
        if (window?.Content is FrameworkElement root)
        {
            root.RequestedTheme = theme switch
            {
                "dark" => ElementTheme.Dark,
                "light" => ElementTheme.Light,
                _ => ElementTheme.Default,
            };
        }
        window?.ApplyMiniPanelTheme(theme);
        window?.ApplyTrayTheme(theme);
    }

    public static string CurrentTheme { get; private set; } = "default";

    private const string SingleInstanceKey = "WSLCC.Main";

    private static AppInstance? _primaryInstance;

    protected override async void OnLaunched(Microsoft.UI.Xaml.LaunchActivatedEventArgs args)
    {
        if (!TryAcquireSingleInstance())
        {
            return;
        }

        try
        {
            await WslcHost.Default.InitializeAsync();
            L.SetLanguage(await WslcHost.Default.Settings.GetLanguageAsync());
        }
        catch (Exception ex)
        {
            WriteLog(L.GetFormat("App.Log.Init", ex));
        }
        Microsoft.Windows.Globalization.ApplicationLanguages.PrimaryLanguageOverride =
            L.ResolveLanguage(System.Globalization.CultureInfo.CurrentUICulture.Name).Name;
        InitializeComponent();
        _window = new MainWindow();
        Main = (MainWindow)_window;
        _window.Activate();
        _ = ApplySessionFromSettingsAsync();
        _ = RestoreContainersIfEnabledAsync();
        _ = AutoCheckUpdateAsync();
        _ = MigrateLegacyComposeAsync();
        _ = ApplyStartMinimizedAsync();
    }

    private static bool TryAcquireSingleInstance()
    {
        try
        {
            var instance = AppInstance.FindOrRegisterForKey(SingleInstanceKey);
            if (!instance.IsCurrent)
            {
                _ = instance.RedirectActivationToAsync(AppInstance.GetCurrent().GetActivatedEventArgs());
                Environment.Exit(0);
                return false;
            }

            _primaryInstance = instance;
            instance.Activated += OnPrimaryInstanceActivated;
            return true;
        }
        catch (Exception ex)
        {
            WriteLog(L.GetFormat("App.Log.Instance", ex));
            return true;
        }
    }

    private static void OnPrimaryInstanceActivated(object? sender, AppActivationArguments args)
    {
        Main?.DispatcherQueue.TryEnqueue(() => Main?.ShowAndActivate());
    }

    private async Task ApplyStartMinimizedAsync()
    {
        try
        {
            if (!await WslcHost.Default.Settings.GetStartMinimizedAsync().ConfigureAwait(false)) return;
            await Task.Delay(200).ConfigureAwait(false);
            Main?.DispatcherQueue.TryEnqueue(() => Main?.StartMinimizedToTray());
        }
        catch (Exception ex)
        {
            WriteLog(L.GetFormat("App.Log.Minimize", ex));
        }
    }

    private static async Task ApplySessionFromSettingsAsync()
    {
        try
        {
            var session = await WslcHost.Default.Settings.GetSessionConfigAsync().ConfigureAwait(false);
            WslcHost.Default.Runner.Session = await ResolveSessionNameAsync(session.Name).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            WriteLog(L.GetFormat("App.Log.Session", ex));
        }
    }

    private static async Task<string?> ResolveSessionNameAsync(string configuredName)
    {
        if (string.IsNullOrWhiteSpace(configuredName)) return null;
        try
        {
            var sessions = await WslcHost.Default.System.ListSessionsAsync().ConfigureAwait(false);
            if (sessions.Any(s => s.Name.Equals(configuredName, StringComparison.OrdinalIgnoreCase)))
                return configuredName;
            WriteLog(L.GetFormat("App.Log.SessionMissing", configuredName));
        }
        catch (Exception ex)
        {
            WriteLog(L.GetFormat("App.Log.SessionCheck", configuredName, ex));
        }
        return null;
    }

    private static async Task MigrateLegacyComposeAsync()
    {
        try
        {
            await Task.Delay(TimeSpan.FromSeconds(5)).ConfigureAwait(false);
            await WslcHost.Default.Compose.MigrateLegacyComposeProjectsAsync().ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            WriteLog(L.GetFormat("App.Log.Migrate", ex));
        }
    }

    private static async Task AutoCheckUpdateAsync()
    {
        try
        {
            var enabled = await WslcHost.Default.Settings.GetAutoCheckUpdateEnabledAsync().ConfigureAwait(false);
            if (!enabled) return;
            await Task.Delay(TimeSpan.FromSeconds(4)).ConfigureAwait(false);
            _ = WslcHost.Default.Update.CheckAsync();
        }
        catch (Exception ex)
        {
            WriteLog(L.GetFormat("App.Log.Update", ex));
        }
    }

    private static async Task RestoreContainersIfEnabledAsync()
    {
        try
        {
            var enabled = await WslcHost.Default.Settings.GetStartupContainersEnabledAsync().ConfigureAwait(false);
            if (!enabled) return;

            await Task.Delay(TimeSpan.FromSeconds(3)).ConfigureAwait(false);
            var snapshots = await WslcHost.Default.History.QueryLatestSnapshotsAsync().ConfigureAwait(false);
            foreach (var s in snapshots.Where(
                s => s.State?.Equals("running", StringComparison.OrdinalIgnoreCase) == true))
            {
                try
                {
                    await WslcHost.Default.Containers.StartAsync(s.Name).ConfigureAwait(false);
                }
                catch (Exception ex)
                {
                    WriteLog(L.GetFormat("App.Log.Container", s.Name, ex));
                }
            }
        }
        catch (Exception ex)
        {
            WriteLog(L.GetFormat("App.Log.Restore", ex));
        }
    }
}
