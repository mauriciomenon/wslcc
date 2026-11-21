using System.Globalization;
using System.Resources;

namespace WSLCC_App;

public static class L
{
    private static readonly ResourceManager Strings = new("WSLCC.Core.Localization.Strings", typeof(L).Assembly);
    private static CultureInfo _language = ResolveLanguage(CultureInfo.CurrentUICulture.Name);

    public static CultureInfo ResolveLanguage(string language) =>
        CultureInfo.GetCultureInfo(language.Split('-')[0].ToLowerInvariant() switch
        {
            "pt" => "pt-BR",
            "zh" => "zh-Hans",
            _ => "en-US",
        });

    public static void SetLanguage(string preference)
    {
        _language = ResolveLanguage(preference == "system" ? CultureInfo.CurrentUICulture.Name : preference);
        CultureInfo.DefaultThreadCurrentUICulture = _language;
        CultureInfo.CurrentUICulture = _language;
    }

    public static string Get(string key) => Strings.GetString(key, _language) ?? key;

    public static string GetFormat(string key, params object?[] args) =>
        string.Format(CultureInfo.CurrentCulture, Get(key), args);
}
