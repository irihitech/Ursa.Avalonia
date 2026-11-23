using Avalonia;
using Avalonia.Themes.Fluent2;
using Ursa.Controls;
using Ursa.Fluent2.Demo.Services;
using Ursa.Fluent2.Demo.ViewModels;
using Ursa.Themes.Fluent2;

namespace Ursa.Fluent2.Demo;

public partial class MainWindow : UrsaWindow
{
    public MainWindow()
    {
        var styles = Application.Current!.Styles;
        ViewModel = new MainViewModel(styles.OfType<Fluent2Theme>().Single(),
            styles.OfType<UrsaFluent2Theme>().Single(), new DemoInteractionService(this));
        DataContext = ViewModel;
        InitializeComponent();
        Closed += (_, _) => ViewModel.Dispose();
    }

    public MainViewModel ViewModel { get; }
}
