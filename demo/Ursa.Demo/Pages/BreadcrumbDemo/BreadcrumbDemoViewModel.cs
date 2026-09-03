using System.Collections.ObjectModel;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Irihi.Dogma.Controls;
using Ursa.Controls;

using Ursa.Demo.ViewModels.Controls;
using Ursa.Demo.Localizations;
using Irihi.Dogma.Docs;
using Ursa.Demo.Pages.DummyPages;

namespace Ursa.Demo.Pages.BreadcrumbDemo;

[DocCategory(Category_Key, IsClickable = false, Parent = NavigationAndMenusPage.Category_Key)]
[DocPage(Menu_Header, View = typeof(BreadcrumbDemo))]
public class BreadcrumbDemoViewModel: ObservableObject, IPageMetadataProvider
{
    public const string Category_Key = "Breadcrumb";
    public const string Menu_Header = "Menu_Header_Breadcrumb";
    private const string InlineXamlDefinitionAnchorId = "breadcrumb-inline-xaml-definition";
    private const string MvvmBindingAnchorId = "breadcrumb-mvvm-binding";
    private const string SizeStyleClassAnchorId = "breadcrumb-size-style-class";
    public PageMetadataViewModel PageMetadata { get; set; } = new PageMetadataViewModel()
    {
        Title = LanguageManager.Instance.Page_Title_Breadcrumb,
        Description = LanguageManager.Instance.Page_Description_Breadcrumb,
        Breadcrumbs = [new BreadcrumbItemData(LanguageManager.Instance.Menu_Category_NavigationAndMenus), new BreadcrumbItemData(LanguageManager.Instance.Menu_Header_Breadcrumb)],
        Tags = ["Breadcrumb", "Navigation", "Path"],
        DemoViewUrl = "https://github.com/irihitech/Ursa.Avalonia/blob/main/demo/Ursa.Demo/Pages/BreadcrumbDemo/BreadcrumbDemo.axaml",
        DemoViewModelUrl = "https://github.com/irihitech/Ursa.Avalonia/blob/main/demo/Ursa.Demo/Pages/BreadcrumbDemo/BreadcrumbDemoViewModel.cs",
        InlineXamlSupport = true,
        MvvmSupport = true,
    };

    public DemoSectionViewModel InlineXamlDefinitionSection { get; }
    public DemoSectionViewModel MvvmBindingSection { get; }
    public DemoSectionViewModel SizeStyleClassSection { get; }

    public ObservableCollection<AnchorScrollViewerItemViewModel> AnchorItems { get; } =
    [
        new()
        {
            Header = LanguageManager.Instance.Page_Breadcrumb_Section_Inline_Xaml_Definition_Header,
            AnchorId = InlineXamlDefinitionAnchorId
        },
        new()
        {
            Header = LanguageManager.Instance.Page_Breadcrumb_Section_Mvvm_Binding_Header,
            AnchorId = MvvmBindingAnchorId
        },
        new()
        {
            Header = LanguageManager.Instance.Page_Breadcrumb_Section_Size_Style_Class_Header,
            AnchorId = SizeStyleClassAnchorId
        }
    ];

    public ObservableCollection<BreadcrumbDemoItem> Items1 { get; set; } =
    [
        new BreadcrumbDemoItem { Section = "Home", Icon = "Home" },
        new BreadcrumbDemoItem { Section = "Page 1", Icon = "Page" },
        new BreadcrumbDemoItem { Section = "Page 2", Icon = "Page" },
        new BreadcrumbDemoItem { Section = "Page 3", Icon = "Page" },
        new BreadcrumbDemoItem { Section = "Page 4", Icon = "Page", IsReadOnly = true }
    ];

