using System.Collections.ObjectModel;
using Avalonia.Layout;
using CommunityToolkit.Mvvm.ComponentModel;

using Ursa.Demo.ViewModels.Controls;
using Ursa.Demo.Localizations;
using Irihi.Dogma.Docs;
using Irihi.Dogma.Controls;
using Ursa.Demo.Pages.DummyPages;

namespace Ursa.Demo.Pages.RangeSliderDemo;

[DocCategory(Category_Key, IsClickable = false, Parent = ButtonsAndInputsPage.Category_Key)]
[DocPage(Menu_Header, View = typeof(RangeSliderDemo))]
public partial class RangeSliderDemoViewModel: ObservableObject, IPageMetadataProvider
{
    public const string Category_Key = "RangeSlider";
    public const string Menu_Header = "Menu_Header_RangeSlider";
    private const string BasicUsageAnchorId = "range-slider-basic-usage";
    private const string TickSnappingAnchorId = "range-slider-tick-snapping";
    private const string OrientationAnchorId = "range-slider-orientation";

    public PageMetadataViewModel PageMetadata { get; set; } = new PageMetadataViewModel()
    {
        Title = LanguageManager.Instance.Page_Title_RangeSlider,
        Description = LanguageManager.Instance.Page_Description_RangeSlider,
        Breadcrumbs = [new BreadcrumbItemData(LanguageManager.Instance.Menu_Category_ButtonsAndInputs), new BreadcrumbItemData(LanguageManager.Instance.Menu_Header_RangeSlider)],
        Tags = ["RangeSlider", "Slider", "Range"],
        DemoViewUrl = "https://github.com/irihitech/Ursa.Avalonia/blob/main/demo/Ursa.Demo/Pages/RangeSliderDemo/RangeSliderDemo.axaml",
        DemoViewModelUrl = "https://github.com/irihitech/Ursa.Avalonia/blob/main/demo/Ursa.Demo/Pages/RangeSliderDemo/RangeSliderDemoViewModel.cs",
        InlineXamlSupport = true,
        MvvmSupport = true,
    };

    public RangeSliderDemoViewModel()
    {
        BasicUsageSection = new DemoSectionViewModel
        {
            Header = LanguageManager.Instance.Page_RangeSlider_Section_Basic_Usage_Header,
            SectionTag = DemoSectionTag.Function,
            Descriptions = { LanguageManager.Instance.Page_RangeSlider_Section_Basic_Usage_Description },
            AnchorId = BasicUsageAnchorId
        };
        BasicUsageSection.CodeSnippets.Add(new DemoSectionCodeSnippetViewModel
        {
            CodeSnippetLanguage = CodeLanguage.Axaml,
            TabName = LanguageManager.Instance.DemoSection_Tab_Xaml,
            CodeSnippet = """
                          <u:RangeSlider Name="basicRange"
                                         Minimum="0"
                                         Maximum="100"
                                         LowerValue="25"
                                         UpperValue="75" />
                          <TextBlock Text="{Binding #basicRange.LowerValue, StringFormat='Lower value: {0}'}" />
                          <TextBlock Text="{Binding #basicRange.UpperValue, StringFormat='Upper value: {0}'}" />
                          """
        });

        TickSnappingSection = new DemoSectionViewModel
        {
            Header = LanguageManager.Instance.Page_RangeSlider_Section_Tick_Snapping_Header,
            SectionTag = DemoSectionTag.Function,
            Descriptions = { LanguageManager.Instance.Page_RangeSlider_Section_Tick_Snapping_Description },
            AnchorId = TickSnappingAnchorId
        };
        TickSnappingSection.CodeSnippets.Add(new DemoSectionCodeSnippetViewModel
        {
            CodeSnippetLanguage = CodeLanguage.Axaml,
            TabName = LanguageManager.Instance.DemoSection_Tab_Xaml,
            CodeSnippet = """
                          <u:RangeSlider Name="snapRange"
                                         Minimum="0"
                                         Maximum="100"
                                         LowerValue="20"
                                         UpperValue="80"
                                         IsSnapToTick="True"
                                         TickFrequency="10"
                                         TickPlacement="Outside" />
                          """
        });

        OrientationSection = new DemoSectionViewModel
        {
            Header = LanguageManager.Instance.Page_RangeSlider_Section_Orientation_Header,
            SectionTag = DemoSectionTag.Others,
            Descriptions = { LanguageManager.Instance.Page_RangeSlider_Section_Orientation_Description },
            AnchorId = OrientationAnchorId
        };
        OrientationSection.CodeSnippets.Add(new DemoSectionCodeSnippetViewModel
        {
            CodeSnippetLanguage = CodeLanguage.Axaml,
            TabName = LanguageManager.Instance.DemoSection_Tab_Xaml,
            CodeSnippet = """
                          <u:EnumSelector Width="220"
                                         EnumType="Orientation"
                                         Value="{Binding Orientation}" />
                          <u:RangeSlider Name="orientationRange"
                                         Orientation="{Binding Orientation}"
                                         Minimum="0"
                                         Maximum="100"
                                         LowerValue="30"
                                         UpperValue="70"
                                         TickFrequency="10"
                                         TickPlacement="Outside" />
                          """
        });
        OrientationSection.CodeSnippets.Add(new DemoSectionCodeSnippetViewModel
        {
            CodeSnippetLanguage = CodeLanguage.CSharp,
            TabName = LanguageManager.Instance.DemoSection_Tab_ViewModel,
            CodeSnippet = """
                          [ObservableProperty]
                          public partial Orientation Orientation { get; set; } = Orientation.Horizontal;
                          """
        });
    }

    public DemoSectionViewModel BasicUsageSection { get; }
    public DemoSectionViewModel TickSnappingSection { get; }
    public DemoSectionViewModel OrientationSection { get; }

    public ObservableCollection<AnchorScrollViewerItemViewModel> AnchorItems { get; } =
    [
        new()
        {
            Header = LanguageManager.Instance.Page_RangeSlider_Section_Basic_Usage_Header,
            AnchorId = BasicUsageAnchorId
        },
        new()
        {
            Header = LanguageManager.Instance.Page_RangeSlider_Section_Tick_Snapping_Header,
            AnchorId = TickSnappingAnchorId
        },
        new()
        {
            Header = LanguageManager.Instance.Page_RangeSlider_Section_Orientation_Header,
            AnchorId = OrientationAnchorId
        }
    ];

    [ObservableProperty] public partial Orientation Orientation { get; set; } = Orientation.Horizontal;
}
