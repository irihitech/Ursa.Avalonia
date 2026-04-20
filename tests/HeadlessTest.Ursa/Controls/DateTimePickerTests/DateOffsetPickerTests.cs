using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Headless;
using Avalonia.Headless.XUnit;
using Avalonia.Input;
using Avalonia.Layout;
using Avalonia.LogicalTree;
using Avalonia.Threading;
using HeadlessTest.Ursa.TestHelpers;
using Ursa.Controls;

namespace HeadlessTest.Ursa.Controls.DateTimePickerTests;

public class DateOffsetPickerTests
{
    private static DateOffsetPicker CreatePicker() => new()
    {
        Width = 300,
        DisplayFormat = "yyyy-MM-dd",
        HorizontalAlignment = HorizontalAlignment.Left,
        VerticalAlignment = VerticalAlignment.Top
    };

    [AvaloniaFact]
    public void Default_SelectedOffset_Is_First_OffsetDefinition()
    {
        var window = new Window();
        var picker = CreatePicker();
        window.Content = picker;
        window.Show();
        Dispatcher.UIThread.RunJobs();

        Assert.NotNull(picker.SelectedOffset);
        Assert.Equal(picker.OffsetDefinitions?.FirstOrDefault(), picker.SelectedOffset);
    }

    [AvaloniaFact]
    public void OffsetDefinitions_Change_Preserves_Equivalent_SelectedOffset()
    {
        var window = new Window();
        var picker = CreatePicker();
        window.Content = picker;
        window.Show();
        Dispatcher.UIThread.RunJobs();

        var utc = new OffsetDefinition { Offset = OffsetValue.Utc };
        // local is value-equal to the default SelectedOffset (OffsetDefinition.Local)
        var local = new OffsetDefinition { Offset = OffsetValue.Local };
        picker.OffsetDefinitions = new OffsetDefinitions([utc, local]);
        Dispatcher.UIThread.RunJobs();

        // Selection is preserved because local == OffsetDefinition.Local by value
        Assert.NotNull(picker.SelectedOffset);
        Assert.Equal(OffsetValue.Local, picker.SelectedOffset!.Offset);
        
        picker.OffsetDefinitions = new OffsetDefinitions([utc]); // remove local
        Dispatcher.UIThread.RunJobs();
        
        Assert.NotNull(picker.SelectedOffset);
        Assert.Equal(OffsetValue.Utc, picker.SelectedOffset!.Offset); // selection falls back to first
    }

    [AvaloniaFact]
    public void Set_SelectedDate_Updates_TextBox()
    {
        var window = new Window();
        var picker = CreatePicker();
        window.Content = picker;
        window.Show();
        Dispatcher.UIThread.RunJobs();

        picker.SelectedDate = new DateTimeOffset(new DateTime(2025, 6, 15), TimeSpan.Zero);
        Dispatcher.UIThread.RunJobs();

        var textBox = picker.GetTemplateChildOfType<TextBox>(DatePickerBase.PART_TextBox);
        Assert.NotNull(textBox);
        Assert.Equal("2025-06-15", textBox.Text);
    }

    [AvaloniaFact]
    public void Clear_Sets_SelectedDate_To_Null()
    {
        var window = new Window();
        var picker = CreatePicker();
        window.Content = picker;
        window.Show();
        Dispatcher.UIThread.RunJobs();

        picker.SelectedDate = new DateTimeOffset(new DateTime(2025, 6, 15), TimeSpan.Zero);
        Dispatcher.UIThread.RunJobs();
        Assert.NotNull(picker.SelectedDate);

        picker.Clear();
        Dispatcher.UIThread.RunJobs();
        Assert.Null(picker.SelectedDate);
    }

    [AvaloniaFact]
    public void SelectedOffset_Change_Preserves_DateTime_Updates_Offset()
    {
        var window = new Window();
        var picker = CreatePicker();
        var utc = new OffsetDefinition { Offset = OffsetValue.Utc };
        var plus8 = new OffsetDefinition { Offset = OffsetValue.Parse("+08:00") };
        picker.OffsetDefinitions = new OffsetDefinitions([utc, plus8]);
        window.Content = picker;
        window.Show();
        Dispatcher.UIThread.RunJobs();

        picker.SelectedDate = new DateTimeOffset(new DateTime(2025, 6, 15), TimeSpan.Zero);
        Dispatcher.UIThread.RunJobs();
        Assert.Equal(TimeSpan.Zero, picker.SelectedDate!.Value.Offset);

        picker.SelectedOffset = plus8;
        Dispatcher.UIThread.RunJobs();

        Assert.NotNull(picker.SelectedDate);
        Assert.Equal(TimeSpan.FromHours(8), picker.SelectedDate.Value.Offset);
        // local date/time components are preserved
        Assert.Equal(new DateTime(2025, 6, 15), picker.SelectedDate.Value.DateTime);
    }

