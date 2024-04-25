using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Presenters;
using Avalonia.Controls.Primitives;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Layout;
using Avalonia.VisualTree;

namespace Ursa.Controls.Panels;

public enum TimePickerPanelType
{
    Hour,
    Minute,
    Second,
    TimePeriod // AM/PM
}
/// <summary>
/// The panel to display items for time selection 
/// </summary>
public class UrsaTimePickerPanel: Panel, ILogicalScrollable
{
    private ScrollContentPresenter? _parentScroller;
    private double _extendOne;
    private Vector _offset;
    private bool _initialized;
    private int _countItemAboveBelowSelected;

    public Vector Offset
    {
        get => _offset;
        set => SetOffset(value);
    }

    private int _increment;

    public int Increment
    {
        get => _increment;
        set => _increment = value;
    }

    private int _selectedIndex;
    public int SelectedIndex
    {
        get => _selectedIndex;
        set
        {
            _selectedIndex = value;
        }
    }

    private int _selectedValue;

    public int SelectedValue
    {
        get => _selectedValue;
        set => _selectedValue = value;
    }

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

    protected override void OnAttachedToVisualTree(VisualTreeAttachmentEventArgs e)
    {
        base.OnAttachedToVisualTree(e);
        _parentScroller?.RemoveHandler(Gestures.ScrollGestureEndedEvent, OnScrollGestureEnded);
        _parentScroller = this.GetVisualParent() as ScrollContentPresenter;
        _parentScroller?.AddHandler(Gestures.ScrollGestureEndedEvent, OnScrollGestureEnded);
    }

    protected override void OnDetachedFromVisualTree(VisualTreeAttachmentEventArgs e)
    {
        base.OnDetachedFromVisualTree(e);
        _parentScroller?.RemoveHandler(Gestures.ScrollGestureEndedEvent, OnScrollGestureEnded);
        _parentScroller = null;
    }

    public override void ApplyTemplate()
    {
        base.ApplyTemplate();
        AddHandler(TappedEvent, OnItemTapped, RoutingStrategies.Bubble);
    }

    private void OnItemTapped(object sender, TappedEventArgs e)
    {
        if (e.Source is Visual source && GetItemFromSource(source) is { Tag: int tag })
        {
            SelectedValue = tag;
            e.Handled = true;
        }
    }

    private ListBoxItem? GetItemFromSource(Visual source)
    {
        var item = source;
        while (item != null && !(item is ListBoxItem))
        {
            item = item.GetVisualParent();
        }
        return item as ListBoxItem;
    }

    protected override Size MeasureOverride(Size availableSize)
    {
        if (double.IsInfinity(availableSize.Width) || double.IsInfinity(availableSize.Height))
            throw new InvalidOperationException("Panel must have finite height");
        if(_initialized) UpdateHelperInfo();
        double initY = availableSize.Height / 2.0 - ItemHeight / 2.0;
        _countItemAboveBelowSelected = (int)Math.Ceiling(initY / ItemHeight);
        
        var children = Children;
        
        CreateOrDestroyItems(children);
        
        for(int i = 0; i< children.Count; i++)
        {
            children[i].Measure(availableSize);
        }

        if (!_initialized)
        {
            UpdateItems();
            RaiseScrollInvalidated(System.EventArgs.Empty);
            _initialized = true;
        }

        return availableSize;
    }

    protected override Size ArrangeOverride(Size finalSize)
    {
        if (Children.Count == 0)
        {
            return base.ArrangeOverride(finalSize);
        }

        var itemHeight = ItemHeight;
        var children = Children;
        Rect rc;
        double initY = finalSize.Height / 2.0 - itemHeight / 2.0;

        if (ShouldLoop)
        {
            var currentSet = Math.Truncate(Offset.Y / _extendOne);
            initY += _extendOne * currentSet + (_selectedIndex - _countItemAboveBelowSelected) * ItemHeight;
            foreach (var child in children)
            {
                rc = new Rect(0, initY-Offset.Y, finalSize.Width, itemHeight);
                child.Arrange(rc);
                initY += itemHeight;
            }
        }
        else
        {
            var first = Math.Max(0, _selectedIndex - _countItemAboveBelowSelected);
            foreach (var child in children)
            {
                rc = new Rect(0, initY+first+itemHeight-Offset.Y, finalSize.Width, itemHeight);
                child.Arrange(rc);
                initY += itemHeight;
            }
        }
        return finalSize;
    }

