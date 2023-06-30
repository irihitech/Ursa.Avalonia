using System.Collections.ObjectModel;

namespace Ursa.Demo.ViewModels;

public class ButtonGroupDemoViewModel: ViewModelBase
{
    public ObservableCollection<string> Items { get; set; } = new ()
    {
        "Ding", "Otter", "Husky", "Mr. 17", "Cass"
    };
}