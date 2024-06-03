using System.Collections;
using System.Collections.ObjectModel;
using Avalonia;
using Avalonia.Collections;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Primitives;
using Avalonia.Controls.Templates;
using Avalonia.Data;
using Avalonia.Input;

namespace Ursa.Controls;

[PseudoClasses(PC_Selected)]
[TemplatePart(PART_ItemsControl, typeof(ItemsControl))]
public class Rating : TemplatedControl
{
    public const string PART_ItemsControl = "PART_ItemsControl";
    protected const string PC_Selected = ":selected";

    private ItemsControl? _itemsControl;

    public static readonly StyledProperty<double> ValueProperty =
        AvaloniaProperty.Register<Rating, double>(nameof(Value), defaultBindingMode: BindingMode.TwoWay);

    public static readonly StyledProperty<bool> AllowClearProperty =
        AvaloniaProperty.Register<Rating, bool>(nameof(AllowClear), true);

    public static readonly StyledProperty<bool> AllowHalfProperty =
        AvaloniaProperty.Register<Rating, bool>(nameof(AllowHalf));

    public static readonly StyledProperty<bool> AllowFocusProperty =
        AvaloniaProperty.Register<Rating, bool>(nameof(AllowFocus));

    public static readonly StyledProperty<object> CharacterProperty =
        AvaloniaProperty.Register<Rating, object>(nameof(Character));

    public static readonly StyledProperty<int> CountProperty =
        AvaloniaProperty.Register<Rating, int>(nameof(Count), 5);

    public static readonly StyledProperty<double> DefaultValueProperty =
        AvaloniaProperty.Register<Rating, double>(nameof(DefaultValue));

    public static readonly StyledProperty<IList<string>> TooltipsProperty =
        AvaloniaProperty.Register<Rating, IList<string>>(nameof(Tooltips));

    public static readonly StyledProperty<string> SelectedTooltipProperty =
        AvaloniaProperty.Register<Rating, string>(nameof(SelectedTooltip));

    public string SelectedTooltip
    {
        get => GetValue(SelectedTooltipProperty);
        set => SetValue(SelectedTooltipProperty, value);
    }

    public static readonly StyledProperty<IDataTemplate?> ItemTemplateProperty =
        AvaloniaProperty.Register<Rating, IDataTemplate?>(nameof(ItemTemplate));

    public double Value
    {
        get => GetValue(ValueProperty);
        set => SetValue(ValueProperty, value);
    }

    public bool AllowClear
    {
        get => GetValue(AllowClearProperty);
        set => SetValue(AllowClearProperty, value);
    }

    public bool AllowHalf
    {
        get => GetValue(AllowHalfProperty);
        set => SetValue(AllowHalfProperty, value);
    }

    public bool AllowFocus
    {
        get => GetValue(AllowFocusProperty);
        set => SetValue(AllowFocusProperty, value);
    }

    public object Character
    {
        get => GetValue(CharacterProperty);
        set => SetValue(CharacterProperty, value);
    }

    public int Count
    {
        get => GetValue(CountProperty);
        set => SetValue(CountProperty, value);
    }

    public double DefaultValue
    {
        get => GetValue(DefaultValueProperty);
        set => SetValue(DefaultValueProperty, value);
    }

    public IList<string> Tooltips
    {
        get => GetValue(TooltipsProperty);
        set => SetValue(TooltipsProperty, value);
    }

    public IDataTemplate? ItemTemplate
    {
        get => GetValue(ItemTemplateProperty);
        set => SetValue(ItemTemplateProperty, value);
    }

    public static readonly DirectProperty<Rating, IList> ItemsProperty =
        AvaloniaProperty.RegisterDirect<Rating, IList>(
            nameof(Items), o => o.Items);

    private IList _items;

    public IList Items
    {
        get => _items;
        private set => SetAndRaise(ItemsProperty, ref _items, value);
    }

    public Rating()
    {
        Items = new AvaloniaList<object>();
        Tooltips = new ObservableCollection<string>();
    }

    static Rating()
    {
        ValueProperty.Changed.AddClassHandler<Rating>((s, e) => s.OnValueChanged(e));
        CountProperty.Changed.AddClassHandler<Rating>((s, e) => s.OnCountChanged(e));
    }

    private void OnValueChanged(AvaloniaPropertyChangedEventArgs e)
    {
        UpdateItems((int)Value - 1);
    }

    private void OnCountChanged(AvaloniaPropertyChangedEventArgs e)
    {
        if (!IsLoaded) return;

        var currentCount = Items.Count;
        var newCount = e.GetNewValue<int>();

        if (currentCount < newCount)
        {
            var itemsToAdd = newCount - currentCount;
            for (var i = 0; i < itemsToAdd; i++)
            {
                Items.Add(new RatingCharacter());
            }
        }
        else if (currentCount > newCount)
        {
            var itemsToRemove = currentCount - newCount;
            for (var i = 0; i < itemsToRemove && currentCount > i; i++)
            {
                Items.RemoveAt(currentCount - i - 1);
            }
        }

        if (Value > newCount)
        {
            SetCurrentValue(ValueProperty, Math.Max(newCount, 0));
        }
    }

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);

        _itemsControl = e.NameScope.Find<ItemsControl>(PART_ItemsControl);
        for (var i = 0; i < Count; i++)
        {
            Items.Add(new RatingCharacter());
        }

        UpdateItems((int)DefaultValue - 1);
        if (DefaultValue > Count)
        {
            SetCurrentValue(ValueProperty, Math.Max(Count, 0));
        }
        else
        {
            SetCurrentValue(ValueProperty, DefaultValue);
        }
    }

    public void Preview(RatingCharacter o)
    {
        var index = Items.IndexOf(o);
        var tooltipsCount = Tooltips.Count;
        if (tooltipsCount > 0)
        {
            if (index < tooltipsCount)
            {
                SetCurrentValue(SelectedTooltipProperty, Tooltips[index]);
            }
            else
            {
                SetCurrentValue(SelectedTooltipProperty, string.Empty);
            }
        }

        UpdateItems(index);
    }

    protected override void OnPointerExited(PointerEventArgs e)
    {
        UpdateItems((int)Value - 1);
    }

    public void Select(RatingCharacter o)
    {
        var index = Items.IndexOf(o);
        if (AllowClear && index == (int)Value - 1)
        {
            UpdateItems(-1);
            SetCurrentValue(ValueProperty, 0);
        }
        else
        {
            UpdateItems(index);
            SetCurrentValue(ValueProperty, index + 1);
        }
    }

    private void UpdateItems(int index)
    {
        for (var i = 0; i <= index && i < Items.Count; i++)
        {
            if (Items[i] is RatingCharacter item)
            {
                item.Select(true);
            }
        }

        for (var i = index + 1; i >= 0 && i < Items.Count; i++)
        {
            if (Items[i] is RatingCharacter item)
            {
                item.Select(false);
            }
        }
    }
}