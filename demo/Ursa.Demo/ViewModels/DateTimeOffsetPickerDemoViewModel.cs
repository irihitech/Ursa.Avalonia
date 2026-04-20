using System;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Ursa.Demo.ViewModels;

public partial class DateTimeOffsetPickerDemoViewModel : ObservableObject
{
    [ObservableProperty] private DateTimeOffset? _selectedDateTime;

    public DateTimeOffsetPickerDemoViewModel()
    {
        SelectedDateTime = DateTimeOffset.Now;
    }
}
