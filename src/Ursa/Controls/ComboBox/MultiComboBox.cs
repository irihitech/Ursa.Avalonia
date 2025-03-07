using System.Collections;
using System.Collections.Specialized;
using Avalonia;
using Avalonia.Collections;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Primitives;
using Avalonia.Controls.Templates;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Metadata;
using Irihi.Avalonia.Shared.Contracts;
using Irihi.Avalonia.Shared.Helpers;

namespace Ursa.Controls;

/// <summary>
/// This control inherits from <see cref="SelectingItemsControl"/>, but it only supports MVVM pattern. 
/// </summary>
[TemplatePart(PART_BackgroundBorder, typeof(Border))]
[PseudoClasses(PC_DropDownOpen, PC_Empty)]
public class MultiComboBox : SelectingItemsControl, IInnerContentControl, IPopupInnerContent
{
    public const string PART_BackgroundBorder = "PART_BackgroundBorder";
    public const string PC_DropDownOpen = ":dropdownopen";
    public const string PC_Empty = ":selection-empty";

    private static readonly ITemplate<Panel?> _defaultPanel =
        new FuncTemplate<Panel?>(() => new VirtualizingStackPanel());

    public static readonly StyledProperty<bool> IsDropDownOpenProperty =
        ComboBox.IsDropDownOpenProperty.AddOwner<MultiComboBox>();

    public static readonly StyledProperty<double> MaxDropDownHeightProperty =
        AvaloniaProperty.Register<MultiComboBox, double>(
            nameof(MaxDropDownHeight));

    public static readonly StyledProperty<double> MaxSelectionBoxHeightProperty =
        AvaloniaProperty.Register<MultiComboBox, double>(
            nameof(MaxSelectionBoxHeight));

    public new static readonly StyledProperty<IList?> SelectedItemsProperty =
        AvaloniaProperty.Register<MultiComboBox, IList?>(
            nameof(SelectedItems));

    public static readonly StyledProperty<object?> InnerLeftContentProperty =
        AvaloniaProperty.Register<MultiComboBox, object?>(
            nameof(InnerLeftContent));

    public static readonly StyledProperty<object?> InnerRightContentProperty =
        AvaloniaProperty.Register<MultiComboBox, object?>(
            nameof(InnerRightContent));


    public static readonly StyledProperty<IDataTemplate?> SelectedItemTemplateProperty =
        AvaloniaProperty.Register<MultiComboBox, IDataTemplate?>(
            nameof(SelectedItemTemplate));

    public static readonly StyledProperty<string?> WatermarkProperty =
        TextBox.WatermarkProperty.AddOwner<MultiComboBox>();

    public static readonly StyledProperty<object?> PopupInnerTopContentProperty =
        AvaloniaProperty.Register<MultiComboBox, object?>(
            nameof(PopupInnerTopContent));

    public static readonly StyledProperty<object?> PopupInnerBottomContentProperty =
        AvaloniaProperty.Register<MultiComboBox, object?>(
            nameof(PopupInnerBottomContent));

    private Border? _rootBorder;

    static MultiComboBox()
    {
        FocusableProperty.OverrideDefaultValue<MultiComboBox>(true);
        ItemsPanelProperty.OverrideDefaultValue<MultiComboBox>(_defaultPanel);
        IsDropDownOpenProperty.AffectsPseudoClass<MultiComboBox>(PC_DropDownOpen);
        SelectedItemsProperty.Changed.AddClassHandler<MultiComboBox, IList?>((box, args) =>
            box.OnSelectedItemsChanged(args));
    }

    public MultiComboBox()
    {
        SelectedItems = new AvaloniaList<object>();
        if (SelectedItems is INotifyCollectionChanged c) c.CollectionChanged += OnSelectedItemsCollectionChanged;
    }

    public bool IsDropDownOpen
    {
        get => GetValue(IsDropDownOpenProperty);
        set => SetValue(IsDropDownOpenProperty, value);
    }

    public double MaxDropDownHeight
    {
        get => GetValue(MaxDropDownHeightProperty);
        set => SetValue(MaxDropDownHeightProperty, value);
    }

