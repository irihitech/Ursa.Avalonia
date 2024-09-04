using Avalonia;
using Avalonia.Controls;
using Ursa.Controls;
using Ursa.Demo.ViewModels;

namespace Ursa.Demo.Pages;

public partial class ToastDemo : UserControl
{
    private ToastDemoViewModel _viewModel;

    public ToastDemo()
    {
        InitializeComponent();
        _viewModel = new ToastDemoViewModel();
        DataContext = _viewModel;
    }

    protected override void OnAttachedToVisualTree(VisualTreeAttachmentEventArgs e)
    {
        base.OnAttachedToVisualTree(e);
        var topLevel = TopLevel.GetTopLevel(this);
        _viewModel.ToastManager = new WindowToastManager(topLevel) { MaxItems = 3 };
    }
}