    [AvaloniaFact]
    public void SelectedOffset_Change_Does_Not_Affect_Null_SelectedDate()
    {
        var window = new Window();
        var picker = CreatePicker();
        var utc = new OffsetDefinition { Offset = OffsetValue.Utc };
        var plus8 = new OffsetDefinition { Offset = OffsetValue.Parse("+08:00") };
        picker.OffsetDefinitions = new OffsetDefinitions([utc, plus8]);
        window.Content = picker;
        window.Show();
        Dispatcher.UIThread.RunJobs();

        Assert.Null(picker.SelectedDate);
        picker.SelectedOffset = plus8;
        Dispatcher.UIThread.RunJobs();

        Assert.Null(picker.SelectedDate);
    }

    [AvaloniaFact]
    public void Click_TextBox_Opens_Popup()
    {
        var window = new Window();
        var picker = CreatePicker();
        window.Content = picker;
        window.Show();
        Dispatcher.UIThread.RunJobs();

        var textBox = picker.GetTemplateChildOfType<TextBox>(DatePickerBase.PART_TextBox);
        Assert.NotNull(textBox);
        var position = textBox.TranslatePoint(new Point(5, 5), window);
        Assert.NotNull(position);

        Assert.False(picker.IsDropdownOpen);
        window.MouseDown(position.Value, MouseButton.Left);
        Dispatcher.UIThread.RunJobs();
        Assert.True(picker.IsDropdownOpen);
    }

    [AvaloniaFact]
    public void ComboBox_Click_Does_Not_Open_Popup()
    {
        var window = new Window();
        var picker = CreatePicker();
        window.Content = picker;
        window.Show();
        Dispatcher.UIThread.RunJobs();

        var comboBox = picker.GetTemplateChildOfType<ComboBox>(DateOffsetPicker.PART_OffsetComboBox);
        Assert.NotNull(comboBox);

        var position = comboBox.TranslatePoint(new Point(5, 5), window);
        Assert.NotNull(position);

        Assert.False(picker.IsDropdownOpen);
        window.MouseDown(position.Value, MouseButton.Left);
        Dispatcher.UIThread.RunJobs();
        Assert.False(picker.IsDropdownOpen);
    }

    [AvaloniaFact]
    public void DateSelected_From_Calendar_Uses_Current_Offset()
    {
        var window = new Window { Width = 800, Height = 800 };
        var picker = CreatePicker();
        var plus8 = new OffsetDefinition { Offset = OffsetValue.Parse("+08:00") };
        picker.OffsetDefinitions = new OffsetDefinitions([plus8]);
        window.Content = picker;
        window.Show();
        Dispatcher.UIThread.RunJobs();

        var textBox = picker.GetTemplateChildOfType<TextBox>(DatePickerBase.PART_TextBox);
        var position = textBox!.TranslatePoint(new Point(5, 5), window);
        window.MouseDown(position!.Value, MouseButton.Left);
        Dispatcher.UIThread.RunJobs();

        var popup = picker.GetTemplateChildOfType<Popup>(DatePickerBase.PART_Popup);
        var calendar = popup?.GetLogicalDescendants().OfType<DatePickerCalendarView>().FirstOrDefault();
        calendar?.RaiseEvent(new DatePickerCalendarDayButtonEventArgs(new DateOnly(2025, 6, 15))
            { RoutedEvent = DatePickerCalendarView.DateSelectedEvent });
        Dispatcher.UIThread.RunJobs();

        Assert.NotNull(picker.SelectedDate);
        Assert.Equal(TimeSpan.FromHours(8), picker.SelectedDate!.Value.Offset);
        Assert.Equal(2025, picker.SelectedDate.Value.DateTime.Year);
        Assert.Equal(6, picker.SelectedDate.Value.DateTime.Month);
        Assert.Equal(15, picker.SelectedDate.Value.DateTime.Day);
    }
}
