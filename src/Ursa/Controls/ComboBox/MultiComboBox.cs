using System.Collections;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using Avalonia;
using Avalonia.Collections;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Primitives;
using Avalonia.Controls.Templates;
using Irihi.Avalonia.Shared.Helpers;

namespace Ursa.Controls;

public class MultiComboBox: SelectingItemsControl
{
    private static ITemplate<Panel?> _defaultPanel = new FuncTemplate<Panel?>(() => new VirtualizingStackPanel());

    public static readonly StyledProperty<bool> IsDropDownOpenProperty =
        ComboBox.IsDropDownOpenProperty.AddOwner<MultiComboBox>();
    
    public bool IsDropDownOpen
    {
        get => GetValue(IsDropDownOpenProperty);
        set => SetValue(IsDropDownOpenProperty, value);
    }

    public static readonly StyledProperty<double> MaxDropdownHeightProperty = AvaloniaProperty.Register<MultiComboBox, double>(
        nameof(MaxDropdownHeight));

    public double MaxDropdownHeight
    {
        get => GetValue(MaxDropdownHeightProperty);
        set => SetValue(MaxDropdownHeightProperty, value);
    }

    public new static readonly StyledProperty<IList?> SelectedItemsProperty = AvaloniaProperty.Register<MultiComboBox, IList?>(
        nameof(SelectedItems), new AvaloniaList<object>());

    public new IList? SelectedItems
    {
        get => GetValue(SelectedItemsProperty);
        set => SetValue(SelectedItemsProperty, value);
    }
    
    static MultiComboBox()
    {
        FocusableProperty.OverrideDefaultValue<MultiComboBox>(true);
        ItemsPanelProperty.OverrideDefaultValue<MultiComboBox>(_defaultPanel);
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

    private Dictionary<int, IDisposable?> _disposables = new Dictionary<int, IDisposable?>();

    protected override void PrepareContainerForItemOverride(Control container, object? item, int index)
    {
        base.PrepareContainerForItemOverride(container, item, index);
        if(_disposables.TryGetValue(index, out var d))
        {
            d?.Dispose();
            _disposables.Remove(index);
        }
        if (container is MultiComboBoxItem comboBoxItem)
        {
            comboBoxItem.IsSelected = SelectedItems?.Contains(item) ?? false;
            var disposable = MultiComboBoxItem.IsSelectedProperty.Changed.Subscribe(a =>
            {
                if (a.Sender == comboBoxItem)
                {
                    if (comboBoxItem.IsSelected)
                    {
                        SelectedItems?.Add(item);
                    }
                    else
                    {
                        SelectedItems?.Remove(item);
                    }
                }
            });
            _disposables[index] = disposable;
        }
    }

    internal void ItemFocused(MultiComboBoxItem dropDownItem)
    {
        if (IsDropDownOpen && dropDownItem.IsFocused && dropDownItem.IsArrangeValid)
        {
            dropDownItem.BringIntoView();
        }
    }
    
}