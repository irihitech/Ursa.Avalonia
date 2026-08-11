using System.Collections.ObjectModel;

using Ursa.Demo.ViewModels;
using Ursa.Demo.ViewModels.Controls;
using Ursa.Demo.Localizations;
using Irihi.Dogma.Docs;
using Ursa.Demo.Pages.DummyPages;

namespace Ursa.Demo.Pages.TagInputDemo;

[DocCategory(Category_Key, IsClickable = false, Parent = ButtonsAndInputsPage.Category_Key)]
[DocPage(Menu_Header, View = typeof(TagInputDemo))]
public class TagInputDemoViewModel: ViewModelBase, IPageMetadataProvider
{
    public const string Category_Key = "TagInput";
    public const string Menu_Header = "Menu_Header_TagInput";
    public PageMetadataViewModel PageMetadata { get; set; } = new PageMetadataViewModel()
    {
        Title = LanguageManager.Instance.Page_Title_TagInput,
        Description = LanguageManager.Instance.Page_Description_TagInput,
        Breadcrumbs = [new BreadcrumbItemData(LanguageManager.Instance.Menu_Category_ButtonsAndInputs), new BreadcrumbItemData(LanguageManager.Instance.Menu_Header_TagInput)],
        Tags = ["TagInput", "Input", "Tag"],
        DemoViewUrl = "https://github.com/irihitech/Ursa.Avalonia/blob/main/demo/Ursa.Demo/Pages/TagInputDemo/TagInputDemo.axaml",
        DemoViewModelUrl = "https://github.com/irihitech/Ursa.Avalonia/blob/main/demo/Ursa.Demo/Pages/TagInputDemo/TagInputDemoViewModel.cs",
        InlineXamlSupport = true,
        MvvmSupport = true,
    };

    private ObservableCollection<string> _tags = new () ;
    public ObservableCollection<string> Tags
    {
        get => _tags;
        set => SetProperty(ref _tags, value);
    }

    private ObservableCollection<string> _distinctTags = new();
    public ObservableCollection<string> DistinctTags
    {
        get => _distinctTags;
        set => SetProperty(ref _distinctTags, value);
    }
}