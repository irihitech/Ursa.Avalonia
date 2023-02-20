using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Presenters;
using Avalonia.Controls.Primitives;
using Avalonia.Media;
using Avalonia.Styling;
using Ursa.Common;

namespace Ursa.Controls;

[TemplatePart(PART_ContentPresenter, typeof(ContentPresenter))]
[TemplatePart(PART_BadgeContainer, typeof(Border))]
[TemplatePart(PART_BadgeContentPresenter, typeof(ContentPresenter))]
public class Badge: ContentControl
{
    public const string PART_ContentPresenter = "PART_ContentPresenter";
    public const string PART_BadgeContainer = "PART_BadgeContainer";
    public const string PART_BadgeContentPresenter = "PART_BadgeContentPresenter";
    
    private ContentPresenter? _content;
    private Border? _badgeContainer;
    private ContentPresenter? _badgeContent;

    public static readonly StyledProperty<ControlTheme> BadgeThemeProperty = AvaloniaProperty.Register<Badge, ControlTheme>(
        nameof(BadgeTheme));
    public ControlTheme BadgeTheme
    {
        get => GetValue(BadgeThemeProperty);
        set => SetValue(BadgeThemeProperty, value);
    }

    public static readonly StyledProperty<bool> DotProperty = AvaloniaProperty.Register<Badge, bool>(
        nameof(Dot));
    public bool Dot
    {
        get => GetValue(DotProperty);
        set => SetValue(DotProperty, value);
    }

    public static readonly StyledProperty<object?> BadgeContentProperty = AvaloniaProperty.Register<Badge, object?>(
        nameof(BadgeContent));
    public object? BadgeContent
    {
        get => GetValue(BadgeContentProperty);
        set => SetValue(BadgeContentProperty, value);
    }

    public static readonly StyledProperty<CornerPosition> CornerPositionProperty = AvaloniaProperty.Register<Badge, CornerPosition>(
        nameof(CornerPosition));
    public CornerPosition CornerPosition
    {
        get => GetValue(CornerPositionProperty);
        set => SetValue(CornerPositionProperty, value);
    }

    public static readonly StyledProperty<int> OverflowCountProperty = AvaloniaProperty.Register<Badge, int>(
        nameof(OverflowCount));
    public int OverflowCount
    {
        get => GetValue(OverflowCountProperty);
        set => SetValue(OverflowCountProperty, value);
    }

    static Badge()
    {
        BadgeContentProperty.Changed.AddClassHandler<Badge>((badge, args) => badge.UpdateBadgePosition());
    }

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);
        _content = e.NameScope.Find<ContentPresenter>(PART_ContentPresenter);
        _badgeContainer = e.NameScope.Find<Border>(PART_BadgeContainer);
        _badgeContent = e.NameScope.Find<ContentPresenter>(PART_BadgeContentPresenter);
    }

    protected override void OnLoaded()
    {
        base.OnLoaded();
        UpdateBadgePosition();
    }

    protected override Size ArrangeOverride(Size finalSize)
    {
        UpdateBadgePosition();
        return base.ArrangeOverride(finalSize);
    }

    private void UpdateBadgePosition()
    {
        var vertical = CornerPosition is CornerPosition.BottomLeft or CornerPosition.BottomRight ? 1 : -1;
        var horizontal = CornerPosition is CornerPosition.TopRight or CornerPosition.BottomRight ? 1 : -1;
        if (_badgeContainer is not null && _content?.Child is not null)
        {
            _badgeContainer.RenderTransform = new TransformGroup()
            {
                Children = new Transforms()
                {
                    new TranslateTransform(horizontal*_badgeContainer.Bounds.Width/2,vertical*_badgeContainer.Bounds.Height/2)
                }
            };
        }
    }
}