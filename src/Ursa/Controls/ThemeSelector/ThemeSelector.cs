using Avalonia;
using Avalonia.Controls.Templates;
using Avalonia.Metadata;

namespace Ursa.Controls;

public class ThemeSelector: ThemeSelectorBase
{
    public static readonly StyledProperty<IDataTemplate?> ItemTemplateProperty = AvaloniaProperty.Register<ThemeSelector, IDataTemplate?>(
        nameof(ItemTemplate));

    [InheritDataTypeFromItems(nameof(ThemeSource))]
    public IDataTemplate? ItemTemplate
    {
        get => GetValue(ItemTemplateProperty);
        set => SetValue(ItemTemplateProperty, value);
    }
}