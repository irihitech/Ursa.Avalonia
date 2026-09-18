using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Platform.Storage;
using Avalonia.Threading;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Irihi.Dogma.Docs;
using Ursa.Controls;
using Ursa.Demo.Models;

namespace Ursa.Demo.Pages.IntroductionDemo;

[DocCategory(Category_Key, Order = 0)]
[DocPage(Menu_Header, View = typeof(IntroductionDemo))]
public partial class IntroductionDemoViewModel : ObservableObject
{
    public const string Category_Key = "Introduction";
    public const string Menu_Header = "Menu_Header_Introduction";
    public const string LocalHost = "IntroductionLocalHost";

    private static readonly IReadOnlyDictionary<string, string> _keyToUrlMapping =
        new Dictionary<string, string>()
        {
            ["github"] = "https://github.com/irihitech/Ursa.Avalonia",
            ["docs"] = "https://ursa.irihi.tech/",
            ["nuget"] = "https://www.nuget.org/packages/Ursa.Avalonia",
        };

    internal ILauncher? Launcher { get; set; }

    public ObservableCollection<string> ButtonGroupItems { get; set; } = new()
    {
        "Docs", "Themes", "NuGet",
    };

    public ObservableCollection<string> ComboBoxItems { get; set; } = new()
    {
        "Avatar", "Banner", "Drawer", "Form", "PinCode", "QRCode", "Timeline",
    };

    public ObservableCollection<ControlData> AutoCompleteItems { get; set; } = new()
    {
        new() { MenuHeader = "Avatar", Chinese = "头像" },
        new() { MenuHeader = "Badge", Chinese = "徽标" },
        new() { MenuHeader = "Dialog", Chinese = "对话框" },
        new() { MenuHeader = "Notification", Chinese = "通知" },
    };

    public ObservableCollection<ControlData> AutoCompleteSelectedItems { get; set; } = [];

    public ObservableCollection<string> TagItems { get; set; } = new()
    {
        "avalonia", "cross-platform",
    };

    [ObservableProperty] public partial double RatingValue { get; set; } = 3.5;

    [ObservableProperty] public partial IPAddress? IpAddress { get; set; } = new IPAddress(new byte[] { 192, 168, 1, 1 });

    [ObservableProperty] public partial double LowerValue { get; set; } = 20;

    [ObservableProperty] public partial double UpperValue { get; set; } = 80;

    [ObservableProperty] public partial DateTime StartDate { get; set; } = DateTime.Today;

    [ObservableProperty] public partial DateTime EndDate { get; set; } = DateTime.Today.AddDays(7);

    [ObservableProperty] public partial DateTime CurrentDateTime { get; set; } = DateTime.Now;

    [ObservableProperty] public partial TimeSpan StartTime { get; set; } = new TimeSpan(9, 0, 0);

    [ObservableProperty] public partial TimeSpan EndTime { get; set; } = new TimeSpan(17, 0, 0);

    [ObservableProperty] public partial int MetricValue { get; set; } = 68420;

    [ObservableProperty] public partial int MetricDelta { get; set; }

    [ObservableProperty] public partial bool MetricIsUp { get; set; } = true;

    [ObservableProperty] public partial bool MetricIsDown { get; set; }

    private readonly Random _random = new();

    public IntroductionDemoViewModel()
    {
        if (Design.IsDesignMode) return;
        var metricTimer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(1500) };
        metricTimer.Tick += (_, _) =>
        {
            var delta = _random.Next(-80, 121);
            MetricValue = Math.Max(0, MetricValue + delta);
            MetricDelta = delta;
            MetricIsUp = delta >= 0;
            MetricIsDown = delta < 0;
        };
        metricTimer.Start();
    }

    [RelayCommand]
    private async Task NavigateAsync(string? key)
    {
        if (Launcher is not null && key is not null &&
            _keyToUrlMapping.TryGetValue(key.ToLower(), out var uri))
        {
            await Launcher.LaunchUriAsync(new Uri(uri));
        }
    }

    [RelayCommand]
    private async Task ShowDialogAsync()
    {
        await OverlayMessageBox.ShowAsync(
            "This removes the local package cache. The action cannot be undone.",
            "Clean install?",
            LocalHost,
            icon: MessageBoxIcon.Question,
            button: MessageBoxButton.OKCancel);
    }
}
