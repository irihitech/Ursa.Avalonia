using Avalonia;
using Avalonia.Animation;
using Avalonia.Animation.Easings;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Presenters;
using Avalonia.Controls.Primitives;
using Avalonia.Controls.Templates;
using Avalonia.Data;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.LogicalTree;
using Avalonia.Metadata;
using Avalonia.Styling;
using Avalonia.Threading;
using Irihi.Avalonia.Shared.Helpers;

namespace Ursa.Controls;

[PseudoClasses(PC_HorizontalCollapsed)]
[TemplatePart(Name = PART_Container)]
[TemplatePart(Name = PART_Items)]
public class NavMenu : ItemsControl
{
    public const string PC_HorizontalCollapsed = ":horizontal-collapsed";
    public const string PART_Container = nameof(PART_Container);
    public const string PART_Items = nameof(PART_Items);

    public static readonly StyledProperty<object?> SelectedItemProperty = AvaloniaProperty.Register<NavMenu, object?>(
        nameof(SelectedItem), defaultBindingMode: BindingMode.TwoWay);

    public static readonly StyledProperty<IBinding?> IconBindingProperty =
        AvaloniaProperty.Register<NavMenu, IBinding?>(
            nameof(IconBinding));

    public static readonly StyledProperty<IBinding?> HeaderBindingProperty =
        AvaloniaProperty.Register<NavMenu, IBinding?>(
            nameof(HeaderBinding));

    public static readonly StyledProperty<IBinding?> SubMenuBindingProperty =
        AvaloniaProperty.Register<NavMenu, IBinding?>(
            nameof(SubMenuBinding));

    public static readonly StyledProperty<IBinding?> CommandBindingProperty =
        AvaloniaProperty.Register<NavMenu, IBinding?>(
            nameof(CommandBinding));

    public static readonly StyledProperty<IDataTemplate?> HeaderTemplateProperty =
        AvaloniaProperty.Register<NavMenu, IDataTemplate?>(
            nameof(HeaderTemplate));

    public static readonly StyledProperty<IDataTemplate?> IconTemplateProperty =
        AvaloniaProperty.Register<NavMenu, IDataTemplate?>(
            nameof(IconTemplate));

    public static readonly StyledProperty<IPageTransition?> ItemsTransitionProperty =
        AvaloniaProperty.Register<NavMenu, IPageTransition?>(
            nameof(ItemsTransition));

    public static readonly StyledProperty<IPageTransition?> ContainerTransitionProperty =
        AvaloniaProperty.Register<NavMenu, IPageTransition?>(
            nameof(ContainerTransition));

    public IPageTransition? ContainerTransition
    {
        get => GetValue(ContainerTransitionProperty);
        set => SetValue(ContainerTransitionProperty, value);
    }

    public IPageTransition? ItemsTransition
    {
        get => GetValue(ItemsTransitionProperty);
        set => SetValue(ItemsTransitionProperty, value);
    }

    public static readonly StyledProperty<double> SubMenuIndentProperty = AvaloniaProperty.Register<NavMenu, double>(
        nameof(SubMenuIndent));

    public static readonly StyledProperty<bool> IsHorizontalCollapsedProperty =
        AvaloniaProperty.Register<NavMenu, bool>(
            nameof(IsHorizontalCollapsed));

    public static readonly StyledProperty<object?> HeaderProperty =
        HeaderedContentControl.HeaderProperty.AddOwner<NavMenu>();

    public static readonly StyledProperty<object?> FooterProperty = AvaloniaProperty.Register<NavMenu, object?>(
        nameof(Footer));

    public static readonly StyledProperty<double> ExpandWidthProperty = AvaloniaProperty.Register<NavMenu, double>(
        nameof(ExpandWidth), double.NaN);

    public static readonly StyledProperty<double> CollapseWidthProperty = AvaloniaProperty.Register<NavMenu, double>(
        nameof(CollapseWidth), double.NaN);

    public static readonly AttachedProperty<bool> CanToggleProperty =
        AvaloniaProperty.RegisterAttached<NavMenu, InputElement, bool>("CanToggle");

    public static readonly RoutedEvent<SelectionChangedEventArgs> SelectionChangedEvent =
        RoutedEvent.Register<NavMenu, SelectionChangedEventArgs>(nameof(SelectionChanged), RoutingStrategies.Bubble);

    private bool _updateFromUI;

    static NavMenu()
    {
        SelectedItemProperty.Changed.AddClassHandler<NavMenu, object?>((o, e) => o.OnSelectedItemChange(e));
        IsHorizontalCollapsedProperty.AffectsPseudoClass<NavMenu>(PC_HorizontalCollapsed);
        CanToggleProperty.Changed.AddClassHandler<InputElement, bool>(OnInputRegisteredAsToggle);
    }

    public object? SelectedItem
    {
        get => GetValue(SelectedItemProperty);
        set => SetValue(SelectedItemProperty, value);
    }

    [AssignBinding]
    [InheritDataTypeFromItems(nameof(ItemsSource))]
    public IBinding? IconBinding
    {
        get => GetValue(IconBindingProperty);
        set => SetValue(IconBindingProperty, value);
    }

