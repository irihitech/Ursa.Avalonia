using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using Irihi.Dogma.Controls;
using Irihi.Dogma.Docs;
using Ursa.Common;
using Ursa.Demo.Localizations;
using Ursa.Demo.Pages.DummyPages;
using Ursa.Demo.ViewModels.Controls;

namespace Ursa.Demo.Pages.IconButtonDemo;

[DocCategory(Category_Key, IsClickable = false, Parent = ButtonsAndInputsPage.Category_Key)]
[DocPage(Menu_Header, View = typeof(IconButtonDemo))]
public partial class IconButtonDemoViewModel : ObservableObject, IPageMetadataProvider
{
    public const string Category_Key = "IconButton";
    public const string Menu_Header = "Menu_Header_IconButton";
    private const string BasicUsageAnchorId = "icon-button-basic-usage";
    private const string ThemeVariantsAnchorId = "icon-button-theme-variants";
    private const string StyleClassesAnchorId = "icon-button-style-classes";
    private const string IconPlacementAnchorId = "icon-button-icon-placement";
    private const string LoadingStateAnchorId = "icon-button-loading-state";
    private bool _isLoading;

    public PageMetadataViewModel PageMetadata { get; set; } = new()
    {
        Title = LanguageManager.Instance.Page_Title_IconButton,
        Description = LanguageManager.Instance.Page_Description_IconButton,
        Breadcrumbs =
        [
            new BreadcrumbItemData(LanguageManager.Instance.Menu_Category_ButtonsAndInputs),
            new BreadcrumbItemData(LanguageManager.Instance.Menu_Header_IconButton)
        ],
        Tags = ["IconButton", "Button", "Icon"],
        DemoViewUrl = "https://github.com/irihitech/Ursa.Avalonia/blob/main/demo/Ursa.Demo/Pages/IconButtonDemo/IconButtonDemo.axaml",
        DemoViewModelUrl = "https://github.com/irihitech/Ursa.Avalonia/blob/main/demo/Ursa.Demo/Pages/IconButtonDemo/IconButtonDemoViewModel.cs",
        InlineXamlSupport = true,
        MvvmSupport = true
    };

    public DemoSectionViewModel BasicUsageSection { get; }
    public DemoSectionViewModel ThemeVariantsSection { get; }
    public DemoSectionViewModel StyleClassesSection { get; }
    public DemoSectionViewModel IconPlacementSection { get; }
    public DemoSectionViewModel LoadingStateSection { get; }

    public bool IsLoading
    {
        get => _isLoading;
        set => SetProperty(ref _isLoading, value);
    }

    public ObservableCollection<AnchorScrollViewerItemViewModel> AnchorItems { get; } =
    [
        new()
        {
            Header = LanguageManager.Instance.Page_IconButton_Section_Basic_Usage_Header,
            AnchorId = BasicUsageAnchorId
        },
        new()
        {
            Header = LanguageManager.Instance.Page_IconButton_Section_Theme_Variants_Header,
            AnchorId = ThemeVariantsAnchorId
        },
        new()
        {
            Header = LanguageManager.Instance.Page_IconButton_Section_Style_Classes_Header,
            AnchorId = StyleClassesAnchorId
        },
        new()
        {
            Header = LanguageManager.Instance.Page_IconButton_Section_Icon_Placement_Header,
            AnchorId = IconPlacementAnchorId
        },
        new()
        {
            Header = LanguageManager.Instance.Page_IconButton_Section_Loading_State_Header,
            AnchorId = LoadingStateAnchorId
        }
    ];

