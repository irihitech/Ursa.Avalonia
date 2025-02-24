using System;
using System.Threading.Tasks;
using System.Timers;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Threading;
using Sandbox.ViewModels;
using Ursa.Controls;

namespace Sandbox.Views;

public partial class MainSplashWindow : SplashWindow
{
    public MainSplashWindow()
    {
        InitializeComponent();
    }
    
    protected override async Task<Window> CreateNextWindow()
    {
        return new MainWindow()
        {
            DataContext = new MainWindowViewModel()
        };
    }
}