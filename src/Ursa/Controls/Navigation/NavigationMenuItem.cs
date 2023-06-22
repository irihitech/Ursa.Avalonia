using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Presenters;
using Avalonia.Controls.Primitives;
using Avalonia.Data;
using Avalonia.Input;
using Avalonia.LogicalTree;
using Avalonia.Markup.Xaml.Templates;
using Avalonia.Reactive;
using Avalonia.VisualTree;

namespace Ursa.Controls;

[PseudoClasses(PC_Closed, PC_Selected, PC_Empty)]
public class NavigationMenuItem: HeaderedSelectingItemsControl
{
    public const string PC_Closed = ":closed";
    public const string PC_Selected = ":selected";
    public const string PC_Empty = ":empty";

    private NavigationMenu? _rootMenu;
    private IDisposable? _ownerSubscription;
    private IDisposable? _itemsBinding;

    protected override void OnAttachedToVisualTree(VisualTreeAttachmentEventArgs e)
    {
        base.OnAttachedToVisualTree(e);
        _rootMenu = this.FindAncestorOfType<NavigationMenu>();
        if (ItemTemplate == null && _rootMenu?.ItemTemplate != null)
        {
            SetCurrentValue(ItemTemplateProperty, _rootMenu.ItemTemplate);
        }
        if (ItemContainerTheme == null && _rootMenu?.ItemContainerTheme != null)
        {
            SetCurrentValue(ItemContainerThemeProperty, _rootMenu.ItemContainerTheme);
        }
    }

    protected override void OnPointerPressed(PointerPressedEventArgs e)
    {
        base.OnPointerPressed(e);
        // Leaf menu node, can be selected. 
        if (this.ItemCount == 0)
        {
            var parents = this.GetSelfAndLogicalAncestors();
            if (_rootMenu is not null && parents.Contains(_rootMenu))
            {
                object? o = this.DataContext ?? this;
                _rootMenu.SelectedMenuItem = o;
            }
        }

        e.Handled = true;
        SetSelection(this, true, true);
    }

    internal void SetSelection(NavigationMenuItem? source, bool selected, bool propagateToParent = false)
    {
        this.PseudoClasses.Set(PC_Selected, selected);
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
                    item.SetSelection(this, false, false);
                }
            }
        }

        if (propagateToParent)
        {
            var parent = this.FindAncestorOfType<NavigationMenuItem>();
            if (parent != null)
            {
                parent.SetSelection(this, selected, true);
            }
            else
            {
                if (selected)
                {
                    _rootMenu?.UpdateSelection(this);
                }
            }
        }
    }
}