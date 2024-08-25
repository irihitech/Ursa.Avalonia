using System;
using System.Collections.ObjectModel;
using System.Threading.Channels;
using System.Threading.Tasks;
using System.Windows.Input;
using Avalonia;
using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Ursa.Controls;
using Ursa.Demo.Dialogs;

namespace Ursa.Demo.ViewModels;

public partial class DialogDemoViewModel: ObservableObject
{
    public const string LocalHost = "LocalHost";
    public DefaultWindowDialogDemoViewModel DefaultWindowDialogDemoViewModel { get; set; } = new();
    public CustomWindowDialogDemoViewModel CustomWindowDialogDemoViewModel { get; set; } = new();
    public DefaultOverlayDialogDemoViewModel DefaultOverlayDialogDemoViewModel { get; set; } = new();
    public CustomOverlayDialogDemoViewModel CustomOverlayDialogDemoViewModel { get; set; } = new();
}

public partial class DefaultWindowDialogDemoViewModel: ObservableObject
{
    [ObservableProperty] private WindowStartupLocation _location;
    [ObservableProperty] private int? _x;
    [ObservableProperty] private int? _y;
    [ObservableProperty] private string? _title;
    [ObservableProperty] private DialogMode _mode;
    [ObservableProperty] private DialogButton _button;
    [ObservableProperty] private bool _showInTaskBar;
    [ObservableProperty] private bool? _isCloseButtonVisible;

    public ICommand ShowDialogCommand { get; }

    public DefaultWindowDialogDemoViewModel()
    {
        ShowDialogCommand = new AsyncRelayCommand(ShowDialog);
        Mode = DialogMode.None;
        Button = DialogButton.OKCancel;
        Location = WindowStartupLocation.CenterScreen;
    }

    private async Task ShowDialog()
    {
        var options = new DialogOptions()
        {
            Title = Title,
            Mode = Mode,
            Button = Button,
            ShowInTaskBar = ShowInTaskBar,
            IsCloseButtonVisible = IsCloseButtonVisible,
            StartupLocation = Location,
        };
        if (X.HasValue && Y.HasValue)
        {
            options.Position = new PixelPoint(X.Value, Y.Value);
        }
        await Dialog.ShowModal<PlainDialog, PlainDialogViewModel>(new PlainDialogViewModel(), options: options);
    }
}

public partial class CustomWindowDialogDemoViewModel: ObservableObject
{
    [ObservableProperty] private WindowStartupLocation _location;
    [ObservableProperty] private int? _x;
    [ObservableProperty] private int? _y;
    [ObservableProperty] private string? _title;
    [ObservableProperty] private bool _showInTaskBar;
    [ObservableProperty] private bool? _isCloseButtonVisible;
    [ObservableProperty] private bool _isModal;

    public ICommand ShowDialogCommand { get; }

    public CustomWindowDialogDemoViewModel()
    {
        ShowDialogCommand = new AsyncRelayCommand(ShowDialog);
        Location = WindowStartupLocation.CenterScreen;
        IsModal = true;
    }

    private async Task ShowDialog()
    {
        var options = new DialogOptions()
        {
            Title = Title,
            ShowInTaskBar = ShowInTaskBar,
            IsCloseButtonVisible = IsCloseButtonVisible,
            StartupLocation = Location,
        };
        if (X.HasValue && Y.HasValue)
        {
            options.Position = new PixelPoint(X.Value, Y.Value);
        }

        if (IsModal)
        {
            await Dialog.ShowCustomModal<DialogWithAction, DialogWithActionViewModel, object>(new DialogWithActionViewModel(),
                options: options);
        }
        else
        {
            Dialog.ShowCustom<DialogWithAction, DialogWithActionViewModel>(new DialogWithActionViewModel(),
                options: options);
        }
    }
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

    public ICommand ShowDialogCommand { get; }

    public DefaultOverlayDialogDemoViewModel()
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
            Mode = Mode,
            Buttons = Button,
            Title = Title,
            CanLightDismiss = CanLightDismiss,
            CanDragMove = CanDragMove,
            IsCloseButtonVisible = IsCloseButtonVisible,
        };
        string? dialogHostId = IsLocal ? DialogDemoViewModel.LocalHost : null;
        if (IsModal)
        {
            await OverlayDialog.ShowModal<PlainDialog, PlainDialogViewModel>(new PlainDialogViewModel(), dialogHostId, options: options);
        }
        else
        {
            OverlayDialog.Show<PlainDialog, PlainDialogViewModel>(new PlainDialogViewModel(), dialogHostId, options: options);
        }
    }
}

public partial class CustomOverlayDialogDemoViewModel: ObservableObject
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
            Mode = Mode,
            Buttons = Button,
            Title = Title,
            CanLightDismiss = CanLightDismiss,
            CanDragMove = CanDragMove,
            IsCloseButtonVisible = IsCloseButtonVisible,
        };
        var dialogHostId = IsLocal ? DialogDemoViewModel.LocalHost : null;
        if (IsModal)
        {
            await OverlayDialog.ShowCustomModal<DialogWithAction, DialogWithActionViewModel, object>(new DialogWithActionViewModel(), dialogHostId, options: options);
        }
        else
        {
            OverlayDialog.ShowCustom<DialogWithAction, DialogWithActionViewModel>(new DialogWithActionViewModel(), dialogHostId, options: options);
        }
    }
}