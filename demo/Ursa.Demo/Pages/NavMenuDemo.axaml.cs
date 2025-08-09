using Avalonia.Controls;
using Avalonia.Interactivity;
using Ursa.Controls;
using Ursa.Helpers;

namespace Ursa.Demo.Pages;

public partial class NavMenuDemo : UserControl
{
    private readonly NavMenuAnimationHelper _animationHelper;

    public NavMenuDemo()
    {
        InitializeComponent();
        _animationHelper = new NavMenuAnimationHelper(menu);
    }

    protected override void OnLoaded(RoutedEventArgs e)
    {
        base.OnLoaded(e);
        _animationHelper.Start();
    }
}