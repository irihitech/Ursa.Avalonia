using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Data;
using Irihi.Avalonia.Shared.Contracts;

namespace Ursa.Controls;

public class TimePicker : TemplatedControl, IClearControl
{
    public static readonly StyledProperty<string> DisplayFormatProperty = AvaloniaProperty.Register<TimePicker, string>(
        nameof(DisplayFormat), "HH:mm:ss");

    public static readonly StyledProperty<string> PanelFormatProperty = AvaloniaProperty.Register<TimePicker, string>(
        nameof(PanelFormat), "HH mm ss");

    public static readonly StyledProperty<TimeSpan?> SelectedTimeProperty =
        AvaloniaProperty.Register<TimePicker, TimeSpan?>(
            nameof(SelectedTime));

    public static readonly StyledProperty<bool> NeedConfirmationProperty = AvaloniaProperty.Register<TimePicker, bool>(
        nameof(NeedConfirmation));

    public static readonly StyledProperty<bool> IsLoopingProperty = AvaloniaProperty.Register<TimePicker, bool>(
        nameof(IsLooping));

    private TimeSpan? _selectedTimeHolder;

    static TimePicker()
    {
        PanelFormatProperty.Changed.AddClassHandler<TimePicker, string>((picker, args) =>
            picker.OnPanelFormatChanged(args));
    }

    public string DisplayFormat
    {
        get => GetValue(DisplayFormatProperty);
        set => SetValue(DisplayFormatProperty, value);
    }

    public string PanelFormat
    {
        get => GetValue(PanelFormatProperty);
        set => SetValue(PanelFormatProperty, value);
    }

    public TimeSpan? SelectedTime
    {
        get => GetValue(SelectedTimeProperty);
        set => SetValue(SelectedTimeProperty, value);
    }

    public bool NeedConfirmation
    {
        get => GetValue(NeedConfirmationProperty);
        set => SetValue(NeedConfirmationProperty, value);
    }

    public bool IsLooping
    {
        get => GetValue(IsLoopingProperty);
        set => SetValue(IsLoopingProperty, value);
    }

    public void Clear()
    {
        SetCurrentValue(SelectedTimeProperty, null);
    }

    private void OnPanelFormatChanged(AvaloniaPropertyChangedEventArgs<string> args)
    {
        var format = args.NewValue.Value;
        var parts = format.Split(' ', '-', ':');
    }


    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);
    }

    private void OnSelectionChanged()
    {
        if (NeedConfirmation)
            _selectedTimeHolder = new TimeSpan();
        else
            SelectedTime = new TimeSpan();
    }

    public void Confirm()
    {
        if (NeedConfirmation)
            // TODO: close popup. 
            SetCurrentValue(SelectedTimeProperty, _selectedTimeHolder);
    }

    protected override void UpdateDataValidation(AvaloniaProperty property, BindingValueType state, Exception? error)
    {
        base.UpdateDataValidation(property, state, error);
        if (property == SelectedTimeProperty) DataValidationErrors.SetError(this, error);
    }
}