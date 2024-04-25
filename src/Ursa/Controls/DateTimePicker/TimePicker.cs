using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Data;
using Irihi.Avalonia.Shared.Contracts;

namespace Ursa.Controls.TimePicker;

public class TimePicker: TemplatedControl, IClearControl
{
    private TimeSpan? _selectedTimeHolder;
    
    public static readonly StyledProperty<string> DisplayFormatProperty = AvaloniaProperty.Register<TimePicker, string>(
        nameof(DisplayFormat), defaultValue: "HH:mm:ss");

    public string DisplayFormat
    {
        get => GetValue(DisplayFormatProperty);
        set => SetValue(DisplayFormatProperty, value);
    }

    public static readonly StyledProperty<string> PanelFormatProperty = AvaloniaProperty.Register<TimePicker, string>(
        nameof(PanelFormat), defaultValue: "HH mm ss");

    public string PanelFormat
    {
        get => GetValue(PanelFormatProperty);
        set => SetValue(PanelFormatProperty, value);
    }

    public static readonly StyledProperty<TimeSpan?> SelectedTimeProperty = AvaloniaProperty.Register<TimePicker, TimeSpan?>(
        nameof(SelectedTime));

    public TimeSpan? SelectedTime
    {
        get => GetValue(SelectedTimeProperty);
        set => SetValue(SelectedTimeProperty, value);
    }
    
    public static readonly StyledProperty<bool> NeedConfirmationProperty = AvaloniaProperty.Register<TimePicker, bool>(
        nameof(NeedConfirmation));

    public bool NeedConfirmation
    {
        get => GetValue(NeedConfirmationProperty);
        set => SetValue(NeedConfirmationProperty, value);
    }

    public static readonly StyledProperty<bool> IsLoopingProperty = AvaloniaProperty.Register<TimePicker, bool>(
        nameof(IsLooping));
    
    public bool IsLooping
    {
        get => GetValue(IsLoopingProperty);
        set => SetValue(IsLoopingProperty, value);
    }

    static TimePicker()
    {
        PanelFormatProperty.Changed.AddClassHandler<TimePicker, string>((picker, args)=> picker.OnPanelFormatChanged(args));
    }

    private void OnPanelFormatChanged(AvaloniaPropertyChangedEventArgs<string> args)
    {
        var format = args.NewValue.Value;
        string[] parts = format.Split(new char[] { ' ', '-', ':' });
    }


    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);
        
    }

    private void OnSelectionChanged()
    {
        if (NeedConfirmation)
        {
            _selectedTimeHolder = new TimeSpan();
        }
        else
        {
            SelectedTime = new TimeSpan();
        }
    }

    public void Clear()
    {
        SetCurrentValue(SelectedTimeProperty, null);
    }
    
    public void Confirm()
    {
        if (NeedConfirmation)
        {
            // TODO: close popup. 
            SetCurrentValue(SelectedTimeProperty, _selectedTimeHolder);
        }
    }

    protected override void UpdateDataValidation(AvaloniaProperty property, BindingValueType state, Exception? error)
    {
        base.UpdateDataValidation(property, state, error);
        if (property == SelectedTimeProperty)
        {
            DataValidationErrors.SetError(this, error);
        }
    }
}