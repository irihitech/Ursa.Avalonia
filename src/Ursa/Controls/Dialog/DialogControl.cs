using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Primitives;
using Avalonia.Input;
using Avalonia.Interactivity;

namespace Ursa.Controls;

[TemplatePart(PART_CloseButton, typeof(Button))]
[TemplatePart(PART_TitleArea, typeof(Panel))]
[PseudoClasses(PC_ExtendClientArea)]
public class DialogControl: ContentControl
{
    public const string PART_CloseButton = "PART_CloseButton";
    public const string PART_TitleArea = "PART_TitleArea";
    public const string PC_ExtendClientArea = ":extend";
    public const string PC_Modal = ":modal";
    
    private Button? _closeButton;
    private Panel? _titleArea;
    public event EventHandler<object?>? OnClose;
    public event EventHandler<DialogLayerChangeEventArgs>? OnLayerChange;

    public static readonly StyledProperty<string?> TitleProperty = AvaloniaProperty.Register<DialogControl, string?>(
        nameof(Title));

    public string? Title
    {
        get => GetValue(TitleProperty);
        set => SetValue(TitleProperty, value);
    }

    public static readonly StyledProperty<bool> ExtendToClientAreaProperty = AvaloniaProperty.Register<DialogControl, bool>(
        nameof(ExtendToClientArea));

    public bool ExtendToClientArea
    {
        get => GetValue(ExtendToClientAreaProperty);
        set => SetValue(ExtendToClientAreaProperty, value);
    }

    static DialogControl()
    {
        DataContextProperty.Changed.AddClassHandler<DialogControl, object?>((o, e) => o.OnDataContextChange(e));
        ExtendToClientAreaProperty.Changed.AddClassHandler<DialogControl, bool>((o, e) => o.PseudoClasses.Set(PC_ExtendClientArea, e.NewValue.Value));
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
        PseudoClasses.Set(PC_Modal, true);
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

    public void UpdateLayer(object? o)
    {
        if (o is DialogLayerChangeType t)
        {
            OnLayerChange?.Invoke(this, new DialogLayerChangeEventArgs(t));
        }
    }
}