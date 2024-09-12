using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.VisualTree;
using Ursa.Controls;
using Ursa.Demo.ViewModels;

namespace Ursa.Demo.Pages;

public partial class ToastDemo : UserControl
{
    private ToastDemoViewModel _viewModel;
    private TopLevel? _topLevel;

    public ToastDemo()
    {
        InitializeComponent();
        _viewModel = new ToastDemoViewModel();
        DataContext = _viewModel;
    }

    protected override void OnAttachedToVisualTree(VisualTreeAttachmentEventArgs e)
    {
        base.OnAttachedToVisualTree(e);
        _topLevel = TopLevel.GetTopLevel(this);
        _viewModel.ToastManager = new WindowToastManager(_topLevel) { MaxItems = 3 };
    }

    protected override void OnDetachedFromVisualTree(VisualTreeAttachmentEventArgs e)
    {
        base.OnDetachedFromVisualTree(e);
        var adorner = _topLevel.FindDescendantOfType<VisualLayerManager>()?.AdornerLayer;
        if (adorner is not null && _viewModel.ToastManager is not null)
        {
            adorner.Children.Remove(_viewModel.ToastManager);
        }
    }
}