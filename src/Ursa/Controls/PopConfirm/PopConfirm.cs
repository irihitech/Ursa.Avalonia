using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Input;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Presenters;
using Avalonia.Controls.Primitives;
using Avalonia.Controls.Templates;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.LogicalTree;
using Irihi.Avalonia.Shared.Helpers;

namespace Ursa.Controls;

[PseudoClasses(PC_DropdownOpen, PC_Icon)]
public class PopConfirm : ContentControl
{
    public const string PART_CloseButton = "PART_CloseButton";
    public const string PART_ConfirmButton = "PART_ConfirmButton";
    public const string PART_CancelButton = "PART_CancelButton";
    public const string PART_Popup = "PART_Popup";
    public const string PC_DropdownOpen = ":dropdownopen";
    public const string PC_Icon = ":icon";

    public static readonly StyledProperty<object?> PopupHeaderProperty = AvaloniaProperty.Register<PopConfirm, object?>(
        nameof(PopupHeader));

    public static readonly StyledProperty<IDataTemplate?> PopupHeaderTemplateProperty =
        AvaloniaProperty.Register<PopConfirm, IDataTemplate?>(
            nameof(PopupHeaderTemplate));

    public static readonly StyledProperty<object?> PopupContentProperty =
        AvaloniaProperty.Register<PopConfirm, object?>(
            nameof(PopupContent));

    public static readonly StyledProperty<IDataTemplate?> PopupContentTemplateProperty =
        AvaloniaProperty.Register<PopConfirm, IDataTemplate?>(
            nameof(PopupContentTemplate));

    public static readonly StyledProperty<ICommand?> ConfirmCommandProperty =
        AvaloniaProperty.Register<PopConfirm, ICommand?>(
            nameof(ConfirmCommand));

    public static readonly StyledProperty<ICommand?> CancelCommandProperty =
        AvaloniaProperty.Register<PopConfirm, ICommand?>(
            nameof(CancelCommand));

    public static readonly StyledProperty<object?> ConfirmCommandParameterProperty =
        AvaloniaProperty.Register<PopConfirm, object?>(
            nameof(ConfirmCommandParameter));

    public static readonly StyledProperty<object?> CancelCommandParameterProperty =
        AvaloniaProperty.Register<PopConfirm, object?>(
            nameof(CancelCommandParameter));

    public static readonly StyledProperty<PopConfirmTriggerMode> TriggerModeProperty =
        AvaloniaProperty.Register<PopConfirm, PopConfirmTriggerMode>(
            nameof(TriggerMode), PopConfirmTriggerMode.Click);

    public static readonly StyledProperty<bool> HandleAsyncCommandProperty =
        AvaloniaProperty.Register<PopConfirm, bool>(
            nameof(HandleAsyncCommand), true);

    public static readonly StyledProperty<bool> IsDropdownOpenProperty = AvaloniaProperty.Register<PopConfirm, bool>(
        nameof(IsDropdownOpen));

    public static readonly StyledProperty<PlacementMode> PlacementProperty =
        Popup.PlacementProperty.AddOwner<PopConfirm>(new StyledPropertyMetadata<PlacementMode>());

    public static readonly StyledProperty<object?> IconProperty = Banner.IconProperty.AddOwner<PopConfirm>();

    private Button? _closeButton;
    private Button? _cancelButton;


    private IDisposable? _childChangeDisposable;

    private Button? _confirmButton;
    private Popup? _popup;

    static PopConfirm()
    {
        IsDropdownOpenProperty.AffectsPseudoClass<PopConfirm>(PC_DropdownOpen);
        TriggerModeProperty.Changed.AddClassHandler<PopConfirm, PopConfirmTriggerMode>((pop, args) =>
            pop.OnTriggerModeChanged(args));
    }

    public object? Icon
    {
        get => GetValue(IconProperty);
        set => SetValue(IconProperty, value);
    }

    public object? PopupHeader
    {
        get => GetValue(PopupHeaderProperty);
        set => SetValue(PopupHeaderProperty, value);
    }

    public IDataTemplate? PopupHeaderTemplate
    {
        get => GetValue(PopupHeaderTemplateProperty);
        set => SetValue(PopupHeaderTemplateProperty, value);
    }

    public object? PopupContent
    {
        get => GetValue(PopupContentProperty);
        set => SetValue(PopupContentProperty, value);
    }

    public IDataTemplate? PopupContentTemplate
    {
        get => GetValue(PopupContentTemplateProperty);
        set => SetValue(PopupContentTemplateProperty, value);
    }

    public ICommand? ConfirmCommand
    {
        get => GetValue(ConfirmCommandProperty);
        set => SetValue(ConfirmCommandProperty, value);
    }

    public ICommand? CancelCommand
    {
        get => GetValue(CancelCommandProperty);
        set => SetValue(CancelCommandProperty, value);
    }

    public object? ConfirmCommandParameter
    {
        get => GetValue(ConfirmCommandParameterProperty);
        set => SetValue(ConfirmCommandParameterProperty, value);
    }

    public object? CancelCommandParameter
    {
        get => GetValue(CancelCommandParameterProperty);
        set => SetValue(CancelCommandParameterProperty, value);
    }

    public PopConfirmTriggerMode TriggerMode
    {
        get => GetValue(TriggerModeProperty);
        set => SetValue(TriggerModeProperty, value);
    }

    public bool HandleAsyncCommand
    {
        get => GetValue(HandleAsyncCommandProperty);
        set => SetValue(HandleAsyncCommandProperty, value);
    }

