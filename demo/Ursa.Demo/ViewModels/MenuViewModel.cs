using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Irihi.Dogma.Docs;
using Irihi.Lingua;
using Ursa.Demo.Models;

namespace Ursa.Demo.ViewModels;

public class MenuViewModel : ViewModelBase
{
    public MenuViewModel()
    {
        MenuItems = BuildTreeItems(UrsaDocSite.Instance.Roots);
    }
    
    private static ObservableCollection<MenuItemViewModel> BuildTreeItems(IReadOnlyList<DocCategoryNode> node)
    {
        var children = node
            .OrderBy(a => a.Metadata.Order)
            .Select(BuildTreeItem);
        return new ObservableCollection<MenuItemViewModel>(children);
    }


    private static MenuItemViewModel BuildTreeItem(DocCategoryNode node)
    {
        var item = new MenuItemViewModel(node);
        var children = BuildTreeItems(node.Children);
        item.Children = new ObservableCollection<MenuItemViewModel>(children);
        return item;
    }

    public ObservableCollection<MenuItemViewModel> MenuItems { get; set; }

    public void FilterMenuItems(string? searchText)
    {
        if (string.IsNullOrWhiteSpace(searchText))
        {
            SetAllVisible(MenuItems);
            return;
        }
        ApplyFilter(MenuItems, searchText);
    }

    private static void SetAllVisible(IEnumerable<MenuItemViewModel> items)
    {
        foreach (var item in items)
        {
            item.IsVisible = true;
            if (item.Children.Count > 0)
                SetAllVisible(item.Children);
        }
    }

    private static bool ApplyFilter(IEnumerable<MenuItemViewModel> items, string searchText)
    {
        var anyVisible = false;
        foreach (var item in items)
        {
            if (item.IsSeparator)
            {
                item.IsVisible = false;
                continue;
            }

            var headerText = (item.MenuHeader as LinguaObservableString)?.CurrentValue;
            var selfMatches = headerText?.Contains(searchText, StringComparison.OrdinalIgnoreCase) == true;
            var childrenVisible = item.Children.Count > 0 && ApplyFilter(item.Children, searchText);

            item.IsVisible = selfMatches || childrenVisible;
            if (item.IsVisible) anyVisible = true;
        }
        return anyVisible;
    }
}
