using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Templates;
using Avalonia.Headless;
using Avalonia.Headless.XUnit;
using Avalonia.Input;
using Avalonia.Threading;
using Avalonia.VisualTree;
using Ursa.Controls;

namespace HeadlessTest.Ursa.Controls.DateTimePicker;

public class CalendarViewTests
{
    [AvaloniaFact]
    public void OnNext_MonthMode_UpdatesToNextMonth()
    {
        var window = new Window();
        var calendarView = new CalendarView
            { Mode = CalendarViewMode.Month };
        window.Content = calendarView;
        window.Show();
        Dispatcher.UIThread.RunJobs();
        calendarView.SyncContextDate(new CalendarContext(2023, 5));
        Dispatcher.UIThread.RunJobs();
        calendarView.ClickNext();
        Assert.Equal(6, calendarView.ContextDate.Month);
        Assert.Equal(2023, calendarView.ContextDate.Year);
    }

    [AvaloniaFact]
    public void OnNext_MonthMode_FastForward_UpdatesToNextYear()
    {
        var window = new Window();
        var calendarView = new CalendarView
            { Mode = CalendarViewMode.Month };
        window.Content = calendarView;
        window.Show();
        Dispatcher.UIThread.RunJobs();
        calendarView.SyncContextDate(new CalendarContext(2023, 5));
        Dispatcher.UIThread.RunJobs();
        calendarView.ClickFastNext();
        Assert.Equal(5, calendarView.ContextDate.Month);
        Assert.Equal(2024, calendarView.ContextDate.Year);
    }

    [AvaloniaFact]
    public void OnNext_YearMode_UpdatesToNextYear()
    {
        var window = new Window();
        var calendarView = new CalendarView
            { Mode = CalendarViewMode.Year };
        window.Content = calendarView;
        window.Show();
        Dispatcher.UIThread.RunJobs();
        calendarView.SyncContextDate(new CalendarContext(2023, 5));
        Dispatcher.UIThread.RunJobs();
        calendarView.ClickNext();
        Assert.Equal(5, calendarView.ContextDate.Month);
        Assert.Equal(2024, calendarView.ContextDate.Year);
    }

    [AvaloniaFact]
    public void OnNext_DecadeMode_UpdatesToNextDecade()
    {
        var window = new Window();
        var calendarView = new CalendarView { Mode = CalendarViewMode.Decade };
        window.Content = calendarView;
        window.Show();
        Dispatcher.UIThread.RunJobs();
        calendarView.SyncContextDate(new CalendarContext(null, null, 2000, 2009));
        Dispatcher.UIThread.RunJobs();
        calendarView.ClickNext();
        Assert.Equal(2010, calendarView.ContextDate.StartYear);
        Assert.Equal(2019, calendarView.ContextDate.EndYear);
    }

    [AvaloniaFact]
    public void OnNext_CenturyMode_UpdatesToNextCentury()
    {
        var window = new Window();
        var calendarView = new CalendarView { Mode = CalendarViewMode.Century };
        window.Content = calendarView;
        window.Show();
        Dispatcher.UIThread.RunJobs();
        calendarView.SyncContextDate(new CalendarContext(null, null, 1900, 1999));
        Dispatcher.UIThread.RunJobs();
        calendarView.ClickNext();
        Assert.Equal(2000, calendarView.ContextDate.StartYear);
        Assert.Equal(2099, calendarView.ContextDate.EndYear);
    }

    [AvaloniaFact]
    public void OnPrevious_MonthMode_UpdatesToPreviousMonth()
    {
        var window = new Window();
        var calendarView = new CalendarView { Mode = CalendarViewMode.Month };
        window.Content = calendarView;
        window.Show();
        Dispatcher.UIThread.RunJobs();
        calendarView.SyncContextDate(new CalendarContext(2023, 5));
        Dispatcher.UIThread.RunJobs();
        calendarView.ClickPrevious();
        Assert.Equal(4, calendarView.ContextDate.Month);
        Assert.Equal(2023, calendarView.ContextDate.Year);
    }

