using System.Collections.ObjectModel;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Notifications;
using Avalonia.Markup.Xaml;
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

public class BannerDemoViewModel : ViewModelBase
{
    private ObservableCollection<NotificationType> _types;

    public ObservableCollection<NotificationType> Types
    {
        get => _types;
        set => SetProperty(ref _types, value);
    }

    private NotificationType _selectedType;

    public NotificationType SelectedType
    {
        get => _selectedType;
        set => SetProperty(ref _selectedType, value);
    }

    private bool _bordered;

    public bool Bordered
    {
        get => _bordered;
        set => SetProperty(ref _bordered, value);
    }

    public BannerDemoViewModel()
    {
        Types = new ObservableCollection<NotificationType>()
        {
            NotificationType.Information, NotificationType.Success, NotificationType.Warning, NotificationType.Error
        };
    }
}