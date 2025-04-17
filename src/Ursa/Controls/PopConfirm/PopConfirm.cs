using System.Windows.Input;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Presenters;
using Avalonia.Controls.Primitives;
using Avalonia.Controls.Templates;

namespace Ursa.Controls;

public class PopConfirm: ContentControl
{
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
        var newValue = args.GetNewValue<ICommand>();
        newValue.CanExecuteChanged += (sender, e) =>
        {
            if (args.Sender is PopConfirm popconfirm)
            {
                var b = newValue.CanExecute(this.ConfirmCommandParameter);
            }
        };
    }

    public PopConfirm()
    {
        
    }

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);
    }

    protected override bool RegisterContentPresenter(ContentPresenter presenter)
    {
        return base.RegisterContentPresenter(presenter);
    }
}