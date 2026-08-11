using Avalonia;
using Avalonia.Controls;
using Ursa.Controls;

namespace Ursa.Demo.Pages.ToastDemo;

public partial class ToastDemo : UserControl
{
    private ToastDemoViewModel? _viewModel;

    public ToastDemo()
    {
        InitializeComponent();
    }

    protected override void OnAttachedToVisualTree(VisualTreeAttachmentEventArgs e)
    {
        base.OnAttachedToVisualTree(e);
        if (DataContext is not ToastDemoViewModel vm) return;
        _viewModel = vm;
        _viewModel.ToastManager = new WindowToastManager(TopLevel.GetTopLevel(this)) { MaxItems = 3 };
    }

    protected override void OnDetachedFromVisualTree(VisualTreeAttachmentEventArgs e)
    {
        base.OnDetachedFromVisualTree(e);
        _viewModel?.ToastManager?.Uninstall();
    }
}