    [AssignBinding]
    [InheritDataTypeFromItems(nameof(ItemsSource))]
    public IBinding? HeaderBinding
    {
        get => GetValue(HeaderBindingProperty);
        set => SetValue(HeaderBindingProperty, value);
    }

    [AssignBinding]
    [InheritDataTypeFromItems(nameof(ItemsSource))]
    public IBinding? SubMenuBinding
    {
        get => GetValue(SubMenuBindingProperty);
        set => SetValue(SubMenuBindingProperty, value);
    }

    [AssignBinding]
    [InheritDataTypeFromItems(nameof(ItemsSource))]
    public IBinding? CommandBinding
    {
        get => GetValue(CommandBindingProperty);
        set => SetValue(CommandBindingProperty, value);
    }

    /// <summary>
    ///     Header Template is used for MenuItem headers, not menu header.
    /// </summary>
    public IDataTemplate? HeaderTemplate
    {
        get => GetValue(HeaderTemplateProperty);
        set => SetValue(HeaderTemplateProperty, value);
    }

    public IDataTemplate? IconTemplate
    {
        get => GetValue(IconTemplateProperty);
        set => SetValue(IconTemplateProperty, value);
    }

    public double SubMenuIndent
    {
        get => GetValue(SubMenuIndentProperty);
        set => SetValue(SubMenuIndentProperty, value);
    }

    public bool IsHorizontalCollapsed
    {
        get => GetValue(IsHorizontalCollapsedProperty);
        set => SetValue(IsHorizontalCollapsedProperty, value);
    }

    public object? Header
    {
        get => GetValue(HeaderProperty);
        set => SetValue(HeaderProperty, value);
    }

    public object? Footer
    {
        get => GetValue(FooterProperty);
        set => SetValue(FooterProperty, value);
    }

    public double ExpandWidth
    {
        get => GetValue(ExpandWidthProperty);
        set => SetValue(ExpandWidthProperty, value);
    }

    public double CollapseWidth
    {
        get => GetValue(CollapseWidthProperty);
        set => SetValue(CollapseWidthProperty, value);
    }

    public static void SetCanToggle(InputElement obj, bool value)
    {
        obj.SetValue(CanToggleProperty, value);
    }

    public static bool GetCanToggle(InputElement obj)
    {
        return obj.GetValue(CanToggleProperty);
    }

    public event EventHandler<SelectionChangedEventArgs>? SelectionChanged
    {
        add => AddHandler(SelectionChangedEvent, value);
        remove => RemoveHandler(SelectionChangedEvent, value);
    }

    private static void OnInputRegisteredAsToggle(InputElement input, AvaloniaPropertyChangedEventArgs<bool> e)
    {
        if (e.NewValue.Value)
            input.AddHandler(PointerPressedEvent, OnElementToggle);
        else
            input.RemoveHandler(PointerPressedEvent, OnElementToggle);
    }

    private static void OnElementToggle(object? sender, RoutedEventArgs args)
    {
        if (sender is not InputElement input) return;
        var nav = input.FindLogicalAncestorOfType<NavMenu>();
        if (nav is null) return;
        var collapsed = nav.IsHorizontalCollapsed;
        nav.IsHorizontalCollapsed = !collapsed;
    }

    protected override void OnLoaded(RoutedEventArgs e)
    {
        base.OnLoaded(e);
        TryToSelectItem(SelectedItem);
    }

    private Control? _container;
    private Control? _items;
    private CancellationTokenSource? _transitionTokenSource;

    public static readonly StyledProperty<double> StartWidthValueInAnimationProperty =
        AvaloniaProperty.Register<NavMenu, double>(
            nameof(StartWidthValueInAnimation));

    public static readonly StyledProperty<double> EndWidthValueInAnimationProperty =
        AvaloniaProperty.Register<NavMenu, double>(
            nameof(EndWidthValueInAnimation));

    public static readonly StyledProperty<Animation?> WidthAnimationProperty =
        AvaloniaProperty.Register<NavMenu, Animation?>(
            nameof(WidthAnimation));

    public Animation? WidthAnimation
    {
        get => GetValue(WidthAnimationProperty);
        set => SetValue(WidthAnimationProperty, value);
    }

    public double EndWidthValueInAnimation
    {
        get => GetValue(EndWidthValueInAnimationProperty);
        set => SetValue(EndWidthValueInAnimationProperty, value);
    }

