using Avalonia;
using Avalonia.Controls.Primitives;

namespace Ursa.Controls;

public class DialogResizer: TemplatedControl
{
    public static readonly StyledProperty<ResizeDirection> ResizeDirectionProperty = AvaloniaProperty.Register<DialogResizer, ResizeDirection>(
        nameof(ResizeDirection));

    /// <summary>
    /// Defines what direction the dialog is allowed to be resized.
    /// </summary>
    public ResizeDirection ResizeDirection
    {
        get => GetValue(ResizeDirectionProperty);
        set => SetValue(ResizeDirectionProperty, value);
    }
}