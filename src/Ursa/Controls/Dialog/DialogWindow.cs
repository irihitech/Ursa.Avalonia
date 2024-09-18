using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Primitives;
using Avalonia.Input;
using Avalonia.Interactivity;
using Irihi.Avalonia.Shared.Contracts;
using Irihi.Avalonia.Shared.Helpers;

namespace Ursa.Controls;

[TemplatePart(PART_CloseButton, typeof(Button))]
[TemplatePart(PART_TitleArea, typeof(Panel))]
public class DialogWindow : Window
{
    public const string PART_CloseButton = "PART_CloseButton";
    public const string PART_TitleArea = "PART_TitleArea";

    protected internal Button? _closeButton;

    private Panel? _titleArea;

    public static readonly StyledProperty<bool> IsManagedResizerVisibleProperty = AvaloniaProperty.Register<DialogWindow, bool>(
        nameof(IsManagedResizerVisible));

    public bool IsManagedResizerVisible
    {
        get => GetValue(IsManagedResizerVisibleProperty);
        set => SetValue(IsManagedResizerVisibleProperty, value);
    }

    static DialogWindow()
    {
        DataContextProperty.Changed.AddClassHandler<DialogWindow, object?>((window, e) =>
            window.OnDataContextChange(e));
    }

    public bool CanDragMove { get; set; } = true;
    internal bool? IsCloseButtonVisible { get; set; }

    protected override Type StyleKeyOverride { get; } = typeof(DialogWindow);


    private void OnDataContextChange(AvaloniaPropertyChangedEventArgs<object?> args)
    {
        if (args.OldValue.Value is IDialogContext oldContext) oldContext.RequestClose -= OnContextRequestClose;

        if (args.NewValue.Value is IDialogContext newContext) newContext.RequestClose += OnContextRequestClose;
    }

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);
        Button.ClickEvent.RemoveHandler(OnCloseButtonClicked, _closeButton);
        _titleArea?.RemoveHandler(PointerPressedEvent, OnTitlePointerPressed);
        _closeButton = e.NameScope.Find<Button>(PART_CloseButton);
        IsVisibleProperty.SetValue(IsCloseButtonVisible ?? true, _closeButton);
        Button.ClickEvent.AddHandler(OnCloseButtonClicked, _closeButton);
        _titleArea = e.NameScope.Find<Panel>(PART_TitleArea);
        IsHitTestVisibleProperty.SetValue(CanDragMove, _titleArea);
        _titleArea?.AddHandler(PointerPressedEvent, OnTitlePointerPressed, RoutingStrategies.Bubble);
    }

    private void OnContextRequestClose(object? sender, object? args)
    {
        Close(args);
    }

    protected virtual void OnCloseButtonClicked(object? sender, RoutedEventArgs args)
    {
        if (DataContext is IDialogContext context)
            context.Close();
        else
            Close(null);
    }

    private void OnTitlePointerPressed(object? sender, PointerPressedEventArgs e)
    {
        if (CanDragMove)
            BeginMoveDrag(e);
    }
}