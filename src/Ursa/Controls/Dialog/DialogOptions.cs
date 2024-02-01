using Avalonia;
using Avalonia.Controls;
using Ursa.Common;

namespace Ursa.Controls;

public class DialogOptions
{
    /// <summary>
    /// The Startup Location of DialogWindow. Default is <see cref="WindowStartupLocation.CenterOwner"/>
    /// </summary>
    public WindowStartupLocation StartupLocation { get; set; } = WindowStartupLocation.CenterOwner;
    /// <summary>
    /// The Position of DialogWindow startup location if <see cref="StartupLocation"/> is <see cref="WindowStartupLocation.Manual"/>
    /// </summary>
    public PixelPoint? Position { get; set; }
    /// <summary>
    /// Title of DialogWindow, Default is null
    /// </summary>
    public string? Title { get; set; }
    /// <summary>
    /// DialogWindow's Mode, Default is <see cref="DialogMode.None"/>
    /// </summary>
    public DialogMode Mode { get; set; } = DialogMode.None;

    public DialogButton Button { get; set; } = DialogButton.OKCancel;
}

public class OverlayDialogOptions
{
    public bool ClickOnMaskToClose { get; set; } = false;
}