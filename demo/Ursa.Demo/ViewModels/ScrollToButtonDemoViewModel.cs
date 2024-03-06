using System.Collections.ObjectModel;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Ursa.Demo.ViewModels;

public class ScrollToButtonDemoViewModel: ObservableObject
{
    public ObservableCollection<string> Items { get; set; }

    public ScrollToButtonDemoViewModel()
    {
        Items = new ObservableCollection<string>(Enumerable.Range(0, 1000).Select(a => "Item " + a));
    }
}