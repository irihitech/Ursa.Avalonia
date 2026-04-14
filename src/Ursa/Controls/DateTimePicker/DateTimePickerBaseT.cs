using Avalonia;
using Avalonia.Data;

namespace Ursa.Controls;

public abstract class DateTimePickerBase<T> : DatePickerBase where T : struct
{
    public static readonly StyledProperty<T?> SelectedDateProperty =
        AvaloniaProperty.Register<DateTimePickerBase<T>, T?>(
            nameof(SelectedDate), defaultBindingMode: BindingMode.TwoWay);

    public T? SelectedDate
    {
        get => GetValue(SelectedDateProperty);
        set => SetValue(SelectedDateProperty, value);
    }
}
