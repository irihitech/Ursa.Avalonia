using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Primitives;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.LogicalTree;

namespace Ursa.Controls;

[PseudoClasses(PC_Selected)]
[TemplatePart(PART_IconGlyph, typeof(Control))]
public class RatingCharacter : TemplatedControl
{
    public const string PART_IconGlyph = "PART_IconGlyph";
    protected const string PC_Selected = ":selected";

    private Control? _icon;

    public static readonly StyledProperty<bool> AllowHalfProperty = AvaloniaProperty.Register<RatingCharacter, bool>(
        nameof(AllowHalf));


    public bool AllowHalf
    {
        get => GetValue(AllowHalfProperty);
        set => SetValue(AllowHalfProperty, value);
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

    internal double Ratio { get; set; }

    protected override void OnLoaded(RoutedEventArgs e)
    {
        base.OnLoaded(e);
        AdjustWidth();
    }

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);
        _icon = e.NameScope.Find<Control>(PART_IconGlyph);
    }

    protected override void OnPointerEntered(PointerEventArgs e)
    {
        var parent = this.GetLogicalAncestors().OfType<Rating>().FirstOrDefault();
        parent?.Preview(this);
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
        parent?.AdjustWidth(parent.Value);
    }

    protected override void OnPointerReleased(PointerReleasedEventArgs e)
    {
        var parent = this.GetLogicalAncestors().OfType<Rating>().FirstOrDefault();
        parent?.PointerReleasedHandler(this);
    }

    internal void Select(bool value)
    {
        PseudoClasses.Set(PC_Selected, value);
    }

    internal void AdjustWidth()
    {
        if (_icon is not null)
        {
            _icon.Width = Bounds.Width * Ratio;
        }
    }
}