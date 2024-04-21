using System;
using System.Timers;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Ursa.Demo.ViewModels;

public partial class ClockDemoViewModel: ObservableObject, IDisposable
{
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