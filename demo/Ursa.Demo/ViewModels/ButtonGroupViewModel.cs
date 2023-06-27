using System.Collections.ObjectModel;

namespace Ursa.Demo.ViewModels;

public class ButtonGroupViewModel: ViewModelBase
{
    public ObservableCollection<string> Items { get; set; } = new ObservableCollection<string>()
    {
        "Ding", "Otter", "Husky", "Mr. 17", "Cass"
    };
}