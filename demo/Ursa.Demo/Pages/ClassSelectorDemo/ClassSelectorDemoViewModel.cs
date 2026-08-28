using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using Irihi.Dogma.Controls;
using Irihi.Dogma.Docs;
using Ursa.Demo.Localizations;
using Ursa.Demo.Pages.DummyPages;
using Ursa.Demo.ViewModels.Controls;

namespace Ursa.Demo.Pages.ClassSelectorDemo;

[DocCategory(CategoryKey, IsClickable = false, Parent = DevUtilitiesPage.Category_Key)]
[DocPage(MenuHeader, View = typeof(ClassSelectorDemo))]
public class ClassSelectorDemoViewModel : ObservableObject, IPageMetadataProvider
{
    public const string CategoryKey = "ClassSelector";
    public const string MenuHeader = "Menu_Header_ClassSelector";
    private const string GroupedSelectionAnchorId = "class-selector-grouped-selection";

    public ClassSelectorDemoViewModel()
    {
        GroupedSelectionSection = new DemoSectionViewModel
        {
            Header = LanguageManager.Instance.Page_ClassSelector_Section_Grouped_Selection_Header,
            Descriptions = { LanguageManager.Instance.Page_ClassSelector_Section_Grouped_Selection_Description },
            SectionTag = DemoSectionTag.Function,
            AnchorId = GroupedSelectionAnchorId
        };
        GroupedSelectionSection.CodeSnippets.Add(new DemoSectionCodeSnippetViewModel
        {
            CodeSnippetLanguage = CodeLanguage.Axaml,
            TabName = LanguageManager.Instance.DemoSection_Tab_Xaml,
            CodeSnippet = """
                          <Button
                              x:Name="target"
                              HorizontalAlignment="Left"
                              Content="Target control" />
                          <u:ClassSelector
                              x:Name="classSelector"
                              Width="300"
                              PlaceholderText="Select style classes"
                              SelectedClasses="{Binding SelectedClasses}"
                              Target="{Binding #target}">
                              <u:ClassSelectorGroup Header="Color">
                                  <u:ClassSelectorItem ClassName="Primary" />
                                  <u:ClassSelectorItem ClassName="Secondary" />
                                  <u:ClassSelectorItem ClassName="Tertiary" />
                                  <u:ClassSelectorItem ClassName="Success" />
                                  <u:ClassSelectorItem ClassName="Warning" />
                                  <u:ClassSelectorItem ClassName="Danger" />
                              </u:ClassSelectorGroup>
                              <u:ClassSelectorGroup Header="Size">
                                  <u:ClassSelectorItem ClassName="Small" />
                                  <u:ClassSelectorItem ClassName="Large" />
                              </u:ClassSelectorGroup>
                          </u:ClassSelector>
                          """
        });
    }

    public PageMetadataViewModel PageMetadata { get; set; } = new()
    {
        Title = LanguageManager.Instance.Page_Title_ClassSelector,
        Description = LanguageManager.Instance.Page_Description_ClassSelector,
        Breadcrumbs =
        [
            new BreadcrumbItemData(LanguageManager.Instance.Menu_Category_DevUtilities),
            new BreadcrumbItemData(LanguageManager.Instance.Menu_Header_ClassSelector)
        ],
        Tags = ["ClassSelector", "Style", "Classes", "Selection"],
        DemoViewUrl = "https://github.com/irihitech/Ursa.Avalonia/blob/main/demo/Ursa.Demo/Pages/ClassSelectorDemo/ClassSelectorDemo.axaml",
        DemoViewModelUrl = "https://github.com/irihitech/Ursa.Avalonia/blob/main/demo/Ursa.Demo/Pages/ClassSelectorDemo/ClassSelectorDemoViewModel.cs",
        InlineXamlSupport = true,
        MvvmSupport = true
    };

    public ObservableCollection<string> SelectedClasses { get; } = [];

    public DemoSectionViewModel GroupedSelectionSection { get; }

    public ObservableCollection<AnchorScrollViewerItemViewModel> AnchorItems { get; } =
    [
        new()
        {
            Header = LanguageManager.Instance.Page_ClassSelector_Section_Grouped_Selection_Header,
            AnchorId = GroupedSelectionAnchorId
        }
    ];
}
