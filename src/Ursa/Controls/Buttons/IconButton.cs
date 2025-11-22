using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Primitives;
using Avalonia.Controls.Templates;
using Irihi.Avalonia.Shared.Helpers;
using Ursa.Common;

namespace Ursa.Controls;

[TemplatePart(PART_RootPanel, typeof(Panel))]
[PseudoClasses(PC_Right, PC_Left, PC_Top, PC_Bottom, PC_Empty, PC_EmptyContent)]
public class IconButton : Button, IIconButton
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

    IPseudoClasses IIconButton.PseudoClasses => PseudoClasses;

    static IconButton()
    {
        ReversibleStackPanelUtils.EnsureBugFixed();
        IconPlacementProperty.Changed.Subscribe(e =>
        {
            if (e.Sender is IIconButton o)
                UpdatePseudoClasses(o, e.NewValue.Value, o.Icon);
        });
        IconPlacementProperty.Changed.Subscribe(e =>
        {
            if (e.Sender is IIconButton o)
                UpdatePseudoClasses(o, o.IconPlacement, e.NewValue.Value);
        });
        ContentProperty.Changed.AddClassHandler<IconButton>((o, _) => o.UpdateEmptyContentPseudoClass());
    }

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);
        UpdateEmptyContentPseudoClass();
        UpdatePseudoClasses(this, IconPlacement, Icon);
    }

    private void UpdateEmptyContentPseudoClass()
    {
        PseudoClasses.Set(PC_EmptyContent, Presenter?.Content is null);
    }

    private static void UpdatePseudoClasses(IIconButton button, Position placement, object? icon)
    {
        if (icon is null)
        {
            button.PseudoClasses.Set(PC_Empty, true);
            button.PseudoClasses.Set(PC_Left, false);
            button.PseudoClasses.Set(PC_Right, false);
            button.PseudoClasses.Set(PC_Top, false);
            button.PseudoClasses.Set(PC_Bottom, false);
            return;
        }

        button.PseudoClasses.Set(PC_Empty, false);
        button.PseudoClasses.Set(PC_Left, placement == Position.Left);
        button.PseudoClasses.Set(PC_Right, placement == Position.Right);
        button.PseudoClasses.Set(PC_Top, placement == Position.Top);
        button.PseudoClasses.Set(PC_Bottom, placement == Position.Bottom);
    }
}