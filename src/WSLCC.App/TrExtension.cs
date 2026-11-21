using Microsoft.UI.Xaml.Markup;

namespace WSLCC_App;

[MarkupExtensionReturnType(ReturnType = typeof(string))]
public sealed class TrExtension : MarkupExtension
{
    public string Key { get; set; } = string.Empty;

    protected override object ProvideValue() => L.Get(Key);
}
