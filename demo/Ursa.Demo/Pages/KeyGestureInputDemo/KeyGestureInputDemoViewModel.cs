using System.Collections.Generic;
using Avalonia.Input;
using CommunityToolkit.Mvvm.ComponentModel;

using Ursa.Demo.ViewModels.Controls;
using Ursa.Demo.Localizations;
using Irihi.Dogma.Docs;
using Ursa.Demo.Pages.DummyPages;

namespace Ursa.Demo.Pages.KeyGestureInputDemo;

[DocCategory(Category_Key, IsClickable = false, Parent = ButtonsAndInputsPage.Category_Key)]
[DocPage(Menu_Header, View = typeof(KeyGestureInputDemo))]
public class KeyGestureInputDemoViewModel: ObservableObject, IPageMetadataProvider
{
    public const string Category_Key = "KeyGestureInput";
    public const string Menu_Header = "Menu_Header_KeyGestureInput";
    public PageMetadataViewModel PageMetadata { get; set; } = new PageMetadataViewModel()
    {
        Title = LanguageManager.Instance.Page_Title_KeyGestureInput,
        Description = LanguageManager.Instance.Page_Description_KeyGestureInput,
        Breadcrumbs = [new BreadcrumbItemData(LanguageManager.Instance.Menu_Category_ButtonsAndInputs), new BreadcrumbItemData(LanguageManager.Instance.Menu_Header_KeyGestureInput)],
        Tags = ["KeyGestureInput", "Input", "HotKey"],
        DemoViewUrl = "https://github.com/irihitech/Ursa.Avalonia/blob/main/demo/Ursa.Demo/Pages/KeyGestureInputDemo/KeyGestureInputDemo.axaml",
        DemoViewModelUrl = "https://github.com/irihitech/Ursa.Avalonia/blob/main/demo/Ursa.Demo/Pages/KeyGestureInputDemo/KeyGestureInputDemoViewModel.cs",
        InlineXamlSupport = true,
    };

    public List<Key> AcceptableKeys { get; set; } = new List<Key>()
    {
        Key.A, Key.B, Key.C,
    };
}