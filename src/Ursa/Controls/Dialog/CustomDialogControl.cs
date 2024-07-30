using Avalonia.Controls.Primitives;
using Irihi.Avalonia.Shared.Contracts;

namespace Ursa.Controls;

public class CustomDialogControl: DialogControlBase
{
    internal bool IsCloseButtonVisible { get; set; }
    
    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);
        if (_closeButton is not null)
        {
            _closeButton.IsVisible = IsCloseButtonVisible;
        }
    }

    public override void Close()
    {
        if (DataContext is IDialogContext context)
        {
            context.Close();
        }
        else
        {
            OnElementClosing(this, null);
        }
    }
}