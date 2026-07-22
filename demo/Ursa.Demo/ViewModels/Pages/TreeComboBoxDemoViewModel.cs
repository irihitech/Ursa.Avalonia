using System.Collections.Generic;
using CommunityToolkit.Mvvm.ComponentModel;

using Ursa.Demo.ViewModels.Controls;
using Ursa.Demo.Localizations;

namespace Ursa.Demo.ViewModels;

public partial class TreeComboBoxDemoViewModel: ObservableObject, IPageMetadataProvider
{
    public PageMetadataViewModel PageMetadata { get; set; } = new PageMetadataViewModel()
    {
        Title = LanguageManager.Instance.Page_Title_TreeComboBox,
        Description = LanguageManager.Instance.Page_Description_TreeComboBox,
        Breadcrumbs = [new BreadcrumbItemData(LanguageManager.Instance.Menu_Category_ButtonsAndInputs), new BreadcrumbItemData(LanguageManager.Instance.Menu_Header_TreeComboBox)],
        Tags = ["TreeComboBox", "ComboBox", "Tree"],
        DemoViewUrl = "https://github.com/irihitech/Ursa.Avalonia/blob/main/demo/Ursa.Demo/Pages/TreeComboBoxDemo.axaml",
        DemoViewModelUrl = "https://github.com/irihitech/Ursa.Avalonia/blob/main/demo/Ursa.Demo/ViewModels/Pages/TreeComboBoxDemoViewModel.cs",
        InlineXamlSupport = true,
        MvvmSupport = true,
    };

    [ObservableProperty] public partial TreeComboBoxItemViewModel? SelectedItem { get; set; }
    public List<TreeComboBoxItemViewModel> Items { get; set; }

    public TreeComboBoxDemoViewModel()
    {
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