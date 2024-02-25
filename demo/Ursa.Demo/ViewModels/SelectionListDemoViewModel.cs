using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Ursa.Demo.ViewModels;

public partial class SelectionListDemoViewModel: ObservableObject
{
    public ObservableCollection<string> Items { get; set; }
    [ObservableProperty] private string? _selectedItem;

    public SelectionListDemoViewModel()
    {
        Items = new ObservableCollection<string>()
        {
            "Ding", "Otter", "Husky", "Mr. 17", "Cass"
        };
    }

    public void Clear()
    {
        SelectedItem = null;
    }
}