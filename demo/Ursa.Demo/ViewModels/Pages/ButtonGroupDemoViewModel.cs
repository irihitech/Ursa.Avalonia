using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using Ursa.Controls;
using Ursa.Demo.ViewModels.Controls;

using Ursa.Demo.Localizations;

namespace Ursa.Demo.ViewModels;

public class ButtonGroupDemoViewModel: ViewModelBase, IPageMetadataProvider
{
    public PageMetadataViewModel  PageMetadata { get; set; } = new PageMetadataViewModel()
    {
        Title = LanguageManager.Instance.Page_Title_ButtonGroup,
        Description = LanguageManager.Instance.Page_Description_ButtonGroup,
        Breadcrumbs = [new BreadcrumbItemData(LanguageManager.Instance.Menu_Category_ButtonsAndInputs), new BreadcrumbItemData(LanguageManager.Instance.Menu_Header_ButtonGroup)],
        Tags = ["ButtonGroup",  "Button", "Command", "Collection" ],
        DemoViewModelUrl = "https://github.com/irihitech/Ursa.Avalonia/blob/main/demo/Ursa.Demo/ViewModels/Pages/ButtonGroupDemoViewModel.cs",
        InlineXamlSupport = true,
        AvaloniaExclusive = false,
        MvvmSupport = true,
    };
    public ObservableCollection<ButtonItem> Items { get; set; } = new ()
    {
        new ButtonItem(){Name = "Ding" },
        new ButtonItem(){Name = "Otter" },
        new ButtonItem(){Name = "Husky" },
        new ButtonItem(){Name = "Mr. 17" },
        new ButtonItem(){Name = "Cass" },
    };
}

public class ButtonItem
{
    public string? Name { get; set; }
    public ICommand InvokeCommand { get; set; }

    public ButtonItem()
    {
        InvokeCommand = new AsyncRelayCommand(Invoke);
    }

    private async Task Invoke()
    {
        await OverlayMessageBox.ShowAsync("Hello " + Name);
    }
}
