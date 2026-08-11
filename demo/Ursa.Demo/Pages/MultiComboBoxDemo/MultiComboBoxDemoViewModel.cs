using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using Ursa.Demo.ViewModels.Controls;
using Ursa.Demo.Localizations;
using Irihi.Dogma.Docs;
using Ursa.Demo.Pages.DummyPages;

namespace Ursa.Demo.Pages.MultiComboBoxDemo;

[DocCategory(Category_Key, IsClickable = false, Parent = ButtonsAndInputsPage.Category_Key)]
[DocPage(Menu_Header, View = typeof(MultiComboBoxDemo))]
public class MultiComboBoxDemoViewModel: ObservableObject, IPageMetadataProvider
{
    public const string Category_Key = "MultiComboBox";
    public const string Menu_Header = "Menu_Header_MultiComboBox";
    public PageMetadataViewModel PageMetadata { get; set; } = new PageMetadataViewModel()
    {
        Title = LanguageManager.Instance.Page_Title_MultiComboBox,
        Description = LanguageManager.Instance.Page_Description_MultiComboBox,
        Breadcrumbs = [new BreadcrumbItemData(LanguageManager.Instance.Menu_Category_ButtonsAndInputs), new BreadcrumbItemData(LanguageManager.Instance.Menu_Header_MultiComboBox)],
        Tags = ["MultiComboBox", "ComboBox", "Selection"],
        DemoViewUrl = "https://github.com/irihitech/Ursa.Avalonia/blob/main/demo/Ursa.Demo/Pages/MultiComboBoxDemo/MultiComboBoxDemo.axaml",
        DemoViewModelUrl = "https://github.com/irihitech/Ursa.Avalonia/blob/main/demo/Ursa.Demo/Pages/MultiComboBoxDemo/MultiComboBoxDemoViewModel.cs",
        InlineXamlSupport = false,
        MvvmSupport = true,
    };

    public ObservableCollection<string> Items { get; set; }
    
    public ObservableCollection<string> SelectedItems { get; set; }

    public ICommand SelectAllCommand => new RelayCommand(() =>
    {
        SelectedItems.Clear();
        foreach (var item in Items)
        {
            SelectedItems.Add(item);
        }
    });
    
    public ICommand ClearAllCommand => new RelayCommand(() =>
    {
        SelectedItems.Clear();
    });
    
    public ICommand InvertSelectionCommand => new RelayCommand(() =>
    {
        var selectedItems = new List<string>(SelectedItems);
        SelectedItems.Clear();
        foreach (var item in Items)
        {
            if (!selectedItems.Contains(item))
            {
                SelectedItems.Add(item);
            }
        }
    });
    
    public MultiComboBoxDemoViewModel()
    {
        Items = new ObservableCollection<string>()
        {
            "Item 1",
            "Item 2",
            "Item 3",
            "Item 4",
            "Item 5",
            "Item 6",
            "Item 7",
            "Item 8",
            "Illinois",
            "Indiana",
            "Iowa",
            "Kansas",
            "Kentucky",
            "Louisiana",
            "Maine",
            "Maryland",
            "Massachusetts",
            "Michigan",
            "Minnesota",
            "Mississippi",
            "Missouri",
            "Montana",
            "Nebraska",
            "Nevada",
            "New Hampshire",
            "New Jersey",
            "New Mexico",
            "New York",
            "North Carolina",
            "North Dakota",
            "Ohio",
            "Oklahoma",
            "Oregon",
            "Pennsylvania",
            "Rhode Island",
        };
        SelectedItems = new ObservableCollection<string>();
    }
}
