using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Controls.Templates;
using Avalonia.Headless.XUnit;
using Avalonia.Threading;
using TimePickerPresenter = Ursa.Controls.TimePickerPresenter;

namespace HeadlessTest.Ursa.Controls.DateTimePickerTests;

public class TimePickerPresenterTests
{
    [AvaloniaFact]
    public void TimePickerPresenter_DefaultValues_ShouldBeCorrect()
    {
        var presenter = new TimePickerPresenter();

        Assert.Equal(1, presenter.MinuteIncrement);
        Assert.Equal(1, presenter.SecondIncrement);
        Assert.Null(presenter.TimeHolder);
        Assert.Equal("HH mm ss t", presenter.PanelFormat);
    }

    [AvaloniaFact]
    public void TimePickerPresenter_SetTime_ShouldUpdateTimeProperty()
    {
        var presenter = new TimePickerPresenter();
        var time = new TimeOnly(10, 30, 45);

        presenter.TimeHolder = time;

        Assert.Equal(time, presenter.TimeHolder);
    }

    [AvaloniaTheory]
    [InlineData("hh mm ss", 0, 2, 4, 0, true, true, true, false)]
    [InlineData("hh mm ss t", 0, 2, 4, 6, true, true, true, true)]
    [InlineData("hh mm", 0, 2, 0, 0, true, true, false, false)]
    [InlineData("mm ss", 0, 0, 2, 0, false, true, true, false)]
    [InlineData("ss", 0, 0, 0, 0, false, false, true, false)]
    [InlineData("t", 0, 0, 0, 0, false, false, false, true)]
    public void TimePickerPresenter_SetPanelFormat_ShouldUpdatePanelLayout(
        string format, int hourColumn, int minuteColumn, int secondColumn, int amColumn,
        bool isHourPanelVisible, bool isMinutePanelVisible, bool isSecondPanelVisible, bool isAmPanelVisible
        )
    {
        var window = new Window();
        var presenter = new TimePickerPresenter();
        window.Content = presenter;
        window.Show();
        Dispatcher.UIThread.RunJobs();
        presenter.PanelFormat = format;
        Dispatcher.UIThread.RunJobs();
        
        Assert.Equal(format, presenter.PanelFormat);
        
        var hourPanel = presenter.GetTemplateChildren().FirstOrDefault(a => a.Name == TimePickerPresenter.PART_HourScrollPanel);
        var minutePanel = presenter.GetTemplateChildren().FirstOrDefault(a => a.Name == TimePickerPresenter.PART_MinuteScrollPanel);
        var secondPanel = presenter.GetTemplateChildren().FirstOrDefault(a => a.Name == TimePickerPresenter.PART_SecondScrollPanel);
        var amPanel = presenter.GetTemplateChildren().FirstOrDefault(a => a.Name == TimePickerPresenter.PART_AmPmScrollPanel);

        Assert.NotNull(hourPanel);
        Assert.NotNull(minutePanel);
        Assert.NotNull(secondPanel);
        Assert.NotNull(amPanel);
        
        Assert.Equal(hourColumn, Grid.GetColumn(hourPanel));
        Assert.Equal(minuteColumn, Grid.GetColumn(minutePanel));
        Assert.Equal(secondColumn, Grid.GetColumn(secondPanel));
        Assert.Equal(amColumn, Grid.GetColumn(amPanel));
        
        Assert.Equal(isHourPanelVisible, hourPanel.IsVisible);
        Assert.Equal(isMinutePanelVisible, minutePanel.IsVisible);
        Assert.Equal(isSecondPanelVisible, secondPanel.IsVisible);
        Assert.Equal(isAmPanelVisible, amPanel.IsVisible);
    }
    
    

    [AvaloniaFact]
    public void TimePickerPresenter_Confirm_IsObsolete_ButStillFiresEvent()
    {
        var presenter = new TimePickerPresenter();
        var time = new TimeOnly(10, 30, 45);
        var eventRaised = 0;
        TimeOnly? eventResult = null;
        presenter.SelectedTimeChanged += (o, e) =>
        {
            eventRaised++;
            eventResult = e.NewTime;
        };
        presenter.TimeHolder = time;
        // Confirm on the presenter now fires a backward-compat event with TimeHolder value
#pragma warning disable CS0618
        presenter.Confirm();
#pragma warning restore CS0618
        Assert.Equal(time, eventResult);
        Assert.Equal(1, eventRaised);
    }

    [AvaloniaFact]
    public void TimePickerPresenter_SyncTime_Should_Not_RaiseEvent()
    {
        var presenter = new TimePickerPresenter();
        var oldTime = new TimeOnly(10, 30, 45);
        var newTime = new TimeOnly(11, 45, 30);
        presenter.SyncTime(oldTime);
        var eventRaised = false;
        presenter.SelectedTimeChanged += (sender, args) =>
        {
            eventRaised = true;
        };

        presenter.SyncTime(newTime);
        Assert.False(eventRaised);
    }

