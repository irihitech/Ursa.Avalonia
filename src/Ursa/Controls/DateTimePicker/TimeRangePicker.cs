using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Primitives;
using Irihi.Avalonia.Shared.Common;

namespace Ursa.Controls;

[TemplatePart(PART_StartTextBox, typeof(TextBox))]
[TemplatePart(PART_EndTextBox, typeof(TextBox))]
[TemplatePart(PartNames.PART_Popup, typeof(Popup))]
[TemplatePart(PART_StartPresenter, typeof(TimePickerPresenter))]
[TemplatePart(PART_EndPresenter, typeof(TimePickerPresenter))]
[TemplatePart(PART_Button, typeof(Button))]
public class TimeRangePicker: TemplatedControl
{
    public const string PART_StartTextBox = "PART_StartTextBox";
    public const string PART_EndTextBox = "PART_EndTextBox";
    public const string PART_StartPresenter = "PART_StartPresenter";
    public const string PART_EndPresenter = "PART_EndPresenter";
    public const string PART_Button = "PART_Button";

    public static readonly StyledProperty<TimeSpan?> StartTimeProperty = AvaloniaProperty.Register<TimeRangePicker, TimeSpan?>(
        nameof(StartTime));

    public TimeSpan? StartTime
    {
        get => GetValue(StartTimeProperty);
        set => SetValue(StartTimeProperty, value);
    }

    public static readonly StyledProperty<TimeSpan?> EndTimeProperty = AvaloniaProperty.Register<TimeRangePicker, TimeSpan?>(
        nameof(EndTime));

    public TimeSpan? EndTime
    {
        get => GetValue(EndTimeProperty);
        set => SetValue(EndTimeProperty, value);
    }
}