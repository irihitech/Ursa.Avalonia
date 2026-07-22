using System.Collections.Generic;
using Avalonia.Platform.Storage;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using Ursa.Demo.ViewModels.Controls;
using Ursa.Demo.Localizations;

namespace Ursa.Demo.ViewModels;

public partial class PathPickerDemoViewModel : ViewModelBase, IPageMetadataProvider
{
    public PageMetadataViewModel PageMetadata { get; set; } = new PageMetadataViewModel()
    {
        Title = LanguageManager.Instance.Page_Title_PathPicker,
        Description = LanguageManager.Instance.Page_Description_PathPicker,
        Breadcrumbs = [new BreadcrumbItemData(LanguageManager.Instance.Menu_Category_ButtonsAndInputs), new BreadcrumbItemData(LanguageManager.Instance.Menu_Header_PathPicker)],
        Tags = ["PathPicker", "Input", "File"],
        DemoViewUrl = "https://github.com/irihitech/Ursa.Avalonia/blob/main/demo/Ursa.Demo/Pages/PathPickerDemo.axaml",
        DemoViewModelUrl = "https://github.com/irihitech/Ursa.Avalonia/blob/main/demo/Ursa.Demo/ViewModels/Pages/PathPickerDemoViewModel.cs",
        InlineXamlSupport = true,
        MvvmSupport = true,
        AvaloniaExclusive = true,
    };

    [ObservableProperty] public partial string? Path { get; set; }
    [ObservableProperty] public partial IReadOnlyList<string>? Paths { get; set; }
    [ObservableProperty] public partial int CommandTriggerCount { get; set; } = 0;

    [RelayCommand]
    private void Selected(IReadOnlyList<IStorageItem> items)
    {
        CommandTriggerCount++;
    }
}
