using System;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using Irihi.Avalonia.Shared.Contracts;

namespace Ursa.Demo.Dialogs;

public partial class DefaultDemoDialogViewModel: ObservableObject
{
    public ObservableCollection<string> Cities { get; set; }
    [ObservableProperty] private string? _owner;
    [ObservableProperty] private string? _department;
    [ObservableProperty] private string? _target;
    [ObservableProperty] private string? _city;

    public DefaultDemoDialogViewModel()
    {
        Cities =
        [
            "Shanghai", "Beijing", "Hulunbuir", "Shenzhen", "Hangzhou", "Nanjing", "Chengdu", "Wuhan", "Chongqing",
            "Suzhou", "Tianjin", "Xi'an", "Qingdao", "Dalian"
        ];
    }
}