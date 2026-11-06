using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Ursa.Themes.Fluent2.Locale;

internal class ko_kr: ResourceDictionary
{
    public ko_kr()
    {
        AvaloniaXamlLoader.Load(this);
        this["STRING_PAGINATION_PAGE"] = string.Empty;
    }
}
