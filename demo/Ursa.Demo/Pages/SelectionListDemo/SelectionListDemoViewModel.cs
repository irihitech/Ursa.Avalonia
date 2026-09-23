using Avalonia.Collections;
using CommunityToolkit.Mvvm.ComponentModel;
using Irihi.Dogma.Controls;
using Irihi.Dogma.Docs;
using Ursa.Demo.Localizations;
using Ursa.Demo.Pages.DummyPages;
using Ursa.Demo.ViewModels.Controls;

namespace Ursa.Demo.Pages.SelectionListDemo;

[DocCategory(Category_Key, IsClickable = false, Parent = ButtonsAndInputsPage.Category_Key)]
[DocPage(Menu_Header, View = typeof(SelectionListDemo))]
public partial class SelectionListDemoViewModel : ObservableObject, IPageMetadataProvider
{
    public const string Category_Key = "SelectionList";
    public const string Menu_Header = "Menu_Header_SelectionList";

    private const string BasicUsageAnchorId = "selection-list-basic-usage";
    private const string CustomIndicatorAnchorId = "selection-list-custom-indicator";
    private const string FlyoutAnchorId = "selection-list-flyout";
    private const string ClearableAnchorId = "selection-list-clearable";

    public PageMetadataViewModel PageMetadata { get; set; } = new()
    {
        Title = LanguageManager.Instance.Page_Title_SelectionList,
        Description = LanguageManager.Instance.Page_Description_SelectionList,
        Breadcrumbs = [new BreadcrumbItemData(LanguageManager.Instance.Menu_Category_ButtonsAndInputs), new BreadcrumbItemData(LanguageManager.Instance.Menu_Header_SelectionList)],
        Tags = ["SelectionList", "List", "Selection"],
        DemoViewUrl = "https://github.com/irihitech/Ursa.Avalonia/blob/main/demo/Ursa.Demo/Pages/SelectionListDemo/SelectionListDemo.axaml",
        DemoViewModelUrl = "https://github.com/irihitech/Ursa.Avalonia/blob/main/demo/Ursa.Demo/Pages/SelectionListDemo/SelectionListDemoViewModel.cs",
        InlineXamlSupport = true,
        MvvmSupport = true,
    };

