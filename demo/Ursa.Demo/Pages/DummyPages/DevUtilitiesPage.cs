using Irihi.Dogma.Docs;

namespace Ursa.Demo.Pages.DummyPages;

[DocCategory(Category_Key, IsClickable = false, Order = 8)]
[DocPage(Menu_Header)]
public class DevUtilitiesPage
{
    public const string Category_Key = "DevUtilities";
    public const string Menu_Header = "Menu_Category_DevUtilities";
}
