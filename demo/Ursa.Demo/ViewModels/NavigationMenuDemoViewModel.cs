using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Ursa.Demo.ViewModels;

public class NavigationMenuDemoViewModel: ObservableObject
{
    public ObservableCollection<NavigationMenuItemViewModel> MenuItems { get; set; } = new()
    {
        new NavigationMenuItemViewModel()
        {
            MenuHeader = "1",
            Children = new ObservableCollection<NavigationMenuItemViewModel>()
            {
                new NavigationMenuItemViewModel(){
                    MenuHeader = "11" , 
                    Children = new ObservableCollection<NavigationMenuItemViewModel>()
                    {
                        new NavigationMenuItemViewModel(){MenuHeader = "111"},
                        new NavigationMenuItemViewModel(){MenuHeader = "112"}
                    }},
                new NavigationMenuItemViewModel(){MenuHeader = "12"}
            }
        },
        new NavigationMenuItemViewModel()
        {
            MenuHeader = "2",
            Children = new ObservableCollection<NavigationMenuItemViewModel>()
            {
                new NavigationMenuItemViewModel(){MenuHeader = "21"},
                new NavigationMenuItemViewModel(){MenuHeader = "22"}
            }
        }
    };
}

public class NavigationMenuItemViewModel: ObservableObject
{
    public string MenuHeader { get; set; }
    public ObservableCollection<NavigationMenuItemViewModel> Children { get; set; } = new();
}