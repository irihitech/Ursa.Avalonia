using Avalonia;
using Avalonia.Data;

namespace Ursa.Controls;

public abstract class DatePickerBase<T> : DatePickerBase where T : struct
{
    public static readonly StyledProperty<T?> SelectedDateProperty =
        AvaloniaProperty.Register<DatePickerBase<T>, T?>(
            nameof(SelectedDate), defaultBindingMode: BindingMode.TwoWay);

    public T? SelectedDate
    {
        get => GetValue(SelectedDateProperty);
        set => SetValue(SelectedDateProperty, value);
    }
}
