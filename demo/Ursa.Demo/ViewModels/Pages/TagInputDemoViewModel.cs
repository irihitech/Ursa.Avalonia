using System.Collections.ObjectModel;

using Ursa.Demo.ViewModels.Controls;
using Ursa.Demo.Localizations;

namespace Ursa.Demo.ViewModels;

public class TagInputDemoViewModel: ViewModelBase, IPageMetadataProvider
{
    public PageMetadataViewModel PageMetadata { get; set; } = new PageMetadataViewModel()
    {
        Title = LanguageManager.Instance.Page_Title_TagInput,
        Description = LanguageManager.Instance.Page_Description_TagInput,
        Breadcrumbs = [new BreadcrumbItemData(LanguageManager.Instance.Menu_Category_ButtonsAndInputs), new BreadcrumbItemData(LanguageManager.Instance.Menu_Header_TagInput)],
        Tags = ["TagInput", "Input", "Tag"],
        DemoViewUrl = "https://github.com/irihitech/Ursa.Avalonia/blob/main/demo/Ursa.Demo/Pages/TagInputDemo.axaml",
        DemoViewModelUrl = "https://github.com/irihitech/Ursa.Avalonia/blob/main/demo/Ursa.Demo/ViewModels/Pages/TagInputDemoViewModel.cs",
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