using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Ursa.Themes.Fluent2.Locale;

internal class ru_ru: ResourceDictionary
{
    public ru_ru()
    {
        AvaloniaXamlLoader.Load(this);
        this["STRING_PAGINATION_PAGE"] = string.Empty;
    }
}