    public bool IsDropdownOpen
    {
        get => GetValue(IsDropdownOpenProperty);
        set => SetValue(IsDropdownOpenProperty, value);
    }

    public PlacementMode Placement
    {
        get => GetValue(PlacementProperty);
        set => SetValue(PlacementProperty, value);
    }

    private void OnTriggerModeChanged(AvaloniaPropertyChangedEventArgs<PopConfirmTriggerMode> args)
    {
        var child = Presenter?.Child;
        TeardownChildrenEventSubscriptions(child);
        SetupChildrenEventSubscriptions(child, args.GetNewValue<PopConfirmTriggerMode>());
    }

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);
        _closeButton = e.NameScope.Find<Button>(PART_CloseButton);
        _confirmButton = e.NameScope.Find<Button>(PART_ConfirmButton);
        _cancelButton = e.NameScope.Find<Button>(PART_CancelButton);
        _popup = e.NameScope.Find<Popup>(PART_Popup);
        Button.ClickEvent.AddHandler(OnButtonClicked, _closeButton, _cancelButton, _confirmButton);
    }

    private void OnButtonClicked(object? sender, RoutedEventArgs e)
    {
        if (!HandleAsyncCommand) _popup?.SetValue(Popup.IsOpenProperty, false);
        // This is a hack for MVVM toolkit and Prism that uses INotifyPropertyChanged for async command. It counts the number of
        // IsRunning property changes to determine when the command is finished.
        if (sender is Button { Command: { } command and (INotifyPropertyChanged or IDisposable) } button)
        {
            var count = 0;

            void OnCanExecuteChanged(object? _, System.EventArgs a)
            {
                count++;
                if (count < 2) return;
                var canExecute = command.CanExecute(button.CommandParameter);
                if (canExecute)
                {
                    _popup?.SetValue(Popup.IsOpenProperty, false);
                }
                command.CanExecuteChanged -= OnCanExecuteChanged;
            }

            command.CanExecuteChanged += OnCanExecuteChanged;
        }
        else
        {
            _popup?.SetValue(Popup.IsOpenProperty, false);
        }
    }

    protected override bool RegisterContentPresenter(ContentPresenter presenter)
    {
        var result = base.RegisterContentPresenter(presenter);
        if (result)
            _childChangeDisposable = presenter.GetPropertyChangedObservable(ContentPresenter.ChildProperty)
                                              .Subscribe(OnChildChanged);
        return result;
    }

    private void OnChildChanged(AvaloniaPropertyChangedEventArgs arg)
    {
        TeardownChildrenEventSubscriptions(arg.GetOldValue<Control?>());
        SetupChildrenEventSubscriptions(arg.GetNewValue<Control?>(), TriggerMode);
    }

    private void SetupChildrenEventSubscriptions(Control? child, PopConfirmTriggerMode mode)
    {
        if (child is null) return;
        if (mode.HasFlag(PopConfirmTriggerMode.Click))
        {
            if (child is Button button)
                Button.ClickEvent.AddHandler(OnMainButtonClicked, button);
            else
                PointerPressedEvent.AddHandler(OnMainElementPressed, child);
        }
        if (mode.HasFlag(PopConfirmTriggerMode.Focus))
        {
            GotFocusEvent.AddHandler(OnMainElementGotFocus, child);
            LostFocusEvent.AddHandler(OnMainElementLostFocus, child);
        }
    }

    private void OnMainElementLostFocus(object? sender, RoutedEventArgs e)
    {
        var newFocus = TopLevel.GetTopLevel(this)?.FocusManager?.GetFocusedElement();
        if (newFocus is Visual v && _popup?.IsInsidePopup(v) == true) return;
        SetCurrentValue(IsDropdownOpenProperty, false);
    }

    private bool _suppressButtonClickEvent;
    private void OnMainElementGotFocus(object? sender, GotFocusEventArgs e)
    {
        Debug.WriteLine("Got Focus");
        if (TriggerMode.HasFlag(PopConfirmTriggerMode.Click) && TriggerMode.HasFlag(PopConfirmTriggerMode.Focus))
        {
            _suppressButtonClickEvent = true;
        }
        SetCurrentValue(IsDropdownOpenProperty, true);
    }

    private void TeardownChildrenEventSubscriptions(Control? child)
    {
        if (child is null) return;
        PointerPressedEvent.RemoveHandler(OnMainElementPressed, child);
        Button.ClickEvent.RemoveHandler(OnMainButtonClicked, child);
        GotFocusEvent.RemoveHandler(OnMainElementGotFocus, child);
        LostFocusEvent.RemoveHandler(OnMainElementLostFocus, child);
    }

    private void OnMainButtonClicked(object? sender, RoutedEventArgs e)
    {
        Debug.WriteLine("Main Button Clicked");
        if (!_suppressButtonClickEvent)
        {
            SetCurrentValue(IsDropdownOpenProperty, !IsDropdownOpen);
        }
        _suppressButtonClickEvent = false;
    }


    private void OnMainElementPressed(object? sender, PointerPressedEventArgs e)
    {
        SetCurrentValue(IsDropdownOpenProperty, !IsDropdownOpen);
    }


    protected override void OnDetachedFromLogicalTree(LogicalTreeAttachmentEventArgs e)
    {
        _childChangeDisposable?.Dispose();
        base.OnDetachedFromLogicalTree(e);
    }
}