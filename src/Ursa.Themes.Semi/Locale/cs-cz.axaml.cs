using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Ursa.Themes.Semi.Locale;

public class cs_cz: ResourceDictionary
{
    public cs_cz()
    {
        AvaloniaXamlLoader.Load(this);
        this["STRING_PAGINATION_PAGE"] = string.Empty;
    }
}