    [AvaloniaFact]
    public void OnPrevious_YearMode_UpdatesToPreviousYear()
    {
        var window = new Window();
        var calendarView = new CalendarView { Mode = CalendarViewMode.Year };
        window.Content = calendarView;
        window.Show();
        Dispatcher.UIThread.RunJobs();
        calendarView.SyncContextDate(new CalendarContext(2023, 5));
        Dispatcher.UIThread.RunJobs();
        calendarView.ClickPrevious();
        Assert.Equal(2022, calendarView.ContextDate.Year);
    }

    [AvaloniaFact]
    public void OnPrevious_DecadeMode_UpdatesToPreviousDecade()
    {
        var window = new Window();
        var calendarView = new CalendarView { Mode = CalendarViewMode.Decade };
        window.Content = calendarView;
        window.Show();
        Dispatcher.UIThread.RunJobs();
        calendarView.SyncContextDate(new CalendarContext(null, null, 2000, 2009));
        Dispatcher.UIThread.RunJobs();
        calendarView.ClickPrevious();
        Assert.Equal(1990, calendarView.ContextDate.StartYear);
        Assert.Equal(1999, calendarView.ContextDate.EndYear);
    }

    [AvaloniaFact]
    public void OnPrevious_CenturyMode_UpdatesToPreviousCentury()
    {
        var window = new Window();
        var calendarView = new CalendarView { Mode = CalendarViewMode.Century };
        window.Content = calendarView;
        window.Show();
        Dispatcher.UIThread.RunJobs();
        calendarView.SyncContextDate(new CalendarContext(null, null, 1900, 1999));
        Dispatcher.UIThread.RunJobs();
        calendarView.ClickPrevious();
        Assert.Equal(1800, calendarView.ContextDate.StartYear);
        Assert.Equal(1899, calendarView.ContextDate.EndYear);
    }

    [AvaloniaFact]
    public void OnFastPrevious_MonthMode_UpdatesToPreviousYear()
    {
        var window = new Window();
        var calendarView = new CalendarView { Mode = CalendarViewMode.Month };
        window.Content = calendarView;
        window.Show();
        Dispatcher.UIThread.RunJobs();
        calendarView.SyncContextDate(new CalendarContext(2023, 5));
        Dispatcher.UIThread.RunJobs();
        calendarView.ClickFastPrevious();
        Assert.Equal(2022, calendarView.ContextDate.Year);
    }

    [AvaloniaFact]
    public void OnHeaderButtonClick_YearMode_SwitchesToDecadeMode()
    {
        var window = new Window();
        var calendarView = new CalendarView { Mode = CalendarViewMode.Year };
        window.Content = calendarView;
        window.Show();
        Dispatcher.UIThread.RunJobs();
        calendarView.SyncContextDate(new CalendarContext(2023, 5));
        Dispatcher.UIThread.RunJobs();
        calendarView.ClickHeaderButton();
        Assert.Equal(CalendarViewMode.Decade, calendarView.Mode);
        Assert.Equal(2020, calendarView.ContextDate.StartYear);
        Assert.Equal(2029, calendarView.ContextDate.EndYear);
    }

    [AvaloniaFact]
    public void OnHeaderButtonClick_DecadeMode_SwitchesToCenturyMode()
    {
        var window = new Window();
        var calendarView = new CalendarView { Mode = CalendarViewMode.Decade };
        window.Content = calendarView;
        window.Show();
        Dispatcher.UIThread.RunJobs();
        calendarView.SyncContextDate(new CalendarContext(null, null, 2000, 2009));
        Dispatcher.UIThread.RunJobs();
        calendarView.ClickHeaderButton();
        Assert.Equal(CalendarViewMode.Century, calendarView.Mode);
    }

