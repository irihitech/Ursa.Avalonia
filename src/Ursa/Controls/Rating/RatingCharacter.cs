using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Primitives;
using Avalonia.Input;
using Avalonia.LogicalTree;

namespace Ursa.Controls;

[PseudoClasses(PC_Selected)]
[TemplatePart(PART_IconGlyph, typeof(PathIcon))]
public class RatingCharacter : ContentControl
{
    public const string PART_IconGlyph = "PART_IconGlyph";
    protected const string PC_Selected = ":selected";

    private PathIcon? _icon;

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);
        _icon = e.NameScope.Find<PathIcon>(PART_IconGlyph);
    }

    protected override void OnPointerEntered(PointerEventArgs e)
    {
        var parent = this.GetLogicalAncestors().OfType<Rating>().FirstOrDefault();
        parent?.Preview(this);
    }

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