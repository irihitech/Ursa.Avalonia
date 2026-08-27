using CommunityToolkit.Mvvm.ComponentModel;
using Ursa.Common;
using System.Collections.ObjectModel;
using Irihi.Dogma.Controls;

using Ursa.Demo.ViewModels.Controls;
using Ursa.Demo.Localizations;
using Irihi.Dogma.Docs;
using Ursa.Demo.Pages.DummyPages;

namespace Ursa.Demo.Pages.IconButtonDemo;

[DocCategory(Category_Key, IsClickable = false, Parent = ButtonsAndInputsPage.Category_Key)]
[DocPage(Menu_Header, View = typeof(IconButtonDemo))]
public partial class IconButtonDemoViewModel : ObservableObject, IPageMetadataProvider
{
    public const string Category_Key = "IconButton";
    public const string Menu_Header = "Menu_Header_IconButton";
    private const string ActionButtonVariantsAnchorId = "icon-button-action-button-variants";
    private const string ToggleButtonVariantsAnchorId = "icon-button-toggle-button-variants";
    private const string LoadingAndCustomIconsAnchorId = "icon-button-loading-and-custom-icons";
    public PageMetadataViewModel PageMetadata { get; set; } = new PageMetadataViewModel()
    {
        Title = LanguageManager.Instance.Page_Title_IconButton,
        Description = LanguageManager.Instance.Page_Description_IconButton,
        Breadcrumbs = [new BreadcrumbItemData(LanguageManager.Instance.Menu_Category_ButtonsAndInputs), new BreadcrumbItemData(LanguageManager.Instance.Menu_Header_IconButton)],
        Tags = ["IconButton", "Button", "Icon"],
        DemoViewUrl = "https://github.com/irihitech/Ursa.Avalonia/blob/main/demo/Ursa.Demo/Pages/IconButtonDemo/IconButtonDemo.axaml",
        DemoViewModelUrl = "https://github.com/irihitech/Ursa.Avalonia/blob/main/demo/Ursa.Demo/Pages/IconButtonDemo/IconButtonDemoViewModel.cs",
        InlineXamlSupport = true,
        MvvmSupport = true,
    };

    public DemoSectionViewModel ActionButtonVariantsSection { get; }
    public DemoSectionViewModel ToggleButtonVariantsSection { get; }
    public DemoSectionViewModel LoadingAndCustomIconsSection { get; }
    public ObservableCollection<AnchorScrollViewerItemViewModel> AnchorItems { get; } =
    [
        new()
        {
            Header = LanguageManager.Instance.Page_IconButton_Section_Action_Button_Variants_Header,
            AnchorId = ActionButtonVariantsAnchorId
        },
        new()
        {
            Header = LanguageManager.Instance.Page_IconButton_Section_Toggle_Button_Variants_Header,
            AnchorId = ToggleButtonVariantsAnchorId
        },
        new()
        {
            Header = LanguageManager.Instance.Page_IconButton_Section_Loading_And_Custom_Icons_Header,
            AnchorId = LoadingAndCustomIconsAnchorId
        }
    ];

    public IconButtonDemoViewModel()
    {
        ActionButtonVariantsSection = new DemoSectionViewModel
        {
            Header = LanguageManager.Instance.Page_IconButton_Section_Action_Button_Variants_Header,
            Descriptions = { LanguageManager.Instance.Page_IconButton_Section_Action_Button_Variants_Description },
            SectionTag = DemoSectionTag.Style,
            AnchorId = ActionButtonVariantsAnchorId
        };
        ActionButtonVariantsSection.CodeSnippets.Add(new DemoSectionCodeSnippetViewModel
        {
            CodeSnippetLanguage = CodeLanguage.Axaml,
            TabName = LanguageManager.Instance.DemoSection_Tab_Xaml,
            CodeSnippet = """
                          <u:IconButton Icon="{StaticResource SemiIconCamera}" Content="Primary" Classes="Primary" />
                          <u:IconDropDownButton Icon="{StaticResource SemiIconCamera}" Content="Primary" Classes="Primary" />
                          <u:IconSplitButton Icon="{StaticResource SemiIconCamera}" Content="Primary" Classes="Primary" />
                          """
        });

        ToggleButtonVariantsSection = new DemoSectionViewModel
        {
            Header = LanguageManager.Instance.Page_IconButton_Section_Toggle_Button_Variants_Header,
            Descriptions = { LanguageManager.Instance.Page_IconButton_Section_Toggle_Button_Variants_Description },
            SectionTag = DemoSectionTag.Style,
            AnchorId = ToggleButtonVariantsAnchorId
        };
        ToggleButtonVariantsSection.CodeSnippets.Add(new DemoSectionCodeSnippetViewModel
        {
            CodeSnippetLanguage = CodeLanguage.Axaml,
            TabName = LanguageManager.Instance.DemoSection_Tab_Xaml,
            CodeSnippet = """
                          <u:IconToggleButton
                              Icon="{StaticResource SemiIconCamera}"
                              IsThreeState="True"
                              IsChecked="{x:Null}"
                              Content="Indeterminate" />
                          <u:IconToggleSplitButton
                              Icon="{StaticResource SemiIconCamera}"
                              IsChecked="True"
                              Content="Checked" />
                          """
        });

        LoadingAndCustomIconsSection = new DemoSectionViewModel
        {
            Header = LanguageManager.Instance.Page_IconButton_Section_Loading_And_Custom_Icons_Header,
            Descriptions = { LanguageManager.Instance.Page_IconButton_Section_Loading_And_Custom_Icons_Description },
            SectionTag = DemoSectionTag.Function,
            AnchorId = LoadingAndCustomIconsAnchorId
        };
        LoadingAndCustomIconsSection.CodeSnippets.Add(new DemoSectionCodeSnippetViewModel
        {
            CodeSnippetLanguage = CodeLanguage.Axaml,
            TabName = LanguageManager.Instance.DemoSection_Tab_Xaml,
            CodeSnippet = """
                          <u:IconButton
                              IsLoading="{Binding IsLoading2}"
                              IconPlacement="{Binding SelectedPosition}"
                              Icon="{StaticResource SemiIconCamera}"
                              Content="Hello Camera" />
                          """
        });
        LoadingAndCustomIconsSection.CodeSnippets.Add(new DemoSectionCodeSnippetViewModel
        {
            CodeSnippetLanguage = CodeLanguage.CSharp,
            TabName = LanguageManager.Instance.DemoSection_Tab_ViewModel,
            CodeSnippet = """
                          [ObservableProperty] public partial bool IsLoading { get; set; }
                          [ObservableProperty] public partial bool IsLoading2 { get; set; }
                          [ObservableProperty] public partial Position SelectedPosition { get; set; }
                          """
        });
    }

    [ObservableProperty] public partial bool IsLoading { get; set; }
    [ObservableProperty] public partial bool IsLoading2 { get; set; }
    [ObservableProperty] public partial Position SelectedPosition { get; set; }
}