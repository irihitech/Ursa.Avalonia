using Avalonia;
using Avalonia.Controls;
using Avalonia.LogicalTree;
using Ursa.Controls;
using Ursa.Demo.ViewModels;

namespace Ursa.Demo.Pages;

public partial class NotificationDemo : UserControl
{
    private NotificationDemoViewModel _viewModel;

    public NotificationDemo()
    {
        InitializeComponent();
        _viewModel = new NotificationDemoViewModel();
        DataContext = _viewModel;
    }

    protected override void OnAttachedToVisualTree(VisualTreeAttachmentEventArgs e)
    {
        base.OnAttachedToVisualTree(e);
        var topLevel = TopLevel.GetTopLevel(this);
        _viewModel.NotificationManager = new WindowNotificationManager(topLevel) { MaxItems = 3 };
    }

    protected override void OnDetachedFromLogicalTree(LogicalTreeAttachmentEventArgs e)
    {
        base.OnDetachedFromLogicalTree(e);
        _viewModel.NotificationManager?.Uninstall();
    }
}