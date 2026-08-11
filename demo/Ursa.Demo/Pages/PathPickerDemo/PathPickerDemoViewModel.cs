using System.Collections.Generic;
using Avalonia.Platform.Storage;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using Ursa.Demo.ViewModels;
using Ursa.Demo.ViewModels.Controls;
using Ursa.Demo.Localizations;
using Irihi.Dogma.Docs;
using Ursa.Demo.Pages.DummyPages;

namespace Ursa.Demo.Pages.PathPickerDemo;

[DocCategory(Category_Key, IsClickable = false, Parent = ButtonsAndInputsPage.Category_Key)]
[DocPage(Menu_Header, View = typeof(PathPickerDemo))]
public partial class PathPickerDemoViewModel : ViewModelBase, IPageMetadataProvider
{
    public const string Category_Key = "PathPicker";
    public const string Menu_Header = "Menu_Header_PathPicker";
    public PageMetadataViewModel PageMetadata { get; set; } = new PageMetadataViewModel()
    {
        Title = LanguageManager.Instance.Page_Title_PathPicker,
        Description = LanguageManager.Instance.Page_Description_PathPicker,
        Breadcrumbs = [new BreadcrumbItemData(LanguageManager.Instance.Menu_Category_ButtonsAndInputs), new BreadcrumbItemData(LanguageManager.Instance.Menu_Header_PathPicker)],
        Tags = ["PathPicker", "Input", "File"],
        DemoViewUrl = "https://github.com/irihitech/Ursa.Avalonia/blob/main/demo/Ursa.Demo/Pages/PathPickerDemo/PathPickerDemo.axaml",
        DemoViewModelUrl = "https://github.com/irihitech/Ursa.Avalonia/blob/main/demo/Ursa.Demo/Pages/PathPickerDemo/PathPickerDemoViewModel.cs",
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