    public IconButtonDemoViewModel()
    {
        BasicUsageSection = new DemoSectionViewModel
        {
            Header = LanguageManager.Instance.Page_IconButton_Section_Basic_Usage_Header,
            Descriptions = { LanguageManager.Instance.Page_IconButton_Section_Basic_Usage_Description },
            SectionTag = DemoSectionTag.Function,
            AnchorId = BasicUsageAnchorId
        };

        BasicUsageSection.CodeSnippets.Add(new DemoSectionCodeSnippetViewModel
        {
            CodeSnippetLanguage = CodeLanguage.Axaml,
            TabName = LanguageManager.Instance.DemoSection_Tab_Xaml,
            CodeSnippet = """
                          <u:IconButton Icon="{StaticResource SemiIconCamera}" Content="IconButton" />
                          <u:IconDropDownButton Icon="{StaticResource SemiIconCamera}" Content="IconDropDownButton" />
                          <u:IconRepeatButton Icon="{StaticResource SemiIconCamera}" Content="IconRepeatButton" />
                          <u:IconSplitButton Icon="{StaticResource SemiIconCamera}" Content="IconSplitButton" />
                          <u:IconToggleButton Icon="{StaticResource SemiIconCamera}" Content="IconToggleButton" />
                          <u:IconToggleSplitButton Icon="{StaticResource SemiIconCamera}" Content="IconToggleSplitButton" />
                          """
        });

        ThemeVariantsSection = new DemoSectionViewModel
        {
            Header = LanguageManager.Instance.Page_IconButton_Section_Theme_Variants_Header,
            Descriptions = { LanguageManager.Instance.Page_IconButton_Section_Theme_Variants_Description },
            SectionTag = DemoSectionTag.Style,
            AnchorId = ThemeVariantsAnchorId
        };

        ThemeVariantsSection.CodeSnippets.Add(new DemoSectionCodeSnippetViewModel
        {
            CodeSnippetLanguage = CodeLanguage.Axaml,
            TabName = LanguageManager.Instance.DemoSection_Tab_Xaml,
            CodeSnippet = """
                          <u:IconButton Icon="{StaticResource SemiIconCamera}" Content="SolidIconButton" Theme="{StaticResource SolidIconButton}" />
                          <u:IconButton Icon="{StaticResource SemiIconCamera}" Content="OutlineIconButton" Theme="{StaticResource OutlineIconButton}" />
                          <u:IconButton Icon="{StaticResource SemiIconCamera}" Content="BorderlessIconButton" Theme="{StaticResource BorderlessIconButton}" />
                          """
        });

        StyleClassesSection = new DemoSectionViewModel
        {
            Header = LanguageManager.Instance.Page_IconButton_Section_Style_Classes_Header,
            Descriptions = { LanguageManager.Instance.Page_IconButton_Section_Style_Classes_Description },
            SectionTag = DemoSectionTag.Style,
            AnchorId = StyleClassesAnchorId
        };

        StyleClassesSection.CodeSnippets.Add(new DemoSectionCodeSnippetViewModel
        {
            CodeSnippetLanguage = CodeLanguage.Axaml,
            TabName = LanguageManager.Instance.DemoSection_Tab_Xaml,
            CodeSnippet = """
                          <u:IconButton
                          Name="styleTarget"
                          Icon="{StaticResource SemiIconCamera}"
                          Content="Style Target" />
                          """
        });

        IconPlacementSection = new DemoSectionViewModel
        {
            Header = LanguageManager.Instance.Page_IconButton_Section_Icon_Placement_Header,
            Descriptions = { LanguageManager.Instance.Page_IconButton_Section_Icon_Placement_Description },
            SectionTag = DemoSectionTag.Function,
            AnchorId = IconPlacementAnchorId
        };

        IconPlacementSection.CodeSnippets.Add(new DemoSectionCodeSnippetViewModel
        {
            CodeSnippetLanguage = CodeLanguage.Axaml,
            TabName = LanguageManager.Instance.DemoSection_Tab_Xaml,
            CodeSnippet = """
                          <u:IconButton
                              Classes="Large"
                              Icon="{StaticResource SemiIconCamera}"
                              Content="IconButton"
                              IconPlacement="Left" />
                          """
        });

        IconPlacementSection.CodeSnippets.Add(new DemoSectionCodeSnippetViewModel
        {
            CodeSnippetLanguage = CodeLanguage.CSharp,
            TabName = LanguageManager.Instance.DemoSection_Tab_ViewModel,
            CodeSnippet = """
                          private Position _iconPlacement = Position.Left;
                          public Position IconPlacement
                          {
                              get => _iconPlacement;
                              set => SetProperty(ref _iconPlacement, value);
                          }
                          """
        });

        LoadingStateSection = new DemoSectionViewModel
        {
            Header = LanguageManager.Instance.Page_IconButton_Section_Loading_State_Header,
            Descriptions = { LanguageManager.Instance.Page_IconButton_Section_Loading_State_Description },
            SectionTag = DemoSectionTag.Function,
            AnchorId = LoadingStateAnchorId
        };

        LoadingStateSection.CodeSnippets.Add(new DemoSectionCodeSnippetViewModel
        {
            CodeSnippetLanguage = CodeLanguage.Axaml,
            TabName = LanguageManager.Instance.DemoSection_Tab_Xaml,
            CodeSnippet = """
                          <DockPanel HorizontalSpacing="12">
                              <u:IconButton
                                  Icon="{StaticResource SemiIconCamera}"
                                  Content="IconButton"
                                  IsLoading="True" />
                          </DockPanel>
                          """
        });

    }
}
