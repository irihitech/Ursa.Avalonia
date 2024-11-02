namespace Ursa.Controls;

public enum HorizontalPosition
{
    Left,
    Center,
    Right
}

public enum VerticalPosition
{
    Top,
    Center,
    Bottom
}

public class OverlayDialogOptions
{
    internal static OverlayDialogOptions Default { get; } = new();
    public bool FullScreen { get; set; }
    public HorizontalPosition HorizontalAnchor { get; set; } = HorizontalPosition.Center;
    public VerticalPosition VerticalAnchor { get; set; } = VerticalPosition.Center;

    /// <summary>
    ///     This attribute is only used when HorizontalAnchor is not Center
    /// </summary>
    public double? HorizontalOffset { get; set; } = null;

    /// <summary>
    ///     This attribute is only used when VerticalAnchor is not Center
    /// </summary>
    public double? VerticalOffset { get; set; } = null;

    /// <summary>
    ///     Only works for DefaultDialogControl
    /// </summary>
    public DialogMode Mode { get; set; } = DialogMode.None;

    /// <summary>
    ///     Only works for DefaultDialogControl
    /// </summary>
    public DialogButton Buttons { get; set; } = DialogButton.OKCancel;

    /// <summary>
    ///     Only works for DefaultDialogControl
    /// </summary>
    public string? Title { get; set; } = null;

    /// <summary>
    ///     Only works for CustomDialogControl
    /// </summary>
    public bool? IsCloseButtonVisible { get; set; } = true;

    [Obsolete] public bool ShowCloseButton { get; set; } = true;

    public bool CanLightDismiss { get; set; }
    public bool CanDragMove { get; set; } = true;

    /// <summary>
    ///     The hash code of the top level dialog host. This is used to identify the dialog host if there are multiple dialog
    ///     hosts with the same id. If this is not provided, the dialog will be added to the first dialog host with the same
    ///     id.
    /// </summary>
    public int? TopLevelHashCode { get; set; }

    public bool CanResize { get; set; }
    
    public string? StyleClass { get; set; }
}