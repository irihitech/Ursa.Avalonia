using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Primitives;
using Avalonia.Media;
using Avalonia.Styling;
using Ursa.Common;

namespace Ursa.Controls;

/// <summary>
/// Represents a Badge control that can display a notification indicator or count on top of other content.
/// </summary>
[TemplatePart(PART_BadgeContainer, typeof(Control))]
public class Badge : HeaderedContentControl
{
    public const string PART_ContentPresenter = "PART_ContentPresenter";
    public const string PART_BadgeContainer = "PART_BadgeContainer";
    public const string PART_HeaderPresenter = "PART_HeaderPresenter";

    /// <summary>
    /// Defines the theme applied to the badge.
    /// </summary>
    public static readonly StyledProperty<ControlTheme> BadgeThemeProperty =
        AvaloniaProperty.Register<Badge, ControlTheme>(nameof(BadgeTheme));

    /// <summary>
    /// Defines whether the badge should be displayed as a dot.
    /// </summary>
    public static readonly StyledProperty<bool> DotProperty =
        AvaloniaProperty.Register<Badge, bool>(nameof(Dot));

    /// <summary>
    /// Defines the corner position where the badge should be displayed.
    /// </summary>
    public static readonly StyledProperty<CornerPosition> CornerPositionProperty =
        AvaloniaProperty.Register<Badge, CornerPosition>(nameof(CornerPosition));

    /// <summary>
    /// Defines the maximum count to display before showing overflow indicator.
    /// </summary>
    public static readonly StyledProperty<int> OverflowCountProperty =
        AvaloniaProperty.Register<Badge, int>(nameof(OverflowCount));

    /// <summary>
    /// Defines the font size of the badge text.
    /// </summary>
    public static readonly StyledProperty<double> BadgeFontSizeProperty =
        AvaloniaProperty.Register<Badge, double>(nameof(BadgeFontSize));

    private Control? _badgeContainer;

    static Badge()
    {
        CornerPositionProperty.Changed.AddClassHandler<Badge>((badge, _) => badge.UpdateBadgePosition());
        DotProperty.Changed.AddClassHandler<Badge>((badge, _) => badge.UpdateBadgePosition());
    }

    /// <summary>
    /// Gets or sets the theme applied to the badge.
    /// </summary>
    public ControlTheme BadgeTheme
    {
        get => GetValue(BadgeThemeProperty);
        set => SetValue(BadgeThemeProperty, value);
    }

    /// <summary>
    /// Gets or sets a value indicating whether the badge should be displayed as a dot.
    /// </summary>
    public bool Dot
    {
        get => GetValue(DotProperty);
        set => SetValue(DotProperty, value);
    }

    /// <summary>
    /// Gets or sets the corner position where the badge should be displayed.
    /// </summary>
    public CornerPosition CornerPosition
    {
        get => GetValue(CornerPositionProperty);
        set => SetValue(CornerPositionProperty, value);
    }

    /// <summary>
    /// Gets or sets the maximum count to display before showing overflow indicator.
    /// </summary>
    public int OverflowCount
    {
        get => GetValue(OverflowCountProperty);
        set => SetValue(OverflowCountProperty, value);
    }

    /// <summary>
    /// Gets or sets the font size of the badge text.
    /// </summary>
    public double BadgeFontSize
    {
        get => GetValue(BadgeFontSizeProperty);
        set => SetValue(BadgeFontSizeProperty, value);
    }

    /// <inheritdoc/>
    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        _badgeContainer?.RemoveHandler(SizeChangedEvent, OnBadgeSizeChanged);
        base.OnApplyTemplate(e);
        _badgeContainer = e.NameScope.Find<Control>(PART_BadgeContainer);
        _badgeContainer?.AddHandler(SizeChangedEvent, OnBadgeSizeChanged);
    }

    /// <summary>
    /// Handles the size changed event of the badge container and updates its position.
    /// </summary>
    private void OnBadgeSizeChanged(object? sender, SizeChangedEventArgs e)
    {
        UpdateBadgePosition();
    }

    /// <summary>
    /// Updates the badge position based on the current corner position and size.
    /// </summary>
    private void UpdateBadgePosition()
    {
        var vertical = CornerPosition is CornerPosition.BottomLeft or CornerPosition.BottomRight ? 1 : -1;
        var horizontal = CornerPosition is CornerPosition.TopRight or CornerPosition.BottomRight ? 1 : -1;
        if (_badgeContainer is not null && Presenter?.Child is not null)
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