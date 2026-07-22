using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Avalonia;
using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Ursa.Controls;
using Ursa.Demo.Dialogs;
using Ursa.Demo.ViewModels.Controls;

using Ursa.Demo.Localizations;

namespace Ursa.Demo.ViewModels;

public partial class WindowDialogDemoViewModel : ObservableObject, IPageMetadataProvider
{
    public PageMetadataViewModel PageMetadata { get; set; } = new PageMetadataViewModel()
    {
        Title = LanguageManager.Instance.Page_Title_WindowDialog,
        Description = LanguageManager.Instance.Page_Description_WindowDialog,
        Breadcrumbs = [new BreadcrumbItemData(LanguageManager.Instance.Menu_Category_DialogAndFeedbacks), new BreadcrumbItemData(LanguageManager.Instance.Menu_Header_WindowDialog)],
        Tags = ["Dialog", "Modal", "Window"],
        DemoViewUrl = "https://github.com/irihitech/Ursa.Avalonia/blob/main/demo/Ursa.Demo/Pages/WindowDialogDemo.axaml",
        DemoViewModelUrl = "https://github.com/irihitech/Ursa.Avalonia/blob/main/demo/Ursa.Demo/ViewModels/Pages/WindowDialogDemoViewModel.cs",
        InlineXamlSupport = false,
        MvvmSupport = true,
        AvaloniaExclusive = true,
    };

    public DefaultWindowDialogDemoViewModel DefaultWindowDialogDemoViewModel { get; set; } = new();
    public CustomWindowDialogDemoViewModel CustomWindowDialogDemoViewModel { get; set; } = new();
}

public partial class DefaultWindowDialogDemoViewModel : ObservableObject
{
    [ObservableProperty] private WindowStartupLocation _location;
    [ObservableProperty] private int? _x;
    [ObservableProperty] private int? _y;
    [ObservableProperty] private string? _title;
    [ObservableProperty] private DialogMode _mode;
    [ObservableProperty] private DialogButton _button;
    [ObservableProperty] private bool _showInTaskBar;
    [ObservableProperty] private bool? _isCloseButtonVisible;
    [ObservableProperty] private bool _canDragMove;
    [ObservableProperty] private bool _canResize;
    [ObservableProperty] private string? _styleClass;

    public ICommand ShowDialogCommand { get; }

    public DefaultWindowDialogDemoViewModel()
    {
        ShowDialogCommand = new AsyncRelayCommand(ShowDialog);
        Mode = DialogMode.None;
        Button = DialogButton.OKCancel;
        Location = WindowStartupLocation.CenterScreen;
        CanDragMove = true;
    }

    private async Task ShowDialog()
    {
        if (OperatingSystem.IsBrowser() || OperatingSystem.IsAndroid() || OperatingSystem.IsIOS())
        {
            await OverlayMessageBox.ShowAsync("Window dialogs are not supported on this platform. Please use overlay dialogs instead.");
            return;
        }
        var options = new DialogOptions()
        {
            Title = Title,
            Mode = Mode,
            Button = Button,
            ShowInTaskBar = ShowInTaskBar,
            IsCloseButtonVisible = IsCloseButtonVisible,
            StartupLocation = Location,
            CanDragMove = CanDragMove,
            CanResize = CanResize,
            StyleClass = StyleClass,
        };
        if (X.HasValue && Y.HasValue)
        {
            options.Position = new PixelPoint(X.Value, Y.Value);
        }
        await Dialog.ShowStandardAsync<DefaultDemoDialog, DefaultDemoDialogViewModel>(new DefaultDemoDialogViewModel(), options: options);
    }
}

public partial class CustomWindowDialogDemoViewModel : ObservableObject
{
    [ObservableProperty] private WindowStartupLocation _location;
    [ObservableProperty] private int? _x;
    [ObservableProperty] private int? _y;
    [ObservableProperty] private string? _title;
    [ObservableProperty] private bool _showInTaskBar;
    [ObservableProperty] private bool? _isCloseButtonVisible;
    [ObservableProperty] private bool _isModal;
    [ObservableProperty] private bool _canDragMove;
    [ObservableProperty] private bool _canResize;

    public ICommand ShowDialogCommand { get; }

    public CustomWindowDialogDemoViewModel()
    {
        ShowDialogCommand = new AsyncRelayCommand(ShowDialog);
        Location = WindowStartupLocation.CenterScreen;
        IsModal = true;
        CanDragMove = true;
    }

    private async Task ShowDialog()
    {
        if (OperatingSystem.IsBrowser() || OperatingSystem.IsAndroid() || OperatingSystem.IsIOS())
        {
            await OverlayMessageBox.ShowAsync("Window dialogs are not supported on this platform. Please use overlay dialogs instead.");
            return;
        }
        var options = new DialogOptions()
        {
            Title = Title,
            ShowInTaskBar = ShowInTaskBar,
            IsCloseButtonVisible = IsCloseButtonVisible,
            StartupLocation = Location,
            CanDragMove = CanDragMove,
            CanResize = CanResize,
        };
        if (X.HasValue && Y.HasValue)
        {
            options.Position = new PixelPoint(X.Value, Y.Value);
        }

        if (IsModal)
        {
            await Dialog.ShowCustomAsync<CustomDemoDialog, CustomDemoDialogViewModel, object>(new CustomDemoDialogViewModel(),
                options: options);
        }
        else
        {
            Dialog.ShowCustom<CustomDemoDialog, CustomDemoDialogViewModel>(new CustomDemoDialogViewModel(),
                options: options);
        }
    }
}
