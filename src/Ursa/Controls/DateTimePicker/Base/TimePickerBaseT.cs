using Avalonia;
using Avalonia.Controls;
using Avalonia.Data;

namespace Ursa.Controls;

public abstract class TimePickerBase<T> : TimePickerBase where T : struct
{
    public static readonly StyledProperty<T?> SelectedTimeProperty =
        AvaloniaProperty.Register<TimePickerBase<T>, T?>(
            nameof(SelectedTime), defaultBindingMode: BindingMode.TwoWay, enableDataValidation: true);

    public T? SelectedTime
    {
        get => GetValue(SelectedTimeProperty);
        set => SetValue(SelectedTimeProperty, value);
    }

    /// <summary>Converts a <typeparamref name="T"/> value to <see cref="TimeOnly"/> for presenter sync.</summary>
    protected abstract TimeOnly? ToTimeOnly(T? value);

    /// <summary>Creates a <typeparamref name="T"/> value from a presenter-selected <see cref="TimeOnly"/>.</summary>
    protected abstract T FromTimeOnly(TimeOnly time);

    /// <summary>Parses a text string with the given format into a <typeparamref name="T"/> value, or <see langword="null"/> on failure.</summary>
    protected abstract T? Parse(string? text, string? format);

    /// <summary>Formats a <typeparamref name="T"/> value to a display string using the given format.</summary>
    protected abstract string? Format(T? value, string? format);

    protected override TimeOnly? GetSelectedTimeOnly() => ToTimeOnly(SelectedTime);

    protected override void SyncToUI()
    {
        _textBox?.SetValue(TextBox.TextProperty,
            Format(SelectedTime, DisplayFormat ?? DEFAULT_TIME_DISPLAY_FORMAT));
        _presenter?.SyncTime(GetSelectedTimeOnly());
    }

    protected override void CommitInput()
    {
        var format = DisplayFormat ?? DEFAULT_TIME_DISPLAY_FORMAT;
        if (string.IsNullOrWhiteSpace(_textBox?.Text))
        {
            SetCurrentValue(SelectedTimeProperty, (T?)null);
            _presenter?.SyncTime(null);
            return;
        }

        var parsed = Parse(_textBox?.Text, format);
        if (parsed.HasValue)
        {
            SetCurrentValue(SelectedTimeProperty, parsed);
        }
        else
        {
            SetCurrentValue(SelectedTimeProperty, (T?)null);
            _presenter?.SyncTime(null);
        }
    }

    protected override void OnPresenterTimeSelected(TimeOnly? time)
    {
        SetCurrentValue(SelectedTimeProperty,
            time.HasValue ? (T?)FromTimeOnly(time.Value) : (T?)null);
    }

    protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
    {
        base.OnPropertyChanged(change);
        if (change.Property == SelectedTimeProperty)
            SyncToUI();
        else if (change.Property == DisplayFormatProperty && _textBox is not null)
            SyncToUI();
    }

    public override void Clear()
    {
        SetCurrentValue(SelectedTimeProperty, (T?)null);
        _presenter?.SyncTime(null);
    }
}
