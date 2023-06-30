using System.Collections.ObjectModel;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;

namespace Ursa.Demo.ViewModels;

public class MenuItemViewModel: ViewModelBase
{
    public string MenuHeader { get; set; }
    public string MenuIconName { get; set; }
    public string Key { get; set; }
    
    public bool IsSeparator { get; set; }
    public ObservableCollection<MenuItemViewModel> Children { get; set; } = new();
    
    public ICommand ActivateCommand { get; set; }

    public MenuItemViewModel()
    {
        ActivateCommand = new RelayCommand(OnActivate);
    }

    private void OnActivate()
    {
        if (IsSeparator) return;
        WeakReferenceMessenger.Default.Send<string>(Key);
    }
}