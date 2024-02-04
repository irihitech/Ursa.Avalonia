using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Primitives;
using Avalonia.Input;
using Avalonia.Input.GestureRecognizers;
using Avalonia.Interactivity;
using Avalonia.Threading;
using Ursa.Common;
using Ursa.EventArgs;

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

    internal HorizontalPosition HorizontalAnchor { get; set; } = HorizontalPosition.Center;
    internal VerticalPosition VerticalAnchor { get; set; } = VerticalPosition.Center;
    internal HorizontalPosition ActualHorizontalAnchor { get; set; }
    internal VerticalPosition ActualVerticalAnchor { get; set; }
    internal double? HorizontalOffset { get; set; }
    internal double? VerticalOffset { get; set; }
    internal double? HorizontalOffsetRatio { get; set; }
    internal double? VerticalOffsetRatio { get; set; }
    internal bool CanClickOnMaskToClose { get; set; }
    internal bool IsCloseButtonVisible { get; set; }

    public static readonly RoutedEvent<DialogLayerChangeEventArgs> LayerChangedEvent = RoutedEvent.Register<DialogControl, DialogLayerChangeEventArgs>(
        nameof(LayerChanged), RoutingStrategies.Bubble);
    public event EventHandler<DialogLayerChangeEventArgs> LayerChanged
    {
        add => AddHandler(LayerChangedEvent, value);
        remove => RemoveHandler(LayerChangedEvent, value);
    }
    
    public static readonly RoutedEvent<ResultEventArgs> ClosedEvent = RoutedEvent.Register<DrawerControlBase, ResultEventArgs>(
        nameof(Closed), RoutingStrategies.Bubble);
    
    public event EventHandler<ResultEventArgs> Closed
    {
        add => AddHandler(ClosedEvent, value);
        remove => RemoveHandler(ClosedEvent, value);
    }

    static DialogControl()
    { 
        DataContextProperty.Changed.AddClassHandler<DialogControl, object?>((o, e) => o.OnDataContextChange(e));
    }

    private void OnDataContextChange(AvaloniaPropertyChangedEventArgs<object?> args)
    {
        if (args.OldValue.Value is IDialogContext oldContext)
        {
            oldContext.RequestClose -= OnContextRequestClose;
        }
        if (args.NewValue.Value is IDialogContext newContext)
        {
            newContext.RequestClose += OnContextRequestClose;
        }
    }
    

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);
        EventHelper.UnregisterClickEvent(OnCloseButtonClick, _closeButton);

        _titleArea?.RemoveHandler(PointerMovedEvent, OnTitlePointerMove);
        _titleArea?.RemoveHandler(PointerPressedEvent, OnTitlePointerPressed);
        _titleArea?.RemoveHandler(PointerReleasedEvent, OnTitlePointerRelease);
        
        _closeButton = e.NameScope.Find<Button>(PART_CloseButton);
        _titleArea = e.NameScope.Find<Panel>(PART_TitleArea);
        if (_closeButton is not null)
        {
            _closeButton.IsVisible = IsCloseButtonVisible;
        }
        _titleArea?.AddHandler(PointerMovedEvent, OnTitlePointerMove, RoutingStrategies.Bubble);
        _titleArea?.AddHandler(PointerPressedEvent, OnTitlePointerPressed, RoutingStrategies.Bubble);
        _titleArea?.AddHandler(PointerReleasedEvent, OnTitlePointerRelease, RoutingStrategies.Bubble);
        EventHelper.RegisterClickEvent(OnCloseButtonClick, _closeButton);
    }
    
    private void OnTitlePointerPressed(object sender, PointerPressedEventArgs e)
    {
        e.Source = this;
    }

    private void OnTitlePointerMove(object sender, PointerEventArgs e)
    {
        e.Source = this;
    }

    private void OnTitlePointerRelease(object sender, PointerReleasedEventArgs e)
    {
        e.Source = this;
    }


    public Task<T?> ShowAsync<T>(CancellationToken? token = default)
    { 
        var tcs = new TaskCompletionSource<T?>();
        token?.Register(() =>
        {
            Dispatcher.UIThread.Invoke(CloseDialog);
        });
        
        void OnCloseHandler(object sender, ResultEventArgs? args)
        {
            if (args?.Result is T result)
            {
                tcs.SetResult(result);
            }
            else
            {
                tcs.SetResult(default);
            }
            RemoveHandler(ClosedEvent, OnCloseHandler);
        }

        AddHandler(ClosedEvent, OnCloseHandler);
        return tcs.Task;
    }

    private void OnCloseButtonClick(object sender, RoutedEventArgs args) => CloseDialog();

    private void OnContextRequestClose(object sender, object? args)
    {
        RaiseEvent(new ResultEventArgs(ClosedEvent, args));
    }
    

    public void UpdateLayer(object? o)
    {
        if (o is DialogLayerChangeType t)
        {
            RaiseEvent(new DialogLayerChangeEventArgs(LayerChangedEvent, t));
        }
    }
    
    /// <summary>
    /// Used for inherited classes to invoke the DialogControlClosing event.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="args"></param>
    protected virtual void OnDialogControlClosing(object sender, object? args)
    {
        RaiseEvent(new ResultEventArgs(ClosedEvent, args));
    }

    internal void SetAsModal(bool modal)
    {
        PseudoClasses.Set(PC_Modal, modal);
    }

    /// <summary>
    /// This method is exposed internally for closing the dialog from neither context nor closing by clicking on the close button.
    /// It is also exposed to be bound to context flyout.
    /// It is virtual because inherited classes may return a different result by default. 
    /// </summary>
    internal virtual void CloseDialog()
    {
        if (DataContext is IDialogContext context)
        {
            context.Close();
        }
        else
        {
            OnDialogControlClosing(this, null);
        }
    }
}