using Avalonia.Controls;
using Avalonia.Interactivity;
using Ursa.Controls;

namespace Ursa.Demo.Pages;

public partial class NavMenuDemo : UserControl
{
    private readonly WHAnimationHelper _animationHelper;

    public NavMenuDemo()
    {
        InitializeComponent();
        _animationHelper = new WHAnimationHelper(menu, NavMenu.IsHorizontalCollapsedProperty);
    }

    protected override void OnLoaded(RoutedEventArgs e)
    {
        base.OnLoaded(e);
        _animationHelper.Start();
    }
}