using System.Collections.Generic;
using CommunityToolkit.Mvvm.ComponentModel;

using Ursa.Demo.ViewModels.Controls;
namespace Ursa.Demo.ViewModels;

public partial class TreeComboBoxDemoViewModel: ObservableObject
{
    public PageMetadataViewModel PageMetadata { get; set; } = new PageMetadataViewModel()
    {
        Title = "TreeComboBox",
        Description = "TreeComboBox is a combo box that displays items in a hierarchical tree structure.",
        Breadcrumbs = ["Buttons & Inputs", "TreeComboBox"],
        Tags = ["TreeComboBox", "ComboBox", "Tree"],
        DemoViewUrl = "https://github.com/irihitech/Ursa.Avalonia/blob/main/demo/Ursa.Demo/Pages/TreeComboBoxDemo.axaml",
        DemoViewModelUrl = "https://github.com/irihitech/Ursa.Avalonia/blob/main/demo/Ursa.Demo/ViewModels/Pages/TreeComboBoxDemoViewModel.cs",
        InlineXamlSupport = true,
        MvvmSupport = true,
    };

    [ObservableProperty] private TreeComboBoxItemViewModel? _selectedItem;
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
    [ObservableProperty] private string? _itemName;
    [ObservableProperty] private bool _isSelectable = true;
    public List<TreeComboBoxItemViewModel> Children { get; set; } = new ();
}