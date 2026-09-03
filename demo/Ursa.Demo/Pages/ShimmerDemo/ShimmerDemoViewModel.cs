using System.Collections.ObjectModel;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Irihi.Dogma.Controls;
using Irihi.Dogma.Docs;
using Ursa.Demo.Localizations;
using Ursa.Demo.Pages.DummyPages;
using Ursa.Demo.ViewModels;
using Ursa.Demo.ViewModels.Controls;

namespace Ursa.Demo.Pages.ShimmerDemo;

[DocCategory(Category_Key, IsClickable = false, Parent = DialogAndFeedbacksPage.Category_Key)]
[DocPage(Menu_Header, View = typeof(ShimmerDemo))]
public partial class ShimmerDemoViewModel : ViewModelBase, IPageMetadataProvider
{
    public const string Category_Key = "Shimmer";
    public const string Menu_Header = "Menu_Header_Shimmer";
    private const string BasicUsageAnchorId = "shimmer-basic-usage";
    private const string CustomColorsAnchorId = "shimmer-custom-colors";
    private const string AttachedPropertiesAnchorId = "shimmer-attached-properties";
    private const string LiveStateAnchorId = "shimmer-live-state";

    public PageMetadataViewModel PageMetadata { get; set; } = new()
    {
        Title = LanguageManager.Instance.Page_Title_Shimmer,
        Description = LanguageManager.Instance.Page_Description_Shimmer,
        Breadcrumbs = [new BreadcrumbItemData(LanguageManager.Instance.Menu_Category_DialogAndFeedbacks), new BreadcrumbItemData(LanguageManager.Instance.Menu_Header_Shimmer)],
        Tags = ["Shimmer", "Loading", "Text"],
        DemoViewUrl = "https://github.com/irihitech/Ursa.Avalonia/blob/main/demo/Ursa.Demo/Pages/ShimmerDemo/ShimmerDemo.axaml",
        DemoViewModelUrl = "https://github.com/irihitech/Ursa.Avalonia/blob/main/demo/Ursa.Demo/Pages/ShimmerDemo/ShimmerDemoViewModel.cs",
        InlineXamlSupport = true,
    };

