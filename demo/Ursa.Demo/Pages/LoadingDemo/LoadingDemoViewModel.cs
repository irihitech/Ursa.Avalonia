using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using Ursa.Demo.ViewModels;
using Ursa.Demo.ViewModels.Controls;
using Ursa.Demo.Localizations;
using Irihi.Dogma.Docs;
using Irihi.Dogma.Controls;
using Ursa.Demo.Pages.DummyPages;

namespace Ursa.Demo.Pages.LoadingDemo;

[DocCategory(Category_Key, IsClickable = false, Parent = DialogAndFeedbacksPage.Category_Key)]
[DocPage(Menu_Header, View = typeof(LoadingDemo))]
public partial class LoadingDemoViewModel : ViewModelBase, IPageMetadataProvider
{
    public const string Category_Key = "Loading";
    public const string Menu_Header = "Menu_Header_Loading";
    private const string IndicatorSizesAnchorId = "loading-indicator-sizes";
    private const string LoadingOverlayAnchorId = "loading-overlay";
    private const string LoadingMessageAnchorId = "loading-message-template";
    private const string CustomIndicatorAnchorId = "loading-custom-indicator";

    public PageMetadataViewModel PageMetadata { get; set; } = new PageMetadataViewModel()
    {
        Title = LanguageManager.Instance.Page_Title_Loading,
        Description = LanguageManager.Instance.Page_Description_Loading,
        Breadcrumbs = [new BreadcrumbItemData(LanguageManager.Instance.Menu_Category_DialogAndFeedbacks), new BreadcrumbItemData(LanguageManager.Instance.Menu_Header_Loading)],
        Tags = ["Loading", "Spinner", "Progress"],
        DemoViewUrl = "https://github.com/irihitech/Ursa.Avalonia/blob/main/demo/Ursa.Demo/Pages/LoadingDemo/LoadingDemo.axaml",
        DemoViewModelUrl = "https://github.com/irihitech/Ursa.Avalonia/blob/main/demo/Ursa.Demo/Pages/LoadingDemo/LoadingDemoViewModel.cs",
        InlineXamlSupport = true,
    };

    public LoadingDemoViewModel()
    {
        IndicatorSizesSection = new DemoSectionViewModel
        {
            Header = LanguageManager.Instance.Page_Loading_Section_Indicator_Sizes_Header,
            Descriptions = { LanguageManager.Instance.Page_Loading_Section_Indicator_Sizes_Description },
            SectionTag = DemoSectionTag.Style,
            AnchorId = IndicatorSizesAnchorId
        };
        IndicatorSizesSection.CodeSnippets.Add(new DemoSectionCodeSnippetViewModel
        {
            CodeSnippetLanguage = CodeLanguage.Axaml,
            TabName = LanguageManager.Instance.DemoSection_Tab_Xaml,
            CodeSnippet = """
                          <u:LoadingIcon Classes="Small" />
                          <u:LoadingIcon />
                          <u:LoadingIcon Classes="Large" />
                          """
        });

        LoadingOverlaySection = new DemoSectionViewModel
        {
            Header = LanguageManager.Instance.Page_Loading_Section_Overlay_Header,
            Descriptions = { LanguageManager.Instance.Page_Loading_Section_Overlay_Description },
            SectionTag = DemoSectionTag.Function,
            AnchorId = LoadingOverlayAnchorId
        };
        LoadingOverlaySection.CodeSnippets.Add(new DemoSectionCodeSnippetViewModel
        {
            CodeSnippetLanguage = CodeLanguage.Axaml,
            TabName = LanguageManager.Instance.DemoSection_Tab_Xaml,
            CodeSnippet = """
                          <u:LoadingContainer
                              IsLoading="{Binding IsOverlayLoading}"
                              LoadingMessage="Loading calendar...">
                              <Calendar />
                          </u:LoadingContainer>
                          """
        });
        LoadingOverlaySection.CodeSnippets.Add(new DemoSectionCodeSnippetViewModel
        {
            CodeSnippetLanguage = CodeLanguage.CSharp,
            TabName = LanguageManager.Instance.DemoSection_Tab_ViewModel,
            CodeSnippet = """
                          [ObservableProperty]
                          public partial bool IsOverlayLoading { get; set; } = true;
                          """
        });

        LoadingMessageSection = new DemoSectionViewModel
        {
            Header = LanguageManager.Instance.Page_Loading_Section_Message_Template_Header,
            Descriptions = { LanguageManager.Instance.Page_Loading_Section_Message_Template_Description },
            SectionTag = DemoSectionTag.Function,
            AnchorId = LoadingMessageAnchorId
        };
        LoadingMessageSection.CodeSnippets.Add(new DemoSectionCodeSnippetViewModel
        {
            CodeSnippetLanguage = CodeLanguage.Axaml,
            TabName = LanguageManager.Instance.DemoSection_Tab_Xaml,
            CodeSnippet = """
                          <u:LoadingContainer IsLoading="True" LoadingMessage="Syncing your data">
                              <u:LoadingContainer.LoadingMessageTemplate>
                                  <DataTemplate DataType="x:String">
                                      <TextBlock Text="{Binding}" FontSize="16" FontWeight="SemiBold" />
                                  </DataTemplate>
                              </u:LoadingContainer.LoadingMessageTemplate>
                              <Calendar />
                          </u:LoadingContainer>
                          """
        });

        CustomIndicatorSection = new DemoSectionViewModel
        {
            Header = LanguageManager.Instance.Page_Loading_Section_Custom_Indicator_Header,
            Descriptions = { LanguageManager.Instance.Page_Loading_Section_Custom_Indicator_Description },
            SectionTag = DemoSectionTag.Function,
            AnchorId = CustomIndicatorAnchorId
        };
        CustomIndicatorSection.CodeSnippets.Add(new DemoSectionCodeSnippetViewModel
        {
            CodeSnippetLanguage = CodeLanguage.Axaml,
            TabName = LanguageManager.Instance.DemoSection_Tab_Xaml,
            CodeSnippet = """
                          <u:LoadingContainer IsLoading="True" LoadingMessage="Preparing report">
                              <u:LoadingContainer.Indicator>
                                  <ProgressBar Width="140" IsIndeterminate="True" />
                              </u:LoadingContainer.Indicator>
                              <StackPanel>
                                  <TextBlock Text="Monthly report" />
                                  <TextBlock Text="Report content" />
                              </StackPanel>
                          </u:LoadingContainer>
                          """
        });
    }

    public DemoSectionViewModel IndicatorSizesSection { get; }
    public DemoSectionViewModel LoadingOverlaySection { get; }
    public DemoSectionViewModel LoadingMessageSection { get; }
    public DemoSectionViewModel CustomIndicatorSection { get; }

    public ObservableCollection<AnchorScrollViewerItemViewModel> AnchorItems { get; set; } =
    [
        new()
        {
            Header = LanguageManager.Instance.Page_Loading_Section_Indicator_Sizes_Header,
            AnchorId = IndicatorSizesAnchorId
        },
        new()
        {
            Header = LanguageManager.Instance.Page_Loading_Section_Overlay_Header,
            AnchorId = LoadingOverlayAnchorId
        },
        new()
        {
            Header = LanguageManager.Instance.Page_Loading_Section_Message_Template_Header,
            AnchorId = LoadingMessageAnchorId
        },
        new()
        {
            Header = LanguageManager.Instance.Page_Loading_Section_Custom_Indicator_Header,
            AnchorId = CustomIndicatorAnchorId
        },
    ];

    [ObservableProperty]
    public partial bool IsOverlayLoading { get; set; } = true;
}
