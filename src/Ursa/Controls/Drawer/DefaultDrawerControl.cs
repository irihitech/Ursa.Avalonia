using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Primitives;
using Avalonia.Interactivity;
using Ursa.Common;

namespace Ursa.Controls;

[TemplatePart(PART_YesButton, typeof(Button))]
[TemplatePart(PART_NoButton, typeof(Button))]
[TemplatePart(PART_OKButton, typeof(Button))]
[TemplatePart(PART_CancelButton, typeof(Button))]
public class DefaultDrawerControl: DrawerControlBase
{
    public const string PART_YesButton = "PART_YesButton";
    public const string PART_NoButton = "PART_NoButton";
    public const string PART_OKButton = "PART_OKButton";
    public const string PART_CancelButton = "PART_CancelButton";
    
    private Button? _yesButton;
    private Button? _noButton;
    private Button? _okButton;
    private Button? _cancelButton;
    
    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);
        EventHelper.UnregisterClickEvent(OnDefaultButtonClick, _yesButton, _noButton, _okButton, _cancelButton);
        _yesButton = e.NameScope.Find<Button>(PART_YesButton);
        _noButton = e.NameScope.Find<Button>(PART_NoButton);
        _okButton = e.NameScope.Find<Button>(PART_OKButton);
        _cancelButton = e.NameScope.Find<Button>(PART_CancelButton);
        EventHelper.RegisterClickEvent(OnDefaultButtonClick, _yesButton, _noButton, _okButton, _cancelButton);
    }
    
    private void OnDefaultButtonClick(object? sender, RoutedEventArgs e)
    {
    }
}