using System;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Ursa.Demo.ViewModels;

public partial class DateOnlyPickerDemoViewModel : ObservableObject
{
    [ObservableProperty] private DateOnly? _selectedDate;

    public DateOnlyPickerDemoViewModel()
    {
        SelectedDate = DateOnly.FromDateTime(DateTime.Today);
    }
}
