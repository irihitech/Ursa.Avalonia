using Avalonia.Controls;
using Ursa.Controls;
using Ursa.Fluent2.Demo.Pages;
using Ursa.Fluent2.Demo.ViewModels;

namespace Ursa.Fluent2.Demo.Services;

public sealed class DemoInteractionService(Window owner) : IDemoInteractionService
{
    public async Task OpenSampleAsync(string name)
    {
        var content = new SampleMessage { DataContext = $"{name} rendered with Fluent 2." };
        switch (name)
        {
            case "Dialog":
                Dialog.ShowStandard(content, vm: null, owner: owner);
                break;
            case "ModalDialog":
                await OverlayDialog.ShowStandardAsync(content, vm: null,
                    options: new OverlayDialogOptions
                    {
                        Title = "Modal overlay dialog",
                        Mode = DialogMode.Info,
                        Buttons = DialogButton.OKCancel,
                        IsCloseButtonVisible = false
                    });
                break;
            case "CustomDialogControl":
                await OverlayDialog.ShowCustomAsync<SampleMessage>(content, vm: null);
                break;
            case "CustomDialogWindow":
                Dialog.ShowCustom<CustomDialogSample, CustomDialogSampleViewModel>(
                    new CustomDialogSampleViewModel(), owner: owner);
                break;
            case "StandardDialogControl":
                OverlayDialog.ShowStandard(content, vm: null);
                break;
            case "CustomDrawerControl":
                OverlayDrawer.ShowCustom(content, vm: null);
                break;
            case "WindowNotificationManager":
                WindowNotificationManager.TryGetNotificationManager(owner, out var notificationManager);
                (notificationManager ?? new WindowNotificationManager(owner)).Show("Fluent 2 notification sample");
                break;
            case "WindowToastManager":
                WindowToastManager.TryGetToastManager(owner, out var toastManager);
                (toastManager ?? new WindowToastManager(owner)).Show("Fluent 2 toast sample");
                break;
            default:
                Window window = name switch
                {
                    "UrsaWindow" => new UrsaWindow(),
                    "SplashWindow" => new DemoSplashWindow(),
                    "StandardDialogWindow" => new StandardDialogWindow(),
                    "MessageBoxWindow" => new MessageBoxWindow(MessageBoxButton.OK),
                    _ => throw new ArgumentOutOfRangeException(nameof(name), name, null)
                };
                window.Title = $"{name} · Fluent 2";
                window.Width = 480;
                window.Height = 320;
                window.Content ??= content;
                window.Show(owner);
                break;
        }
    }

    private sealed class DemoSplashWindow : SplashWindow
    {
        protected override Task<Window?> CreateNextWindow() => Task.FromResult<Window?>(null);
    }
}
