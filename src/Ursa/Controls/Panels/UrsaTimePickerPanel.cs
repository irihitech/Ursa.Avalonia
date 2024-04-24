using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Input;

namespace Ursa.Controls.Panels;

/// <summary>
/// The panel to display items for time selection 
/// </summary>
public class UrsaTimePickerPanel: Panel
{
    /// <summary>
    /// Defines whether the panel is looping.
    /// This is ont applicable for columns like year and AM/PM designation. 
    /// </summary>
    public static readonly StyledProperty<bool> IsLoopingProperty = AvaloniaProperty.Register<UrsaTimePickerPanel, bool>(
        nameof(IsLooping));

    /// <summary>
    /// Gets or sets the value of <see cref="IsLoopingProperty"/>.
    /// </summary>
    public bool IsLooping
    {
        get => GetValue(IsLoopingProperty);
        set => SetValue(IsLoopingProperty, value);
    }
}