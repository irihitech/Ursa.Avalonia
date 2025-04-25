using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Ursa.Controls;
using Ursa.Demo.ViewModels;

namespace Ursa.Demo.Pages;

public partial class PopConfirmDemo : UserControl
{
    public PopConfirmDemo()
    {
        InitializeComponent();
    }

    protected override void OnAttachedToVisualTree(VisualTreeAttachmentEventArgs e)
    {
        base.OnAttachedToVisualTree(e);
        if (this.DataContext is not PopConfirmDemoViewModel vm) return;
        var manager = WindowToastManager.TryGetToastManager(TopLevel.GetTopLevel(this), out var m)
            ? m
            : new WindowToastManager(TopLevel.GetTopLevel(this));
        vm.ToastManager = manager;
    }
}