using Avalonia;
using Avalonia.Controls;
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

        WindowNotificationManager.TryGetNotificationManager(topLevel, out var manager);
        if (manager is not null)
        {
            _viewModel.NotificationManager = manager;
        }
    }
}