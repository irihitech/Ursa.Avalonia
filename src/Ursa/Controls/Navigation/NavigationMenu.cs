using System.Collections;
using System.Collections.Specialized;
using Avalonia;
using Avalonia.Collections;
using Avalonia.Controls;
using Avalonia.Controls.Presenters;
using Avalonia.Controls.Primitives;
using Avalonia.Controls.Templates;
using Avalonia.Input;
using Avalonia.Markup.Xaml.Templates;
using Avalonia.Metadata;

namespace Ursa.Controls;

public class NavigationMenu: HeaderedSelectingItemsControl
{
    public static readonly StyledProperty<object?> FooterProperty = AvaloniaProperty.Register<NavigationMenu, object?>(
        nameof(Footer));

    public object? Footer
    {
        get => GetValue(FooterProperty);
        set => SetValue(FooterProperty, value);
    }

    public static readonly StyledProperty<IDataTemplate> FooterTemplateProperty = AvaloniaProperty.Register<NavigationMenu, IDataTemplate>(
        nameof(FooterTemplate));

    public IDataTemplate FooterTemplate
    {
        get => GetValue(FooterTemplateProperty);
        set => SetValue(FooterTemplateProperty, value);
    }

    public static readonly StyledProperty<object?> SelectedMenuItemProperty = AvaloniaProperty.Register<NavigationMenu, object?>(
        nameof(SelectedMenuItem));

    public object? SelectedMenuItem
    {
        get => GetValue(SelectedMenuItemProperty);
        set => SetValue(SelectedMenuItemProperty, value);
    }
    
    internal void UpdateSelection(NavigationMenuItem source)
    {
        var children = this.ItemsPanelRoot?.Children;
        if (children is not null)
        {
            foreach (var child in children)
            {
                NavigationMenuItem? item = null;
                if (child is NavigationMenuItem i)
                {
                    item = i;
                }
                else if (child is ContentPresenter { Child: NavigationMenuItem i2 })
                {
                    item = i2;
                }
                if (item != null)
                {
                    if(Equals(item, source)) continue;
                    item.SetSelection(null, false, false);
                }
            }
        }
    }
}
