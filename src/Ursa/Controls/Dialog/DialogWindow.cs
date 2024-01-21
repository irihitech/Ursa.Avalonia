using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Primitives;

namespace Ursa.Controls;

[TemplatePart(PART_CloseButton, typeof(Button))]
public class DialogWindow: Window
{
    public const string PART_CloseButton = "PART_CloseButton";
    
    protected override Type StyleKeyOverride { get; } = typeof(DialogWindow);

    private Button? _closeButton;

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);
        _closeButton = e.NameScope.Find<Button>(PART_CloseButton);
        
        if (_closeButton != null)
        {
            _closeButton.Click += (sender, args) =>
            {
                Close();
            };
        }
        
    }
}