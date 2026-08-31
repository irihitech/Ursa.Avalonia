using System.Collections.Generic;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using Irihi.Dogma.Controls;

using Ursa.Demo.ViewModels.Controls;
using Ursa.Demo.Localizations;
using Irihi.Dogma.Docs;
using Ursa.Demo.Pages.DummyPages;

namespace Ursa.Demo.Pages.TreeComboBoxDemo;

[DocCategory(Category_Key, IsClickable = false, Parent = ButtonsAndInputsPage.Category_Key)]
[DocPage(Menu_Header, View = typeof(TreeComboBoxDemo))]
public partial class TreeComboBoxDemoViewModel: ObservableObject, IPageMetadataProvider
{
    public const string Category_Key = "TreeComboBox";
    public const string Menu_Header = "Menu_Header_TreeComboBox";
    private const string InlineItemsAnchorId = "tree-combo-box-inline-items";
    private const string DataBindingAnchorId = "tree-combo-box-data-binding";
    private const string StyleAndPopupCustomizationAnchorId = "tree-combo-box-style-and-popup-customization";
    public PageMetadataViewModel PageMetadata { get; set; } = new PageMetadataViewModel()
    {
        Title = LanguageManager.Instance.Page_Title_TreeComboBox,
        Description = LanguageManager.Instance.Page_Description_TreeComboBox,
        Breadcrumbs = [new BreadcrumbItemData(LanguageManager.Instance.Menu_Category_ButtonsAndInputs), new BreadcrumbItemData(LanguageManager.Instance.Menu_Header_TreeComboBox)],
        Tags = ["TreeComboBox", "ComboBox", "Tree"],
        DemoViewUrl = "https://github.com/irihitech/Ursa.Avalonia/blob/main/demo/Ursa.Demo/Pages/TreeComboBoxDemo/TreeComboBoxDemo.axaml",
        DemoViewModelUrl = "https://github.com/irihitech/Ursa.Avalonia/blob/main/demo/Ursa.Demo/Pages/TreeComboBoxDemo/TreeComboBoxDemoViewModel.cs",
        InlineXamlSupport = true,
        MvvmSupport = true,
    };

    [ObservableProperty] public partial TreeComboBoxItemViewModel? SelectedItem { get; set; }
    public List<TreeComboBoxItemViewModel> Items { get; set; }
    public DemoSectionViewModel InlineItemsSection { get; }
    public DemoSectionViewModel DataBindingSection { get; }
    public DemoSectionViewModel StyleAndPopupCustomizationSection { get; }
    public ObservableCollection<AnchorScrollViewerItemViewModel> AnchorItems { get; } =
    [
        new()
        {
            Header = LanguageManager.Instance.Page_TreeComboBox_Section_Inline_Items_Header,
            AnchorId = InlineItemsAnchorId
        },
        new()
        {
            Header = LanguageManager.Instance.Page_TreeComboBox_Section_Data_Binding_Header,
            AnchorId = DataBindingAnchorId
        },
        new()
        {
            Header = LanguageManager.Instance.Page_TreeComboBox_Section_Style_And_Popup_Customization_Header,
            AnchorId = StyleAndPopupCustomizationAnchorId
        }
    ];

