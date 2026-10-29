using Avalonia.Controls;
using Avalonia.Interactivity;
using Ursa.Controls;

namespace Ursa.Fluent2.Demo.Pages;

public class DemoPageView : UserControl
{
    protected async void OpenSample(object? sender, RoutedEventArgs e)
    {
        var button = (Button)sender!;
        var name = (string)button.Tag!;
        var content = new SampleMessage { DataContext = $"{name} rendered with Fluent 2." };
        switch (name)
        {
            case "Dialog":
                Dialog.ShowStandard(content, vm: null, owner: TopLevel.GetTopLevel(button) as Window);
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
                    new CustomDialogSampleViewModel(), owner: TopLevel.GetTopLevel(button) as Window);
                break;
            case "StandardDialogControl":
                OverlayDialog.ShowStandard(content, vm: null);
                break;
            case "CustomDrawerControl":
                OverlayDrawer.ShowCustom(content, vm: null);
                break;
            case "WindowNotificationManager":
                var topLevel = TopLevel.GetTopLevel(button);
                if (topLevel is not null)
                {
                    if (!WindowNotificationManager.TryGetNotificationManager(topLevel, out var manager))
                        manager = new WindowNotificationManager(topLevel);
                    manager?.Show("Fluent 2 notification sample");
                }
                break;
            case "WindowToastManager":
                var toastTopLevel = TopLevel.GetTopLevel(button);
                if (toastTopLevel is not null)
                {
                    if (!WindowToastManager.TryGetToastManager(toastTopLevel, out var manager))
                        manager = new WindowToastManager(toastTopLevel);
                    manager?.Show("Fluent 2 toast sample");
                }
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
                if (TopLevel.GetTopLevel(button) is Window owner)
                    window.Show(owner);
                else
                    window.Show();
                break;
        }
    }

    private sealed class DemoSplashWindow : SplashWindow
    {
        protected override Task<Window?> CreateNextWindow() => Task.FromResult<Window?>(null);
    }
}
