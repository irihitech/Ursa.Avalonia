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

    private Panel? _rootPanel;

    public static readonly StyledProperty<object?> IconProperty =
        AvaloniaProperty.Register<IconButton, object?>(nameof(Icon));

    public object? Icon
    {
        get => GetValue(IconProperty);
        set => SetValue(IconProperty, value);
    }

    public static readonly StyledProperty<IDataTemplate?> IconTemplateProperty =
        AvaloniaProperty.Register<IconButton, IDataTemplate?>(nameof(IconTemplate));

    public IDataTemplate? IconTemplate
    {
        get => GetValue(IconTemplateProperty);
        set => SetValue(IconTemplateProperty, value);
    }

    public static readonly StyledProperty<bool> IsLoadingProperty =
        AvaloniaProperty.Register<IconButton, bool>(nameof(IsLoading));

    public bool IsLoading
    {
        get => GetValue(IsLoadingProperty);
        set => SetValue(IsLoadingProperty, value);
    }

    public static readonly StyledProperty<Position> IconPlacementProperty =
        AvaloniaProperty.Register<IconButton, Position>(nameof(IconPlacement), defaultValue: Position.Left);

    public Position IconPlacement
    {
        get => GetValue(IconPlacementProperty);
        set => SetValue(IconPlacementProperty, value);
    }

    static IconButton()
    {
        IconPlacementProperty.Changed.AddClassHandler<IconButton, Position>((o, e) =>
        {
            o.SetPlacement(e.NewValue.Value, o.Icon);
            o.InvalidateRootPanel();
        });
        IconProperty.Changed.AddClassHandler<IconButton, object?>((o, e) =>
        {
            o.SetPlacement(o.IconPlacement, e.NewValue.Value);
        });
        ContentProperty.Changed.AddClassHandler<IconButton>((o, e) => o.SetEmptyContent());
    }

    private void InvalidateRootPanel() => _rootPanel?.InvalidateArrange();

    private void SetEmptyContent()
    {
        PseudoClasses.Set(PC_EmptyContent, Presenter?.Content is null);
    }

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);
        _rootPanel = e.NameScope.Find<Panel>(PART_RootPanel);
        SetEmptyContent();
        SetPlacement(IconPlacement, Icon);
    }

    private void SetPlacement(Position placement, object? icon)
    {
        if (icon is null)
        {
            PseudoClasses.Set(PC_Empty, true);
            PseudoClasses.Set(PC_Left, false);
            PseudoClasses.Set(PC_Right, false);
            PseudoClasses.Set(PC_Top, false);
            PseudoClasses.Set(PC_Bottom, false);
            return;
        }

        PseudoClasses.Set(PC_Empty, false);
        PseudoClasses.Set(PC_Left, placement == Position.Left);
        PseudoClasses.Set(PC_Right, placement == Position.Right);
        PseudoClasses.Set(PC_Top, placement == Position.Top);
        PseudoClasses.Set(PC_Bottom, placement == Position.Bottom);
    }
}