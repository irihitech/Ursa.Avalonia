using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Ursa.Demo.ViewModels;

public class NavigationMenuDemoViewModel: ObservableObject
{
    public ObservableCollection<MenuItemViewModel> MenuItems { get; set; } = new()
    {
        new MenuItemViewModel()
        {
            MenuHeader = "任务管理",
            MenuIconName = "User",
            Children = new ObservableCollection<MenuItemViewModel>()
            {
                new (){
                    MenuHeader = "公告管理" , 
                    MenuIconName = "Star",
                    Children = new ObservableCollection<MenuItemViewModel>()
                    {
                        new () {MenuHeader = "公告设置"},
                        new () {MenuHeader = "公告处理"}
                    }},
                new (){MenuHeader = "任务查询"}
            }
        },
        new MenuItemViewModel()
        {
            MenuHeader = "附加功能",
            IsSeparator = true,
        },
        new MenuItemViewModel()
        {
            MenuHeader = "任务平台",
            MenuIconName = "Gear",
            Children = new ObservableCollection<MenuItemViewModel>()
            {
                new (){MenuHeader = "任务管理"},
                new (){MenuHeader = "用户任务查询"}
            }
        }
    };
}

