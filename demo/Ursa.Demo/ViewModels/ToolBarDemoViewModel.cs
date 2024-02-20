using System.Collections.ObjectModel;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Ursa.Controls;

namespace Ursa.Demo.ViewModels;

public partial class ToolBarDemoViewModel: ObservableObject
{
    public ObservableCollection<ToolBarItemViewModel> Items { get; set; }
    public ToolBarDemoViewModel()
    {
        Items = new()
        {
            new ToolBarButtonItemViewModel() { Content = "New" },
            new ToolBarButtonItemViewModel() { Content = "Open" },
            new ToolBarButtonItemViewModel() { Content = "Save" },
            new ToolBarCheckBoxItemViweModel() { Content = "Bold" },
            new ToolBarCheckBoxItemViweModel() { Content = "Italic" },
            new ToolBarComboBoxItemViewModel() { Content = "Font Size", Items = new (){ "10", "12", "14"  } }
        };
    }
}

public abstract class ToolBarItemViewModel: ObservableObject
{
    
}

public class ToolBarButtonItemViewModel: ToolBarItemViewModel
{
    public string Content { get; set; }
    public ICommand Command { get; set; }

    public ToolBarButtonItemViewModel()
    {
        Command = new AsyncRelayCommand(async () => { await MessageBox.ShowOverlayAsync(Content); });
    }
}

public class ToolBarCheckBoxItemViweModel: ToolBarItemViewModel
{
    public string Content { get; set; }
    public bool IsChecked { get; set; }
    public ICommand Command { get; set; }

    public ToolBarCheckBoxItemViweModel()
    {
        Command = new AsyncRelayCommand(async () => { await MessageBox.ShowOverlayAsync(Content); });
    }
}

public class ToolBarComboBoxItemViewModel: ToolBarItemViewModel
{
    public string Content { get; set; }
    public ObservableCollection<string> Items { get; set; }

    private string _selectedItem;
    public string SelectedItem
    {
        get => _selectedItem;
        set
        {
            SetProperty(ref _selectedItem, value);
            MessageBox.ShowOverlayAsync(value);
        }
    }
    
    
    
}