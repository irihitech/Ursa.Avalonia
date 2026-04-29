using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using Avalonia.Platform.Storage;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using Ursa.Demo.ViewModels.Controls;
namespace Ursa.Demo.ViewModels;

public partial class AboutUsDemoViewModel : ObservableObject
{
    public PageMetadataViewModel PageMetadata { get; set; } = new PageMetadataViewModel()
    {
        Title = "About Us",
        Description = "About Us displays information about the Ursa library and its creators.",
        Breadcrumbs = ["About Us"],
        Tags = ["AboutUs"],
        DemoViewUrl = "https://github.com/irihitech/Ursa.Avalonia/blob/main/demo/Ursa.Demo/Pages/AboutUsDemo.axaml",
        DemoViewModelUrl = "https://github.com/irihitech/Ursa.Avalonia/blob/main/demo/Ursa.Demo/ViewModels/AboutUsDemoViewModel.cs",
    };

    public ICommand NavigateCommand { get; set; }

    internal ILauncher? Launcher { get; set; }

    public AboutUsDemoViewModel()
    {
        NavigateCommand = new AsyncRelayCommand<string>(OnNavigateAsync);
    }

    private static readonly IReadOnlyDictionary<string, string> _keyToUrlMapping = new Dictionary<string, string>()
    {
        ["semi"] = "https://github.com/irihitech/Semi.Avalonia",
        ["ursa"] = "https://github.com/irihitech/Ursa.Avalonia",
        ["mantra"] = "https://www.bilibili.com/video/BV15pfKYbEEQ",
        ["huska"] = "https://www.bilibili.com/video/BV1knj1zWE4A",
    };

    private async Task OnNavigateAsync(string? arg)
    {
        if (Launcher is not null && arg is not null && _keyToUrlMapping.TryGetValue(arg.ToLower(), out var uri))
        {
            await Launcher.LaunchUriAsync(new Uri(uri));
        }
    }
}