    [AvaloniaFact]
    public void OnYearItemSelected_CenturyMode_SwitchesToDecadeMode()
    {
        var window = new Window();
        var calendarView = new CalendarView { Mode = CalendarViewMode.Century };
        window.Content = calendarView;
        window.Show();
        Dispatcher.UIThread.RunJobs();
        calendarView.SyncContextDate(new CalendarContext(null, null, 1900, 1999));
        Dispatcher.UIThread.RunJobs();
        var yearButton = calendarView.FindDescendantOfType<CalendarYearButton>();
        var position = yearButton?.TranslatePoint(new Point(6, 6), window);
        Assert.NotNull(position);
        window.MouseUp(position.Value, MouseButton.Left);
        Assert.Equal(CalendarViewMode.Decade, calendarView.Mode);
    }

    [AvaloniaFact]
    public void OnYearItemSelected_DecadeMode_SwitchesToYearMode()
    {
        var window = new Window();
        var calendarView = new CalendarView { Mode = CalendarViewMode.Decade };
        window.Content = calendarView;
        window.Show();
        Dispatcher.UIThread.RunJobs();
        calendarView.SyncContextDate(new CalendarContext(null, null, 2000, 2009));
        Dispatcher.UIThread.RunJobs();
        var yearButton = calendarView.FindDescendantOfType<CalendarYearButton>();
        var position = yearButton?.TranslatePoint(new Point(6, 6), window);
        Assert.NotNull(position);
        window.MouseUp(position.Value, MouseButton.Left);
        Assert.Equal(CalendarViewMode.Year, calendarView.Mode);
    }

    [AvaloniaFact]
    public void OnYearItemSelected_YearMode_SwitchesToMonthMode()
    {
        var window = new Window();
        var calendarView = new CalendarView { Mode = CalendarViewMode.Year };
        window.Content = calendarView;
        window.Show();
        Dispatcher.UIThread.RunJobs();
        calendarView.SyncContextDate(new CalendarContext(2023, 5));
        Dispatcher.UIThread.RunJobs();
        var yearButton = calendarView.FindDescendantOfType<CalendarYearButton>();
        var position = yearButton?.TranslatePoint(new Point(6, 6), window);
        Assert.NotNull(position);
        window.MouseUp(position.Value, MouseButton.Left);
        Assert.Equal(CalendarViewMode.Month, calendarView.Mode);
    }
    
    [AvaloniaFact]
    public void Update_FirstDayOfWeek_UpdatesCalendar()
    {
        var window = new Window();
        var calendarView = new CalendarView { Mode = CalendarViewMode.Month, FirstDayOfWeek = DayOfWeek.Sunday};
        window.Content = calendarView;
        window.Show();
        Dispatcher.UIThread.RunJobs();
        calendarView.SyncContextDate(new CalendarContext(2023, 5));
        Dispatcher.UIThread.RunJobs();
        var monthGrid = calendarView.GetTemplateChildren()
                                    .FirstOrDefault(a => a is Grid && a.Name == CalendarView.PART_MonthGrid) as Grid;
        Assert.NotNull(monthGrid);
        var dayHeader = monthGrid.Children.OfType<TextBlock>().FirstOrDefault();
        var dayButton = monthGrid.Children.OfType<CalendarDayButton>().FirstOrDefault();
        Assert.NotNull(dayHeader);
        Assert.NotNull(dayButton);
        Assert.Equal('S', dayHeader.Text?[0]);
        Assert.Equal(30, (dayButton.DataContext as DateTime?)?.Day);
        
        calendarView.FirstDayOfWeek = DayOfWeek.Tuesday;
        
        dayHeader = monthGrid.Children.OfType<TextBlock>().FirstOrDefault();
        dayButton = monthGrid.Children.OfType<CalendarDayButton>().FirstOrDefault();
        Assert.NotNull(dayHeader);
        Assert.NotNull(dayButton);
        Assert.Equal('T', dayHeader.Text?[0]);
        Assert.Equal(25, (dayButton.DataContext as DateTime?)?.Day);
        
    }
    
}