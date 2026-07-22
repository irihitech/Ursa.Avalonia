using System.Threading.Tasks;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Ursa.Controls;
using Ursa.Demo.Dialogs;
using Ursa.Demo.ViewModels.Controls;

using Ursa.Demo.Localizations;

namespace Ursa.Demo.ViewModels;

public partial class OverlayDialogDemoViewModel : ObservableObject, IPageMetadataProvider
{
    public PageMetadataViewModel PageMetadata { get; set; } = new PageMetadataViewModel()
    {
        Title = LanguageManager.Instance.Page_Title_OverlayDialog,
        Description = LanguageManager.Instance.Page_Description_OverlayDialog,
        Breadcrumbs = [new BreadcrumbItemData(LanguageManager.Instance.Menu_Category_DialogAndFeedbacks), new BreadcrumbItemData(LanguageManager.Instance.Menu_Header_OverlayDialog)],
        Tags = ["Dialog", "Modal", "Overlay"],
        DemoViewUrl = "https://github.com/irihitech/Ursa.Avalonia/blob/main/demo/Ursa.Demo/Pages/OverlayDialogDemo.axaml",
        DemoViewModelUrl = "https://github.com/irihitech/Ursa.Avalonia/blob/main/demo/Ursa.Demo/ViewModels/Pages/OverlayDialogDemoViewModel.cs",
        InlineXamlSupport = false,
        MvvmSupport = true,
        AvaloniaExclusive = true,
    };

    public const string LocalHost = "LocalHost";
    public DefaultOverlayDialogDemoViewModel DefaultOverlayDialogDemoViewModel { get; set; } = new();
    public CustomOverlayDialogDemoViewModel CustomOverlayDialogDemoViewModel { get; set; } = new();
}

public partial class DefaultOverlayDialogDemoViewModel : ObservableObject
{
    [ObservableProperty] private HorizontalPosition _horizontalAnchor;
    [ObservableProperty] private VerticalPosition _verticalAnchor;
    [ObservableProperty] private double? _horizontalOffset;
    [ObservableProperty] private double? _verticalOffset;
    [ObservableProperty] private bool _fullScreen;
    [ObservableProperty] private DialogMode _mode;
    [ObservableProperty] private DialogButton _button;
    [ObservableProperty] private string? _title;
    [ObservableProperty] private bool _canLightDismiss;
    [ObservableProperty] private bool _canDragMove;
    [ObservableProperty] private bool? _isCloseButtonVisible;
    [ObservableProperty] private bool _isModal;
    [ObservableProperty] private bool _isLocal;
    [ObservableProperty] private bool _canResize;
    [ObservableProperty] private string? _styleClass;

    public ICommand ShowDialogCommand { get; }

    public DefaultOverlayDialogDemoViewModel()
    {
        ShowDialogCommand = new AsyncRelayCommand(ShowDialog);
        HorizontalAnchor = HorizontalPosition.Center;
        VerticalAnchor = VerticalPosition.Center;
        CanDragMove = true;
        IsModal = true;
        IsCloseButtonVisible = true;
        Button = DialogButton.OKCancel;
    }

    private async Task ShowDialog()
    {
        var options = new OverlayDialogOptions()
        {
            FullScreen = FullScreen,
            HorizontalAnchor = HorizontalAnchor,
            VerticalAnchor = VerticalAnchor,
            HorizontalOffset = HorizontalOffset,
            VerticalOffset = VerticalOffset,
            Mode = Mode,
            Buttons = Button,
            Title = Title,
            CanLightDismiss = CanLightDismiss,
            CanDragMove = CanDragMove,
            IsCloseButtonVisible = IsCloseButtonVisible,
            CanResize = CanResize,
            StyleClass = StyleClass,
        };
        string? dialogHostId = IsLocal ? OverlayDialogDemoViewModel.LocalHost : null;
        if (IsModal)
        {
            await OverlayDialog.ShowStandardAsync<DefaultDemoDialog, DefaultDemoDialogViewModel>(new DefaultDemoDialogViewModel(), dialogHostId, options: options);
        }
        else
        {
            OverlayDialog.ShowStandard<DefaultDemoDialog, DefaultDemoDialogViewModel>(new DefaultDemoDialogViewModel(), dialogHostId, options: options);
        }
    }
}

public partial class CustomOverlayDialogDemoViewModel : ObservableObject
{
    [ObservableProperty] private HorizontalPosition _horizontalAnchor;
    [ObservableProperty] private VerticalPosition _verticalAnchor;
    [ObservableProperty] private double? _horizontalOffset;
    [ObservableProperty] private double? _verticalOffset;
    [ObservableProperty] private bool _fullScreen;
    [ObservableProperty] private string? _title;
    [ObservableProperty] private bool _canLightDismiss;
    [ObservableProperty] private bool _canDragMove;
    [ObservableProperty] private bool? _isCloseButtonVisible;
    [ObservableProperty] private bool _isModal;
    [ObservableProperty] private bool _isLocal;
    [ObservableProperty] private bool _canResize;

    public ICommand ShowDialogCommand { get; }

    public CustomOverlayDialogDemoViewModel()
    {
        ShowDialogCommand = new AsyncRelayCommand(ShowDialog);
        HorizontalAnchor = HorizontalPosition.Center;
        VerticalAnchor = VerticalPosition.Center;
        CanDragMove = true;
        IsModal = true;
    }

    private async Task ShowDialog()
    {
        var options = new OverlayDialogOptions()
        {
            FullScreen = FullScreen,
            HorizontalAnchor = HorizontalAnchor,
            VerticalAnchor = VerticalAnchor,
            HorizontalOffset = HorizontalOffset,
            VerticalOffset = VerticalOffset,
            Title = Title,
            CanLightDismiss = CanLightDismiss,
            CanDragMove = CanDragMove,
            IsCloseButtonVisible = IsCloseButtonVisible,
            CanResize = CanResize,
        };
        var dialogHostId = IsLocal ? OverlayDialogDemoViewModel.LocalHost : null;
        if (IsModal)
        {
            await OverlayDialog.ShowCustomAsync<CustomDemoDialog, CustomDemoDialogViewModel, object>(new CustomDemoDialogViewModel(), dialogHostId, options: options);
        }
        else
        {
            OverlayDialog.ShowCustom<CustomDemoDialog, CustomDemoDialogViewModel>(new CustomDemoDialogViewModel(), dialogHostId, options: options);
        }
    }
}
