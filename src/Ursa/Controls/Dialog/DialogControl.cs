using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Primitives;
using Avalonia.Input;
using Avalonia.Interactivity;
using Ursa.Common;

namespace Ursa.Controls;

[TemplatePart(PART_CloseButton, typeof(Button))]
[TemplatePart(PART_TitleArea, typeof(Panel))]
[PseudoClasses(PC_Modal)]
public class DialogControl: ContentControl
{
    public const string PART_CloseButton = "PART_CloseButton";
    public const string PART_TitleArea = "PART_TitleArea";
    public const string PC_Modal = ":modal";
    
    protected internal Button? _closeButton;
    private Panel? _titleArea;
    
    internal HorizontalPosition HorizontalAnchor { get; set; }
    internal VerticalPosition VerticalAnchor { get; set; }
    internal double? InitialHorizontalOffset { get; set; }
    internal double? InitialVerticalOffset { get; set; }
    internal bool CanClickOnMaskToClose { get; set; }
    
    public event EventHandler<DialogLayerChangeEventArgs>? LayerChanged;
    public event EventHandler<object?>? DialogControlClosing;

    static DialogControl()
    { 
        DataContextProperty.Changed.AddClassHandler<DialogControl, object?>((o, e) => o.OnDataContextChange(e));
    }

    private void OnDataContextChange(AvaloniaPropertyChangedEventArgs<object?> args)
    {
        if (args.OldValue.Value is IDialogContext oldContext)
        {
            oldContext.RequestClose-= OnContextRequestClose;
        }

        if (args.NewValue.Value is IDialogContext newContext)
        {
            newContext.RequestClose += OnContextRequestClose;
        }
    }
    

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);
        EventHelper.UnregisterClickEvent(Close, _closeButton);

        _titleArea?.RemoveHandler(PointerMovedEvent, OnTitlePointerMove);
        _titleArea?.RemoveHandler(PointerPressedEvent, OnTitlePointerPressed);
        
        _closeButton = e.NameScope.Find<Button>(PART_CloseButton);
        _titleArea = e.NameScope.Find<Panel>(PART_TitleArea);

        
        _titleArea?.AddHandler(PointerMovedEvent, OnTitlePointerMove, RoutingStrategies.Bubble);
        _titleArea?.AddHandler(PointerPressedEvent, OnTitlePointerPressed, RoutingStrategies.Bubble);
        EventHelper.RegisterClickEvent(Close, _closeButton);
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
                DialogControlClosing-= OnCloseHandler;
            }
            else
            {
                tcs.SetResult(default(T));
                DialogControlClosing-= OnCloseHandler;
            }
        }

        this.DialogControlClosing += OnCloseHandler;
        return tcs.Task;
    }
    
    private void Close(object sender, RoutedEventArgs args)
    {
        if (this.DataContext is IDialogContext context)
        {
            context.Close();
        }
        else
        {
            DialogControlClosing?.Invoke(this, DialogResult.None);
        }
    }

    private void OnContextRequestClose(object sender, object? args)
    {
        if (this.DataContext is IDialogContext context)
        {
            DialogControlClosing?.Invoke(this, args);
        }
    }
    

    public void UpdateLayer(object? o)
    {
        if (o is DialogLayerChangeType t)
        {
            LayerChanged?.Invoke(this, new DialogLayerChangeEventArgs(t));
        }
    }
    
    protected virtual void OnDialogControlClosing(object sender, object? args)
    {
        DialogControlClosing?.Invoke(this, args);
    }

    internal void SetAsModal(bool modal)
    {
        PseudoClasses.Set(PC_Modal, modal);
    }

    public void Close()
    {
        if (this.DataContext is IDialogContext context)
        {
            context.Close();
        }
        else
        {
            DialogControlClosing?.Invoke(this, DialogResult.None);
        }
    }
}