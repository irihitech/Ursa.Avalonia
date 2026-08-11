using Irihi.Dogma.Docs;

namespace Ursa.Demo.Pages.DummyPages;

[DocCategory(Category_Key, IsClickable = false, Order = 3)]
[DocPage(Menu_Header)]
public class ButtonsAndInputsPage
{
    public const string Category_Key = "Input";
    public const string Menu_Header = "Menu_Category_ButtonsAndInputs";
}
