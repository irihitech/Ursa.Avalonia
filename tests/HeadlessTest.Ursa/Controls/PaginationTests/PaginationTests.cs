using Avalonia;
using Avalonia.Controls;
using Avalonia.Headless.XUnit;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml.MarkupExtensions;
using Avalonia.Threading;
using HeadlessTest.Ursa.TestHelpers;
using Ursa.Controls;

namespace HeadlessTest.Ursa.Controls.PaginationTests;

public class PaginationTests
{
    [AvaloniaFact]
    public void CurrentPage_Can_Be_Initialized()
    {
        var window = new Window();
        var pagination = new Pagination
        {
            CurrentPage = 3,
            PageSize = 50,
            TotalCount = 300,
        };
        window.Content = pagination;
        window.Show();
        Dispatcher.UIThread.RunJobs();
        Assert.Equal(6, pagination.PageCount);
        Assert.Equal(300, pagination.TotalCount);
        Assert.Equal(50, pagination.PageSize);
        Assert.Equal(3, pagination.CurrentPage);
    }
    
    [AvaloniaFact]
    public void CurrentPage_Can_Be_Initialized_With_Another_Initialization_Order()
    {
        var window = new Window();
        var pagination = new Pagination
        {
            CurrentPage = 3,
            TotalCount = 300,
            PageSize = 50,
        };
        window.Content = pagination;
        window.Show();
        Dispatcher.UIThread.RunJobs();
        Assert.Equal(6, pagination.PageCount);
        Assert.Equal(300, pagination.TotalCount);
        Assert.Equal(50, pagination.PageSize);
        Assert.Equal(3, pagination.CurrentPage);
    }
    
    [AvaloniaFact]
    public void Change_TotalCount_Should_Update_PageCount()
    {
        var window = new Window();
        var pagination = new Pagination
        {
            CurrentPage = 3,
            PageSize = 50,
            TotalCount = 300,
        };
        window.Content = pagination;
        window.Show();
        Dispatcher.UIThread.RunJobs();
        Assert.Equal(6, pagination.PageCount);
        Assert.Equal(300, pagination.TotalCount);
        Assert.Equal(50, pagination.PageSize);
        Assert.Equal(3, pagination.CurrentPage);
        pagination.TotalCount = 500;
        Dispatcher.UIThread.RunJobs();
        Assert.Equal(10, pagination.PageCount);
        Assert.Equal(500, pagination.TotalCount);
        Assert.Equal(50, pagination.PageSize);
        Assert.Equal(3, pagination.CurrentPage);
    }
    
    [AvaloniaFact]
    public void Change_PageSize_Should_Update_PageCount()
    {
        var window = new Window();
        var pagination = new Pagination
        {
            CurrentPage = 3,
            PageSize = 50,
            TotalCount = 300,
        };
        window.Content = pagination;
        window.Show();
        Dispatcher.UIThread.RunJobs();
        Assert.Equal(6, pagination.PageCount);
        Assert.Equal(300, pagination.TotalCount);
        Assert.Equal(50, pagination.PageSize);
        Assert.Equal(3, pagination.CurrentPage);
        pagination.PageSize = 100;
        Dispatcher.UIThread.RunJobs();
        Assert.Equal(3, pagination.PageCount);
        Assert.Equal(300, pagination.TotalCount);
        Assert.Equal(100, pagination.PageSize);
        Assert.Equal(3, pagination.CurrentPage);
    }

    [AvaloniaFact]
    public void Change_TotalCount_Clamps_Current_Page()
    {
        var window = new Window();
        var pagination = new Pagination
        {
            CurrentPage = 3,
            PageSize = 50,
            TotalCount = 300,
        };
        window.Content = pagination;
        window.Show();
        Dispatcher.UIThread.RunJobs();
        Assert.Equal(6, pagination.PageCount);
        Assert.Equal(300, pagination.TotalCount);
        Assert.Equal(50, pagination.PageSize);
        Assert.Equal(3, pagination.CurrentPage);
        pagination.TotalCount = 10;
        Dispatcher.UIThread.RunJobs();
        Assert.Equal(1, pagination.PageCount);
        Assert.Equal(10, pagination.TotalCount);
        Assert.Equal(50, pagination.PageSize);
        Assert.Equal(1, pagination.CurrentPage);
    }

    [AvaloniaFact]
    public void CurrentPage_Is_Clamped()
    {
        var window = new Window();
        var pagination = new Pagination
        {
            CurrentPage = 3,
            PageSize = 50,
            TotalCount = 300,
        };
        window.Content = pagination;
        window.Show();
        Dispatcher.UIThread.RunJobs();
        Assert.Equal(6, pagination.PageCount);
        Assert.Equal(300, pagination.TotalCount);
        Assert.Equal(50, pagination.PageSize);
        Assert.Equal(3, pagination.CurrentPage);
        pagination.CurrentPage = 10;
        Dispatcher.UIThread.RunJobs();
        Assert.Equal(6, pagination.PageCount);
        Assert.Equal(300, pagination.TotalCount);
        Assert.Equal(50, pagination.PageSize);
        Assert.Equal(6, pagination.CurrentPage);
    }
    
