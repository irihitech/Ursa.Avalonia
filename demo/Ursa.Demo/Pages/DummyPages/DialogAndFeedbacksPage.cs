using Irihi.Dogma.Docs;

namespace Ursa.Demo.Pages.DummyPages;

[DocCategory(Category_Key, IsClickable = false, Order = 4)]
[DocPage(Menu_Header)]
public class DialogAndFeedbacksPage
{
    public const string Category_Key = "Feedback";
    public const string Menu_Header = "Menu_Category_DialogAndFeedbacks";
}
