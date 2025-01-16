using System.Collections.Generic;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Ursa.Demo.ViewModels;

public partial class PathPickerDemoViewModel : ViewModelBase
{
    [ObservableProperty] private string? _path;
    [ObservableProperty] private IReadOnlyList<string>? _paths;
    [ObservableProperty] private int _commandTriggerCount = 0;

    [RelayCommand]
    private void Selected(IReadOnlyList<string> paths)
    {
        CommandTriggerCount++;
    }
}