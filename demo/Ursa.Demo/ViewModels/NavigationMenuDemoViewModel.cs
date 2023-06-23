using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Ursa.Demo.ViewModels;

public class NavigationMenuDemoViewModel: ObservableObject
{
    public ObservableCollection<NavigationMenuItemViewModel> MenuItems { get; set; } = new()
    {
        new NavigationMenuItemViewModel()
        {
            MenuHeader = "任务管理",
            MenuIconName = "User",
            Children = new ObservableCollection<NavigationMenuItemViewModel>()
            {
                new (){
                    MenuHeader = "公告管理" , 
                    MenuIconName = "Star",
                    Children = new ObservableCollection<NavigationMenuItemViewModel>()
                    {
                        new () {MenuHeader = "公告设置"},
                        new () {MenuHeader = "公告处理"}
                    }},
                new (){MenuHeader = "任务查询"}
            }
        },
        new NavigationMenuItemViewModel()
        {
            MenuHeader = "附加功能",
            IsSeparator = true,
        },
        new NavigationMenuItemViewModel()
        {
            MenuHeader = "任务平台",
            MenuIconName = "Gear",
            Children = new ObservableCollection<NavigationMenuItemViewModel>()
            {
                new (){MenuHeader = "任务管理"},
                new (){MenuHeader = "用户任务查询"}
            }
        }
    };
}

public class NavigationMenuItemViewModel: ObservableObject
{
    public string MenuHeader { get; set; }
    public string MenuIconName { get; set; }
    
    public bool IsSeparator { get; set; }
    public ObservableCollection<NavigationMenuItemViewModel> Children { get; set; } = new();
}