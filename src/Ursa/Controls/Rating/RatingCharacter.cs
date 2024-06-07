using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Primitives;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.LogicalTree;

namespace Ursa.Controls;

[PseudoClasses(PC_Selected)]
[TemplatePart(PART_IconBorder, typeof(Border))]
public class RatingCharacter : TemplatedControl
{
    public const string PART_IconBorder = "PART_IconBorder";
    protected const string PC_Selected = ":selected";

    private Border? _icon;

    public static readonly StyledProperty<bool> AllowHalfProperty =
        Rating.AllowHalfProperty.AddOwner<RatingCharacter>();

    public static readonly StyledProperty<object> CharacterProperty =
        Rating.CharacterProperty.AddOwner<RatingCharacter>();

    public static readonly StyledProperty<double> SizeProperty =
        Rating.SizeProperty.AddOwner<RatingCharacter>();

    public bool AllowHalf
    {
        get => GetValue(AllowHalfProperty);
        set => SetValue(AllowHalfProperty, value);
    }

    public object Character
    {
        get => GetValue(CharacterProperty);
        set => SetValue(CharacterProperty, value);
    }

    public double Size
    {
        get => GetValue(SizeProperty);
        set => SetValue(SizeProperty, value);
    }

    internal bool IsLast { get; set; }

    private bool _isHalf;

    internal bool IsHalf
    {
        get => _isHalf;
        set
        {
            if (!AllowHalf) return;
            _isHalf = value;
            if (_icon is null) return;
            _icon.Width = value ? Bounds.Width * 0.5 : Bounds.Width;
        }
    }

    internal double Ratio { get; set; } = 1;

    protected override void OnLoaded(RoutedEventArgs e)
    {
        base.OnLoaded(e);
        ApplyRatio();
    }

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);
        _icon = e.NameScope.Find<Border>(PART_IconBorder);
    }

    protected override void OnPointerEntered(PointerEventArgs e)
    {
        var parent = this.GetLogicalAncestors().OfType<Rating>().FirstOrDefault();
        parent?.PointerEnteredHandler(this);
    }

    protected override void OnPointerMoved(PointerEventArgs e)
    {
        if (!AllowHalf) return;
        var p = e.GetPosition(this);
        IsHalf = p.X < Bounds.Width * 0.5;
    }

    protected override void OnPointerExited(PointerEventArgs e)
    {
        var parent = this.GetLogicalAncestors().OfType<Rating>().FirstOrDefault();
        parent?.UpdateItemsByValue(parent.Value);
    }

    protected override void OnPointerReleased(PointerReleasedEventArgs e)
    {
        var parent = this.GetLogicalAncestors().OfType<Rating>().FirstOrDefault();
        parent?.PointerReleasedHandler(this);
    }

    internal void SetSelectedState(bool value)
    {
        PseudoClasses.Set(PC_Selected, value);
    }

    internal void ApplyRatio()
    {
        if (_icon is not null)
        {
            _icon.Width = Bounds.Width * Ratio;
        }
    }
}