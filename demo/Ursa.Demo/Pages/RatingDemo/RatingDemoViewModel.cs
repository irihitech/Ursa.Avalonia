using System.Collections.ObjectModel;

using CommunityToolkit.Mvvm.ComponentModel;
using Irihi.Dogma.Controls;
using Irihi.Dogma.Docs;
using Ursa.Demo.Localizations;
using Ursa.Demo.Pages.DummyPages;
using Ursa.Demo.ViewModels;
using Ursa.Demo.ViewModels.Controls;

namespace Ursa.Demo.Pages.RatingDemo;

[DocCategory(Category_Key, IsClickable = false, Parent = ButtonsAndInputsPage.Category_Key)]
[DocPage(Menu_Header, View = typeof(RatingDemo))]
public partial class RatingDemoViewModel : ViewModelBase, IPageMetadataProvider
{
    public const string Category_Key = "Rating";
    public const string Menu_Header = "Menu_Header_Rating";
    private const string BasicUsageAnchorId = "rating-basic-usage";
    private const string CustomCharacterAnchorId = "rating-custom-character";
    private const string SmallStyleAnchorId = "rating-small-style";

    public PageMetadataViewModel PageMetadata { get; set; } = new PageMetadataViewModel()
    {
        Title = LanguageManager.Instance.Page_Title_Rating,
        Description = LanguageManager.Instance.Page_Description_Rating,
        Breadcrumbs = [new BreadcrumbItemData(LanguageManager.Instance.Menu_Category_ButtonsAndInputs), new BreadcrumbItemData(LanguageManager.Instance.Menu_Header_Rating)],
        Tags = ["Rating", "Star", "Input"],
        DemoViewUrl = "https://github.com/irihitech/Ursa.Avalonia/blob/main/demo/Ursa.Demo/Pages/RatingDemo/RatingDemo.axaml",
        DemoViewModelUrl = "https://github.com/irihitech/Ursa.Avalonia/blob/main/demo/Ursa.Demo/Pages/RatingDemo/RatingDemoViewModel.cs",
        InlineXamlSupport = true,
        MvvmSupport = true,
    };

    public RatingDemoViewModel()
    {
        BasicSection = new DemoSectionViewModel
        {
            Header = LanguageManager.Instance.Page_Rating_Section_Basic_Usage_Header,
            SectionTag = DemoSectionTag.Function,
            Descriptions = { LanguageManager.Instance.Page_Rating_Section_Basic_Usage_Description },
            AnchorId = BasicUsageAnchorId,
        };
        BasicSection.CodeSnippets.Add(new DemoSectionCodeSnippetViewModel
        {
            CodeSnippetLanguage = CodeLanguage.Axaml,
            TabName = LanguageManager.Instance.DemoSection_Tab_Xaml,
            CodeSnippet = """
                          <u:Rating
                              AllowClear="{Binding AllowClear}"
                              AllowHalf="{Binding AllowHalf}"
                              Count="{Binding Count}"
                              DefaultValue="{Binding DefaultValue}"
                              IsEnabled="{Binding IsEnabled}"
                              Value="{Binding Value}" />

                          <TextBlock Classes="Secondary"
                                     Text="{Binding Value}" />
                          """
        });
        BasicSection.CodeSnippets.Add(new DemoSectionCodeSnippetViewModel
        {
            CodeSnippetLanguage = CodeLanguage.CSharp,
            TabName = LanguageManager.Instance.DemoSection_Tab_ViewModel,
            CodeSnippet = """
                          [ObservableProperty] public partial bool AllowClear { get; set; } = true;
                          [ObservableProperty] public partial bool AllowHalf { get; set; } = true;
                          [ObservableProperty] public partial bool IsEnabled { get; set; } = true;
                          [ObservableProperty] public partial double Value { get; set; }
                          [ObservableProperty] public partial double DefaultValue { get; set; } = 2.3;
                          [ObservableProperty] public partial int Count { get; set; } = 5;
                          """
        });
        CustomCharacterSection = new DemoSectionViewModel
        {
            Header = LanguageManager.Instance.Page_Rating_Section_Custom_Character_Header,
            SectionTag = DemoSectionTag.Others,
            Descriptions = { LanguageManager.Instance.Page_Rating_Section_Custom_Character_Description },
            AnchorId = CustomCharacterAnchorId,
        };
        CustomCharacterSection.CodeSnippets.Add(new DemoSectionCodeSnippetViewModel
        {
            CodeSnippetLanguage = CodeLanguage.Axaml,
            TabName = LanguageManager.Instance.DemoSection_Tab_Xaml,
            CodeSnippet = """
                          <u:Rating
                              AllowClear="{Binding AllowClear}"
                              AllowHalf="{Binding AllowHalf}"
                              Character="{StaticResource SemiIconLikeHeart}"
                              Count="{Binding Count}"
                              DefaultValue="{Binding DefaultValue}"
                              Foreground="{StaticResource SemiRed5}"
                              Size="48"
                              Value="{Binding Value}" />
                          """
        });
        SmallStyleSection = new DemoSectionViewModel
        {
            Header = LanguageManager.Instance.Page_Rating_Section_Small_Style_Header,
            SectionTag = DemoSectionTag.Style,
            Descriptions = { LanguageManager.Instance.Page_Rating_Section_Small_Style_Description },
            AnchorId = SmallStyleAnchorId,
        };
        SmallStyleSection.CodeSnippets.Add(new DemoSectionCodeSnippetViewModel
        {
            CodeSnippetLanguage = CodeLanguage.Axaml,
            TabName = LanguageManager.Instance.DemoSection_Tab_Xaml,
            CodeSnippet = """
                          <u:Rating
                              AllowClear="{Binding AllowClear}"
                              AllowHalf="{Binding AllowHalf}"
                              Classes="Small"
                              Count="{Binding Count}"
                              DefaultValue="{Binding DefaultValue}"
                              Value="{Binding Value}" />
                          """
        });
    }

    [ObservableProperty] public partial bool AllowClear { get; set; } = true;
    [ObservableProperty] public partial bool AllowHalf { get; set; } = true;
    [ObservableProperty] public partial bool IsEnabled { get; set; } = true;
    [ObservableProperty] public partial double Value { get; set; }
    [ObservableProperty] public partial double DefaultValue { get; set; } = 2.3;
    [ObservableProperty] public partial int Count { get; set; } = 5;

    public DemoSectionViewModel BasicSection { get; }
    public DemoSectionViewModel CustomCharacterSection { get; }
    public DemoSectionViewModel SmallStyleSection { get; }

    public ObservableCollection<AnchorScrollViewerItemViewModel> AnchorItems { get; set; } =
    [
        new()
        {
            Header = LanguageManager.Instance.Page_Rating_Section_Basic_Usage_Header,
            AnchorId = BasicUsageAnchorId,
        },
        new()
        {
            Header = LanguageManager.Instance.Page_Rating_Section_Custom_Character_Header,
            AnchorId = CustomCharacterAnchorId,
        },
        new()
        {
            Header = LanguageManager.Instance.Page_Rating_Section_Small_Style_Header,
            AnchorId = SmallStyleAnchorId,
        },
    ];
}