using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Primitives;
using Avalonia.Controls.Templates;
using Ursa.Common;

namespace Ursa.Controls;

[TemplatePart(PART_RootPanel, typeof(Panel))]
[PseudoClasses(PC_Right, PC_Left, PC_Top, PC_Bottom, PC_Empty, PC_EmptyContent)]
public class IconButton : Button
{
    public const string PC_Right = ":right";
    public const string PC_Left = ":left";
    public const string PC_Top = ":top";
    public const string PC_Bottom = ":bottom";
    public const string PC_Empty = ":empty";
    public const string PC_EmptyContent = ":empty-content";
    public const string PART_RootPanel = "PART_RootPanel";

    public static readonly StyledProperty<object?> IconProperty =
        AvaloniaProperty.Register<IconButton, object?>(nameof(Icon));

    public static object? GetIcon(ContentControl o) => o.GetValue(IconProperty);
    public static void SetIcon(ContentControl o, object? value) => o.SetValue(IconProperty, value);

    public object? Icon
    {
        get => GetValue(IconProperty);
        set => SetValue(IconProperty, value);
    }

    public static readonly StyledProperty<IDataTemplate?> IconTemplateProperty =
        AvaloniaProperty.Register<IconButton, IDataTemplate?>(nameof(IconTemplate));

    public static IDataTemplate? GetIconTemplate(ContentControl o) => o.GetValue(IconTemplateProperty);
    public static void SetIconTemplate(ContentControl o, IDataTemplate? value) => o.SetValue(IconTemplateProperty, value);

    public IDataTemplate? IconTemplate
    {
        get => GetValue(IconTemplateProperty);
        set => SetValue(IconTemplateProperty, value);
    }

    public static readonly StyledProperty<bool> IsLoadingProperty =
        AvaloniaProperty.Register<IconButton, bool>(nameof(IsLoading));

    public static bool GetIsLoading(ContentControl o) => o.GetValue(IsLoadingProperty);
    public static void SetIsLoading(ContentControl o, bool value) => o.SetValue(IsLoadingProperty, value);

    public bool IsLoading
    {
        get => GetValue(IsLoadingProperty);
        set => SetValue(IsLoadingProperty, value);
    }

    public static readonly StyledProperty<Position> IconPlacementProperty =
        AvaloniaProperty.Register<IconButton, Position>(nameof(IconPlacement), defaultValue: Position.Left);

    public static Position GetIconPlacement(ContentControl o) => o.GetValue(IconPlacementProperty);
    public static void SetIconPlacement(ContentControl o, Position value) => o.SetValue(IconPlacementProperty, value);

    public Position IconPlacement
    {
        get => GetValue(IconPlacementProperty);
        set => SetValue(IconPlacementProperty, value);
    }

    static IconButton()
    {
        ReversibleStackPanelUtils.EnsureBugFixed();
        IconPlacementProperty.Changed.AddClassHandler<ContentControl, Position>((o, e) =>
        {
            UpdateIconPseudoClasses(o, e.NewValue.Value, GetIcon(o));
        });
        IconProperty.Changed.AddClassHandler<ContentControl, object?>((o, e) =>
        {
            UpdateIconPseudoClasses(o, GetIconPlacement(o), e.NewValue.Value);
        });
        ContentProperty.Changed.AddClassHandler<ContentControl, object?>((o, _) =>
        {
            UpdateEmptyContentPseudoClass(o);
        });
    }

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);
        UpdateEmptyContentPseudoClass(this);
        UpdateIconPseudoClasses(this, IconPlacement, Icon);
    }

    internal static void UpdatePseudoClasses(ContentControl button)
    {
        UpdateEmptyContentPseudoClass(button);
        UpdateIconPseudoClasses(button, GetIconPlacement(button), GetIcon(button));
    }

    private static void UpdateEmptyContentPseudoClass(ContentControl button)
    {
        IPseudoClasses pseudo = button.Classes;
        pseudo.Set(PC_EmptyContent, button.Content is null);
    }

    private static void UpdateIconPseudoClasses(ContentControl button, Position placement, object? icon)
    {
        IPseudoClasses pseudo = button.Classes;
        var hasIcon = icon is not null;
        pseudo.Set(PC_Empty, !hasIcon);
        pseudo.Set(PC_Left, hasIcon && placement == Position.Left);
        pseudo.Set(PC_Right, hasIcon && placement == Position.Right);
        pseudo.Set(PC_Top, hasIcon && placement == Position.Top);
        pseudo.Set(PC_Bottom, hasIcon && placement == Position.Bottom);
    }
}