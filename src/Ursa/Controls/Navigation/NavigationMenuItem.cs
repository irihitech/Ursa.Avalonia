using System.Security.Cryptography.X509Certificates;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Mixins;
using Avalonia.Controls.Presenters;
using Avalonia.Controls.Primitives;
using Avalonia.Controls.Templates;
using Avalonia.Data;
using Avalonia.Input;
using Avalonia.LogicalTree;
using Avalonia.Markup.Xaml.Templates;
using Avalonia.Reactive;
using Avalonia.VisualTree;

namespace Ursa.Controls;

[PseudoClasses(PC_Closed, PC_Selected, PC_Highlighted, PC_Collapsed)]
public class NavigationMenuItem: HeaderedSelectingItemsControl
{
    public const string PC_Closed = ":closed";
    public const string PC_Selected = ":selected";
    public const string PC_Highlighted= ":highlighted";
    public const string PC_Collapsed = ":collapsed";

    private NavigationMenu? _rootMenu;
    private IDisposable? _ownerSubscription;
    private IDisposable? _itemsBinding;
    private bool _isCollapsed;

    public static readonly StyledProperty<bool> IsClosedProperty = AvaloniaProperty.Register<NavigationMenuItem, bool>(
        nameof(IsClosed));

    public bool IsClosed
    {
        get => GetValue(IsClosedProperty);
        set => SetValue(IsClosedProperty, value);
    }

    public static readonly StyledProperty<object?> IconProperty = AvaloniaProperty.Register<NavigationMenuItem, object?>(
        nameof(Icon));

    public object? Icon
    {
        get => GetValue(IconProperty);
        set => SetValue(IconProperty, value);
    }

    public static readonly StyledProperty<IDataTemplate> IconTemplateProperty = AvaloniaProperty.Register<NavigationMenuItem, IDataTemplate>(
        nameof(IconTemplate));

    public IDataTemplate IconTemplate
    {
        get => GetValue(IconTemplateProperty);
        set => SetValue(IconTemplateProperty, value);
    }

    private int _level;

    public static readonly DirectProperty<NavigationMenuItem, int> LevelProperty = AvaloniaProperty.RegisterDirect<NavigationMenuItem, int>(
        nameof(Level), o => o.Level);

    public int Level
    {
        get => _level;
        private set => SetAndRaise(LevelProperty, ref _level, value);
    }

    static NavigationMenuItem()
    {
        IsClosedProperty.Changed.AddClassHandler<NavigationMenuItem>((o, e) => o.OnIsClosedChanged(e));
        PressedMixin.Attach<NavigationMenuItem>();
    }

    private void OnIsClosedChanged(AvaloniaPropertyChangedEventArgs args)
    {
        bool newValue = args.GetNewValue<bool>();
        PseudoClasses.Set(PC_Closed, newValue);
    }


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

        _rootMenu?.GetObservable(NavigationMenu.IsClosedProperty)
            .Subscribe(new AnonymousObserver<bool>(a => this.IsClosed = a));
        
        Level = CalculateDistanceFromLogicalParent<NavigationMenu>(this) - 1;
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
                _rootMenu.SelectedItem = o;
            }
            SetSelection(this, true, true);
        }
        // Non-leaf node, act as a toggle button.
        else
        {
            _isCollapsed = !_isCollapsed;
            this.PseudoClasses.Set(PC_Collapsed, _isCollapsed);
        }
        e.Handled = true;
        
    }

    internal void SetSelection(NavigationMenuItem? source, bool selected, bool propagateToParent = false)
    {
        if (Equals(this, source) && this.ItemCount == 0)
        {
            this.PseudoClasses.Set(PC_Highlighted, selected);
            this.PseudoClasses.Set(PC_Selected, selected);
        }
        else
        {
            this.PseudoClasses.Set(PC_Selected, false);
            this.PseudoClasses.Set(PC_Highlighted, selected);
        }
        var children = this.ItemsPanelRoot?.Children;
        if (children is not null)
        {
            foreach (var child in children)
            {
                NavigationMenuItem? item = GetMenuItemFromControl(child);
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

    internal void UpdateSelectionFromSelectedItem(object? o)
    {
        if (o is null)
        {
            this.SetSelection(this, false, false);
            return;
        }

        if (Equals(this, o) || Equals(this.DataContext, o))
        {
            this.SetSelection(this, true, true);
        }
        else
        {
            var children = this.ItemsPanelRoot?.Children;
            if (children is not null)
            {
                foreach (var child in children)
                {
                    NavigationMenuItem? item = GetMenuItemFromControl(child);
                    if (item != null)
                    {
                        item.UpdateSelectionFromSelectedItem(o);
                    }
                }
            }
        }
    }
    
    private static int CalculateDistanceFromLogicalParent<T>(ILogical? logical, int @default = -1) where T : class
    {
        var result = 0;

        while (logical != null && !(logical is T))
        {
            ++result;
            logical = logical.LogicalParent;
        }

        return logical != null ? result : @default;
    }

    public static NavigationMenuItem? GetMenuItemFromControl(Control? control)
    {
        if (control is null) return null;
        if (control is NavigationMenuItem item) return item;
        if (control is ContentPresenter { Child: NavigationMenuItem item2 }) return item2;
        return null;
    }
}