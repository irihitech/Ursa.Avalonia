using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Shapes;
using Avalonia.Media;

namespace Ursa.Controls.Shapes;

/// <summary>
/// A rectangle, with no corner radius.
/// </summary>
public class PureRectangle: Control
{
    public static readonly StyledProperty<IBrush?> BackgroundProperty = AvaloniaProperty.Register<PureRectangle, IBrush?>(
        nameof(Background));

    public IBrush? Background
    {
        get => GetValue(BackgroundProperty);
        set => SetValue(BackgroundProperty, value);
    }
    static PureRectangle()
    {
        FocusableProperty.OverrideDefaultValue<PureRectangle>(false);
    }

    public override void Render(DrawingContext context)
    {
        context.DrawRectangle(Background, null, new Rect(Bounds.Size));
    }
}