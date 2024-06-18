using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Presenters;
using Avalonia.Media;

namespace Ursa.Controls;

[TemplatePart(PART_HoverMask, typeof(ContentPresenter))]
public class Avatar : Button
{
    public const string PART_TopPresenter = "PART_TopPresenter";
    public const string PART_BottomPresenter = "PART_BottomPresenter";
    public const string PART_HoverMask = "PART_HoverMask";

    public static readonly StyledProperty<bool> ContentMotionProperty = AvaloniaProperty.Register<Avatar, bool>(
        nameof(ContentMotion));

    public static readonly StyledProperty<double> GapProperty = AvaloniaProperty.Register<Avatar, double>(
        nameof(Gap));

    public static readonly StyledProperty<IImage?> SourceProperty = AvaloniaProperty.Register<Avatar, IImage?>(
        nameof(Source));

    public static readonly StyledProperty<object?> HoverMaskProperty = AvaloniaProperty.Register<Avatar, object?>(
        nameof(HoverMask));

    public bool ContentMotion
    {
        get => GetValue(ContentMotionProperty);
        set => SetValue(ContentMotionProperty, value);
    }

    public double Gap
    {
        get => GetValue(GapProperty);
        set => SetValue(GapProperty, value);
    }

    public IImage? Source
    {
        get => GetValue(SourceProperty);
        set => SetValue(SourceProperty, value);
    }

    public object? HoverMask
    {
        get => GetValue(HoverMaskProperty);
        set => SetValue(HoverMaskProperty, value);
    }
}