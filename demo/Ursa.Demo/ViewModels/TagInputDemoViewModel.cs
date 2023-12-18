using System.Collections.ObjectModel;

namespace Ursa.Demo.ViewModels;

public class TagInputDemoViewModel: ViewModelBase
{
    private ObservableCollection<string> _Tags ;
    public ObservableCollection<string> Tags
    {
        get { return _Tags; }
        set { SetProperty(ref _Tags, value); }
    }
}