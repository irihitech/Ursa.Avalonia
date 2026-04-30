using System.Collections.Generic;
using Avalonia.Platform.Storage;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using Ursa.Demo.ViewModels.Controls;
namespace Ursa.Demo.ViewModels;

public partial class PathPickerDemoViewModel : ViewModelBase
{
    public PageMetadataViewModel PageMetadata { get; set; } = new PageMetadataViewModel()
    {
        Title = "PathPicker",
        Description = "PathPicker allows users to select file or folder paths from the filesystem.",
        Breadcrumbs = ["Buttons & Inputs", "PathPicker"],
        Tags = ["PathPicker", "Input", "File"],
        DemoViewUrl = "https://github.com/irihitech/Ursa.Avalonia/blob/main/demo/Ursa.Demo/Pages/PathPickerDemo.axaml",
        DemoViewModelUrl = "https://github.com/irihitech/Ursa.Avalonia/blob/main/demo/Ursa.Demo/ViewModels/Pages/PathPickerDemoViewModel.cs",
        InlineXamlSupport = true,
        MvvmSupport = true,
        AvaloniaExclusive = true,
    };

    [ObservableProperty] private string? _path;
    [ObservableProperty] private IReadOnlyList<string>? _paths;
    [ObservableProperty] private int _commandTriggerCount = 0;

    [RelayCommand]
    private void Selected(IReadOnlyList<IStorageItem> items)
    {
        CommandTriggerCount++;
    }
}
