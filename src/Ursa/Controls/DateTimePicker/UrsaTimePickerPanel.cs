using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Input;
using Avalonia.Interactivity;

namespace Ursa.Controls.Panels;

/// <summary>
/// The panel to display items for time selection 
/// </summary>
public class UrsaTimePickerPanel: Panel, ILogicalScrollable
{
    public static readonly StyledProperty<double> ItemHeightProperty =
        AvaloniaProperty.Register<UrsaTimePickerPanel, double>(
            nameof(ItemHeight), defaultValue: 32);

    public double ItemHeight
    {
        get => GetValue(ItemHeightProperty);
        set => SetValue(ItemHeightProperty, value);
    }

    public static readonly StyledProperty<bool> ShouldLoopProperty = AvaloniaProperty.Register<UrsaTimePickerPanel, bool>(
        nameof(ShouldLoop));

    public bool ShouldLoop
    {
        get => GetValue(ShouldLoopProperty);
        set => SetValue(ShouldLoopProperty, value);
    }
    
    static UrsaTimePickerPanel()
    {
        ItemHeightProperty.Changed.AddClassHandler<UrsaTimePickerPanel, double>((panel, args) => panel.OnItemHeightChanged(args));
        AffectsArrange<UrsaTimePickerPanel>(ItemHeightProperty);
        AffectsMeasure<UrsaTimePickerPanel>(ItemHeightProperty);
    }

    private Size _scrollSize;
    private Size _pageScrollSize;
    private void OnItemHeightChanged(AvaloniaPropertyChangedEventArgs<double> args)
    {
        var newValue = args.NewValue.Value;
        _scrollSize = new Size(0, newValue);
        _pageScrollSize = new Size(0, newValue * 3);
    }

    public event EventHandler? OnSelectionChanged;
    public Size Extent { get; private set; }
    public Vector Offset { get; set; }
    public Size Viewport => Bounds.Size;
    public bool BringIntoView(Control target, Rect targetRect) => false;
    public Control? GetControlInDirection(NavigationDirection direction, Control? from) => null;
    public void RaiseScrollInvalidated(System.EventArgs e) => ScrollInvalidated?.Invoke(this, e);
    public bool CanHorizontallyScroll { get => false; set { } }
    public bool CanVerticallyScroll { get => false; set {} }
    public bool IsLogicalScrollEnabled => true;
    public Size ScrollSize => _scrollSize;
    public Size PageScrollSize => _pageScrollSize;
    public event EventHandler? ScrollInvalidated;
}