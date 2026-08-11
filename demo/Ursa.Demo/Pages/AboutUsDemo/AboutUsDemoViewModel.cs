using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using Avalonia.Platform.Storage;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Irihi.Dogma.Docs;

namespace Ursa.Demo.Pages.AboutUsDemo;

[DocCategory(Category_Key, Order = 1)]
[DocPage(Menu_Header, View = typeof(AboutUsDemo))]
public partial class AboutUsDemoViewModel : ObservableObject
{
    public const string Category_Key = "AboutUs";
    public const string Menu_Header = "Menu_Header_AboutUs";
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
        ["lingua"] = "https://github.com/irihitech/Irihi.Lingua",
        ["mafia"] = "https://github.com/irihitech/Irihi.Mafia",
    };

    private async Task OnNavigateAsync(string? arg)
    {
        if (Launcher is not null && arg is not null && _keyToUrlMapping.TryGetValue(arg.ToLower(), out var uri))
        {
            await Launcher.LaunchUriAsync(new Uri(uri));
        }
    }
}
