using Avalonia;
using Avalonia.Controls;
using Avalonia.Headless;
using Avalonia.Headless.XUnit;
using Avalonia.Input;
using Avalonia.Layout;
using Avalonia.Threading;
using HeadlessTest.Ursa.TestHelpers;
using Ursa.Controls;

namespace HeadlessTest.Ursa.Controls.DateTimePickerTests;

public class DateOffsetRangePickerTests
{
    private static DateOffsetRangePicker CreatePicker() => new()
    {
        Width = 500,
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
    }

    [AvaloniaFact]
    public void Set_SelectedDates_Update_TextBoxes()
    {
        var window = new Window();
        var picker = CreatePicker();
        window.Content = picker;
        window.Show();
        Dispatcher.UIThread.RunJobs();

        picker.SelectedStartDate = new DateTimeOffset(new DateTime(2025, 1, 10), TimeSpan.Zero);
        picker.SelectedEndDate = new DateTimeOffset(new DateTime(2025, 1, 20), TimeSpan.Zero);
        Dispatcher.UIThread.RunJobs();

        var startTextBox = picker.GetTemplateChildOfType<TextBox>(DateRangePickerBase.PART_StartTextBox);
        var endTextBox = picker.GetTemplateChildOfType<TextBox>(DateRangePickerBase.PART_EndTextBox);
        Assert.NotNull(startTextBox);
        Assert.NotNull(endTextBox);
        Assert.Equal("2025-01-10", startTextBox.Text);
        Assert.Equal("2025-01-20", endTextBox.Text);
    }

    [AvaloniaFact]
    public void Clear_Clears_Both_Dates()
    {
        var window = new Window();
        var picker = CreatePicker();
        window.Content = picker;
        window.Show();
        Dispatcher.UIThread.RunJobs();

        picker.SelectedStartDate = new DateTimeOffset(new DateTime(2025, 1, 10), TimeSpan.Zero);
        picker.SelectedEndDate = new DateTimeOffset(new DateTime(2025, 1, 20), TimeSpan.Zero);
        Dispatcher.UIThread.RunJobs();
        Assert.NotNull(picker.SelectedStartDate);
        Assert.NotNull(picker.SelectedEndDate);

        picker.Clear();
        Dispatcher.UIThread.RunJobs();
        Assert.Null(picker.SelectedStartDate);
        Assert.Null(picker.SelectedEndDate);
    }

    [AvaloniaFact]
    public void SelectedOffset_Change_Preserves_DateTime_Updates_Offset_On_Both_Dates()
    {
        var window = new Window();
        var picker = CreatePicker();
        var utc = new OffsetDefinition { Offset = OffsetValue.Utc };
        var plus8 = new OffsetDefinition { Offset = OffsetValue.Parse("+08:00") };
        picker.OffsetDefinitions = new OffsetDefinitions([utc, plus8]);
        window.Content = picker;
        window.Show();
        Dispatcher.UIThread.RunJobs();

        picker.SelectedStartDate = new DateTimeOffset(new DateTime(2025, 3, 1), TimeSpan.Zero);
        picker.SelectedEndDate = new DateTimeOffset(new DateTime(2025, 3, 31), TimeSpan.Zero);
        Dispatcher.UIThread.RunJobs();

        picker.SelectedOffset = plus8;
        Dispatcher.UIThread.RunJobs();

        Assert.NotNull(picker.SelectedStartDate);
        Assert.NotNull(picker.SelectedEndDate);
        Assert.Equal(TimeSpan.FromHours(8), picker.SelectedStartDate!.Value.Offset);
        Assert.Equal(TimeSpan.FromHours(8), picker.SelectedEndDate!.Value.Offset);
        Assert.Equal(new DateTime(2025, 3, 1), picker.SelectedStartDate.Value.DateTime);
        Assert.Equal(new DateTime(2025, 3, 31), picker.SelectedEndDate.Value.DateTime);
    }

    [AvaloniaFact]
    public void SelectedOffset_Change_Does_Not_Affect_Null_Dates()
    {
        var window = new Window();
        var picker = CreatePicker();
        var utc = new OffsetDefinition { Offset = OffsetValue.Utc };
        var plus8 = new OffsetDefinition { Offset = OffsetValue.Parse("+08:00") };
        picker.OffsetDefinitions = new OffsetDefinitions([utc, plus8]);
        window.Content = picker;
        window.Show();
        Dispatcher.UIThread.RunJobs();

        Assert.Null(picker.SelectedStartDate);
        Assert.Null(picker.SelectedEndDate);
        picker.SelectedOffset = plus8;
        Dispatcher.UIThread.RunJobs();

        Assert.Null(picker.SelectedStartDate);
        Assert.Null(picker.SelectedEndDate);
    }

    [AvaloniaFact]
    public void Click_StartTextBox_Opens_Popup()
    {
        var window = new Window();
        var picker = CreatePicker();
        window.Content = picker;
        window.Show();
        Dispatcher.UIThread.RunJobs();

        var startTextBox = picker.GetTemplateChildOfType<TextBox>(DateRangePickerBase.PART_StartTextBox);
        Assert.NotNull(startTextBox);
        var position = startTextBox.TranslatePoint(new Point(5, 5), window);
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

        var comboBox = picker.GetTemplateChildOfType<ComboBox>(DateOffsetRangePicker.PART_OffsetComboBox);
        Assert.NotNull(comboBox);
        var position = comboBox.TranslatePoint(new Point(5, 5), window);
        Assert.NotNull(position);

        Assert.False(picker.IsDropdownOpen);
        window.MouseDown(position.Value, MouseButton.Left);
        Dispatcher.UIThread.RunJobs();
        Assert.False(picker.IsDropdownOpen);
    }
}
