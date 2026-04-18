using System;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Ursa.Demo.ViewModels;

public partial class TimeOnlyPickerDemoViewModel : ObservableObject
{
    [ObservableProperty] private TimeOnly? _time;

    public TimeOnlyPickerDemoViewModel()
    {
        Time = new TimeOnly(12, 20, 0);
    }
}
