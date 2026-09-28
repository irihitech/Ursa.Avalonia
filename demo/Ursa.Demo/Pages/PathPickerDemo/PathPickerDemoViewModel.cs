using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Avalonia.Platform.Storage;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Irihi.Dogma.Controls;
using Irihi.Dogma.Docs;
using Ursa.Demo.Localizations;
using Ursa.Demo.Pages.DummyPages;
using Ursa.Demo.ViewModels;
using Ursa.Demo.ViewModels.Controls;

namespace Ursa.Demo.Pages.PathPickerDemo;

[DocCategory(Category_Key, IsClickable = false, Parent = ButtonsAndInputsPage.Category_Key)]
[DocPage(Menu_Header, View = typeof(PathPickerDemo))]
public partial class PathPickerDemoViewModel : ViewModelBase, IPageMetadataProvider
{
    public const string Category_Key = "PathPicker";
    public const string Menu_Header = "Menu_Header_PathPicker";
    private const string OpenFileAnchorId = "path-picker-open-file";
    private const string SaveFileAnchorId = "path-picker-save-file";
    private const string FolderSelectionAnchorId = "path-picker-folder-selection";
    private const string MultipleSelectionAnchorId = "path-picker-multiple-selection";
    private const string CommandAnchorId = "path-picker-command";
    private const string CommandCancelAnchorId = "path-picker-command-cancel";
    private const string AppearanceAnchorId = "path-picker-appearance";

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

    public DemoSectionViewModel OpenFileSection { get; }
    public DemoSectionViewModel SaveFileSection { get; }
    public DemoSectionViewModel FolderSelectionSection { get; }
    public DemoSectionViewModel MultipleSelectionSection { get; }
    public DemoSectionViewModel CommandSection { get; }
    public DemoSectionViewModel CommandCancelSection { get; }
    public DemoSectionViewModel AppearanceSection { get; }

    public ObservableCollection<AnchorScrollViewerItemViewModel> AnchorItems { get; } =
    [
        new()
        {
            Header = LanguageManager.Instance.Page_PathPicker_Section_Open_File_Header,
            AnchorId = OpenFileAnchorId
        },
        new()
        {
            Header = LanguageManager.Instance.Page_PathPicker_Section_Save_File_Header,
            AnchorId = SaveFileAnchorId
        },
        new()
        {
            Header = LanguageManager.Instance.Page_PathPicker_Section_Folder_Selection_Header,
            AnchorId = FolderSelectionAnchorId
        },
        new()
        {
            Header = LanguageManager.Instance.Page_PathPicker_Section_Multiple_Selection_Header,
            AnchorId = MultipleSelectionAnchorId
        },
        new()
        {
            Header = LanguageManager.Instance.Page_PathPicker_Section_Command_Header,
            AnchorId = CommandAnchorId
        },
        new()
        {
            Header = LanguageManager.Instance.Page_PathPicker_Section_Command_Cancel_Header,
            AnchorId = CommandCancelAnchorId
        },
        new()
        {
            Header = LanguageManager.Instance.Page_PathPicker_Section_Appearance_Header,
            AnchorId = AppearanceAnchorId
        },
    ];

    [ObservableProperty] public partial IReadOnlyList<string> SelectedFiles { get; set; } = [];
    [ObservableProperty] public partial string? SelectedItemInfo { get; set; }
    [ObservableProperty] public partial bool IsOmitCommandOnCancel { get; set; } = true;
    [ObservableProperty] public partial bool IsClearSelectionOnCancel { get; set; }
    [ObservableProperty] public partial int CommandTriggerCount { get; set; }

