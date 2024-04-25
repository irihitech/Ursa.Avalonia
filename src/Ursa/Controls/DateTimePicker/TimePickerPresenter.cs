using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Primitives;

namespace Ursa.Controls;

[TemplatePart(PART_PickerContainer, typeof(Grid))]
[TemplatePart(PART_HourSelector, typeof(DateTimePickerPanel))]
[TemplatePart(PART_MinuteSelector, typeof(DateTimePickerPanel))]
[TemplatePart(PART_SecondSelector, typeof(DateTimePickerPanel))]
[TemplatePart(PART_AmPmSelector, typeof(DateTimePickerPanel))]
public class TimePickerPresenter: TemplatedControl
{
    public const string PART_HourSelector = "PART_HourSelector";
    public const string PART_MinuteSelector = "PART_MinuteSelector";
    public const string PART_SecondSelector = "PART_SecondSelector";
    public const string PART_AmPmSelector = "PART_AmPmSelector";
    public const string PART_PickerContainer = "PART_PickerContainer";
    
    private DateTimePickerPanel? _hourSelector;
    private DateTimePickerPanel? _minuteSelector;
    private DateTimePickerPanel? _secondSelector;
    private DateTimePickerPanel? _ampmSelector;
    private Grid? _pickerContainer;
    
    public static readonly StyledProperty<bool> NeedsConfirmationProperty = AvaloniaProperty.Register<TimePickerPresenter, bool>(
        nameof(NeedsConfirmation));

    public bool NeedsConfirmation
    {
        get => GetValue(NeedsConfirmationProperty);
        set => SetValue(NeedsConfirmationProperty, value);
    }

    public static readonly StyledProperty<int> MinuteIncrementProperty = AvaloniaProperty.Register<TimePickerPresenter, int>(
        nameof(MinuteIncrement));

    public int MinuteIncrement
    {
        get => GetValue(MinuteIncrementProperty);
        set => SetValue(MinuteIncrementProperty, value);
    }

    public static readonly StyledProperty<TimeSpan?> TimeProperty = AvaloniaProperty.Register<TimePickerPresenter, TimeSpan?>(
        nameof(Time));

    public TimeSpan? Time
    {
        get => GetValue(TimeProperty);
        set => SetValue(TimeProperty, value);
    }

    public static readonly StyledProperty<bool> Use12HoursProperty = AvaloniaProperty.Register<TimePickerPresenter, bool>(
        nameof(Use12Hours));

    public bool Use12Hours
    {
        get => GetValue(Use12HoursProperty);
        set => SetValue(Use12HoursProperty, value);
    }

    public static readonly StyledProperty<string> PanelFormatProperty = AvaloniaProperty.Register<TimePickerPresenter, string>(
        nameof(PanelFormat));

    public string PanelFormat
    {
        get => GetValue(PanelFormatProperty);
        set => SetValue(PanelFormatProperty, value);
    }

    public TimePickerPresenter()
    {
        SetCurrentValue(TimeProperty, DateTime.Now.TimeOfDay);
    }

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);
        _hourSelector = e.NameScope.Find<DateTimePickerPanel>(PART_HourSelector);
        _minuteSelector = e.NameScope.Find<DateTimePickerPanel>(PART_MinuteSelector);
        _secondSelector = e.NameScope.Find<DateTimePickerPanel>(PART_SecondSelector);
        _ampmSelector = e.NameScope.Find<DateTimePickerPanel>(PART_AmPmSelector);
        _pickerContainer = e.NameScope.Find<Grid>(PART_PickerContainer);
        Initialize();
    }

    private void Initialize()
    {
        if (_pickerContainer is null) return;
        var use12Clock = Use12Hours;
        if (_hourSelector is not null)
        {
            _hourSelector.MaximumValue = use12Clock ? 12 : 23;
            _hourSelector.MinimumValue = use12Clock ? 1 : 0;
            _hourSelector.ItemFormat = "%h";
            var hour = Time?.Hours;
            _hourSelector.SelectedValue = hour ?? 0;
        }
        if(_minuteSelector is not null)
        {
            _minuteSelector.MaximumValue = 59;
            _minuteSelector.MinimumValue = 0;
            _minuteSelector.ItemFormat = "mm";
            var minute = Time?.Minutes;
            _minuteSelector.SelectedValue = minute ?? 0;
        }
        if(_secondSelector is not null)
        {
            _secondSelector.MaximumValue = 59;
            _secondSelector.MinimumValue = 0;
            _secondSelector.ItemFormat = "mm";
            var second = Time?.Seconds;
            _secondSelector.SelectedValue = second ?? 0;
        }
        if(_ampmSelector is not null)
        {
            _ampmSelector.MaximumValue = 1;
            _ampmSelector.MinimumValue = 0;
            _ampmSelector.ItemFormat = "%t";
            var ampm = Time?.Hours switch
            {
                >= 12 => 1,
                _ => 0
            };
            _ampmSelector.SelectedValue = ampm;
        }
    }
}