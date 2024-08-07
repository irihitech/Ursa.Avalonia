using System;
using System.ComponentModel;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Ursa.Demo.ViewModels;

public partial class DatePickerDemoViewModel: ObservableObject
{
    [ObservableProperty] private DateTime? _selectedDate;
    [ObservableProperty] private DateTime? _startDate;
    [ObservableProperty] private DateTime? _endDate;

    public DatePickerDemoViewModel()
    {
        SelectedDate = DateTime.Today;
        StartDate = DateTime.Today;
        EndDate = DateTime.Today.AddDays(7);
    }

    protected override void OnPropertyChanged(PropertyChangedEventArgs e)
    {
        base.OnPropertyChanged(e);
        if (e.PropertyName == nameof(SelectedDate))
        {
            
        }
    }
}