    public PathPickerDemoViewModel()
    {
        OpenFileSection = new DemoSectionViewModel
        {
            Header = LanguageManager.Instance.Page_PathPicker_Section_Open_File_Header,
            Descriptions = { LanguageManager.Instance.Page_PathPicker_Section_Open_File_Description },
            SectionTag = DemoSectionTag.Function,
            AnchorId = OpenFileAnchorId
        };
        AddXamlSnippet(OpenFileSection, """
            <u:PathPicker
                Title="Select a file"
                UsePickerType="OpenFile"
                FileFilter="[Text,*.txt][JSON,*.json]" />
            """);

        SaveFileSection = new DemoSectionViewModel
        {
            Header = LanguageManager.Instance.Page_PathPicker_Section_Save_File_Header,
            Descriptions = { LanguageManager.Instance.Page_PathPicker_Section_Save_File_Description },
            SectionTag = DemoSectionTag.Function,
            AnchorId = SaveFileAnchorId
        };
        AddXamlSnippet(SaveFileSection, """
            <u:PathPicker
                Title="Save report"
                UsePickerType="SaveFile"
                SuggestedFileName="report"
                DefaultFileExtension="json"
                FileFilter="[JSON,*.json]" />
            """);

        FolderSelectionSection = new DemoSectionViewModel
        {
            Header = LanguageManager.Instance.Page_PathPicker_Section_Folder_Selection_Header,
            Descriptions = { LanguageManager.Instance.Page_PathPicker_Section_Folder_Selection_Description },
            SectionTag = DemoSectionTag.Function,
            AnchorId = FolderSelectionAnchorId
        };
        AddXamlSnippet(FolderSelectionSection, """
            <u:PathPicker
                Title="Select output folder"
                UsePickerType="OpenFolder" />
            """);

        MultipleSelectionSection = new DemoSectionViewModel
        {
            Header = LanguageManager.Instance.Page_PathPicker_Section_Multiple_Selection_Header,
            Descriptions = { LanguageManager.Instance.Page_PathPicker_Section_Multiple_Selection_Description },
            SectionTag = DemoSectionTag.Function,
            AnchorId = MultipleSelectionAnchorId
        };
        AddXamlSnippet(MultipleSelectionSection, """
            <u:PathPicker
                Title="Select images"
                UsePickerType="OpenFile"
                AllowMultiple="True"
                FileFilter="[Images,*.png,*.jpg,*.jpeg]"
                SelectedPaths="{Binding SelectedFiles, Mode=OneWayToSource}" />

            <ListBox ItemsSource="{Binding SelectedFiles}" />
            """);
        AddViewModelSnippet(MultipleSelectionSection, """
            [ObservableProperty]
            public partial IReadOnlyList<string> SelectedFiles { get; set; } = [];
            """);

        CommandSection = new DemoSectionViewModel
        {
            Header = LanguageManager.Instance.Page_PathPicker_Section_Command_Header,
            Descriptions = { LanguageManager.Instance.Page_PathPicker_Section_Command_Description },
            SectionTag = DemoSectionTag.Function,
            AnchorId = CommandAnchorId
        };
        AddXamlSnippet(CommandSection, """
            <u:PathPicker
                Title="Select a file"
                UsePickerType="OpenFile"
                IsOmitCommandOnCancel="True"
                Command="{Binding InspectStorageItemsCommand}" />
            """);
        AddViewModelSnippet(CommandSection, """
            using Avalonia.Platform.Storage;

            [ObservableProperty]
            public partial string? SelectedItemInfo { get; set; }

            [RelayCommand]
            private async Task InspectStorageItems(IReadOnlyList<IStorageItem> items)
            {
                SelectedItemInfo = null;
                foreach (var item in items)
                {
                    using (item)
                    {
                        switch (item)
                        {
                            case IStorageFile file:
                            {
                                var localFilePath = file.TryGetLocalPath();
                                await using var stream = await file.OpenReadAsync();
                                SelectedItemInfo =
                                    $"{file.Name}: {localFilePath ?? file.Path.ToString()}, stream readable: {stream.CanRead}";
                                break;
                            }

                            case IStorageFolder folder:
                            {
                                var localFolderPath = folder.TryGetLocalPath();
                                SelectedItemInfo =
                                    $"{folder.Name}: {localFolderPath ?? folder.Path.ToString()}";
                                break;
                            }
                        }
                    }
                }
            }
            """);

        CommandCancelSection = new DemoSectionViewModel
        {
            Header = LanguageManager.Instance.Page_PathPicker_Section_Command_Cancel_Header,
            Descriptions = { LanguageManager.Instance.Page_PathPicker_Section_Command_Cancel_Description },
            SectionTag = DemoSectionTag.Function,
            AnchorId = CommandCancelAnchorId
        };
        AddXamlSnippet(CommandCancelSection, """
            <u:PathPicker
                Title="Select files"
                UsePickerType="OpenFile"
                Command="{Binding SelectedCommand}"
                IsOmitCommandOnCancel="{Binding IsOmitCommandOnCancel}"
                IsClearSelectionOnCancel="{Binding IsClearSelectionOnCancel}" />
            """);
        AddViewModelSnippet(CommandCancelSection, """
            [RelayCommand]
            private void Selected(IReadOnlyList<IStorageItem> items)
            {
                CommandTriggerCount++;
            }
            """);

        AppearanceSection = new DemoSectionViewModel
        {
            Header = LanguageManager.Instance.Page_PathPicker_Section_Appearance_Header,
            Descriptions = { LanguageManager.Instance.Page_PathPicker_Section_Appearance_Description },
            SectionTag = DemoSectionTag.Style,
            AnchorId = AppearanceAnchorId
        };
        AddXamlSnippet(AppearanceSection, """
            <u:PathPicker Classes="Top" Title="Top layout" />

            <u:PathPicker
                Theme="{DynamicResource ButtonPathPicker}"
                Title="Button only" />

            <u:PathPicker
                Theme="{DynamicResource ListPathPicker}"
                Title="Expandable list"
                AllowMultiple="True" />

            <u:PathPicker Title="Custom button content">
                <u:PathPicker.ButtonContent>
                    <StackPanel Orientation="Horizontal" Spacing="4">
                        <PathIcon Data="{DynamicResource SemiIconSearch}" />
                        <TextBlock Text="Browse" />
                    </StackPanel>
                </u:PathPicker.ButtonContent>
            </u:PathPicker>
            """);
    }