    public double StartWidthValueInAnimation
    {
        get => GetValue(StartWidthValueInAnimationProperty);
        set => SetValue(StartWidthValueInAnimationProperty, value);
    }

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        _container = e.NameScope.Find<Control>(PART_Container);
        _items = e.NameScope.Find<Control>(PART_Items);
        base.OnApplyTemplate(e);
    }

    // 在动画过程中Bounds可能多次触发，此字段用以防止重复启动同一段动画。
    private bool _animationIsRunning;
    // 过盈收缩缓解。在NavMenu的收缩过程中如果使用第一次返回的Bounds作为EndWidthValueInAnimation的话在动画结束后将有一次重绘
    // ，这次重绘是非常影响用户体验的。目前为什么会有此重绘原因未知，暂无更多精力调查。所以在NavMenu的收缩过程中我们使用第二次返回的Bounds
    // 作为EndWidthValueInAnimation的值。
    private bool _excessShrinkRelief;

    protected override void OnDetachedFromVisualTree(VisualTreeAttachmentEventArgs e)
    {
        base.OnDetachedFromVisualTree(e);
        _transitionTokenSource?.Cancel();
        _transitionTokenSource?.Dispose();
    }

    protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
    {
        if (change.Property == IsHorizontalCollapsedProperty
            && _container != null
            && _items != null)
        {
            _transitionTokenSource?.Cancel();
            _transitionTokenSource?.Dispose();
            _transitionTokenSource = new CancellationTokenSource();
            StartWidthValueInAnimation = Bounds.Width;
            _animationIsRunning = false;
            _excessShrinkRelief = false;
            Width = IsHorizontalCollapsed ? CollapseWidth : ExpandWidth;
        }

        if (change.Property == BoundsProperty
            && _transitionTokenSource?.IsCancellationRequested is false
            && _animationIsRunning is false)
        {
            if (_excessShrinkRelief is false
                && IsHorizontalCollapsed)
            {
                _excessShrinkRelief = true;
                return;
            }

            _animationIsRunning = true;
            EndWidthValueInAnimation = Bounds.Width;

            List<Task?> tasks = new();
            var forward = IsHorizontalCollapsed;

            WidthAnimation = new()
            {
                Duration = TimeSpan.FromSeconds(0.2),
                Delay = TimeSpan.FromSeconds(0.1),
                Easing = new QuadraticEaseInOut(),
                FillMode = FillMode.Both,
                Children =
                {
                    new KeyFrame()
                    {
                        Cue = new Cue(0.0d),
                        Setters =
                        {
                            new Setter(NavMenu.WidthProperty, StartWidthValueInAnimation)
                        }
                    },
                    new KeyFrame()
                    {
                        Cue = new Cue(1d),
                        Setters =
                        {
                            new Setter(NavMenu.WidthProperty, EndWidthValueInAnimation)
                        }
                    }
                }
            };


            tasks.Add(WidthAnimation?.RunAsync(this, _transitionTokenSource.Token));
            tasks.Add(ContainerTransition?.Start(null, _container, forward, _transitionTokenSource.Token));
            tasks.Add(ItemsTransition?.Start(null, _items, forward, _transitionTokenSource.Token));

            // this.Width = 100d;

            Task.WhenAll(tasks.Where(x => x is not null)).GetAwaiter().OnCompleted(() =>
            {
                Dispatcher.UIThread.Post(() => { Width = IsHorizontalCollapsed ? CollapseWidth : ExpandWidth; });
            });
        }

        base.OnPropertyChanged(change);
    }

    /// <summary>
    ///     this implementation only works in the case that only leaf menu item is allowed to select. It will be changed if we
    ///     introduce parent level selection in the future.
    /// </summary>
    /// <param name="args"></param>
    private void OnSelectedItemChange(AvaloniaPropertyChangedEventArgs<object?> args)
    {
        var a = new SelectionChangedEventArgs(
            SelectionChangedEvent,
            new[] { args.OldValue.Value },
            new[] { args.NewValue.Value });
        if (_updateFromUI)
        {
            RaiseEvent(a);
            return;
        }

        var newValue = args.NewValue.Value;
        if (newValue is null)
        {
            ClearAll();
            RaiseEvent(a);
            return;
        }

        var found = TryToSelectItem(newValue);
        if (!found) ClearAll();
        RaiseEvent(a);
    }

    private bool TryToSelectItem(object? item)
    {
        if (item is null) return false;
        var leaves = GetLeafMenus();
        var found = false;
        foreach (var leaf in leaves)
            if (leaf == item || leaf.DataContext == item)
            {
                leaf.SelectItem(leaf);
                found = true;
            }

        return found;
    }

    protected override bool NeedsContainerOverride(object? item, int index, out object? recycleKey)
    {
        return NeedsContainer<NavMenuItem>(item, out recycleKey);
    }

    protected override Control CreateContainerForItemOverride(object? item, int index, object? recycleKey)
    {
        return new NavMenuItem();
    }

    internal void SelectItem(NavMenuItem item, NavMenuItem parent)
    {
        _updateFromUI = true;
        foreach (var child in LogicalChildren)
        {
            if (Equals(child, parent)) continue;
            if (child is NavMenuItem navMenuItem) navMenuItem.ClearSelection();
        }

        if (item.DataContext is not null && item.DataContext != DataContext)
            SelectedItem = item.DataContext;
        else
            SelectedItem = item;
        item.BringIntoView();
        _updateFromUI = false;
    }

    private IEnumerable<NavMenuItem> GetLeafMenus()
    {
        foreach (var child in LogicalChildren)
            if (child is NavMenuItem item)
            {
                var leafs = item.GetLeafMenus();
                foreach (var leaf in leafs) yield return leaf;
            }
    }

    private void ClearAll()
    {
        foreach (var child in LogicalChildren)
            if (child is NavMenuItem item)
                item.ClearSelection();
    }
}