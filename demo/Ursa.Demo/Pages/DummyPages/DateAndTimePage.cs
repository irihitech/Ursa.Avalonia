using Irihi.Dogma.Docs;

namespace Ursa.Demo.Pages.DummyPages;

[DocCategory(Category_Key, IsClickable = false, Order = 5)]
[DocPage(Menu_Header)]
public class DateAndTimePage
{
    public const string Category_Key = "DateTime";
    public const string Menu_Header = "Menu_Category_DateAndTime";
}
