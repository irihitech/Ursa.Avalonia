using Avalonia;
using Avalonia.Controls;
using Avalonia.Data;
using Irihi.Avalonia.Shared.Common;

namespace Ursa.Controls;

public abstract class TimeRangePickerBase<T> : TimeRangePickerBase where T : struct
{
    public static readonly StyledProperty<T?> SelectedStartTimeProperty =
        AvaloniaProperty.Register<TimeRangePickerBase<T>, T?>(
            nameof(SelectedStartTime), defaultBindingMode: BindingMode.TwoWay, enableDataValidation: true);

    public static readonly StyledProperty<T?> SelectedEndTimeProperty =
        AvaloniaProperty.Register<TimeRangePickerBase<T>, T?>(
            nameof(SelectedEndTime), defaultBindingMode: BindingMode.TwoWay, enableDataValidation: true);

    public T? SelectedStartTime
    {
        get => GetValue(SelectedStartTimeProperty);
        set => SetValue(SelectedStartTimeProperty, value);
    }

    public T? SelectedEndTime
    {
        get => GetValue(SelectedEndTimeProperty);
        set => SetValue(SelectedEndTimeProperty, value);
    }

    /// <summary>Converts a <typeparamref name="T"/> value to <see cref="TimeOnly"/> for presenter sync.</summary>
    protected abstract TimeOnly? ToTimeOnly(T? value);

    /// <summary>Creates a <typeparamref name="T"/> value from a presenter-selected <see cref="TimeOnly"/>.</summary>
    protected abstract T FromTimeOnly(TimeOnly time);

    /// <summary>Parses a text string with the given format into a <typeparamref name="T"/> value, or <see langword="null"/> on failure.</summary>
    protected abstract T? Parse(string? text, string? format);

    /// <summary>Formats a <typeparamref name="T"/> value to a display string using the given format.</summary>
    protected abstract string? Format(T? value, string? format);

    protected override TimeOnly? GetSelectedStartTimeOnly() => ToTimeOnly(SelectedStartTime);
    protected override TimeOnly? GetSelectedEndTimeOnly() => ToTimeOnly(SelectedEndTime);

    protected override void OnStartPresenterTimeSelected(TimeOnly? time)
    {
        SetCurrentValue(SelectedStartTimeProperty,
            time.HasValue ? (T?)FromTimeOnly(time.Value) : (T?)null);
    }

    protected override void OnEndPresenterTimeSelected(TimeOnly? time)
    {
        SetCurrentValue(SelectedEndTimeProperty,
            time.HasValue ? (T?)FromTimeOnly(time.Value) : (T?)null);
    }

    protected override void SyncToUI()
    {
        var format = DisplayFormat ?? DEFAULT_TIME_DISPLAY_FORMAT;
        _startTextBox?.SetCurrentValue(TextBox.TextProperty, Format(SelectedStartTime, format));
        _endTextBox?.SetCurrentValue(TextBox.TextProperty, Format(SelectedEndTime, format));
        _startPresenter?.SyncTime(GetSelectedStartTimeOnly());
        _endPresenter?.SyncTime(GetSelectedEndTimeOnly());
    }

    protected override void CommitInput()
    {
        var format = DisplayFormat ?? DEFAULT_TIME_DISPLAY_FORMAT;

        if (string.IsNullOrWhiteSpace(_startTextBox?.Text))
            SetCurrentValue(SelectedStartTimeProperty, (T?)null);
        else
            SetCurrentValue(SelectedStartTimeProperty, Parse(_startTextBox?.Text, format));

        if (string.IsNullOrWhiteSpace(_endTextBox?.Text))
            SetCurrentValue(SelectedEndTimeProperty, (T?)null);
        else
            SetCurrentValue(SelectedEndTimeProperty, Parse(_endTextBox?.Text, format));
    }

    protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
    {
        base.OnPropertyChanged(change);
        if (change.Property == SelectedStartTimeProperty || change.Property == SelectedEndTimeProperty)
        {
            SyncToUI();
            PseudoClasses.Set(PseudoClassName.PC_Empty, SelectedStartTime is null && SelectedEndTime is null);
        }
        else if (change.Property == DisplayFormatProperty && _startTextBox is not null)
        {
            SyncToUI();
        }
    }

    public override void Clear()
    {
        SetCurrentValue(SelectedStartTimeProperty, (T?)null);
        SetCurrentValue(SelectedEndTimeProperty, (T?)null);
        _startPresenter?.SyncTime(null);
        _endPresenter?.SyncTime(null);
        ResetStatus();
    }
}
