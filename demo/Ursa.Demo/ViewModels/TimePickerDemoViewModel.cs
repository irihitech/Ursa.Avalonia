using System;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Ursa.Demo.ViewModels;

public partial class TimePickerDemoViewModel: ObservableObject
{
    [ObservableProperty] private TimeSpan? _time;
    [ObservableProperty] private TimeSpan? _startTime;
    [ObservableProperty] private TimeSpan? _endTime;

    public TimePickerDemoViewModel()
    {
        Time = new TimeSpan(12, 20, 0);
        StartTime = new TimeSpan(8, 21, 0);
        EndTime = new TimeSpan(18, 22, 0);
    }
}