using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Ursa.Themes.Fluent2.Locale;

internal class ja_jp: ResourceDictionary
{
    public ja_jp()
    {
        AvaloniaXamlLoader.Load(this);
        this["STRING_PAGINATION_PAGE"] = string.Empty;
    }
}
