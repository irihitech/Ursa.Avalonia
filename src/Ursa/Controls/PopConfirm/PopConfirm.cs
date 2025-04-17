using System.ComponentModel;
using System.Windows.Input;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Presenters;
using Avalonia.Controls.Primitives;
using Avalonia.Controls.Templates;
using Avalonia.Interactivity;
using Avalonia.LogicalTree;
using Irihi.Avalonia.Shared.Helpers;

namespace Ursa.Controls;

public class PopConfirm: ContentControl
{
    public const string PART_ConfirmButton = "PART_ConfirmButton";
    public const string PART_CancelButton = "PART_CancelButton";
    public const string PART_Popup = "PART_Popup";
    
    private Button? _confirmButton;
    private Button? _cancelButton;
    private Popup? _popup;
    
    public static readonly StyledProperty<object?> PopupHeaderProperty = AvaloniaProperty.Register<PopConfirm, object?>(
        nameof(PopupHeader));

    public object? PopupHeader
    {
        get => GetValue(PopupHeaderProperty);
        set => SetValue(PopupHeaderProperty, value);
    }

    public static readonly StyledProperty<IDataTemplate?> PopupHeaderTemplateProperty = AvaloniaProperty.Register<PopConfirm, IDataTemplate?>(
        nameof(PopupHeaderTemplate));

    public IDataTemplate? PopupHeaderTemplate
    {
        get => GetValue(PopupHeaderTemplateProperty);
        set => SetValue(PopupHeaderTemplateProperty, value);
    }

    public static readonly StyledProperty<object?> PopupContentProperty = AvaloniaProperty.Register<PopConfirm, object?>(
        nameof(PopupContent));

    public object? PopupContent
    {
        get => GetValue(PopupContentProperty);
        set => SetValue(PopupContentProperty, value);
    }

    public static readonly StyledProperty<IDataTemplate?> PopupContentTemplateProperty = AvaloniaProperty.Register<PopConfirm, IDataTemplate?>(
        nameof(PopupContentTemplate));

    public IDataTemplate? PopupContentTemplate
    {
        get => GetValue(PopupContentTemplateProperty);
        set => SetValue(PopupContentTemplateProperty, value);
    }

    public static readonly StyledProperty<ICommand?> ConfirmCommandProperty = AvaloniaProperty.Register<PopConfirm, ICommand?>(
        nameof(ConfirmCommand));

    public ICommand? ConfirmCommand
    {
        get => GetValue(ConfirmCommandProperty);
        set => SetValue(ConfirmCommandProperty, value);
    }

    public static readonly StyledProperty<ICommand?> CancelCommandProperty = AvaloniaProperty.Register<PopConfirm, ICommand?>(
        nameof(CancelCommand));

    public ICommand? CancelCommand
    {
        get => GetValue(CancelCommandProperty);
        set => SetValue(CancelCommandProperty, value);
    }

    public static readonly StyledProperty<object?> ConfirmCommandParameterProperty = AvaloniaProperty.Register<PopConfirm, object?>(
        nameof(ConfirmCommandParameter));

    public object? ConfirmCommandParameter
    {
        get => GetValue(ConfirmCommandParameterProperty);
        set => SetValue(ConfirmCommandParameterProperty, value);
    }

    public static readonly StyledProperty<object?> CancelCommandParameterProperty = AvaloniaProperty.Register<PopConfirm, object?>(
        nameof(CancelCommandParameter));

    public object? CancelCommandParameter
    {
        get => GetValue(CancelCommandParameterProperty);
        set => SetValue(CancelCommandParameterProperty, value);
    }

    public static readonly StyledProperty<PopConfirmTriggerMode> TriggerModeProperty =
        AvaloniaProperty.Register<PopConfirm, PopConfirmTriggerMode>(
            nameof(TriggerMode));

    public PopConfirmTriggerMode TriggerMode
    {
        get => GetValue(TriggerModeProperty);
        set => SetValue(TriggerModeProperty, value);
    }

    static PopConfirm()
    {
        ConfirmCommandProperty.Changed.AddClassHandler<PopConfirm, ICommand?>((popconfirm, args) => popconfirm.OnCommandChanged(args));
    }

    private void OnCommandChanged(AvaloniaPropertyChangedEventArgs<ICommand?> args)
    {
        
    }

    public PopConfirm()
    {
        
    }

    protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
    {
        base.OnPropertyChanged(change);
        
    }

    private void OnContentChanged()
    {
        
    }

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);
        _confirmButton = e.NameScope.Find<Button>(PART_ConfirmButton);
        _cancelButton = e.NameScope.Find<Button>(PART_CancelButton);
        _popup = e.NameScope.Find<Popup>(PART_Popup);
        Button.ClickEvent.AddHandler(OnButtonClicked, _confirmButton, _cancelButton);
    }

    private void OnButtonClicked(object sender, RoutedEventArgs e)
    {
        // This is a hack for MVVM toolkit that uses INotifyPropertyChanged for async command. It counts the number of
        // IsRunning property changes to determine when the command is finished.
        if (sender is Button button &&  button.Command is INotifyPropertyChanged inpc)
        {
            var count = 0;
            void OnCommandPropertyChanged(object? sender, PropertyChangedEventArgs args)
            {
                if (args.PropertyName != "IsRunning") return;
                count++;
                if (count != 2) return;
                inpc.PropertyChanged -= OnCommandPropertyChanged;
                _popup?.SetValue(Popup.IsOpenProperty, false);
            }
            inpc.PropertyChanged += OnCommandPropertyChanged;
        }
        else
        {
            _popup?.SetValue(Popup.IsOpenProperty, false);
        }
    }

    

    private IDisposable? _childChangeDisposable;

    protected override bool RegisterContentPresenter(ContentPresenter presenter)
    {
        var result = base.RegisterContentPresenter(presenter);
        _childChangeDisposable = presenter.GetPropertyChangedObservable(ContentPresenter.ChildProperty).Subscribe(OnChildChanged);
        return result;
    }

    private void OnChildChanged(AvaloniaPropertyChangedEventArgs arg)
    {
        if (arg.GetNewValue<Control?>() is null) return;
    }

    protected override void OnDetachedFromLogicalTree(LogicalTreeAttachmentEventArgs e)
    {
        _childChangeDisposable?.Dispose();
        base.OnDetachedFromLogicalTree(e);
    }
}