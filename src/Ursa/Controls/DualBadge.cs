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

    static DualBadge()
    {
        IconProperty.Changed.AddClassHandler<DualBadge>((o, _) => o.OnIconChanged());
        HeaderProperty.Changed.AddClassHandler<DualBadge>((o, _) => o.OnHeaderChanged());
        ContentProperty.Changed.AddClassHandler<DualBadge>((o, _) => o.OnContentChanged());
    }

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);
        OnIconChanged();
        OnHeaderChanged();
        OnContentChanged();
    }

    private void OnIconChanged()
    {
        PseudoClasses.Set(PC_IconEmpty, Icon is null);
    }

    private void OnHeaderChanged()
    {
        PseudoClasses.Set(PC_HeaderEmpty, Header is null);
    }

    private void OnContentChanged()
    {
        PseudoClasses.Set(PC_ContentEmpty, Content is null);
    }
}