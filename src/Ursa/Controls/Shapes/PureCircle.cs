using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Media;

namespace Ursa.Controls.Shapes;

public class PureCircle: Control
{
    public static readonly StyledProperty<IBrush?> BackgroundProperty =
        TemplatedControl.BackgroundProperty.AddOwner<PureCircle>();

    public IBrush? Background
    {
        get => GetValue(BackgroundProperty);
        set => SetValue(BackgroundProperty, value);
    }

    public static readonly StyledProperty<double> DiameterProperty = AvaloniaProperty.Register<PureCircle, double>(
        nameof(Diameter));

    public double Diameter
    {
        get => GetValue(DiameterProperty);
        set => SetValue(DiameterProperty, value);
    }

    static PureCircle()
    {
        FocusableProperty.OverrideDefaultValue<PureCircle>(false);
        AffectsMeasure<PureCircle>(DiameterProperty);
        AffectsRender<PureCircle>(DiameterProperty, BackgroundProperty);
    }

    protected override Size MeasureOverride(Size availableSize)
    {
        return new Size(Diameter, Diameter);
    }

    public override void Render(DrawingContext context)
    {
        double value = Diameter / 2;
        context.DrawEllipse(Background, null, new(value, value), value, value);
    }
}