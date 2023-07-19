using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Ursa.Demo.Views;

namespace Ursa.Demo.Browser;

public partial class App : Application
{
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is ISingleViewApplicationLifetime single)
        {
            single.MainView = new MainWindow();
        }
        base.OnFrameworkInitializationCompleted();
    }
}