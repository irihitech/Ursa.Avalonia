using Avalonia;
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
    public event EventHandler<object?>? OnClose;

    static DialogControl()
    {
        DataContextProperty.Changed.AddClassHandler<DialogControl, object?>((o, e) => o.OnDataContextChange(e));
    }

    private void OnDataContextChange(AvaloniaPropertyChangedEventArgs<object?> args)
    {
        if (args.OldValue.Value is IDialogContext oldContext)
        {
            oldContext.Closed-= Close;
        }

        if (args.NewValue.Value is IDialogContext newContext)
        {
            newContext.Closed += Close;
        }
        
    }

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
    

    public Task<T> ShowAsync<T>()
    {
        var tcs = new TaskCompletionSource<T>();

        void OnCloseHandler(object sender, object? args)
        {
            if (args is T result)
            {
                tcs.SetResult(result);
                OnClose-= OnCloseHandler;
            }
            else
            {
                tcs.SetResult(default(T));
                OnClose-= OnCloseHandler;
            }
        }

        this.OnClose += OnCloseHandler;
        return tcs.Task;
    }

    private void Close(object sender, object args)
    {
        if (this.DataContext is IDialogContext context)
        {
            OnClose?.Invoke(this, Equals(sender, _closeButton) ? context.DefaultCloseResult : args);
        }
        
        else
        {
            OnClose?.Invoke(this, null);
        }
    }
}