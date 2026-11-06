using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Ursa.Themes.Fluent2.Locale;

internal class fr_fr: ResourceDictionary
{
    public fr_fr()
    {
        AvaloniaXamlLoader.Load(this);
        this["STRING_PAGINATION_PAGE"] = string.Empty;
    }
}
