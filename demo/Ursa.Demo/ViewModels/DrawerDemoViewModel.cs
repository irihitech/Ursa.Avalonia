using System;
using System.Threading.Tasks;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Ursa.Common;
using Ursa.Controls;
using Ursa.Controls.Options;
using Ursa.Demo.Dialogs;

namespace Ursa.Demo.ViewModels;

public partial class DrawerDemoViewModel : ObservableObject
{
    public ICommand ShowDialogCommand { get; set; }

    [ObservableProperty] private Position _position;
    [ObservableProperty] private DialogButton _buttons;

    [ObservableProperty] private bool _canLightDismiss;
    [ObservableProperty] private bool _isModal;
    [ObservableProperty] private bool? _isCloseButtonVisible;
    [ObservableProperty] private string? _title;

    [ObservableProperty] private bool _custom;
    [ObservableProperty] private bool _isLocal;
    [ObservableProperty] private bool _canResize;

    public DrawerDemoViewModel()
    {
        ShowDialogCommand = new AsyncRelayCommand(ShowDefaultDialog);
        Position = Position.Right;
        IsModal = true;
        Title = "Add New";
    }

    private async Task ShowDefaultDialog()
    {
        var options = new DrawerOptions()
        {
            Position = Position,
            Buttons = Buttons,
            CanLightDismiss = CanLightDismiss,
            IsCloseButtonVisible = IsCloseButtonVisible,
            Title = Title,
            CanResize = CanResize,
        };
        var hostId = IsLocal ? "LocalHost" : null;
        if (Custom)
        {
            var vm = new CustomDemoDialogViewModel();
            if (IsModal)
            {
                await Drawer.ShowCustomModal<CustomDemoDialog, CustomDemoDialogViewModel, object?>(vm, hostId, options);
            }
            else
            {
                Drawer.ShowCustom<CustomDemoDialog, CustomDemoDialogViewModel>(vm, hostId, options);
            }
        }
        else
        {
            var vm = new DefaultDemoDialogViewModel();
            if (IsModal)
            {
                await Drawer.ShowModal<DefaultDemoDialog, DefaultDemoDialogViewModel>(vm, hostId, options);
            }
            else
            {
                Drawer.Show<DefaultDemoDialog, DefaultDemoDialogViewModel>(vm, hostId, options);
            }
        }
        
    }
}