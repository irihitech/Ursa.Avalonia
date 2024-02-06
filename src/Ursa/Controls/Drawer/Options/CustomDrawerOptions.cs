using Ursa.Common;

namespace Ursa.Controls.Options;

public class CustomDrawerOptions
{
    internal static CustomDrawerOptions Default => new ();
    public Position Position { get; set; } = Position.Right;
    public bool CanClickOnMaskToClose { get; set; } = true;
    public bool CanLightDismiss { get; set; } = false;
    public bool ShowMask { get; set; } = true;
    public bool IsCloseButtonVisible { get; set; } = true;
    public double? MinWidth { get; set; } = null;
    public double? MinHeight { get; set; } = null;
    public double? MaxWidth { get; set; } = null;
    public double? MaxHeight { get; set; } = null;
}