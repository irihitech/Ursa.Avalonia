using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Primitives;
using Avalonia.Input;
using Avalonia.Interactivity;
using Ursa.Common;

namespace Ursa.Controls;

[TemplatePart(PART_CloseButton, typeof(Button))]
[TemplatePart(PART_OKButton, typeof(Button))]
[TemplatePart(PART_CancelButton, typeof(Button))]
[TemplatePart(PART_YesButton, typeof(Button))]
[TemplatePart(PART_NoButton, typeof(Button))]
[TemplatePart(PART_TitleArea, typeof(Panel))]
[PseudoClasses(PC_ExtendClientArea)]
public class DialogControl: ContentControl
{
    public const string PART_CloseButton = "PART_CloseButton";
    public const string PART_OKButton = "PART_OKButton";
    public const string PART_CancelButton = "PART_CancelButton";
    public const string PART_YesButton = "PART_YesButton";
    public const string PART_NoButton = "PART_NoButton";
    
    public const string PART_TitleArea = "PART_TitleArea";
    public const string PC_ExtendClientArea = ":extend";
    public const string PC_Modal = ":modal";
    
    private Button? _closeButton;
    private Button? _okButton;
    private Button? _cancelButton;
    private Button? _yesButton;
    private Button? _noButton;
    private Panel? _titleArea;
    public event EventHandler<object?>? OnDialogControlClose;
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

    internal DialogButton Buttons { get; set; }

    static DialogControl()
    {
        DataContextProperty.Changed.AddClassHandler<DialogControl, object?>((o, e) => o.OnDataContextChange(e));
        ExtendToClientAreaProperty.Changed.AddClassHandler<DialogControl, bool>((o, e) => o.PseudoClasses.Set(PC_ExtendClientArea, e.NewValue.Value));
    }

    private void OnDataContextChange(AvaloniaPropertyChangedEventArgs<object?> args)
    {
        if (args.OldValue.Value is IDialogContext oldContext)
        {
            oldContext.Closed-= CloseFromContext;
        }

        if (args.NewValue.Value is IDialogContext newContext)
        {
            newContext.Closed += CloseFromContext;
        }
    }
    

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);
        UnRegisterClickEvent(Close, _closeButton);
        UnRegisterClickEvent(DefaultButtonsClose, _okButton, _cancelButton, _yesButton, _noButton);
        _titleArea?.RemoveHandler(PointerMovedEvent, OnTitlePointerMove);
        _titleArea?.RemoveHandler(PointerPressedEvent, OnTitlePointerPressed);
        
        _closeButton = e.NameScope.Find<Button>(PART_CloseButton);
        _titleArea = e.NameScope.Find<Panel>(PART_TitleArea);
        _okButton = e.NameScope.Find<Button>(PART_OKButton);
        _cancelButton = e.NameScope.Find<Button>(PART_CancelButton);
        _yesButton = e.NameScope.Find<Button>(PART_YesButton);
        _noButton = e.NameScope.Find<Button>(PART_NoButton);
        
        _titleArea?.AddHandler(PointerMovedEvent, OnTitlePointerMove, RoutingStrategies.Bubble);
        _titleArea?.AddHandler(PointerPressedEvent, OnTitlePointerPressed, RoutingStrategies.Bubble);
        RegisterClickEvent(Close, _closeButton);
        RegisterClickEvent(DefaultButtonsClose, _yesButton, _noButton, _okButton, _cancelButton);
        SetButtonVisibility();
    }

    private void UnRegisterClickEvent(EventHandler<RoutedEventArgs> action, params Button?[] buttons)
    {
        foreach (var button in buttons)
        {
            if(button is not null) button.Click -= action;
        }
    }
    
    private void RegisterClickEvent(EventHandler<RoutedEventArgs> action, params Button?[] buttons)
    {
        foreach (var button in buttons)
        {
            if(button is not null) button.Click += action;
        }
    }

    private void SetButtonVisibility()
    {
        switch (Buttons)
        {
            case DialogButton.None:
                SetVisibility(_okButton, false);
                SetVisibility(_cancelButton, false);
                SetVisibility(_yesButton, false);
                SetVisibility(_noButton, false);
                break;
            case DialogButton.OK:
                SetVisibility(_okButton, true);
                SetVisibility(_cancelButton, false);
                SetVisibility(_yesButton, false);
                SetVisibility(_noButton, false);
                break;
            case DialogButton.OKCancel:
                SetVisibility(_okButton, true);
                SetVisibility(_cancelButton, true);
                SetVisibility(_yesButton, false);
                SetVisibility(_noButton, false);
                break;
            case DialogButton.YesNo:
                SetVisibility(_okButton, false);
                SetVisibility(_cancelButton, false);
                SetVisibility(_yesButton, true);
                SetVisibility(_noButton, true);
                break;
            case DialogButton.YesNoCancel:
                SetVisibility(_okButton, false);
                SetVisibility(_cancelButton, true);
                SetVisibility(_yesButton, true);
                SetVisibility(_noButton, true);
                break;
        }
    }

    private void SetVisibility(Button? button, bool visible)
    {
        if (button is not null) button.IsVisible = visible;
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
                OnDialogControlClose-= OnCloseHandler;
            }
            else
            {
                tcs.SetResult(default(T));
                OnDialogControlClose-= OnCloseHandler;
            }
        }

        this.OnDialogControlClose += OnCloseHandler;
        return tcs.Task;
    }
    
    private void Close(object sender, RoutedEventArgs args)
    {
        if (this.DataContext is IDialogContext context)
        {
            OnDialogControlClose?.Invoke(this, context.DefaultCloseResult);
        }
        
        else
        {
            OnDialogControlClose?.Invoke(this, null);
        }
    }

    private void CloseFromContext(object sender, object? args)
    {
        if (this.DataContext is IDialogContext context)
        {
            OnDialogControlClose?.Invoke(this, args);
        }
    }

    private void DefaultButtonsClose(object sender, RoutedEventArgs args)
    {
        if (sender is Button button)
        {
            if (button == _okButton)
            {
                OnDialogControlClose?.Invoke(this, DialogResult.OK);
            }
            else if (button == _cancelButton)
            {
                OnDialogControlClose?.Invoke(this, DialogResult.Cancel);
            }
            else if (button == _yesButton)
            {
                OnDialogControlClose?.Invoke(this, DialogResult.Yes);
            }
            else if (button == _noButton)
            {
                OnDialogControlClose?.Invoke(this, DialogResult.No);
            }
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