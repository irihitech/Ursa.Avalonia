using System;
using System.Timers;
using CommunityToolkit.Mvvm.ComponentModel;

using Ursa.Demo.ViewModels.Controls;
namespace Ursa.Demo.ViewModels;

public partial class ClockDemoViewModel: ObservableObject, IDisposable
{
    public PageMetadataViewModel PageMetadata { get; set; } = new PageMetadataViewModel()
    {
        Title = "Clock",
        Description = "Clock displays a visual analog or digital clock.",
        Breadcrumbs = ["Date & Time", "Clock"],
        Tags = ["Clock", "Time", "Display"],
        DemoViewUrl = "https://github.com/irihitech/Ursa.Avalonia/blob/main/demo/Ursa.Demo/Pages/ClockDemo.axaml",
        DemoViewModelUrl = "https://github.com/irihitech/Ursa.Avalonia/blob/main/demo/Ursa.Demo/ViewModels/Pages/ClockDemoViewModel.cs",
        InlineXamlSupport = true,
    };

    private Timer _timer;
    
    [ObservableProperty] private DateTime _time;
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