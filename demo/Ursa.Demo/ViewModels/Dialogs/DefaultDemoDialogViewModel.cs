using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Ursa.Demo.Dialogs;

public partial class DefaultDemoDialogViewModel: ObservableObject
{
    public ObservableCollection<string> Cities { get; set; }
    [ObservableProperty] public partial string? Owner { get; set; }
    [ObservableProperty] public partial string? Department { get; set; }
    [ObservableProperty] public partial string? Target { get; set; }
    [ObservableProperty] public partial string? City { get; set; }

    public DefaultDemoDialogViewModel()
    {
        Cities =
        [
            "Shanghai", "Beijing", "Hulunbuir", "Shenzhen", "Hangzhou", "Nanjing", "Chengdu", "Wuhan", "Chongqing",
            "Suzhou", "Tianjin", "Xi'an", "Qingdao", "Dalian"
        ];
    }
}