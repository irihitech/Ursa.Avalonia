using Avalonia;
using Avalonia.Controls;
using Ursa.Controls;

namespace Ursa.Demo.Pages.PopConfirmDemo;

public partial class PopConfirmDemo : UserControl
{
    public PopConfirmDemo()
    {
        InitializeComponent();
    }

    protected override void OnAttachedToVisualTree(VisualTreeAttachmentEventArgs e)
    {
        base.OnAttachedToVisualTree(e);
        if (DataContext is not PopConfirmDemoViewModel vm) return;
        var topLevel = TopLevel.GetTopLevel(this);
        vm.ToastManager = WindowToastManager.TryGetToastManager(topLevel, out var manager)
            ? manager
            : new WindowToastManager(topLevel);
    }
}