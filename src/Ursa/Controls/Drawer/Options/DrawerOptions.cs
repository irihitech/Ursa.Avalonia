using Ursa.Common;

namespace Ursa.Controls.Options;

public class DrawerOptions
{
    internal static DrawerOptions Default => new ();
    public Position Position { get; set; } = Position.Right;
    public bool CanLightDismiss { get; set; } = true;
    public bool? IsCloseButtonVisible { get; set; } = true;
    public double? MinWidth { get; set; } = null;
    public double? MinHeight { get; set; } = null;
    public double? MaxWidth { get; set; } = null;
    public double? MaxHeight { get; set; } = null;
    public DialogButton Buttons { get; set; } = DialogButton.OKCancel;
    public string? Title { get; set; }
    [Obsolete("Use IsCloseButtonVisible")]
    public bool ShowCloseButton { get; set; } = true;
    /// <summary>
    /// The hash code of the top level dialog host. This is used to identify the dialog host if there are multiple dialog hosts with the same id. If this is not provided, the dialog will be added to the first dialog host with the same id.
    /// </summary>
    public int? TopLevelHashCode { get; set; }

    public bool CanResize { get; set; }
    
    public string? StyleClass { get; set; }
}