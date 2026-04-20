using System;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Ursa.Demo.ViewModels;

public partial class DateOffsetPickerDemoViewModel : ObservableObject
{
    [ObservableProperty] private DateTimeOffset? _selectedDate;

    public DateOffsetPickerDemoViewModel()
    {
        SelectedDate = DateTimeOffset.Now;
    }
}
