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
public class DialogWindow: Window
{
    public const string PART_CloseButton = "PART_CloseButton";
    public const string PART_TitleArea = "PART_TitleArea";
    protected override Type StyleKeyOverride { get; } = typeof(DialogWindow);

    private Button? _closeButton;
    private Panel? _titleArea;

    static DialogWindow()
    {
        DataContextProperty.Changed.AddClassHandler<DialogWindow, object?>((o, e) => o.OnDataContextChange(e));
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
        EventHelper.UnregisterClickEvent(OnDefaultClose, _closeButton);
        _titleArea?.RemoveHandler(PointerPressedEvent, OnTitlePointerPressed);
        _closeButton = e.NameScope.Find<Button>(PART_CloseButton);
        _titleArea = e.NameScope.Find<Panel>(PART_TitleArea);
        _titleArea?.AddHandler(PointerPressedEvent, OnTitlePointerPressed, RoutingStrategies.Bubble);
        EventHelper.RegisterClickEvent(OnDefaultClose, _closeButton);
    }

    private void OnContextRequestClose(object? sender, object? args)
    {
        Close(args);
    }

    private void OnDefaultClose(object sender, RoutedEventArgs args)
    {
        if (DataContext is IDialogContext context)
        {
            context.Close();
        }
        else
        {
            Close(null);
        }
    }
    
    private void OnTitlePointerPressed(object sender, PointerPressedEventArgs e)
    {
        this.BeginMoveDrag(e);
    }
}