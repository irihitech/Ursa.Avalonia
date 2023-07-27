using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Presenters;
using Avalonia.Controls.Primitives;
using Avalonia.Controls.Templates;
using Avalonia.Media;

namespace Ursa.Controls;

[PseudoClasses(PC_IconEmpty, PC_HeaderEmpty, PC_ContentEmpty)]
[TemplatePart(PART_Icon, typeof(ContentPresenter))]
public class DualBadge : HeaderedContentControl
{
    public const string PC_IconEmpty = ":icon-empty";
    public const string PC_HeaderEmpty = ":header-empty";
    public const string PC_ContentEmpty = ":content-empty";
    public const string PART_HeaderPresenter = "PART_HeaderPresenter";
    public const string PART_ContentPresenter = "PART_ContentPresenter";
    public const string PART_Icon = "PART_Icon";

    public static readonly StyledProperty<object?> IconProperty =
        AvaloniaProperty.Register<DualBadge, object?>(nameof(Icon));

    public object? Icon
    {
        get => GetValue(IconProperty);
        set => SetValue(IconProperty, value);
    }

    public static readonly StyledProperty<IDataTemplate?> IconTemplateProperty =
        AvaloniaProperty.Register<DualBadge, IDataTemplate?>(nameof(IconTemplate));

    public IDataTemplate? IconTemplate
    {
        get => GetValue(IconTemplateProperty);
        set => SetValue(IconTemplateProperty, value);
    }

    public static readonly StyledProperty<IBrush?> IconForegroundProperty =
        AvaloniaProperty.Register<DualBadge, IBrush?>(nameof(IconForeground));

    public IBrush? IconForeground
    {
        get => GetValue(IconForegroundProperty);
        set => SetValue(IconForegroundProperty, value);
    }

    public static readonly StyledProperty<IBrush?> HeaderForegroundProperty =
        AvaloniaProperty.Register<DualBadge, IBrush?>(nameof(HeaderForeground));

    public IBrush? HeaderForeground
    {
        get => GetValue(HeaderForegroundProperty);
        set => SetValue(HeaderForegroundProperty, value);
    }

    public static readonly StyledProperty<IBrush?> HeaderBackgroundProperty =
        AvaloniaProperty.Register<DualBadge, IBrush?>(nameof(HeaderBackground));

    public IBrush? HeaderBackground
    {
        get => GetValue(HeaderBackgroundProperty);
        set => SetValue(HeaderBackgroundProperty, value);
    }

    public static readonly StyledProperty<bool> IsIconEmptyProperty = AvaloniaProperty.Register<DualBadge, bool>(
        nameof(IsIconEmpty));

    public bool IsIconEmpty
    {
        get => GetValue(IsIconEmptyProperty);
        set => SetValue(IsIconEmptyProperty, value);
    }

    public static readonly StyledProperty<bool> IsHeaderEmptyProperty = AvaloniaProperty.Register<DualBadge, bool>(
        nameof(IsHeaderEmpty));

    public bool IsHeaderEmpty
    {
        get => GetValue(IsHeaderEmptyProperty);
        set => SetValue(IsHeaderEmptyProperty, value);
    }

    public static readonly StyledProperty<bool> IsContentEmptyProperty = AvaloniaProperty.Register<DualBadge, bool>(
        nameof(IsContentEmpty));

    public bool IsContentEmpty
    {
        get => GetValue(IsContentEmptyProperty);
        set => SetValue(IsContentEmptyProperty, value);
    }


    static DualBadge()
    {
        IsIconEmptyProperty.Changed.AddClassHandler<DualBadge>((o, e) => o.OnIsIconEmptyChanged(e));
        IsHeaderEmptyProperty.Changed.AddClassHandler<DualBadge>((o, e) => o.OnIsHeaderEmptyChanged(e));
        IsContentEmptyProperty.Changed.AddClassHandler<DualBadge>((o, e) => o.OnIsContentEmptyChanged(e));
    }

    private void OnIsIconEmptyChanged(AvaloniaPropertyChangedEventArgs args)
    {
        bool newValue = args.GetNewValue<bool>();
        PseudoClasses.Set(PC_IconEmpty, newValue);
    }

    private void OnIsHeaderEmptyChanged(AvaloniaPropertyChangedEventArgs args)
    {
        bool newValue = args.GetNewValue<bool>();
        PseudoClasses.Set(PC_HeaderEmpty, newValue);
    }

    private void OnIsContentEmptyChanged(AvaloniaPropertyChangedEventArgs args)
    {
        bool newValue = args.GetNewValue<bool>();
        PseudoClasses.Set(PC_ContentEmpty, newValue);
    }
}