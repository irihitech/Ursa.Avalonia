using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using Ursa.Controls;
using Ursa.Demo.ViewModels.Controls;

namespace Ursa.Demo.ViewModels;

public class ButtonGroupDemoViewModel: ViewModelBase
{
    public PageMetadataViewModel  PageMetadata { get; set; } = new PageMetadataViewModel()
    {
        Title = "ButtonGroup",
        Description = "A ButtonGroup is a control that groups multiple buttons together. ",
        Breadcrumbs = ["Input", "Button Group"],
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
