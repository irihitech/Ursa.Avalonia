using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Primitives;
using Avalonia.Input;
using Avalonia.LogicalTree;

namespace Ursa.Controls;

[PseudoClasses(PC_Selected)]
[TemplatePart(PART_IconGlyph, typeof(Control))]
public class RatingCharacter : TemplatedControl
{
    public const string PART_IconGlyph = "PART_IconGlyph";
    protected const string PC_Selected = ":selected";

    private Control? _icon;

    private bool _isHalf;

    internal bool IsHalf
    {
        get => _isHalf;
        set
        {
            _isHalf = value;
            if (_icon is null) return;
            _icon.Width = value ? Bounds.Width * 0.5 : Bounds.Width;
        }
    }

    internal double Ratio { get; set; }

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
        var p = e.GetPosition(this);
        IsHalf = p.X < Bounds.Width * 0.5;
    }

    protected override void OnPointerReleased(PointerReleasedEventArgs e)
    {
        var parent = this.GetLogicalAncestors().OfType<Rating>().FirstOrDefault();
        parent?.PointerReleasedHandler(this);
    }

    public void Select(bool value)
    {
        PseudoClasses.Set(PC_Selected, value);
    }

    public void AdjustWidth()
    {
        if (_icon is not null)
        {
            _icon.Width = Bounds.Width * Ratio;
        }
    }
}