using Irihi.Dogma.Docs;

namespace Ursa.Demo.Pages.DummyPages;

[DocCategory(Category_Key, IsClickable = false, Order = 6)]
[DocPage(Menu_Header)]
public class NavigationAndMenusPage
{
    public const string Category_Key = "Navigation";
    public const string Menu_Header = "Menu_Category_NavigationAndMenus";
}
