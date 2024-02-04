using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Primitives;
using Avalonia.Interactivity;
using Avalonia.Threading;
using Ursa.Common;
using Ursa.EventArgs;

namespace Ursa.Controls;

[TemplatePart(PART_CloseButton, typeof(Button))]
public abstract class DrawerControlBase: ContentControl
{
public const string PART_CloseButton = "PART_CloseButton";
    
    internal bool CanClickOnMaskToClose { get; set; }
    internal bool ShowCloseButton { get; set; }

    protected internal Button? _closeButton;
    
    public static readonly StyledProperty<Position> PositionProperty = AvaloniaProperty.Register<DrawerControlBase, Position>(
        nameof(Position));

    public Position Position
    {
        get => GetValue(PositionProperty);
        set => SetValue(PositionProperty, value);
    }
    
    public static readonly StyledProperty<bool> IsOpenProperty = AvaloniaProperty.Register<DrawerControlBase, bool>(
        nameof(IsOpen));

    public bool IsOpen
    {
        get => GetValue(IsOpenProperty);
        set => SetValue(IsOpenProperty, value);
    }

    public static readonly StyledProperty<bool> IsCloseButtonVisibleProperty =
        AvaloniaProperty.Register<DrawerControlBase, bool>(
            nameof(IsCloseButtonVisible));

    public bool IsCloseButtonVisible
    {
        get => GetValue(IsCloseButtonVisibleProperty);
        set => SetValue(IsCloseButtonVisibleProperty, value);
    }
    
    public static readonly RoutedEvent<ResultEventArgs> ClosedEvent = RoutedEvent.Register<DrawerControlBase, ResultEventArgs>(
        nameof(Closed), RoutingStrategies.Bubble);
    
    public event EventHandler<ResultEventArgs>? Closed
    {
        add => AddHandler(ClosedEvent, value);
        remove => RemoveHandler(ClosedEvent, value);
    }
    
    static DrawerControlBase()
    {
        DataContextProperty.Changed.AddClassHandler<DrawerControlBase, object?>((o, e) => o.OnDataContextChange(e));
    }

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);
        EventHelper.UnregisterClickEvent(OnCloseButtonClick, _closeButton);
        _closeButton = e.NameScope.Find<Button>(PART_CloseButton);
        EventHelper.RegisterClickEvent(OnCloseButtonClick, _closeButton);
    }

    private void OnDataContextChange(AvaloniaPropertyChangedEventArgs<object?> args)
    {
        if(args.OldValue.Value is IDialogContext oldContext)
        {
            oldContext.RequestClose -= OnContextRequestClose;
        }
        if(args.NewValue.Value is IDialogContext newContext)
        {
            newContext.RequestClose += OnContextRequestClose;
        }
    }

    private void OnContextRequestClose(object sender, object? e)
    {
        RaiseEvent(new ResultEventArgs(ClosedEvent, e));
    }

    private void OnCloseButtonClick(object sender, RoutedEventArgs e) => CloseDrawer();

    public Task<T?> ShowAsync<T>(CancellationToken? token = default)
    { 
        var tcs = new TaskCompletionSource<T?>();
        token?.Register(() =>
        {
            Dispatcher.UIThread.Invoke(CloseDrawer);
        });
        
        void OnCloseHandler(object sender, ResultEventArgs args)
        {
            if (args.Result is T result)
            {
                tcs.SetResult(result);
            }
            else
            {
                tcs.SetResult(default(T));
            }
            RemoveHandler(ClosedEvent, OnCloseHandler);
        }
        AddHandler(ClosedEvent, OnCloseHandler);
        return tcs.Task;
    }

    internal virtual void CloseDrawer()
    {
        if (DataContext is IDialogContext context)
        {
            context.Close();
        }
        else
        {
            RaiseEvent(new ResultEventArgs(ClosedEvent, null));
        }
    }
}