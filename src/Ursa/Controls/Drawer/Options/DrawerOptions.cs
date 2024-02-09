using Ursa.Common;

namespace Ursa.Controls.Options;

public class DrawerOptions
{
    internal static DrawerOptions Default => new ();
    public Position Position { get; set; } = Position.Right;
    public bool CanLightDismiss { get; set; } = true;
    public bool IsCloseButtonVisible { get; set; } = true;
    public double? MinWidth { get; set; } = null;
    public double? MinHeight { get; set; } = null;
    public double? MaxWidth { get; set; } = null;
    public double? MaxHeight { get; set; } = null;
    public DialogButton Buttons { get; set; } = DialogButton.OKCancel;
    public string? Title { get; set; }
    public bool ShowCloseButton { get; set; } = true;
}