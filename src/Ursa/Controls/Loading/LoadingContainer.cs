using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Templates;
using Avalonia.Media;

namespace Ursa.Controls;

[PseudoClasses(PC_Loading)]
public class LoadingContainer: ContentControl
{
    public const string PC_Loading = ":loading";
    
    public static readonly StyledProperty<object?> IndicatorProperty = AvaloniaProperty.Register<LoadingContainer, object?>(
        nameof(Indicator));

    public object? Indicator
    {
        get => GetValue(IndicatorProperty);
        set => SetValue(IndicatorProperty, value);
    }

    public static readonly StyledProperty<object?> LoadingMessageProperty = AvaloniaProperty.Register<LoadingContainer, object?>(
        nameof(LoadingMessage));

    public object? LoadingMessage
    {
        get => GetValue(LoadingMessageProperty);
        set => SetValue(LoadingMessageProperty, value);
    }

    public static readonly StyledProperty<IBrush?> MessageForegroundProperty = AvaloniaProperty.Register<LoadingContainer, IBrush?>(
        nameof(MessageForeground));

    public IBrush? MessageForeground
    {
        get => GetValue(MessageForegroundProperty);
        set => SetValue(MessageForegroundProperty, value);
    }

    public static readonly StyledProperty<IDataTemplate> LoadingMessageTemplateProperty = AvaloniaProperty.Register<LoadingContainer, IDataTemplate>(
        nameof(LoadingMessageTemplate));

    public IDataTemplate LoadingMessageTemplate
    {
        get => GetValue(LoadingMessageTemplateProperty);
        set => SetValue(LoadingMessageTemplateProperty, value);
    }

    public static readonly StyledProperty<bool> IsLoadingProperty = AvaloniaProperty.Register<LoadingContainer, bool>(
        nameof(IsLoading));

    public bool IsLoading
    {
        get => GetValue(IsLoadingProperty);
        set => SetValue(IsLoadingProperty, value);
    }

    static LoadingContainer()
    {
        IsLoadingProperty.Changed.AddClassHandler<LoadingContainer>((x, e) => x.OnIsLoadingChanged(e));
    }

    private void OnIsLoadingChanged(AvaloniaPropertyChangedEventArgs args)
    {
        bool newValue = args.GetNewValue<bool>();
        PseudoClasses.Set(PC_Loading, newValue);
    }
}