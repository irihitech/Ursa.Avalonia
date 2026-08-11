using CommunityToolkit.Mvvm.ComponentModel;

using Ursa.Demo.ViewModels.Controls;
using Ursa.Demo.Localizations;
using Irihi.Dogma.Docs;
using Ursa.Demo.Pages.DummyPages;

namespace Ursa.Demo.Pages.ClassInputDemo;

[DocCategory(Category_Key, IsClickable = false, Parent = ButtonsAndInputsPage.Category_Key)]
[DocPage(Menu_Header, View = typeof(ClassInputDemo))]
public class ClassInputDemoViewModel: ObservableObject, IPageMetadataProvider
{
    public const string Category_Key = "Class Input";
    public const string Menu_Header = "Menu_Header_ClassInput";
    public PageMetadataViewModel PageMetadata { get; set; } = new PageMetadataViewModel()
    {
        Title = LanguageManager.Instance.Page_Title_ClassInput,
        Description = LanguageManager.Instance.Page_Description_ClassInput,
        Breadcrumbs = [new BreadcrumbItemData(LanguageManager.Instance.Menu_Category_ButtonsAndInputs), new BreadcrumbItemData(LanguageManager.Instance.Menu_Header_ClassInput)],
        Tags = ["ClassInput", "Input", "CSS"],
        DemoViewUrl = "https://github.com/irihitech/Ursa.Avalonia/blob/main/demo/Ursa.Demo/Pages/ClassInputDemo/ClassInputDemo.axaml",
        DemoViewModelUrl = "https://github.com/irihitech/Ursa.Avalonia/blob/main/demo/Ursa.Demo/Pages/ClassInputDemo/ClassInputDemoViewModel.cs",
        InlineXamlSupport = true,
    };

    
}