    public TreeComboBoxDemoViewModel()
    {
        InlineItemsSection = new DemoSectionViewModel
        {
            Header = LanguageManager.Instance.Page_TreeComboBox_Section_Inline_Items_Header,
            Descriptions = { LanguageManager.Instance.Page_TreeComboBox_Section_Inline_Items_Description },
            SectionTag = DemoSectionTag.Function,
            AnchorId = InlineItemsAnchorId
        };
        InlineItemsSection.CodeSnippets.Add(new DemoSectionCodeSnippetViewModel
        {
            CodeSnippetLanguage = CodeLanguage.Axaml,
            TabName = LanguageManager.Instance.DemoSection_Tab_Xaml,
            CodeSnippet = """
                          <u:TreeComboBox Width="300">
                              <u:TreeComboBoxItem Header="Hello">
                                  <u:TreeComboBoxItem Header="Hello World" />
                              </u:TreeComboBoxItem>
                              <u:TreeComboBoxItem Header="World" />
                          </u:TreeComboBox>
                          """
        });

        DataBindingSection = new DemoSectionViewModel
        {
            Header = LanguageManager.Instance.Page_TreeComboBox_Section_Data_Binding_Header,
            Descriptions = { LanguageManager.Instance.Page_TreeComboBox_Section_Data_Binding_Description },
            SectionTag = DemoSectionTag.Function,
            AnchorId = DataBindingAnchorId
        };
        DataBindingSection.CodeSnippets.Add(new DemoSectionCodeSnippetViewModel
        {
            CodeSnippetLanguage = CodeLanguage.Axaml,
            TabName = LanguageManager.Instance.DemoSection_Tab_Xaml,
            CodeSnippet = """
                          <u:TreeComboBox
                              Width="300"
                              SelectedItem="{Binding SelectedItem}"
                              ItemsSource="{Binding Items}">
                              <u:TreeComboBox.ItemTemplate>
                                  <TreeDataTemplate ItemsSource="{Binding Children}">
                                      <TextBlock Text="{Binding ItemName}" />
                                  </TreeDataTemplate>
                              </u:TreeComboBox.ItemTemplate>
                          </u:TreeComboBox>
                          """
        });
        DataBindingSection.CodeSnippets.Add(new DemoSectionCodeSnippetViewModel
        {
            CodeSnippetLanguage = CodeLanguage.CSharp,
            TabName = LanguageManager.Instance.DemoSection_Tab_ViewModel,
            CodeSnippet = """
                          [ObservableProperty] public partial TreeComboBoxItemViewModel? SelectedItem { get; set; }
                          public List<TreeComboBoxItemViewModel> Items { get; set; }
                          """
        });

        StyleAndPopupCustomizationSection = new DemoSectionViewModel
        {
            Header = LanguageManager.Instance.Page_TreeComboBox_Section_Style_And_Popup_Customization_Header,
            Descriptions = { LanguageManager.Instance.Page_TreeComboBox_Section_Style_And_Popup_Customization_Description },
            SectionTag = DemoSectionTag.Style,
            AnchorId = StyleAndPopupCustomizationAnchorId
        };
        StyleAndPopupCustomizationSection.CodeSnippets.Add(new DemoSectionCodeSnippetViewModel
        {
            CodeSnippetLanguage = CodeLanguage.Axaml,
            TabName = LanguageManager.Instance.DemoSection_Tab_Xaml,
            CodeSnippet = """
                          <u:TreeComboBox
                              Classes="ClearButton"
                              InnerLeftContent="Left"
                              InnerRightContent="Right"
                              PopupInnerTopContent="Top"
                              PopupInnerBottomContent="Bottom"
                              ItemsSource="{Binding Items}" />
                          """
        });

        Items = new List<TreeComboBoxItemViewModel>()
        {
            new TreeComboBoxItemViewModel()
            {
                ItemName = "Item 1",
                Children = new List<TreeComboBoxItemViewModel>()
                {
                    new TreeComboBoxItemViewModel()
                    {
                        ItemName = "Item 1-1 (Not selectable)",
                        IsSelectable = false,
                        Children = new List<TreeComboBoxItemViewModel>()
                        {
                            new TreeComboBoxItemViewModel()
                            {
                                ItemName = "Item 1-1-1"
                            },
                            new TreeComboBoxItemViewModel()
                            {
                                ItemName = "Item 1-1-2"
                            }
                        }
                    },
                    new TreeComboBoxItemViewModel()
                    {
                        ItemName = "Item 1-2"
                    }
                }
            },
            new TreeComboBoxItemViewModel()
            {
                ItemName = "Item 2",
                Children = new List<TreeComboBoxItemViewModel>()
                {
                    new TreeComboBoxItemViewModel()
                    {
                        ItemName = "Item 2-1  (Not selectable)",
                        IsSelectable = false,
                    },
                    new TreeComboBoxItemViewModel()
                    {
                        ItemName = "Item 2-2"
                    }
                }
            },
            new TreeComboBoxItemViewModel()
            {
                ItemName = "Item 3"
            },
        };
    }
}

public partial class TreeComboBoxItemViewModel : ObservableObject
{
    [ObservableProperty] public partial string? ItemName { get; set; }
    [ObservableProperty] public partial bool IsSelectable { get; set; } = true;
    public List<TreeComboBoxItemViewModel> Children { get; set; } = new ();
}