    public ShimmerDemoViewModel()
    {
        BasicSection = new DemoSectionViewModel
        {
            Header = LanguageManager.Instance.Page_Shimmer_Section_Basic_Usage_Header,
            SectionTag = DemoSectionTag.Function,
            Descriptions = { LanguageManager.Instance.Page_Shimmer_Section_Basic_Usage_Description },
            AnchorId = BasicUsageAnchorId,
        };
        BasicSection.CodeSnippets.Add(new DemoSectionCodeSnippetViewModel
        {
            CodeSnippetLanguage = CodeLanguage.Axaml,
            TabName = LanguageManager.Instance.DemoSection_Tab_Xaml,
            CodeSnippet = """
                          <u:ShimmerText FontSize="24"
                                         Text="Thinking..." />

                          <u:ShimmerSelectableText FontSize="24"
                                                   Text="Thinking..." />
                          """
        });

        CustomColorsSection = new DemoSectionViewModel
        {
            Header = LanguageManager.Instance.Page_Shimmer_Section_Custom_Colors_Header,
            SectionTag = DemoSectionTag.Function,
            Descriptions = { LanguageManager.Instance.Page_Shimmer_Section_Custom_Colors_Description },
            AnchorId = CustomColorsAnchorId,
        };
        CustomColorsSection.CodeSnippets.Add(new DemoSectionCodeSnippetViewModel
        {
            CodeSnippetLanguage = CodeLanguage.Axaml,
            TabName = LanguageManager.Instance.DemoSection_Tab_Xaml,
            CodeSnippet = """
                          <u:ShimmerText FontSize="24"
                                         Text="Generating a response…"
                                         BaseColor="{DynamicResource SemiGrey7Color}"
                                         HighlightColor="{DynamicResource SemiBlue5Color}"
                                         Duration="00:00:02.5" />
                          """
        });

        AttachedPropertiesSection = new DemoSectionViewModel
        {
            Header = LanguageManager.Instance.Page_Shimmer_Section_Attached_Properties_Header,
            SectionTag = DemoSectionTag.Others,
            Descriptions = { LanguageManager.Instance.Page_Shimmer_Section_Attached_Properties_Description },
            AnchorId = AttachedPropertiesAnchorId,
        };
        AttachedPropertiesSection.CodeSnippets.Add(new DemoSectionCodeSnippetViewModel
        {
            CodeSnippetLanguage = CodeLanguage.Axaml,
            TabName = LanguageManager.Instance.DemoSection_Tab_Xaml,
            CodeSnippet = """
                          <Border Width="320"
                                  Height="16"
                                  CornerRadius="8"
                                  u:Shimmer.BaseColor="{DynamicResource SemiGrey1Color}"
                                  u:Shimmer.HighlightColor="{DynamicResource SemiGrey5Color}"
                                  u:Shimmer.IsActive="True" />
                          """
        });

        LiveStateSection = new DemoSectionViewModel
        {
            Header = LanguageManager.Instance.Page_Shimmer_Section_Live_State_Header,
            SectionTag = DemoSectionTag.Function,
            Descriptions = { LanguageManager.Instance.Page_Shimmer_Section_Live_State_Description },
            AnchorId = LiveStateAnchorId,
        };
        LiveStateSection.CodeSnippets.Add(new DemoSectionCodeSnippetViewModel
        {
            CodeSnippetLanguage = CodeLanguage.Axaml,
            TabName = LanguageManager.Instance.DemoSection_Tab_Xaml,
            CodeSnippet = """
                          <StackPanel Orientation="Horizontal"
                                      Spacing="8">
                              <Button Content="Pause"
                                      Command="{Binding PauseCommand}" />
                              <Button Content="Resume"
                                      Command="{Binding ResumeCommand}" />
                          </StackPanel>

                          <u:ShimmerText FontSize="24"
                                         Text="Toggle me"
                                         BaseColor="{DynamicResource SemiGrey9Color}"
                                         HighlightColor="{DynamicResource SemiOrange5Color}"
                                         IsActive="{Binding IsActive}" />
                          """
        });
        LiveStateSection.CodeSnippets.Add(new DemoSectionCodeSnippetViewModel
        {
            CodeSnippetLanguage = CodeLanguage.CSharp,
            TabName = LanguageManager.Instance.DemoSection_Tab_ViewModel,
            CodeSnippet = """
                          [ObservableProperty] public partial bool IsActive { get; set; } = true;

                          [RelayCommand]
                          private void Pause() => IsActive = false;

                          [RelayCommand]
                          private void Resume() => IsActive = true;
                          """
        });
    }

    public DemoSectionViewModel BasicSection { get; }
    public DemoSectionViewModel CustomColorsSection { get; }
    public DemoSectionViewModel AttachedPropertiesSection { get; }
    public DemoSectionViewModel LiveStateSection { get; }

    public ObservableCollection<AnchorScrollViewerItemViewModel> AnchorItems { get; set; } =
    [
        new()
        {
            Header = LanguageManager.Instance.Page_Shimmer_Section_Basic_Usage_Header,
            AnchorId = BasicUsageAnchorId,
        },
        new()
        {
            Header = LanguageManager.Instance.Page_Shimmer_Section_Custom_Colors_Header,
            AnchorId = CustomColorsAnchorId,
        },
        new()
        {
            Header = LanguageManager.Instance.Page_Shimmer_Section_Attached_Properties_Header,
            AnchorId = AttachedPropertiesAnchorId,
        },
        new()
        {
            Header = LanguageManager.Instance.Page_Shimmer_Section_Live_State_Header,
            AnchorId = LiveStateAnchorId,
        },
    ];

    [ObservableProperty] public partial bool IsActive { get; set; } = true;

    [RelayCommand]
    private void Pause() => IsActive = false;

    [RelayCommand]
    private void Resume() => IsActive = true;
}
