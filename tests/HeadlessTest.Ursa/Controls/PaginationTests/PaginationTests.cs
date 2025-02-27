using Avalonia;
using Avalonia.Controls;
using Avalonia.Data;
using Avalonia.Headless.XUnit;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml.MarkupExtensions;
using Avalonia.Threading;
using CommunityToolkit.Mvvm.Input;
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
    
    [AvaloniaFact]
    public void Click_On_Previews_Or_Next_Triggers_Command()
    {
        var window = new Window();
        int count = 0;
        var pagination = new Pagination
        {
            CurrentPage = 3,
            PageSize = 50,
            TotalCount = 300,
            Command = new RelayCommand(() => count++)
        };
        window.Content = pagination;
        window.Show();
        Dispatcher.UIThread.RunJobs();
        var previousButton = pagination.GetTemplateChildOfType<Button>(Pagination.PART_PreviousButton);
        var nextButton = pagination.GetTemplateChildOfType<Button>(Pagination.PART_NextButton);
        previousButton?.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
        Dispatcher.UIThread.RunJobs();
        Assert.Equal(1, count);
        nextButton?.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
        Assert.Equal(2, count);
        Dispatcher.UIThread.RunJobs();
    }
    
    [AvaloniaFact]
    public void Click_On_Previews_Or_Next_Triggers_Command_With_Parameter()
    {
        var window = new Window();
        int count = 0;
        var pagination = new Pagination
        {
            CurrentPage = 3,
            PageSize = 50,
            TotalCount = 300,
            Command = new RelayCommand<int>(page => count = page),
            [!Pagination.CommandParameterProperty] = new Binding(nameof(Pagination.CurrentPage))
                { RelativeSource = new RelativeSource(RelativeSourceMode.Self) },
        };
        window.Content = pagination;
        window.Show();
        Dispatcher.UIThread.RunJobs();
        var previousButton = pagination.GetTemplateChildOfType<Button>(Pagination.PART_PreviousButton);
        var nextButton = pagination.GetTemplateChildOfType<Button>(Pagination.PART_NextButton);
        previousButton?.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
        Dispatcher.UIThread.RunJobs();
        Assert.Equal(2, count);
        nextButton?.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
        Dispatcher.UIThread.RunJobs();
        Assert.Equal(3, count);
    }

    [AvaloniaFact]
    public void Buttons_Panel_Is_Correct_Case1()
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
        var buttonsPanel = pagination.GetTemplateChildOfType<Panel>(Pagination.PART_ButtonPanel);
        Assert.NotNull(buttonsPanel);
        Assert.Equal(6, buttonsPanel.Children.Count(a => a.IsVisible));

        pagination.TotalCount = 30000;
        Dispatcher.UIThread.RunJobs();
        Assert.Equal(7, buttonsPanel.Children.Count(a => a.IsVisible));
        
        var buttons = buttonsPanel.Children.OfType<PaginationButton>().ToList();
        Assert.Equal(1, buttons[0].Page);
        Assert.Equal(2, buttons[1].Page);
        Assert.Equal(3, buttons[2].Page);
        Assert.Equal(4, buttons[3].Page);
        Assert.Equal(5, buttons[4].Page);
        Assert.True(buttons[5].IsFastBackward);
        Assert.Equal(600, buttons[6].Page);
        
        pagination.CurrentPage = 300;
        Dispatcher.UIThread.RunJobs();
        Assert.Equal(7, buttonsPanel.Children.Count(a => a.IsVisible));
        Assert.Equal(1, buttons[0].Page);
        Assert.True(buttons[1].IsFastForward);
        Assert.Equal(299, buttons[2].Page);
        Assert.Equal(300, buttons[3].Page);
        Assert.Equal(301, buttons[4].Page);
        Assert.True(buttons[5].IsFastBackward);
        Assert.Equal(600, buttons[6].Page);
        
        pagination.CurrentPage = 600;
        Dispatcher.UIThread.RunJobs();
        Assert.Equal(7, buttonsPanel.Children.Count(a => a.IsVisible));
        Assert.Equal(1, buttons[0].Page);
        Assert.True(buttons[1].IsFastForward);
        Assert.Equal(596, buttons[2].Page);
        Assert.Equal(597, buttons[3].Page);
        Assert.Equal(598, buttons[4].Page);
        Assert.Equal(599, buttons[5].Page);
        Assert.Equal(600, buttons[6].Page);

    }

    [AvaloniaFact]
    public void Button_Panel_Button_Triggers_Command()
    {
        var window = new Window();
        int count = 0;
        var pagination = new Pagination
        {
            CurrentPage = 3,
            PageSize = 50,
            TotalCount = 300,
            Command = new RelayCommand<int>(page => count = page),
            [!Pagination.CommandParameterProperty] = new Binding(nameof(Pagination.CurrentPage))
                { RelativeSource = new RelativeSource(RelativeSourceMode.Self) },
        };
        window.Content = pagination;
        window.Show();
        Dispatcher.UIThread.RunJobs();
        var buttonsPanel = pagination.GetTemplateChildOfType<Panel>(Pagination.PART_ButtonPanel);
        var buttons = buttonsPanel?.Children.OfType<PaginationButton>().ToList();
        Assert.NotNull(buttons);
        buttons[0].RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
        Dispatcher.UIThread.RunJobs();
        Assert.Equal(1, count);
        buttons[1].RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
        Dispatcher.UIThread.RunJobs();
        Assert.Equal(2, count);
    }

    [AvaloniaFact]
    public void Change_CurrentPage_Triggers_Event()
    {
        var window = new Window();
        int count = 0;
        var pagination = new Pagination
        {
            CurrentPage = 3,
            PageSize = 50,
            TotalCount = 300,
        };
        pagination.CurrentPageChanged += (sender, args) => count++;
        window.Content = pagination;
        window.Show();
        Dispatcher.UIThread.RunJobs();
        pagination.CurrentPage = 5;
        Dispatcher.UIThread.RunJobs();
        Assert.Equal(1, count);
    }
    
}