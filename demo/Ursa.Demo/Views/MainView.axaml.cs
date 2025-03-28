using Avalonia;
using Avalonia.Controls;
using Ursa.Controls;
using Ursa.Demo.ViewModels;

namespace Ursa.Demo.Views;

public partial class MainView : UserControl
{
    private MainViewViewModel? _viewModel;

    public MainView()
    {
        InitializeComponent();
    }

    protected override void OnAttachedToVisualTree(VisualTreeAttachmentEventArgs e)
    {
        base.OnAttachedToVisualTree(e);
        _viewModel = DataContext as MainViewViewModel;
        var topLevel = TopLevel.GetTopLevel(this);
        WindowNotificationManager.TryGetNotificationManager(topLevel, out var manager);
        if (manager is not null && _viewModel is not null)
        {
            _viewModel.NotificationManager = manager;
        }
    }
}