    [RelayCommand]
    private void Selected(IReadOnlyList<IStorageItem> items)
    {
        CommandTriggerCount++;
        foreach (var item in items)
        {
            item.Dispose();
        }
    }

    [RelayCommand]
    private async Task InspectStorageItems(IReadOnlyList<IStorageItem> items)
    {
        SelectedItemInfo = null;
        foreach (var item in items)
        {
            using (item)
            {
                switch (item)
                {
                    case IStorageFile file:
                    {
                        var localFilePath = file.TryGetLocalPath();
                        await using var stream = await file.OpenReadAsync();
                        SelectedItemInfo =
                            $"{file.Name}: {localFilePath ?? file.Path.ToString()}, stream readable: {stream.CanRead}";
                        break;
                    }

                    case IStorageFolder folder:
                    {
                        var localFolderPath = folder.TryGetLocalPath();
                        SelectedItemInfo =
                            $"{folder.Name}: {localFolderPath ?? folder.Path.ToString()}";
                        break;
                    }
                }
            }
        }
    }

    private static void AddXamlSnippet(DemoSectionViewModel section, string code)
    {
        section.CodeSnippets.Add(new DemoSectionCodeSnippetViewModel
        {
            CodeSnippetLanguage = CodeLanguage.Axaml,
            TabName = LanguageManager.Instance.DemoSection_Tab_Xaml,
            CodeSnippet = code
        });
    }

    private static void AddViewModelSnippet(DemoSectionViewModel section, string code)
    {
        section.CodeSnippets.Add(new DemoSectionCodeSnippetViewModel
        {
            CodeSnippetLanguage = CodeLanguage.CSharp,
            TabName = LanguageManager.Instance.DemoSection_Tab_ViewModel,
            CodeSnippet = code
        });
    }
}
