using System.Collections.ObjectModel;
using Irihi.Dogma.Controls;
using Irihi.Dogma.Docs;
using Ursa.Demo.Localizations;
using Ursa.Demo.Pages.DummyPages;
using Ursa.Demo.ViewModels.Controls;

namespace Ursa.Demo.Pages.ThemeTogglerDemo;

[DocCategory(Category_Key, IsClickable = false, Parent = ButtonsAndInputsPage.Category_Key)]
[DocPage(Menu_Header, View = typeof(ThemeTogglerDemo))]
public class ThemeTogglerDemoViewModel : IPageMetadataProvider
{
    public const string Category_Key = "ThemeToggler";
    public const string Menu_Header = "Menu_Header_ThemeToggler";
    private const string GlobalThemeAnchorId = "theme-toggler-global";
    private const string ThreeStateAnchorId = "theme-toggler-three-state";
    private const string IndicatorAnchorId = "theme-toggler-indicator";
    private const string ScopedThemeAnchorId = "theme-toggler-scoped-theme";

    public PageMetadataViewModel PageMetadata { get; set; } = new PageMetadataViewModel()
    {
        Title = LanguageManager.Instance.Page_Title_ThemeToggler,
        Description = LanguageManager.Instance.Page_Description_ThemeToggler,
        Breadcrumbs = [new BreadcrumbItemData(LanguageManager.Instance.Menu_Category_ButtonsAndInputs), new BreadcrumbItemData(LanguageManager.Instance.Menu_Header_ThemeToggler)],
        Tags = ["ThemeToggler", "Theme", "Toggle"],
        DemoViewUrl = "https://github.com/irihitech/Ursa.Avalonia/blob/main/demo/Ursa.Demo/Pages/ThemeTogglerDemo/ThemeTogglerDemo.axaml",
        DemoViewModelUrl = "https://github.com/irihitech/Ursa.Avalonia/blob/main/demo/Ursa.Demo/Pages/ThemeTogglerDemo/ThemeTogglerDemoViewModel.cs",
        InlineXamlSupport = true,
    };

    public DemoSectionViewModel GlobalThemeSection { get; }
    public DemoSectionViewModel ThreeStateSection { get; }
    public DemoSectionViewModel IndicatorSection { get; }
    public DemoSectionViewModel ScopedThemeSection { get; }

    public ObservableCollection<AnchorScrollViewerItemViewModel> AnchorItems { get; } =
    [
        new()
        {
            Header = LanguageManager.Instance.Page_ThemeToggler_Section_Global_Header,
            AnchorId = GlobalThemeAnchorId
        },
        new()
        {
            Header = LanguageManager.Instance.Page_ThemeToggler_Section_Three_State_Header,
            AnchorId = ThreeStateAnchorId
        },
        new()
        {
            Header = LanguageManager.Instance.Page_ThemeToggler_Section_Indicator_Header,
            AnchorId = IndicatorAnchorId
        },
        new()
        {
            Header = LanguageManager.Instance.Page_ThemeToggler_Section_Scoped_Theme_Header,
            AnchorId = ScopedThemeAnchorId
        },
    ];

    public ThemeTogglerDemoViewModel()
    {
        GlobalThemeSection = new DemoSectionViewModel
        {
            Header = LanguageManager.Instance.Page_ThemeToggler_Section_Global_Header,
            Descriptions = { LanguageManager.Instance.Page_ThemeToggler_Section_Global_Description },
            SectionTag = DemoSectionTag.Function,
            AnchorId = GlobalThemeAnchorId
        };
        AddXamlSnippet(GlobalThemeSection, """
            <u:ThemeToggleButton />
            """);

        ThreeStateSection = new DemoSectionViewModel
        {
            Header = LanguageManager.Instance.Page_ThemeToggler_Section_Three_State_Header,
            Descriptions = { LanguageManager.Instance.Page_ThemeToggler_Section_Three_State_Description },
            SectionTag = DemoSectionTag.Function,
            AnchorId = ThreeStateAnchorId
        };
        AddXamlSnippet(ThreeStateSection, """
            <u:ThemeToggleButton IsThreeState="True" />
            """);

        IndicatorSection = new DemoSectionViewModel
        {
            Header = LanguageManager.Instance.Page_ThemeToggler_Section_Indicator_Header,
            Descriptions = { LanguageManager.Instance.Page_ThemeToggler_Section_Indicator_Description },
            SectionTag = DemoSectionTag.Function,
            AnchorId = IndicatorAnchorId
        };
        AddXamlSnippet(IndicatorSection, """
            <u:ThemeToggleButton
                IsThreeState="True"
                IsHitTestVisible="False"
                Mode="Indicator" />
            """);

        ScopedThemeSection = new DemoSectionViewModel
        {
            Header = LanguageManager.Instance.Page_ThemeToggler_Section_Scoped_Theme_Header,
            Descriptions = { LanguageManager.Instance.Page_ThemeToggler_Section_Scoped_Theme_Description },
            SectionTag = DemoSectionTag.Function,
            AnchorId = ScopedThemeAnchorId
        };
        AddXamlSnippet(ScopedThemeSection, """
            <Grid ColumnDefinitions="Auto,*">
                <u:ThemeToggleButton
                    Grid.Column="0"
                    TargetScope="{Binding #scope}" />
                <ThemeVariantScope
                    x:Name="scope"
                    Grid.Column="1"
                    RequestedThemeVariant="Dark">
                    <Border Theme="{DynamicResource CardBorder}" Padding="16">
                        <StackPanel>
                            <Button Content="Inside scope" />
                            <u:ThemeToggleButton />
                        </StackPanel>
                    </Border>
                </ThemeVariantScope>
            </Grid>
            """);
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
}
