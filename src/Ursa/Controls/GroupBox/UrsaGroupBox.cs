using Avalonia;
using Avalonia.Controls.Primitives;

namespace Ursa.Controls;

public class UrsaGroupBox: HeaderedContentControl
{
    /// <summary>Defines the <see cref="HeaderSpacing"/> property.</summary>
    public static readonly StyledProperty<double> HeaderSpacingProperty =
        GroupBoxBorder.HeaderSpacingProperty.AddOwner<UrsaGroupBox>();
    
    /// <summary>
    /// Gets or sets the extra space (in device-independent pixels) added on each side of
    /// the <see cref="HeaderedContentControl.Header"/> control within the border gap.  Defaults to 4.
    /// </summary>
    public double HeaderSpacing
    {
        get => GetValue(HeaderSpacingProperty);
        set => SetValue(HeaderSpacingProperty, value);
    }
}
