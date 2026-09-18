using Avalonia.Controls;
using Avalonia.Interactivity;

namespace Ursa.Demo.Pages.IntroductionDemo;

public partial class IntroductionDemo : UserControl
{
    public IntroductionDemo()
    {
        InitializeComponent();
    }

    protected override void OnLoaded(RoutedEventArgs e)
    {
        base.OnLoaded(e);
        if (DataContext is IntroductionDemoViewModel vm)
        {
            vm.Launcher = TopLevel.GetTopLevel(this)?.Launcher;
            vm.ShowDialogCommand.Execute(null);
        }
    }
}
