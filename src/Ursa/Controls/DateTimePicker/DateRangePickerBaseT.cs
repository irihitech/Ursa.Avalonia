using Avalonia;
using Avalonia.Data;

namespace Ursa.Controls;

public abstract class DateRangePickerBase<T> : DatePickerBase where T : struct
{
    public static readonly StyledProperty<T?> SelectedStartDateProperty =
        AvaloniaProperty.Register<DateRangePickerBase<T>, T?>(
            nameof(SelectedStartDate), defaultBindingMode: BindingMode.TwoWay);

    public static readonly StyledProperty<T?> SelectedEndDateProperty =
        AvaloniaProperty.Register<DateRangePickerBase<T>, T?>(
            nameof(SelectedEndDate), defaultBindingMode: BindingMode.TwoWay);

    public T? SelectedStartDate
    {
        get => GetValue(SelectedStartDateProperty);
        set => SetValue(SelectedStartDateProperty, value);
    }

    public T? SelectedEndDate
    {
        get => GetValue(SelectedEndDateProperty);
        set => SetValue(SelectedEndDateProperty, value);
    }
}
