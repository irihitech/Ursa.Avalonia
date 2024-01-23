using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Primitives;
using Avalonia.Input;
using Avalonia.Interactivity;

namespace Ursa.Controls;

[TemplatePart(PART_CloseButton, typeof(Button))]
[TemplatePart(PART_TitleArea, typeof(Panel))]
public class DialogControl: ContentControl
{
    public const string PART_CloseButton = "PART_CloseButton";
    public const string PART_TitleArea = "PART_TitleArea";
    
    private Button? _closeButton;
    private Panel? _titleArea;
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
        _titleArea?.RemoveHandler(PointerMovedEvent, OnTitlePointerMove);
        _titleArea?.RemoveHandler(PointerPressedEvent, OnTitlePointerPressed);
        
        _closeButton = e.NameScope.Find<Button>(PART_CloseButton);
        _titleArea = e.NameScope.Find<Panel>(PART_TitleArea);
        if (_closeButton is not null)
        {
            _closeButton.Click += Close;
        }
        _titleArea?.AddHandler(PointerMovedEvent, OnTitlePointerMove, RoutingStrategies.Bubble);
        _titleArea?.AddHandler(PointerPressedEvent, OnTitlePointerPressed, RoutingStrategies.Bubble);
        
    }

    private void OnTitlePointerPressed(object sender, PointerPressedEventArgs e)
    {
        e.Source = this;
    }

    private void OnTitlePointerMove(object sender, PointerEventArgs e)
    {
        e.Source = this;
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