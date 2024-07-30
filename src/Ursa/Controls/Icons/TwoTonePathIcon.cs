using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Primitives;
using Avalonia.Data;
using Avalonia.Media;
using Irihi.Avalonia.Shared.Helpers;

namespace Ursa.Controls;

[PseudoClasses(PC_Active)]
public class TwoTonePathIcon: TemplatedControl
{
    public const string PC_Active = ":active";
    
    public static readonly StyledProperty<IBrush?> StrokeBrushProperty = AvaloniaProperty.Register<TwoTonePathIcon, IBrush?>(
        nameof(StrokeBrush));

    public IBrush? StrokeBrush
    {
        get => GetValue(StrokeBrushProperty);
        set => SetValue(StrokeBrushProperty, value);
    }
    
    public static readonly StyledProperty<Geometry> DataProperty = AvaloniaProperty.Register<PathIcon, Geometry>(
        nameof(Data));

    public Geometry Data
    {
        get => GetValue(DataProperty);
        set => SetValue(DataProperty, value);
    }

    public static readonly StyledProperty<bool> IsActiveProperty = AvaloniaProperty.Register<TwoTonePathIcon, bool>(
        nameof(IsActive), defaultBindingMode: BindingMode.TwoWay);

    public bool IsActive
    {
        get => GetValue(IsActiveProperty);
        set => SetValue(IsActiveProperty, value);
    }

    public static readonly StyledProperty<IBrush?> ActiveForegroundProperty = AvaloniaProperty.Register<TwoTonePathIcon, IBrush?>(
        nameof(ActiveForeground));

    public IBrush? ActiveForeground
    {
        get => GetValue(ActiveForegroundProperty);
        set => SetValue(ActiveForegroundProperty, value);
    }

    public static readonly StyledProperty<IBrush?> ActiveStrokeBrushProperty = AvaloniaProperty.Register<TwoTonePathIcon, IBrush?>(
        nameof(ActiveStrokeBrush));

    public IBrush? ActiveStrokeBrush
    {
        get => GetValue(ActiveStrokeBrushProperty);
        set => SetValue(ActiveStrokeBrushProperty, value);
    }

    public static readonly StyledProperty<double> StrokeThicknessProperty =
        AvaloniaProperty.Register<TwoTonePathIcon, double>(
            nameof(StrokeThickness));
    public double StrokeThickness
    {
        get => GetValue(StrokeThicknessProperty);
        set => SetValue(StrokeThicknessProperty, value);
    }
    
    static TwoTonePathIcon()
    {
        AffectsRender<TwoTonePathIcon>(
            DataProperty, 
            StrokeBrushProperty, 
            ForegroundProperty,
            ActiveForegroundProperty,
            ActiveStrokeBrushProperty);
        IsActiveProperty.AffectsPseudoClass<TwoTonePathIcon>(PC_Active);
    }

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);
        PseudoClasses.Set(PC_Active, IsActive);
    }
}