    [AvaloniaTheory]
    [MemberData(nameof(GetSelectionMemberData))]
    public void TimePickerPresenter_Time_Updated_When_Panel_Selection_Changed(string format, int hourSelection, int minuteSelection, int secondSelection, int amSelection, TimeOnly expectedTime)
    {
        var window = new Window();
        var presenter = new TimePickerPresenter();
        window.Content = presenter;
        window.Show();
        Dispatcher.UIThread.RunJobs();
        presenter.PanelFormat = format;
        TimeOnly? eventResult = null;
        presenter.SelectedTimeChanged += (o, e) =>
        {
            eventResult = e.NewTime;
        };
        Dispatcher.UIThread.RunJobs();
        
        var hourPanel = presenter.GetTemplateChildren().OfType<DateTimePickerPanel>().FirstOrDefault(a => a.Name == TimePickerPresenter.PART_HourSelector);
        var minutePanel = presenter.GetTemplateChildren().OfType<DateTimePickerPanel>().FirstOrDefault(a => a.Name == TimePickerPresenter.PART_MinuteSelector);
        var secondPanel = presenter.GetTemplateChildren().OfType<DateTimePickerPanel>().FirstOrDefault(a => a.Name == TimePickerPresenter.PART_SecondSelector);
        var amPanel = presenter.GetTemplateChildren().OfType<DateTimePickerPanel>().FirstOrDefault(a => a.Name == TimePickerPresenter.PART_AmPmSelector);
        
        Assert.NotNull(hourPanel);
        Assert.NotNull(minutePanel);
        Assert.NotNull(secondPanel);
        Assert.NotNull(amPanel);
        
        hourPanel.SelectedValue = hourSelection;
        minutePanel.SelectedValue = minuteSelection;
        secondPanel.SelectedValue = secondSelection;
        amPanel.SelectedValue = amSelection;
        
        Assert.Equal(expectedTime, eventResult);        
    }
    
    [AvaloniaTheory]
    [MemberData(nameof(GetSelectionMemberData))]
    public void TimePickerPresenter_TimeHolder_Updated_When_Panel_Selection_Changed(string format, int hourSelection, int minuteSelection, int secondSelection, int amSelection, TimeOnly expectedTime)
    {
        var window = new Window();
        var presenter = new TimePickerPresenter();
        window.Content = presenter;
        window.Show();
        Dispatcher.UIThread.RunJobs();
        presenter.PanelFormat = format;
        Dispatcher.UIThread.RunJobs();
        
        var hourPanel = presenter.GetTemplateChildren().OfType<DateTimePickerPanel>().FirstOrDefault(a => a.Name == TimePickerPresenter.PART_HourSelector);
        var minutePanel = presenter.GetTemplateChildren().OfType<DateTimePickerPanel>().FirstOrDefault(a => a.Name == TimePickerPresenter.PART_MinuteSelector);
        var secondPanel = presenter.GetTemplateChildren().OfType<DateTimePickerPanel>().FirstOrDefault(a => a.Name == TimePickerPresenter.PART_SecondSelector);
        var amPanel = presenter.GetTemplateChildren().OfType<DateTimePickerPanel>().FirstOrDefault(a => a.Name == TimePickerPresenter.PART_AmPmSelector);
        
        Assert.NotNull(hourPanel);
        Assert.NotNull(minutePanel);
        Assert.NotNull(secondPanel);
        Assert.NotNull(amPanel);
        
        hourPanel.SelectedValue = hourSelection;
        minutePanel.SelectedValue = minuteSelection;
        secondPanel.SelectedValue = secondSelection;
        amPanel.SelectedValue = amSelection;
        
        Assert.Equal(expectedTime, presenter.TimeHolder);        
    }

    public static IEnumerable<object[]> GetSelectionMemberData()
    {
        yield return new object[] { "hh mm ss t", 1, 1, 1, 1, new TimeOnly(13, 1, 1) };
        yield return new object[] { "HH mm ss t", 12, 30, 45, 0, new TimeOnly(12, 30, 45) };
        yield return new object[] { "HH mm ss t", 12, 0, 0, 1, new TimeOnly(12, 0, 0) };
        yield return new object[] { "HH mm ss t", 13, 0, 0, 1, new TimeOnly(13, 0, 0) };
        yield return new object[] { "hh mm ss t", 9, 0, 0, 0, new TimeOnly(9, 0, 0) };
        yield return new object[] { "hh mm ss t", 9, 0, 0, 1, new TimeOnly(21, 0, 0) };
        yield return new object[] { "HH mm ss t", 0, 0, 0, 0, new TimeOnly(0, 0, 0) };
    }
}
