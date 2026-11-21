using System.Globalization;

namespace WSLCC_App;

public static class L
{
    private static IReadOnlyDictionary<string, string> Table = AppStrings.En;

    public static void SetLanguage(string preference)
    {
        var language = preference == "system" ? CultureInfo.CurrentUICulture.TwoLetterISOLanguageName : preference;
        var (tag, strings) = language switch
        {
            "zh" or "zh-Hans" => ("zh-Hans", AppStrings.Zh),
            "pt" or "pt-BR" => ("pt-BR", AppStrings.PtBr),
            _ => ("en-US", AppStrings.En)
        };
        Table = strings;
        Microsoft.Windows.Globalization.ApplicationLanguages.PrimaryLanguageOverride = tag;
    }

    public static string Get(string key)
    {
        return Table.TryGetValue(key, out var value) && value.Length > 0 ? value : key;
    }

    public static string GetFormat(string key, params object?[] args)
    {
        try
        {
            return string.Format(Get(key), args ?? []);
        }
        catch
        {
            return key;
        }
    }
}
