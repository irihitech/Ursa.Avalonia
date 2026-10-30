using Avalonia;
using Avalonia.Controls;
using Avalonia.Headless;
using Avalonia.Headless.XUnit;
using Avalonia.Layout;
using Avalonia.Threading;
using HeadlessTest.Ursa.TestHelpers;
using Ursa.Controls;
using DateRangePicker = Ursa.Controls.DateRangePicker;

namespace HeadlessTest.Ursa.Controls.DateTimePickerTests;

public class DateRangePickerTests
{
    [AvaloniaFact]
    public void Clear_Removes_Invalid_Manual_Input()
    {
        var window = new Window();
        var picker = new DateRangePicker()
        {
            Width = 400,
            DisplayFormat = "yyyy-MM-dd",
            HorizontalAlignment = HorizontalAlignment.Left,
            VerticalAlignment = VerticalAlignment.Top
        };
        var focusTarget = new TextBox();
        window.Content = new StackPanel()
        {
            Children =
            {
                picker,
                focusTarget,
            }
        };
        window.Show();
        Dispatcher.UIThread.RunJobs();
        var startTextBox = picker.GetTemplateChildOfType<TextBox>(DateRangePickerBase.PART_StartTextBox);
        var endTextBox = picker.GetTemplateChildOfType<TextBox>(DateRangePickerBase.PART_EndTextBox);
        Assert.NotNull(startTextBox);
        Assert.NotNull(endTextBox);

        startTextBox.Focus();
        Dispatcher.UIThread.RunJobs();
        startTextBox.SetValue(TextBox.TextProperty, "not a date");
        endTextBox.SetValue(TextBox.TextProperty, "not a date");
        focusTarget.Focus();
        Dispatcher.UIThread.RunJobs();

        // The invalid input is rejected but stays in the text boxes.
        Assert.Null(picker.SelectedStartDate);
        Assert.Null(picker.SelectedEndDate);

        picker.Clear();
        Dispatcher.UIThread.RunJobs();

        Assert.Null(picker.SelectedStartDate);
        Assert.Null(picker.SelectedEndDate);
        Assert.True(string.IsNullOrEmpty(startTextBox.Text));
        Assert.True(string.IsNullOrEmpty(endTextBox.Text));
    }

    [AvaloniaFact]
    public void Clear_Removes_Valid_Manual_Input()
    {
        var window = new Window();
        var picker = new DateRangePicker()
        {
            Width = 400,
            DisplayFormat = "yyyy-MM-dd",
            HorizontalAlignment = HorizontalAlignment.Left,
            VerticalAlignment = VerticalAlignment.Top
        };
        var focusTarget = new TextBox();
        window.Content = new StackPanel()
        {
            Children =
            {
                picker,
                focusTarget,
            }
        };
        window.Show();
        Dispatcher.UIThread.RunJobs();
        var startTextBox = picker.GetTemplateChildOfType<TextBox>(DateRangePickerBase.PART_StartTextBox);
        var endTextBox = picker.GetTemplateChildOfType<TextBox>(DateRangePickerBase.PART_EndTextBox);
        Assert.NotNull(startTextBox);
        Assert.NotNull(endTextBox);

        startTextBox.Focus();
        Dispatcher.UIThread.RunJobs();
        startTextBox.SetValue(TextBox.TextProperty, "2025-02-17");
        endTextBox.Focus();
        Dispatcher.UIThread.RunJobs();
        endTextBox.SetValue(TextBox.TextProperty, "2025-02-20");
        focusTarget.Focus();
        Dispatcher.UIThread.RunJobs();

        Assert.Equal(new DateTime(2025, 2, 17), picker.SelectedStartDate);
        Assert.Equal(new DateTime(2025, 2, 20), picker.SelectedEndDate);
        Assert.Equal("2025-02-17", startTextBox.Text);
        Assert.Equal("2025-02-20", endTextBox.Text);

        picker.Clear();
        Dispatcher.UIThread.RunJobs();

        Assert.Null(picker.SelectedStartDate);
        Assert.Null(picker.SelectedEndDate);
        Assert.True(string.IsNullOrEmpty(startTextBox.Text));
        Assert.True(string.IsNullOrEmpty(endTextBox.Text));
    }
}
