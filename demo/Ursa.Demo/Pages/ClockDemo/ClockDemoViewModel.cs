using System;
using System.Timers;
using CommunityToolkit.Mvvm.ComponentModel;

using Ursa.Demo.ViewModels.Controls;
using Ursa.Demo.Localizations;
using Irihi.Dogma.Docs;
using Ursa.Demo.Pages.DummyPages;

namespace Ursa.Demo.Pages.ClockDemo;

[DocCategory(Category_Key, IsClickable = false, Parent = DateAndTimePage.Category_Key)]
[DocPage(Menu_Header, View = typeof(ClockDemo))]
public partial class ClockDemoViewModel: ObservableObject, IPageMetadataProvider, IDisposable
{
    public const string Category_Key = "Clock";
    public const string Menu_Header = "Menu_Header_Clock";
    public PageMetadataViewModel PageMetadata { get; set; } = new PageMetadataViewModel()
    {
        Title = LanguageManager.Instance.Page_Title_Clock,
        Description = LanguageManager.Instance.Page_Description_Clock,
        Breadcrumbs = [new BreadcrumbItemData(LanguageManager.Instance.Menu_Category_DateAndTime), new BreadcrumbItemData(LanguageManager.Instance.Menu_Header_Clock)],
        Tags = ["Clock", "Time", "Display"],
        DemoViewUrl = "https://github.com/irihitech/Ursa.Avalonia/blob/main/demo/Ursa.Demo/Pages/ClockDemo/ClockDemo.axaml",
        DemoViewModelUrl = "https://github.com/irihitech/Ursa.Avalonia/blob/main/demo/Ursa.Demo/Pages/ClockDemo/ClockDemoViewModel.cs",
        InlineXamlSupport = true,
    };

    private Timer _timer;
    
    [ObservableProperty] public partial DateTime Time { get; set; }
    public ClockDemoViewModel()
    {
        Time = DateTime.Now;
        _timer = new Timer(1000);
        _timer.Elapsed += TimerOnElapsed;
        _timer.Start();
    }

    private void TimerOnElapsed(object? sender, ElapsedEventArgs e)
    {
        Time = DateTime.Now;
    }

    public void Dispose()
    {
        _timer.Stop();
        _timer.Elapsed -= TimerOnElapsed;
        _timer.Dispose();
    }
}