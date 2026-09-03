using Irihi.Dogma.Docs;

namespace Ursa.Demo.Pages.DummyPages;

[DocCategory(Category_Key, IsClickable = false, Parent = DateAndTimePage.Category_Key)]
[DocPage(Menu_Header)]
public class TimePickersPage
{
    public const string Category_Key = "TimePickers";
    public const string Menu_Header = "Menu_Category_TimePickers";
}
