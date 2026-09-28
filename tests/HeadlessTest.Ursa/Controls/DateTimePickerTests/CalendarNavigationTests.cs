using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Headless;
using Avalonia.Headless.XUnit;
using Avalonia.Input;
using Avalonia.LogicalTree;
using Avalonia.Threading;
using Avalonia.VisualTree;
using HeadlessTest.Ursa.TestHelpers;
using Ursa.Controls;
using DatePicker = Ursa.Controls.DatePicker;

namespace HeadlessTest.Ursa.Controls.DateTimePickerTests;

public class CalendarNavigationTests
{
    public static IEnumerable<object[]> PickerCases()
    {
        Type[] types =
        [
            typeof(DatePicker), typeof(DateOnlyPicker), typeof(DateOffsetPicker),
            typeof(DateTimePicker), typeof(DateTimeOffsetPicker),
            typeof(DateRangePicker), typeof(DateOnlyRangePicker), typeof(DateOffsetRangePicker)
        ];
        foreach (var type in types)
        foreach (var viaMonth in new[] { false, true })
            yield return [type, viaMonth];
    }

    [AvaloniaTheory]
    [MemberData(nameof(PickerCases))]
    public void Header_Navigation_Preserves_Context_And_Focus(Type pickerType, bool viaMonth)
    {
        var picker = Assert.IsAssignableFrom<TemplatedControl>(Activator.CreateInstance(pickerType));
        picker.Width = 500;
        var window = new Window { Content = picker };
        window.Show();
        Dispatcher.UIThread.RunJobs();
        var textBoxName = picker is DateRangePickerBase
            ? DateRangePickerBase.PART_StartTextBox
            : DatePickerBase.PART_TextBox;
        var textBox = picker.GetTemplateChildOfType<TextBox>(textBoxName);
        Assert.NotNull(textBox);
        textBox.Focus();
        Dispatcher.UIThread.RunJobs();
        var popup = picker.GetTemplateChildOfType<Popup>(DatePickerBase.PART_Popup);
        Assert.NotNull(popup);
        var calendar = popup.GetLogicalDescendants().OfType<DatePickerCalendarView>().First();
        calendar.SyncContextDate(new DatePickerCalendarContext(1987, 5));

        if (viaMonth)
        {
            ClickButton(calendar, DatePickerCalendarView.PART_MonthButton);
            Assert.Equal(DatePickerCalendarViewMode.Year, calendar.Mode);
            Assert.Equal(1987, calendar.ContextDate.Year);
            Assert.True(calendar.GetTemplateChildOfType<Button>(DatePickerCalendarView.PART_HeaderButton)?.IsFocused);
            ClickButton(calendar, DatePickerCalendarView.PART_HeaderButton);
        }
        else
        {
            ClickButton(calendar, DatePickerCalendarView.PART_YearButton);
        }
        Assert.Equal(DatePickerCalendarViewMode.Decade, calendar.Mode);
        Assert.Equal(1980, calendar.ContextDate.StartYear);
        Assert.Equal(1989, calendar.ContextDate.EndYear);
        Assert.True(calendar.GetTemplateChildOfType<Button>(DatePickerCalendarView.PART_HeaderButton)?.IsFocused);

        ClickButton(calendar, DatePickerCalendarView.PART_HeaderButton);
        Assert.Equal(DatePickerCalendarViewMode.Century, calendar.Mode);
        Assert.Equal(1900, calendar.ContextDate.StartYear);
        Assert.Equal(2000, calendar.ContextDate.EndYear);
        Assert.True(popup.IsOpen);

        Click(calendar.GetVisualDescendants().OfType<DatePickerCalendarYearButton>()
            .Single(b => b.DatePickerCalendarContext.StartYear == 1980));
        Assert.Equal(DatePickerCalendarViewMode.Decade, calendar.Mode);
        Click(calendar.GetVisualDescendants().OfType<DatePickerCalendarYearButton>()
            .Single(b => b.DatePickerCalendarContext.Year == 1987));
        Assert.Equal(DatePickerCalendarViewMode.Year, calendar.Mode);
        Click(calendar.GetVisualDescendants().OfType<DatePickerCalendarYearButton>()
            .Single(b => b.DatePickerCalendarContext.Month == 5));
        Assert.Equal(DatePickerCalendarViewMode.Month, calendar.Mode);
        Assert.Equal(1987, calendar.ContextDate.Year);
        Assert.Equal(5, calendar.ContextDate.Month);
        Assert.True(popup.IsOpen);
        Assert.True(calendar.GetTemplateChildOfType<Button>(DatePickerCalendarView.PART_MonthButton)?.IsFocused);
        window.Close();
    }

    [AvaloniaTheory]
    [InlineData(false)]
    [InlineData(true)]
    public void Reopening_Popup_Preserves_Valid_Year_Range(bool century)
    {
        var picker = new DatePicker { SelectedDate = new DateTime(1987, 5, 12) };
        var window = new Window { Content = picker };
        window.Show();
        Dispatcher.UIThread.RunJobs();
        var textBox = picker.GetTemplateChildOfType<TextBox>(DatePickerBase.PART_TextBox);
        Assert.NotNull(textBox);
        textBox.Focus();
        Dispatcher.UIThread.RunJobs();
        var popup = picker.GetTemplateChildOfType<Popup>(DatePickerBase.PART_Popup);
        Assert.NotNull(popup);
        var calendar = Assert.Single(popup.GetLogicalDescendants().OfType<DatePickerCalendarView>());
        ClickButton(calendar, DatePickerCalendarView.PART_YearButton);
        if (century)
            ClickButton(calendar, DatePickerCalendarView.PART_HeaderButton);

        picker.Dismiss();
        Dispatcher.UIThread.RunJobs();
        Click(textBox);
        Assert.True(popup.IsOpen);
        Assert.Equal(century ? DatePickerCalendarViewMode.Century : DatePickerCalendarViewMode.Decade, calendar.Mode);
        Assert.Equal(century ? 1900 : 1980, calendar.ContextDate.StartYear);
        Assert.Equal(century ? 2000 : 1989, calendar.ContextDate.EndYear);
        ClickButton(calendar, DatePickerCalendarView.PART_HeaderButton);
        Assert.Equal(DatePickerCalendarViewMode.Century, calendar.Mode);
        window.Close();
    }

    private static void ClickButton(DatePickerCalendarView calendar, string name)
    {
        var button = calendar.GetTemplateChildOfType<Button>(name);
        Assert.NotNull(button);
        Click(button);
    }

    private static void Click(Control button)
    {
        var root = TopLevel.GetTopLevel(button);
        Assert.NotNull(root);
        var position = button.TranslatePoint(new Point(button.Bounds.Width / 2, button.Bounds.Height / 2), root);
        Assert.NotNull(position);
        root.MouseDown(position.Value, MouseButton.Left);
        root.MouseUp(position.Value, MouseButton.Left);
        Dispatcher.UIThread.RunJobs();
    }
}
