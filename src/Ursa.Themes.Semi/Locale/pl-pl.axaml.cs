using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Ursa.Themes.Semi.Locale;

public class pl_pl: ResourceDictionary
{
    public pl_pl()
    {
        AvaloniaXamlLoader.Load(this);
        this["STRING_PAGINATION_PAGE"] = string.Empty;
    }
}