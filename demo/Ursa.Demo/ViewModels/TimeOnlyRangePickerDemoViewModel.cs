using System;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Ursa.Demo.ViewModels;

public partial class TimeOnlyRangePickerDemoViewModel : ObservableObject
{
    [ObservableProperty] private TimeOnly? _startTime;
    [ObservableProperty] private TimeOnly? _endTime;

    public TimeOnlyRangePickerDemoViewModel()
    {
        StartTime = new TimeOnly(8, 21, 0);
        EndTime = new TimeOnly(18, 22, 0);
    }
}
