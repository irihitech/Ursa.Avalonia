using System.Collections.ObjectModel;
using System.Net;

namespace Sandbox.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    public ObservableCollection<DataGridItem> Items { get; set; }

    public MainWindowViewModel()
    {
        Items = new ObservableCollection<DataGridItem>()
        {
            new DataGridItem() { Name = "John Doe", Age = 42 },
            new DataGridItem() { Name = "Jane Doe", Age = 39 },
            new DataGridItem() { Name = "Sammy Doe", Age = 13 },
            new DataGridItem() { Name = "Barry Doe", Age = 7 },
            new DataGridItem() { Name = "Molly Doe", Age = 5 },
        };
    }
    
    
}

public class DataGridItem
{
    public string? Name { get; set; }
    public int Age { get; set; }
    public IPAddress? Address { get; set; }
}