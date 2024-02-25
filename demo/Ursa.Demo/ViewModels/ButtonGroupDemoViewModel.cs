using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using Ursa.Controls;

namespace Ursa.Demo.ViewModels;

public class ButtonGroupDemoViewModel: ViewModelBase
{
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
        await MessageBox.ShowAsync("Hello " + Name);
    }
}