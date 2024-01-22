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

        if (this.DataContext is IDialogContext context)
        {
            context.Closed += Close;
        }
    }
    

    public Task<T> ShowAsync<T>()
    {
        var tcs = new TaskCompletionSource<T>();
        this.OnClose+= (sender, args) =>
        {
            if (args is T result)
            {
                tcs.SetResult(result);
            }
            else
            {
                tcs.SetResult(default);
            }
        };
        return tcs.Task;
    }

    private void Close(object sender, object args)
    {
        if (this.DataContext is IDialogContext context)
        {
            OnClose?.Invoke(this, args);
        }
        else
        {
            OnClose?.Invoke(this, null);
        }
    }
}