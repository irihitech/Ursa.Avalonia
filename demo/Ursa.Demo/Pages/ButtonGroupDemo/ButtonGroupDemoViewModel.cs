using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using Irihi.Dogma.Controls;
using Irihi.Dogma.Docs;
using Ursa.Controls;
using Ursa.Demo.ViewModels;
using Ursa.Demo.ViewModels.Controls;

using Ursa.Demo.Localizations;
using Ursa.Demo.Pages.DummyPages;

namespace Ursa.Demo.Pages.ButtonGroupDemo;

[DocCategory(Category_Key, IsClickable = false, Parent = ButtonsAndInputsPage.Category_Key)]
[DocPage(Menu_Header, View = typeof(ButtonGroupDemo))]
public class ButtonGroupDemoViewModel: ViewModelBase, IPageMetadataProvider
{
    public const string Category_Key = "ButtonGroup";
    public const string Menu_Header = "Menu_Header_ButtonGroup";
    private const string ItemTemplatesAndCommandsAnchorId = "button-group-item-templates-and-commands";
    private const string StyleClassesAnchorId = "button-group-style-classes";
    public PageMetadataViewModel  PageMetadata { get; set; } = new PageMetadataViewModel()
    {
        Title = LanguageManager.Instance.Page_Title_ButtonGroup,
        Description = LanguageManager.Instance.Page_Description_ButtonGroup,
        Breadcrumbs = [new BreadcrumbItemData(LanguageManager.Instance.Menu_Category_ButtonsAndInputs), new BreadcrumbItemData(LanguageManager.Instance.Menu_Header_ButtonGroup)],
        Tags = ["ButtonGroup",  "Button", "Command", "Collection" ],
        DemoViewModelUrl = "https://github.com/irihitech/Ursa.Avalonia/blob/main/demo/Ursa.Demo/Pages/ButtonGroupDemo/ButtonGroupDemoViewModel.cs",
        InlineXamlSupport = true,
        AvaloniaExclusive = false,
        MvvmSupport = true,
    };
    public DemoSectionViewModel ItemTemplatesAndCommandsSection { get; }
    public DemoSectionViewModel StyleClassesSection { get; }
    public ObservableCollection<AnchorScrollViewerItemViewModel> AnchorItems { get; } =
    [
        new()
        {
            Header = LanguageManager.Instance.Page_ButtonGroup_Section_Item_Templates_And_Commands_Header,
            AnchorId = ItemTemplatesAndCommandsAnchorId
        },
        new()
        {
            Header = LanguageManager.Instance.Page_ButtonGroup_Section_Style_Classes_Header,
            AnchorId = StyleClassesAnchorId
        }
    ];

    public ButtonGroupDemoViewModel()
    {
        ItemTemplatesAndCommandsSection = new DemoSectionViewModel
        {
            Header = LanguageManager.Instance.Page_ButtonGroup_Section_Item_Templates_And_Commands_Header,
            Descriptions = { LanguageManager.Instance.Page_ButtonGroup_Section_Item_Templates_And_Commands_Description },
            SectionTag = DemoSectionTag.Function,
            AnchorId = ItemTemplatesAndCommandsAnchorId
        };
        ItemTemplatesAndCommandsSection.CodeSnippets.Add(new DemoSectionCodeSnippetViewModel
        {
            CodeSnippetLanguage = CodeLanguage.Axaml,
            TabName = LanguageManager.Instance.DemoSection_Tab_Xaml,
            CodeSnippet = """
                          <u:ButtonGroup
                              Classes="Primary Solid"
                              CommandBinding="{Binding InvokeCommand}"
                              ItemsSource="{Binding Items}">
                              <u:ButtonGroup.ItemTemplate>
                                  <DataTemplate x:DataType="vm:ButtonItem">
                                      <TextBlock>
                                          <Run Text="🐼" />
                                          <Run Text="{Binding Name}" />
                                      </TextBlock>
                                  </DataTemplate>
                              </u:ButtonGroup.ItemTemplate>
                          </u:ButtonGroup>
                          """
        });
        ItemTemplatesAndCommandsSection.CodeSnippets.Add(new DemoSectionCodeSnippetViewModel
        {
            CodeSnippetLanguage = CodeLanguage.CSharp,
            TabName = LanguageManager.Instance.DemoSection_Tab_ViewModel,
            CodeSnippet = """
                          public ObservableCollection<ButtonItem> Items { get; set; } = new();
                          """
        });
        ItemTemplatesAndCommandsSection.CodeSnippets.Add(new DemoSectionCodeSnippetViewModel()
        {
            CodeSnippetLanguage =  CodeLanguage.CSharp,
            TabName = LanguageManager.Instance.DemoSection_Tab_ViewModel,
            CodeSnippet = """
                          public class ButtonItem
                          {
                              public string? Name { get; set; }
                              public ICommand InvokeCommand { get; set; }
                          
                              public ButtonItem()
                              {
                                  InvokeCommand = new AsyncRelayCommand(Invoke);
                              }
                          
                              private async Task Invoke()
                              {
                                  await OverlayMessageBox.ShowAsync("Hello " + Name);
                              }
                          }
                          """
        });

        StyleClassesSection = new DemoSectionViewModel
        {
            Header = LanguageManager.Instance.Page_ButtonGroup_Section_Style_Classes_Header,
            Descriptions = { LanguageManager.Instance.Page_ButtonGroup_Section_Style_Classes_Description },
            SectionTag = DemoSectionTag.Style,
            AnchorId = StyleClassesAnchorId
        };
        StyleClassesSection.CodeSnippets.Add(new DemoSectionCodeSnippetViewModel
        {
            CodeSnippetLanguage = CodeLanguage.Axaml,
            TabName = LanguageManager.Instance.DemoSection_Tab_Xaml,
            CodeSnippet = """
                          <u:ButtonGroup
                              Classes="Warning Small"
                              ContentBinding="{Binding Name}"
                              CommandBinding="{Binding InvokeCommand}"
                              ItemsSource="{Binding Items}" />
                          """
        });
    }

    public ObservableCollection<ButtonItem> Items { get; set; } = new ()
    {
        new ButtonItem(){Name = "Ding" },
        new ButtonItem(){Name = "Otter" },
        new ButtonItem(){Name = "Husky" },
        new ButtonItem(){Name = "Mr. 17" },
        new ButtonItem(){Name = "Cass" },
    };
}

public class ButtonItem
{
    public string? Name { get; set; }
    public ICommand InvokeCommand { get; set; }

    public ButtonItem()
    {
        InvokeCommand = new AsyncRelayCommand(Invoke);
    }

    private async Task Invoke()
    {
        await OverlayMessageBox.ShowAsync("Hello " + Name);
    }
}
