using Irihi.Dogma.Docs;

namespace Ursa.Demo.Pages.DummyPages;

[DocCategory(Category_Key, IsClickable = false, Parent = DateAndTimePage.Category_Key)]
[DocPage(Menu_Header)]
public class DatePickersPage
{
    public const string Category_Key = "DatePickers";
    public const string Menu_Header = "Menu_Category_DatePickers";
}
