using System;
using System.Net;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Ursa.Demo.Pages;

public partial class IPv4BoxDemo : UserControl
{
    public IPv4BoxDemo()
    {
        InitializeComponent();
        DataContext = new IPv4DemoViewMode();
    }
}

public partial class IPv4DemoViewMode: ObservableObject
{
    [ObservableProperty]
    private IPAddress? _address;

    public void ChangeAddress()
    {
        long l = Random.Shared.NextInt64(0x00000000FFFFFFFF);
        Address = new IPAddress(l);
    }
}