    public BreadcrumbDemoViewModel()
    {
        InlineXamlDefinitionSection = new DemoSectionViewModel
        {
            Header = LanguageManager.Instance.Page_Breadcrumb_Section_Inline_Xaml_Definition_Header,
            Descriptions = { LanguageManager.Instance.Page_Breadcrumb_Section_Inline_Xaml_Definition_Description },
            SectionTag = DemoSectionTag.Function,
            AnchorId = InlineXamlDefinitionAnchorId
        };

        InlineXamlDefinitionSection.CodeSnippets.Add(new DemoSectionCodeSnippetViewModel
        {
            CodeSnippetLanguage = CodeLanguage.Axaml,
            TabName = LanguageManager.Instance.DemoSection_Tab_Xaml,
            CodeSnippet = """
                          <u:Breadcrumb>
                              <u:BreadcrumbItem Content="Home" Icon="Home" />
                              <u:BreadcrumbItem Content="Components" Icon="Page" />
                              <u:BreadcrumbItem Content="Breadcrumb" Icon="Page" IsReadOnly="True" />
                          </u:Breadcrumb>
                          """
        });

        MvvmBindingSection = new DemoSectionViewModel
        {
            Header = LanguageManager.Instance.Page_Breadcrumb_Section_Mvvm_Binding_Header,
            Descriptions = { LanguageManager.Instance.Page_Breadcrumb_Section_Mvvm_Binding_Description },
            SectionTag = DemoSectionTag.Function,
            AnchorId = MvvmBindingAnchorId
        };

        MvvmBindingSection.CodeSnippets.Add(new DemoSectionCodeSnippetViewModel
        {
            CodeSnippetLanguage = CodeLanguage.Axaml,
            TabName = LanguageManager.Instance.DemoSection_Tab_Xaml,
            CodeSnippet = """
                          <u:Breadcrumb
                              DisplayMemberBinding="{Binding Section}"
                              IconBinding="{Binding Icon}"
                              CommandBinding="{Binding Command}"
                              ItemsSource="{Binding Items1}" />
                          """
        });

        MvvmBindingSection.CodeSnippets.Add(new DemoSectionCodeSnippetViewModel
        {
            CodeSnippetLanguage = CodeLanguage.CSharp,
            TabName = LanguageManager.Instance.DemoSection_Tab_ViewModel,
            CodeSnippet = """
                          public ObservableCollection<BreadcrumbDemoItem> Items1 { get; } =
                          [
                              new BreadcrumbDemoItem { Section = "Home", Icon = "Home" },
                              new BreadcrumbDemoItem { Section = "Page 1", Icon = "Page" },
                              new BreadcrumbDemoItem { Section = "Page 2", Icon = "Page" },
                              new BreadcrumbDemoItem { Section = "Page 3", Icon = "Page" },
                              new BreadcrumbDemoItem { Section = "Page 4", Icon = "Page", IsReadOnly = true }
                          ];
                          """
        });
        MvvmBindingSection.CodeSnippets.Add(new DemoSectionCodeSnippetViewModel()
        {
            CodeSnippetLanguage = CodeLanguage.CSharp,
            TabName = LanguageManager.Instance.DemoSection_Tab_ViewModel,
            CodeSnippet = """
                          public partial class BreadcrumbDemoItem: ObservableObject
                          {
                              public string? Section { get; set; }
                              public string? Icon { get; set; }
                              [ObservableProperty] public partial bool IsReadOnly { get; set; }
                              
                              public ICommand Command { get; set; }

                              public BreadcrumbDemoItem()
                              {
                                  Command = new AsyncRelayCommand(async () =>
                                  {
                                      await OverlayMessageBox.ShowAsync(Section ?? string.Empty);
                                  });
                              }
                          }
                          """
        });

        SizeStyleClassSection = new DemoSectionViewModel
        {
            Header = LanguageManager.Instance.Page_Breadcrumb_Section_Size_Style_Class_Header,
            Descriptions = { LanguageManager.Instance.Page_Breadcrumb_Section_Size_Style_Class_Description },
            SectionTag = DemoSectionTag.Style,
            AnchorId = SizeStyleClassAnchorId
        };

        SizeStyleClassSection.CodeSnippets.Add(new DemoSectionCodeSnippetViewModel
        {
            CodeSnippetLanguage = CodeLanguage.Axaml,
            TabName = LanguageManager.Instance.DemoSection_Tab_Xaml,
            CodeSnippet = """
                          <u:Breadcrumb Classes="Small">
                              <u:BreadcrumbItem Content="Home" Icon="Home" />
                              <u:BreadcrumbItem Content="Components" Icon="Page" />
                              <u:BreadcrumbItem Content="Breadcrumb" Icon="Page" IsReadOnly="True" />
                          </u:Breadcrumb>
                          """
        });
    }
}

public partial class BreadcrumbDemoItem: ObservableObject
{
    public string? Section { get; set; }
    public string? Icon { get; set; }
    [ObservableProperty] public partial bool IsReadOnly { get; set; }
    
    public ICommand Command { get; set; }

    public BreadcrumbDemoItem()
    {
        Command = new AsyncRelayCommand(async () =>
        {
            await OverlayMessageBox.ShowAsync(Section ?? string.Empty);
        });
    }
}
