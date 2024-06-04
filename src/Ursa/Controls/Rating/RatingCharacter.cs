using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Primitives;
using Avalonia.Input;
using Avalonia.LogicalTree;

namespace Ursa.Controls;

[PseudoClasses(PC_Selected, PC_Half)]
[TemplatePart(PART_IconGlyph, typeof(Control))]
public class RatingCharacter : TemplatedControl
{
    public const string PART_IconGlyph = "PART_IconGlyph";
    protected const string PC_Selected = ":selected";
    protected const string PC_Half = ":half";

    private Control? _icon;

    private bool _isHalf;

    internal bool IsHalf
    {
        get => _isHalf;
        set
        {
            if (_isHalf == value) return;
            _isHalf = value;
            if (_icon is null) return;
            _icon.Width = value ? Bounds.Width / 2 : Bounds.Width;
        }
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
        var p = e.GetPosition(this);
        var flag = p.X < Bounds.Width / 2;
        PseudoClasses.Set(PC_Half, flag);
        IsHalf = flag;
        // if (flag)
        // {
        //     _icon.Width = Bounds.Width / 2;
        // }
        // else
        // {
        //     _icon.Width = Bounds.Width;
        // }
    }

    // protected override void OnPointerExited(PointerEventArgs e)
    // {
    //     // _icon.Width = Bounds.Width;
    // }

    protected override void OnPointerReleased(PointerReleasedEventArgs e)
    {
        var parent = this.GetLogicalAncestors().OfType<Rating>().FirstOrDefault();
        parent?.Select(this);
    }

    public void Select(bool value)
    {
        PseudoClasses.Set(PC_Selected, value);
    }
}