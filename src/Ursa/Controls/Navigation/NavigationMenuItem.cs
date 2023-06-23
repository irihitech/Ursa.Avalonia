using System.Windows.Input;
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

[PseudoClasses(PC_Closed, PC_Selected, PC_Highlighted, PC_Collapsed, PC_TopLevel)]
[TemplatePart(PART_Popup, typeof(Popup))]
public class NavigationMenuItem: HeaderedSelectingItemsControl
{
    public const string PC_Closed = ":closed";
    public const string PC_Selected = ":selected";
    public const string PC_Highlighted= ":highlighted";
    public const string PC_Collapsed = ":collapsed";
    public const string PC_TopLevel = ":top-level";
    public const string PART_Popup = "PART_Popup";

    private NavigationMenu? _rootMenu;
    private IDisposable? _ownerSubscription;
    private IDisposable? _itemsBinding;
    private bool _isCollapsed;
    private Popup? _popup;

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
    
    public static readonly DirectProperty<NavigationMenuItem, int> LevelProperty = AvaloniaProperty.RegisterDirect<NavigationMenuItem, int>(
        nameof(Level), o => o.Level);
    private int _level;
    public int Level
    {
        get => _level;
        private set => SetAndRaise(LevelProperty, ref _level, value);
    }
    
    public static readonly StyledProperty<ICommand> CommandProperty = AvaloniaProperty.Register<NavigationMenuItem, ICommand>(
        nameof(Command));

    public ICommand Command
    {
        get => GetValue(CommandProperty);
        set => SetValue(CommandProperty, value);
    }

    public static readonly StyledProperty<object?> CommandParameterProperty = AvaloniaProperty.Register<NavigationMenuItem, object?>(
        nameof(CommandParameter));

    public object? CommandParameter
    {
        get => GetValue(CommandParameterProperty);
        set => SetValue(CommandParameterProperty, value);
    }

    public static readonly DirectProperty<NavigationMenuItem, bool> IsTopLevelMenuItemProperty = AvaloniaProperty.RegisterDirect<NavigationMenuItem, bool>(
        nameof(IsTopLevelMenuItem), o => o.IsTopLevelMenuItem, (o, v) => o.IsTopLevelMenuItem = v);
    private bool _isTopLevelMenuItem;
    public bool IsTopLevelMenuItem
    {
        get => _isTopLevelMenuItem;
        set => SetAndRaise(IsTopLevelMenuItemProperty, ref _isTopLevelMenuItem, value);
    }

    public static readonly StyledProperty<bool> IsPopupOpenProperty = AvaloniaProperty.Register<NavigationMenuItem, bool>(
        nameof(IsPopupOpen));

    public bool IsPopupOpen
    {
        get => GetValue(IsPopupOpenProperty);
        set => SetValue(IsPopupOpenProperty, value);
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

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);
        GetRootMenu();
        if (ItemTemplate == null && _rootMenu?.ItemTemplate != null)
        {
            SetCurrentValue(ItemTemplateProperty, _rootMenu.ItemTemplate);
        }
        if (ItemContainerTheme == null && _rootMenu?.ItemContainerTheme != null)
        {
            SetCurrentValue(ItemContainerThemeProperty, _rootMenu.ItemContainerTheme);
        }

        if (_rootMenu is not null)
        {
            IsClosed = _rootMenu.IsClosed;
        }
        
        _rootMenu?.GetObservable(NavigationMenu.IsClosedProperty)
            .Subscribe(new AnonymousObserver<bool>(a => this.IsClosed = a));
        _rootMenu?.UpdateSelectionFromSelectedItem(_rootMenu.SelectedItem);
        _popup = e.NameScope.Find<Popup>(PART_Popup);
        Level = CalculateDistanceFromLogicalParent<NavigationMenu>(this) - 1;
        bool isTopLevel = Level == 1;
        IsTopLevelMenuItem = isTopLevel;
        PseudoClasses.Set(PC_TopLevel, isTopLevel);
    }

    private void GetRootMenu()
    {
        _rootMenu = this.FindAncestorOfType<NavigationMenu>();
        if (_rootMenu is null)
        {
            var parents = this.FindLogicalAncestorOfType<NavigationMenu>();
            if (parents is not null)
            {
                _rootMenu = parents;
            }
        }
    }

    protected override void OnPointerPressed(PointerPressedEventArgs e)
    {
        base.OnPointerPressed(e);
        // Leaf menu node, can be selected. 
        if (this.ItemCount == 0)
        {
            if (_rootMenu is not null )
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
            if (_popup is not null)
            {
                _popup.IsOpen = !_popup.IsOpen;
            }
        }
        e.Handled = true;
        Command?.Execute(CommandParameter);
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
            var parent = this.FindLogicalAncestorOfType<NavigationMenuItem>();
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