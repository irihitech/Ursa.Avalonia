using Avalonia.Controls;
using Avalonia.Controls.Notifications;
using Avalonia.Markup.Xaml;
using CommunityToolkit.Mvvm.ComponentModel;
using Ursa.Demo.ViewModels;

namespace Ursa.Demo.Pages;

public partial class BannerDemo : UserControl
{
    public BannerDemo()
    {
        InitializeComponent();
        this.DataContext = new BannerDemoViewModel();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}

public partial class BannerDemoViewModel : ViewModelBase
{
    [ObservableProperty] private NotificationType _selectedType;
    [ObservableProperty] private bool _bordered;
    [ObservableProperty] private bool _canClose;
}