    private void OnScrollGestureEnded(object sender, ScrollGestureEndedEventArgs e)
    {
        var snapY = Math.Round(Offset.Y / ItemHeight) * ItemHeight;
        if(!snapY.Equals(Offset.Y))
        {
            Offset = Offset.WithY(snapY);
        }
    }
    
    private void SetOffset(Vector value)
    {
        var oldValue = _offset;
        _offset = value;
        var dy = _offset.Y - oldValue.Y;
        var children = this.Children;
        // TODO
    }

    private int _range;
    private int _maximumValue;
    private int _minimumValue;
    private int _totalItems;
    private void UpdateHelperInfo()
    {
        _range = _maximumValue - _minimumValue + 1;
        _totalItems = (int)Math.Ceiling((double)_range / _increment);
        var itemHeight = ItemHeight;
        _extent = new Size(0, ShouldLoop ? _totalItems * itemHeight * 100 : _totalItems * itemHeight);

        _extendOne = _totalItems * itemHeight;
        _offset = new Vector(0,
            ShouldLoop ? _extendOne * 50 + _selectedIndex * itemHeight : _selectedIndex * itemHeight);
    }

    private void UpdateItems()
    {
        var children = Children;
        var min = _minimumValue;
        var max = _maximumValue;
        var selected = SelectedValue;

        int first;
        if (ShouldLoop)
        {
            first = (_selectedIndex - _countItemAboveBelowSelected) % _totalItems;
            first = first < 0 ? min + (first + _totalItems) * Increment : min + first * Increment;
        }
        else
        {
            first = min + Math.Max(0, _selectedIndex - _countItemAboveBelowSelected) * Increment;
        }

        for (int i = 0; i < children.Count; i++)
        {
            ListBoxItem item = (ListBoxItem)children[i];
            item.Content = first + i * Increment; // TODO
            item.Tag = first;
            item.IsSelected = first == selected;
            first += Increment;
            if(first > max)
            {
                first = min;
            }
        }

    }

    private void CreateOrDestroyItems(Avalonia.Controls.Controls children)
    {
        int totalItemsInViewport = _countItemAboveBelowSelected * 2 + 1;
        if (!ShouldLoop)
        {
            int numItemAboveSelect = _countItemAboveBelowSelected;
            if (_selectedIndex - _countItemAboveBelowSelected < 0)
            {
                numItemAboveSelect = _selectedIndex;
            }
            int numItemBelowSelect = _countItemAboveBelowSelected;
            if (_selectedIndex + _countItemAboveBelowSelected >= _totalItems)
            {
                numItemBelowSelect = _totalItems - _selectedIndex - 1;
            }
            totalItemsInViewport = numItemAboveSelect + numItemBelowSelect + 1;
        }

        while (children.Count<totalItemsInViewport)
        {
            children.Add(new ListBoxItem
            {
                Height = ItemHeight,
                VerticalContentAlignment = VerticalAlignment.Center,
                Focusable = false,
            });
        }

        if (children.Count > totalItemsInViewport)
        {
            var countToRemove = children.Count - totalItemsInViewport;
            children.RemoveRange(children.Count-countToRemove, countToRemove);
        }
    }

    private int CoerceSelected(int newValue)
    {
        if(newValue < _minimumValue)
        {
            return _minimumValue;
        }
        if(newValue > _maximumValue)
        {
            return _maximumValue;
        }
        if (newValue % _increment == 0) return newValue;
        var items = Enumerable.Range(_minimumValue, _range).Where(x => x % _increment == 0).ToList();
        var nearest = items.Aggregate((x, y) => Math.Abs(x - newValue) > Math.Abs(y - newValue) ? y : x);
        return items.IndexOf(nearest) * Increment;
    }
    public event EventHandler? OnSelectionChanged;
    private Size _extent;
    public Size Extent { 
        get => _extent;
        private set => _extent = value;
    }
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
    public event EventHandler? SelectionChanged;
}