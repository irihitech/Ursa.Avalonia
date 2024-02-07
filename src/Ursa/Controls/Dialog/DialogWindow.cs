using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Primitives;
using Avalonia.Input;
using Avalonia.Interactivity;
using Irihi.Avalonia.Shared.Helpers;
using Ursa.Common;

namespace Ursa.Controls;

[TemplatePart(PART_CloseButton, typeof(Button))]
[TemplatePart(PART_TitleArea, typeof(Panel))]
public class DialogWindow: Window
{
    public const string PART_CloseButton = "PART_CloseButton";
    public const string PART_TitleArea = "PART_TitleArea";
    protected override Type StyleKeyOverride { get; } = typeof(DialogWindow);

    protected internal Button? _closeButton;
    private Panel? _titleArea;
    
    internal bool IsCloseButtonVisible { get; set; }

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
        Button.ClickEvent.RemoveHandler(OnCloseButtonClicked, _closeButton);
        _titleArea?.RemoveHandler(PointerPressedEvent, OnTitlePointerPressed);
        _closeButton = e.NameScope.Find<Button>(PART_CloseButton);
        Button.IsVisibleProperty.SetValue(IsCloseButtonVisible, _closeButton);
        Button.ClickEvent.AddHandler(OnCloseButtonClicked, _closeButton);
        _titleArea = e.NameScope.Find<Panel>(PART_TitleArea);
        _titleArea?.AddHandler(PointerPressedEvent, OnTitlePointerPressed, RoutingStrategies.Bubble);

    }

    private void OnContextRequestClose(object? sender, object? args)
    {
        Close(args);
    }

    protected internal virtual void OnCloseButtonClicked(object sender, RoutedEventArgs args)
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