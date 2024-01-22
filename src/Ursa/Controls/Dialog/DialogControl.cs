using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Primitives;
using Avalonia.Interactivity;

namespace Ursa.Controls;

[TemplatePart(PART_CloseButton, typeof(Button))]
public class DialogControl: ContentControl
{
    public const string PART_CloseButton = "PART_CloseButton";
    
    
    
    private Button? _closeButton;
    public event EventHandler<object?> OnClose;

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);
        if (_closeButton != null)
        {
            _closeButton.Click -= Close;
        }
        _closeButton = e.NameScope.Find<Button>(PART_CloseButton);
        if (_closeButton is not null)
        {
            _closeButton.Click += Close;
        }
    }

    public void Show()
    {
        
    }

    private void Close(object sender, object args)
    {
        if (this.DataContext is IDialogContext context)
        {
            OnClose?.Invoke(this, context.DefaultCloseResult);
        }
        else
        {
            OnClose?.Invoke(this, null);
        }
    }
}