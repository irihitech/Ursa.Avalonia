using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Ursa.Themes.Semi.Locale;

public class fr_fr: ResourceDictionary
{
    public fr_fr()
    {
        AvaloniaXamlLoader.Load(this);
        this["STRING_PAGINATION_PAGE"] = string.Empty;
    }
}
