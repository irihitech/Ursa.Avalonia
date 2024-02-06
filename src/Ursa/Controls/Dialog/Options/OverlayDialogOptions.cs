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
    internal static OverlayDialogOptions Default { get; } = new OverlayDialogOptions();
    public bool CanClickOnMaskToClose { get; set; } = false;
    public HorizontalPosition HorizontalAnchor { get; set; } = HorizontalPosition.Center;
    public VerticalPosition VerticalAnchor { get; set; } = VerticalPosition.Center;
    public double? HorizontalOffset { get; set; } = null;
    public double? VerticalOffset { get; set; } = null;
    public DialogMode Mode { get; set; } = DialogMode.None;
    public DialogButton Buttons { get; set; } = DialogButton.OKCancel;
    public string? Title { get; set; } = null;
    public bool IsCloseButtonVisible { get; set; } = true;
    
    public bool CanLightDismiss { get; set; }
}