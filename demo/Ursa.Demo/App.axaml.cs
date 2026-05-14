using System;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Semi.Avalonia;
using Ursa.Demo.ViewModels;
using Ursa.Demo.Views;

namespace Ursa.Demo;

public partial class App : Application
{
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
#if DEBUG
        this.AttachDeveloperTools();
#endif
        DataContext = new ApplicationViewModel();
        if (OperatingSystem.IsLinux())
        {
            Resources.Add("DefaultFontFamily", null);
        }
    }

    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.MainWindow = new MvvmSplashWindow { DataContext = new SplashViewModel() };
        }
        else if (ApplicationLifetime is IActivityApplicationLifetime applicationLifetime)
        {
            applicationLifetime.MainViewFactory = () => new SingleView { DataContext = new MainViewViewModel() };
        }
        else if (ApplicationLifetime is ISingleViewApplicationLifetime singleViewPlatform)
        {
            singleViewPlatform.MainView = new SingleView { DataContext = new MainViewViewModel() };
        }

        this.RegisterFollowSystemTheme();
        base.OnFrameworkInitializationCompleted();
    }
}
