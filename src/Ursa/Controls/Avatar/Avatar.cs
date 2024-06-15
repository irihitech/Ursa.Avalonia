using Avalonia;
using Avalonia.Controls;

namespace Ursa.Controls;

public class Avatar : Button
{
    public const string PART_TopPresenter = "PART_TopPresenter";
    public const string PART_BottomPresenter = "PART_BottomPresenter";
    public const string PART_HoverMask = "PART_HoverMask";

    public static readonly StyledProperty<bool> ContentMotionProperty = AvaloniaProperty.Register<Avatar, bool>(
        nameof(ContentMotion));

    public static readonly StyledProperty<double> GapProperty = AvaloniaProperty.Register<Avatar, double>(
        nameof(Gap));

    public static readonly StyledProperty<string> SourceProperty = AvaloniaProperty.Register<Avatar, string>(
        nameof(Source));

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

    public string Source
    {
        get => GetValue(SourceProperty);
        set => SetValue(SourceProperty, value);
    }
}