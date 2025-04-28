using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Primitives;
using Avalonia.Interactivity;
using Avalonia.Media;
using Avalonia.Styling;
using Ursa.Common;

namespace Ursa.Controls;

[TemplatePart(PART_BadgeContainer, typeof(Border))]
public class Badge : HeaderedContentControl
{
    public const string PART_ContentPresenter = "PART_ContentPresenter";
    public const string PART_BadgeContainer = "PART_BadgeContainer";
    public const string PART_HeaderPresenter = "PART_HeaderPresenter";

    private Border? _badgeContainer;

    public static readonly StyledProperty<ControlTheme> BadgeThemeProperty =
        AvaloniaProperty.Register<Badge, ControlTheme>(nameof(BadgeTheme));

    public ControlTheme BadgeTheme
    {
        get => GetValue(BadgeThemeProperty);
        set => SetValue(BadgeThemeProperty, value);
    }

    public static readonly StyledProperty<bool> DotProperty =
        AvaloniaProperty.Register<Badge, bool>(nameof(Dot));

    public bool Dot
    {
        get => GetValue(DotProperty);
        set => SetValue(DotProperty, value);
    }

    public static readonly StyledProperty<CornerPosition> CornerPositionProperty =
        AvaloniaProperty.Register<Badge, CornerPosition>(nameof(CornerPosition));

    public CornerPosition CornerPosition
    {
        get => GetValue(CornerPositionProperty);
        set => SetValue(CornerPositionProperty, value);
    }

    public static readonly StyledProperty<int> OverflowCountProperty =
        AvaloniaProperty.Register<Badge, int>(nameof(OverflowCount));

    public int OverflowCount
    {
        get => GetValue(OverflowCountProperty);
        set => SetValue(OverflowCountProperty, value);
    }

    public static readonly StyledProperty<double> BadgeFontSizeProperty =
        AvaloniaProperty.Register<Badge, double>(nameof(BadgeFontSize));

    public double BadgeFontSize
    {
        get => GetValue(BadgeFontSizeProperty);
        set => SetValue(BadgeFontSizeProperty, value);
    }

    static Badge()
    {
        CornerPositionProperty.Changed.AddClassHandler<Badge>((badge, _) => badge.UpdateBadgePosition());
        DotProperty.Changed.AddClassHandler<Badge>((badge, _) => badge.UpdateBadgePosition());
        AffectsArrange<Badge>(HeaderProperty, HeaderTemplateProperty);
    }

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);
        _badgeContainer = e.NameScope.Find<Border>(PART_BadgeContainer);
    }

    protected override void OnLoaded(RoutedEventArgs e)
    {
        base.OnLoaded(e);
        UpdateBadgePosition();
    }

    protected override Size ArrangeOverride(Size finalSize)
    {
        var size = base.ArrangeOverride(finalSize);
        UpdateBadgePosition();
        return size;
    }

    private void UpdateBadgePosition()
    {
        var vertical = CornerPosition is CornerPosition.BottomLeft or CornerPosition.BottomRight ? 1 : -1;
        var horizontal = CornerPosition is CornerPosition.TopRight or CornerPosition.BottomRight ? 1 : -1;
        if (_badgeContainer is not null && Presenter?.Child is not null)
        {
            _badgeContainer.RenderTransform = new TransformGroup
            {
                Children =
                [
                    new TranslateTransform(
                        horizontal * _badgeContainer.Bounds.Width / 2,
                        vertical * _badgeContainer.Bounds.Height / 2)
                ]
            };
        }
    }
}