    public SelectionListDemoViewModel()
    {
        BasicItems = ["Ding", "Otter", "Husky", "Mr. 17", "Cass"];
        CustomIndicatorItems = ["North", "East", "South", "West"];
        FlyoutItems = ["Primary", "Secondary", "Tertiary"];
        ClearableItems = ["One", "Two", "Three", "Four"];

        BasicSelectedItem = BasicItems[0];
        CustomIndicatorSelectedItem = CustomIndicatorItems[0];
        FlyoutSelectedItem = FlyoutItems[0];
        ClearableSelectedItem = ClearableItems[0];

        BasicUsageSection = new DemoSectionViewModel
        {
            Header = LanguageManager.Instance.Page_SelectionList_Section_Basic_Usage_Header,
            SectionTag = DemoSectionTag.Function,
            Descriptions = { LanguageManager.Instance.Page_SelectionList_Section_Basic_Usage_Description },
            AnchorId = BasicUsageAnchorId
        };
        BasicUsageSection.CodeSnippets.Add(new DemoSectionCodeSnippetViewModel
        {
            CodeSnippetLanguage = CodeLanguage.Axaml,
            TabName = LanguageManager.Instance.DemoSection_Tab_Xaml,
            CodeSnippet = """
                          <u:SelectionList
                              ItemsSource="{Binding BasicItems}"
                              SelectedItem="{Binding BasicSelectedItem}">
                              <u:SelectionList.ItemsPanel>
                                  <ItemsPanelTemplate>
                                      <StackPanel HorizontalAlignment="Center" Orientation="Horizontal" />
                                  </ItemsPanelTemplate>
                              </u:SelectionList.ItemsPanel>
                          </u:SelectionList>
                          """
        });
        BasicUsageSection.CodeSnippets.Add(new DemoSectionCodeSnippetViewModel
        {
            CodeSnippetLanguage = CodeLanguage.CSharp,
            TabName = LanguageManager.Instance.DemoSection_Tab_ViewModel,
            CodeSnippet = """
                          public ObservableCollection<string> BasicItems { get; }

                          [ObservableProperty]
                          public partial string? BasicSelectedItem { get; set; }
                          """
        });

        CustomIndicatorSection = new DemoSectionViewModel
        {
            Header = LanguageManager.Instance.Page_SelectionList_Section_Custom_Indicator_Header,
            SectionTag = DemoSectionTag.Style,
            Descriptions = { LanguageManager.Instance.Page_SelectionList_Section_Custom_Indicator_Description },
            AnchorId = CustomIndicatorAnchorId
        };
        CustomIndicatorSection.CodeSnippets.Add(new DemoSectionCodeSnippetViewModel
        {
            CodeSnippetLanguage = CodeLanguage.Axaml,
            TabName = LanguageManager.Instance.DemoSection_Tab_Xaml,
            CodeSnippet = """
                          <u:SelectionList
                              ItemsSource="{Binding CustomIndicatorItems}"
                              SelectedItem="{Binding CustomIndicatorSelectedItem}">
                              <u:SelectionList.Indicator>
                                  <Border Background="Transparent" CornerRadius="4">
                                      <Border Width="4"
                                              Margin="0,8"
                                              HorizontalAlignment="Left"
                                              VerticalAlignment="Stretch"
                                              Background="{DynamicResource SemiBlue6}"
                                              CornerRadius="4" />
                                  </Border>
                              </u:SelectionList.Indicator>
                              <u:SelectionList.ItemTemplate>
                                  <DataTemplate>
                                      <Panel Height="40">
                                          <TextBlock Margin="8,0"
                                                     VerticalAlignment="Center"
                                                     Classes.Active="{Binding $parent[u:SelectionListItem].IsSelected, Mode=OneWay}"
                                                     Text="{Binding}" />
                                      </Panel>
                                  </DataTemplate>
                              </u:SelectionList.ItemTemplate>
                          </u:SelectionList>
                          """
        });
        CustomIndicatorSection.CodeSnippets.Add(new DemoSectionCodeSnippetViewModel
        {
            CodeSnippetLanguage = CodeLanguage.CSharp,
            TabName = LanguageManager.Instance.DemoSection_Tab_ViewModel,
            CodeSnippet = """
                          public ObservableCollection<string> CustomIndicatorItems { get; }

                          [ObservableProperty]
                          public partial string? CustomIndicatorSelectedItem { get; set; }
                          """
        });

        FlyoutSection = new DemoSectionViewModel
        {
            Header = LanguageManager.Instance.Page_SelectionList_Section_Flyout_Usage_Header,
            SectionTag = DemoSectionTag.Others,
            Descriptions = { LanguageManager.Instance.Page_SelectionList_Section_Flyout_Usage_Description },
            AnchorId = FlyoutAnchorId
        };
        FlyoutSection.CodeSnippets.Add(new DemoSectionCodeSnippetViewModel
        {
            CodeSnippetLanguage = CodeLanguage.Axaml,
            TabName = LanguageManager.Instance.DemoSection_Tab_Xaml,
            CodeSnippet = """
                          <SplitButton Content="Button With Selection">
                              <SplitButton.Flyout>
                                  <Flyout>
                                      <u:SelectionList
                                          ItemsSource="{Binding FlyoutItems}"
                                          SelectedItem="{Binding FlyoutSelectedItem}" />
                                  </Flyout>
                              </SplitButton.Flyout>
                          </SplitButton>
                          """
        });
        FlyoutSection.CodeSnippets.Add(new DemoSectionCodeSnippetViewModel
        {
            CodeSnippetLanguage = CodeLanguage.CSharp,
            TabName = LanguageManager.Instance.DemoSection_Tab_ViewModel,
            CodeSnippet = """
                          public ObservableCollection<string> FlyoutItems { get; }

                          [ObservableProperty]
                          public partial string? FlyoutSelectedItem { get; set; }
                          """
        });

        ClearableSection = new DemoSectionViewModel
        {
            Header = LanguageManager.Instance.Page_SelectionList_Section_Clear_Selection_Header,
            SectionTag = DemoSectionTag.Function,
            Descriptions = { LanguageManager.Instance.Page_SelectionList_Section_Clear_Selection_Description },
            AnchorId = ClearableAnchorId
        };
        ClearableSection.CodeSnippets.Add(new DemoSectionCodeSnippetViewModel
        {
            CodeSnippetLanguage = CodeLanguage.Axaml,
            TabName = LanguageManager.Instance.DemoSection_Tab_Xaml,
            CodeSnippet = """
                          <u:SelectionList
                              ItemsSource="{Binding ClearableItems}"
                              SelectedItem="{Binding ClearableSelectedItem}" />
                          """
        });
        ClearableSection.CodeSnippets.Add(new DemoSectionCodeSnippetViewModel
        {
            CodeSnippetLanguage = CodeLanguage.CSharp,
            TabName = LanguageManager.Instance.DemoSection_Tab_ViewModel,
            CodeSnippet = """
                          public ObservableCollection<string> ClearableItems { get; }

                          [ObservableProperty]
                          public partial string? ClearableSelectedItem { get; set; }
                          """
        });

    }

    public AvaloniaList<string> BasicItems { get; }
    public AvaloniaList<string> CustomIndicatorItems { get; }
    public AvaloniaList<string> FlyoutItems { get; }
    public AvaloniaList<string> ClearableItems { get; }

    public DemoSectionViewModel BasicUsageSection { get; }
    public DemoSectionViewModel CustomIndicatorSection { get; }
    public DemoSectionViewModel FlyoutSection { get; }
    public DemoSectionViewModel ClearableSection { get; }
    public AvaloniaList<AnchorScrollViewerItemViewModel> AnchorItems { get; } =
    [
        new() { Header = LanguageManager.Instance.Page_SelectionList_Section_Basic_Usage_Header, AnchorId = BasicUsageAnchorId },
        new() { Header = LanguageManager.Instance.Page_SelectionList_Section_Custom_Indicator_Header, AnchorId = CustomIndicatorAnchorId },
        new() { Header = LanguageManager.Instance.Page_SelectionList_Section_Flyout_Usage_Header, AnchorId = FlyoutAnchorId },
        new() { Header = LanguageManager.Instance.Page_SelectionList_Section_Clear_Selection_Header, AnchorId = ClearableAnchorId },
    ];

    [ObservableProperty] public partial string? BasicSelectedItem { get; set; }
    [ObservableProperty] public partial string? CustomIndicatorSelectedItem { get; set; }
    [ObservableProperty] public partial string? FlyoutSelectedItem { get; set; }
    [ObservableProperty] public partial string? ClearableSelectedItem { get; set; }

    public void Clear()
    {
        ClearableSelectedItem = null;
    }
}