    public double MaxSelectionBoxHeight
    {
        get => GetValue(MaxSelectionBoxHeightProperty);
        set => SetValue(MaxSelectionBoxHeightProperty, value);
    }

    public new IList? SelectedItems
    {
        get => GetValue(SelectedItemsProperty);
        set => SetValue(SelectedItemsProperty, value);
    }

    [InheritDataTypeFromItems(nameof(SelectedItems))]
    public IDataTemplate? SelectedItemTemplate
    {
        get => GetValue(SelectedItemTemplateProperty);
        set => SetValue(SelectedItemTemplateProperty, value);
    }

    public string? Watermark
    {
        get => GetValue(WatermarkProperty);
        set => SetValue(WatermarkProperty, value);
    }

    public object? InnerLeftContent
    {
        get => GetValue(InnerLeftContentProperty);
        set => SetValue(InnerLeftContentProperty, value);
    }

    public object? InnerRightContent
    {
        get => GetValue(InnerRightContentProperty);
        set => SetValue(InnerRightContentProperty, value);
    }

    public object? PopupInnerTopContent
    {
        get => GetValue(PopupInnerTopContentProperty);
        set => SetValue(PopupInnerTopContentProperty, value);
    }

    public object? PopupInnerBottomContent
    {
        get => GetValue(PopupInnerBottomContentProperty);
        set => SetValue(PopupInnerBottomContentProperty, value);
    }

    private void OnSelectedItemsChanged(AvaloniaPropertyChangedEventArgs<IList?> args)
    {
        if (args.OldValue.Value is INotifyCollectionChanged old)
            old.CollectionChanged -= OnSelectedItemsCollectionChanged;
        if (args.NewValue.Value is INotifyCollectionChanged @new)
            @new.CollectionChanged += OnSelectedItemsCollectionChanged;
    }

    private void OnSelectedItemsCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        PseudoClasses.Set(PC_Empty, SelectedItems?.Count is null or 0);
        //return;
        var containers = Presenter?.Panel?.Children;
        if (containers is null) return;
        foreach (var container in containers)
        {
            if (container is MultiComboBoxItem i)
            {
                i.UpdateSelection();
            }
        }
    }

    protected override bool NeedsContainerOverride(object? item, int index, out object? recycleKey)
    {
        recycleKey = item;
        return item is not MultiComboBoxItem;
    }

    protected override Control CreateContainerForItemOverride(object? item, int index, object? recycleKey)
    {
        return new MultiComboBoxItem();
    }

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);
        PointerPressedEvent.RemoveHandler(OnBackgroundPointerPressed, _rootBorder);
        _rootBorder = e.NameScope.Find<Border>(PART_BackgroundBorder);
        PointerPressedEvent.AddHandler(OnBackgroundPointerPressed, _rootBorder);
        PseudoClasses.Set(PC_Empty, SelectedItems?.Count == 0);
    }

    private void OnBackgroundPointerPressed(object? sender, PointerPressedEventArgs e)
    {
        SetCurrentValue(IsDropDownOpenProperty, !IsDropDownOpen);
    }

    internal void ItemFocused(MultiComboBoxItem dropDownItem)
    {
        if (IsDropDownOpen && dropDownItem.IsFocused && dropDownItem.IsArrangeValid) dropDownItem.BringIntoView();
    }

    public void Remove(object? o)
    {
        if (o is StyledElement s)
        {
            var data = s.DataContext;
            SelectedItems?.Remove(data);
            var item = Items.FirstOrDefault(a => ReferenceEquals(a, data));
            if (item is not null)
            {
                var container = ContainerFromItem(item);
                if (container is MultiComboBoxItem t) t.IsSelected = false;
            }
        }
    }

    public void Clear()
    {
        this.SelectedItems?.Clear();
        var containers = Presenter?.Panel?.Children;
        if (containers is null) return;
        foreach (var container in containers)
            if (container is MultiComboBoxItem t)
                t.IsSelected = false;
    }

    protected override void OnUnloaded(RoutedEventArgs e)
    {
        base.OnUnloaded(e);
        if (SelectedItems is INotifyCollectionChanged c) c.CollectionChanged -= OnSelectedItemsCollectionChanged;
    }
}