    [AvaloniaFact]
    public void CurrentPage_Is_Clamped_When_PageCount_Changes()
    {
        var window = new Window();
        var pagination = new Pagination
        {
            CurrentPage = 3,
            PageSize = 50,
            TotalCount = 300,
        };
        window.Content = pagination;
        window.Show();
        Dispatcher.UIThread.RunJobs();
        Assert.Equal(6, pagination.PageCount);
        Assert.Equal(300, pagination.TotalCount);
        Assert.Equal(50, pagination.PageSize);
        Assert.Equal(3, pagination.CurrentPage);
        pagination.TotalCount = 10;
        Dispatcher.UIThread.RunJobs();
        Assert.Equal(1, pagination.PageCount);
        Assert.Equal(10, pagination.TotalCount);
        Assert.Equal(50, pagination.PageSize);
        Assert.Equal(1, pagination.CurrentPage);
    }
    
    [AvaloniaFact]
    public void Click_Next_Button_Should_Increment_CurrentPage()
    {
        var window = new Window();
        var pagination = new Pagination
        {
            CurrentPage = 3,
            PageSize = 50,
            TotalCount = 300,
        };
        window.Content = pagination;
        window.Show();
        Dispatcher.UIThread.RunJobs();
        Assert.Equal(6, pagination.PageCount);
        Assert.Equal(300, pagination.TotalCount);
        Assert.Equal(50, pagination.PageSize);
        Assert.Equal(3, pagination.CurrentPage);
        var nextButton = pagination.GetTemplateChildOfType<Button>(Pagination.PART_NextButton);
        nextButton?.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
        Dispatcher.UIThread.RunJobs();
        Assert.Equal(6, pagination.PageCount);
        Assert.Equal(300, pagination.TotalCount);
        Assert.Equal(50, pagination.PageSize);
        Assert.Equal(4, pagination.CurrentPage);
    }
    
    [AvaloniaFact]
    public void Click_Previous_Button_Should_Decrement_CurrentPage()
    {
        var window = new Window();
        var pagination = new Pagination
        {
            CurrentPage = 3,
            PageSize = 50,
            TotalCount = 300,
        };
        window.Content = pagination;
        window.Show();
        Dispatcher.UIThread.RunJobs();
        Assert.Equal(6, pagination.PageCount);
        Assert.Equal(300, pagination.TotalCount);
        Assert.Equal(50, pagination.PageSize);
        Assert.Equal(3, pagination.CurrentPage);
        var previousButton = pagination.GetTemplateChildOfType<Button>(Pagination.PART_PreviousButton);
        previousButton?.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
        Dispatcher.UIThread.RunJobs();
        Assert.Equal(6, pagination.PageCount);
        Assert.Equal(300, pagination.TotalCount);
        Assert.Equal(50, pagination.PageSize);
        Assert.Equal(2, pagination.CurrentPage);
    }
    
    [AvaloniaFact]
    public void Previous_Or_Next_Button_Is_Disabled_When_CurrentPage_Is_Min_Or_Max()
    {
        var window = new Window();
        var pagination = new Pagination
        {
            CurrentPage = 1,
            PageSize = 50,
            TotalCount = 300,
        };
        window.Content = pagination;
        window.Show();
        Dispatcher.UIThread.RunJobs();
        Assert.Equal(6, pagination.PageCount);
        Assert.Equal(300, pagination.TotalCount);
        Assert.Equal(50, pagination.PageSize);
        Assert.Equal(1, pagination.CurrentPage);
        var previousButton = pagination.GetTemplateChildOfType<Button>(Pagination.PART_PreviousButton);
        var nextButton = pagination.GetTemplateChildOfType<Button>(Pagination.PART_NextButton);
        Assert.False(previousButton?.IsEnabled);
        Assert.True(nextButton?.IsEnabled);
        pagination.CurrentPage = 6;
        Dispatcher.UIThread.RunJobs();
        Assert.Equal(6, pagination.PageCount);
        Assert.Equal(300, pagination.TotalCount);
        Assert.Equal(50, pagination.PageSize);
        Assert.Equal(6, pagination.CurrentPage);
        Assert.True(previousButton?.IsEnabled);
        Assert.False(nextButton?.IsEnabled);
    }
    
    [AvaloniaFact]
    public void TinyPagination_Should_Calculate_PageCount_Correctly()
    {
        var window = new Window();
        var pagination = new Pagination
        {
            CurrentPage = 1,
            PageSize = 50,
            TotalCount = 10,
            [!StyledElement.ThemeProperty] = new DynamicResourceExtension("TinyPagination")
        };
        window.Content = pagination;
        window.Show();
        Dispatcher.UIThread.RunJobs();
        Assert.Equal(1, pagination.PageCount);
        Assert.Equal(10, pagination.TotalCount);
        Assert.Equal(50, pagination.PageSize);
        Assert.Equal(1, pagination.CurrentPage);
    }
}