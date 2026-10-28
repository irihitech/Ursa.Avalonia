using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Primitives;
using Avalonia.Controls.Templates;
using Avalonia.Data;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.VisualTree;

namespace Ursa.Controls;

/// <summary>
/// A settings-style row: an optional leading <see cref="Icon"/>, a title (<see cref="Content"/>),
/// optional content next to the title (<see cref="Trailing"/>), a secondary line
/// (<see cref="Description"/>), the current value (<see cref="Note"/>) and an action area
/// (<see cref="RightContent"/>) on the right side, and an optional trailing chevron.
/// The row derives from <see cref="Button"/>, but only expandable (<see cref="Detail"/>)
/// or navigable (<see cref="Button.Command"/> or <see cref="ShowChevron"/>) rows are
/// interactive: they get hover/press feedback and focus.
/// Plain rows and rows whose right side hosts an input control stay quiet, following desktop
/// conventions. The chevron of a detail row points down when collapsed and rotates 180 degrees
/// when expanded; interactions inside the detail area never collapse the row or highlight it.
/// </summary>
[TemplatePart(PART_DetailHost, typeof(Control))]
[PseudoClasses(PC_Expanded, PC_HasDetail, PC_OverDetail, PC_Interactive)]
public class SettingsCard : Button
{
    /// <summary>The name of the detail host part in the control template.</summary>
    public const string PART_DetailHost = "PART_DetailHost";

    private const string PC_Expanded = ":expanded";
    private const string PC_HasDetail = ":has-detail";
    private const string PC_OverDetail = ":over-detail";
    private const string PC_Interactive = ":interactive";

    private Control? _detailHost;

    public SettingsCard()
    {
        Click += OnCardClick;
        UpdateInteractive();
    }

    private void OnCardClick(object? sender, RoutedEventArgs e)
    {
        if (Detail is null) return;
        // Interactions inside the detail area (check boxes, buttons, blank space) are not
        // treated as a click on the row itself.
        if (e.Source is Visual source && _detailHost is { } host && host.IsVisualAncestorOf(source))
        {
            return;
        }

        SetCurrentValue(IsExpandedProperty, !IsExpanded);
    }

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        if (_detailHost is { } old)
        {
            old.PointerEntered -= OnDetailPointerEntered;
            old.PointerExited -= OnDetailPointerExited;
            old.RemoveHandler(PointerPressedEvent, OnDetailPointerPressed);
        }

        base.OnApplyTemplate(e);
        _detailHost = e.NameScope.Find(PART_DetailHost) as Control;
        if (_detailHost is { } neu)
        {
            neu.PointerEntered += OnDetailPointerEntered;
            neu.PointerExited += OnDetailPointerExited;
            // Pointer presses inside the detail area end there instead of bubbling to the row
            // button; otherwise controls that do not handle presses themselves (e.g.
            // KeyGestureInput) would trigger the row's press-release cycle and collapse it.
            neu.AddHandler(PointerPressedEvent, OnDetailPointerPressed);
        }
    }

    private void OnDetailPointerPressed(object? sender, PointerPressedEventArgs e)
    {
        // Forward focus to the nearest focusable control at the press position: such controls
        // do not handle presses on their own, and still need to work with keyboard input after
        // bubbling has been intercepted.
        if (e.Source is Visual source && _detailHost is { } host)
        {
            source.GetSelfAndVisualAncestors()
                .OfType<InputElement>()
                .TakeWhile(i => !ReferenceEquals(i, host))
                .FirstOrDefault(i => i.Focusable)
                ?.Focus(NavigationMethod.Pointer);
        }

        e.Handled = true;
    }

    private void OnDetailPointerEntered(object? sender, PointerEventArgs e)
        => PseudoClasses.Set(PC_OverDetail, true);

    private void OnDetailPointerExited(object? sender, PointerEventArgs e)
        => PseudoClasses.Set(PC_OverDetail, false);

    /// <summary>
    /// Defines the <see cref="Icon"/> styled property.
    /// </summary>
    public static readonly StyledProperty<object?> IconProperty =
        AvaloniaProperty.Register<SettingsCard, object?>(nameof(Icon));

    /// <summary>
    /// Gets or sets the leading icon of the row. A raw <see cref="Avalonia.Media.Geometry"/>
    /// value is rendered through a PathIcon.
    /// </summary>
    public object? Icon
    {
        get => GetValue(IconProperty);
        set => SetValue(IconProperty, value);
    }

    /// <summary>
    /// Defines the <see cref="IconTemplate"/> styled property.
    /// </summary>
    public static readonly StyledProperty<IDataTemplate?> IconTemplateProperty =
        AvaloniaProperty.Register<SettingsCard, IDataTemplate?>(nameof(IconTemplate));

    /// <summary>
    /// Gets or sets the template used to render <see cref="Icon"/>.
    /// </summary>
    public IDataTemplate? IconTemplate
    {
        get => GetValue(IconTemplateProperty);
        set => SetValue(IconTemplateProperty, value);
    }

    /// <summary>
    /// Defines the <see cref="Description"/> styled property.
    /// </summary>
    public static readonly StyledProperty<object?> DescriptionProperty =
        AvaloniaProperty.Register<SettingsCard, object?>(nameof(Description));

    /// <summary>
    /// Gets or sets the secondary text below the title, rendered in a muted color.
    /// </summary>
    public object? Description
    {
        get => GetValue(DescriptionProperty);
        set => SetValue(DescriptionProperty, value);
    }

    /// <summary>
    /// Defines the <see cref="DescriptionTemplate"/> styled property.
    /// </summary>
    public static readonly StyledProperty<IDataTemplate?> DescriptionTemplateProperty =
        AvaloniaProperty.Register<SettingsCard, IDataTemplate?>(nameof(DescriptionTemplate));

    /// <summary>
    /// Gets or sets the template used to render <see cref="Description"/>.
    /// </summary>
    public IDataTemplate? DescriptionTemplate
    {
        get => GetValue(DescriptionTemplateProperty);
        set => SetValue(DescriptionTemplateProperty, value);
    }

    /// <summary>
    /// Defines the <see cref="Note"/> styled property.
    /// </summary>
    public static readonly StyledProperty<object?> NoteProperty =
        AvaloniaProperty.Register<SettingsCard, object?>(nameof(Note));

    /// <summary>
    /// Gets or sets the current value text on the right side (e.g. "Not set", "Shift"),
    /// placed before the action area.
    /// </summary>
    public object? Note
    {
        get => GetValue(NoteProperty);
        set => SetValue(NoteProperty, value);
    }

    /// <summary>
    /// Defines the <see cref="NoteTemplate"/> styled property.
    /// </summary>
    public static readonly StyledProperty<IDataTemplate?> NoteTemplateProperty =
        AvaloniaProperty.Register<SettingsCard, IDataTemplate?>(nameof(NoteTemplate));

    /// <summary>
    /// Gets or sets the template used to render <see cref="Note"/>.
    /// </summary>
    public IDataTemplate? NoteTemplate
    {
        get => GetValue(NoteTemplateProperty);
        set => SetValue(NoteTemplateProperty, value);
    }

    /// <summary>
    /// Defines the <see cref="RightContent"/> styled property.
    /// </summary>
    public static readonly StyledProperty<object?> RightContentProperty =
        AvaloniaProperty.Register<SettingsCard, object?>(nameof(RightContent));

    /// <summary>
    /// Gets or sets the action area on the right side; hosts ToggleSwitch, RadioButton,
    /// NumericUpDown and similar controls.
    /// </summary>
    public object? RightContent
    {
        get => GetValue(RightContentProperty);
        set => SetValue(RightContentProperty, value);
    }

    /// <summary>
    /// Defines the <see cref="RightContentTemplate"/> styled property.
    /// </summary>
    public static readonly StyledProperty<IDataTemplate?> RightContentTemplateProperty =
        AvaloniaProperty.Register<SettingsCard, IDataTemplate?>(nameof(RightContentTemplate));

    /// <summary>
    /// Gets or sets the template used to render <see cref="RightContent"/>.
    /// </summary>
    public IDataTemplate? RightContentTemplate
    {
        get => GetValue(RightContentTemplateProperty);
        set => SetValue(RightContentTemplateProperty, value);
    }

    /// <summary>
    /// Defines the <see cref="Trailing"/> styled property.
    /// </summary>
    public static readonly StyledProperty<object?> TrailingProperty =
        AvaloniaProperty.Register<SettingsCard, object?>(nameof(Trailing));

    /// <summary>
    /// Gets or sets supplementary content next to the title, such as a "modified" badge.
    /// </summary>
    public object? Trailing
    {
        get => GetValue(TrailingProperty);
        set => SetValue(TrailingProperty, value);
    }

    /// <summary>
    /// Defines the <see cref="TrailingTemplate"/> styled property.
    /// </summary>
    public static readonly StyledProperty<IDataTemplate?> TrailingTemplateProperty =
        AvaloniaProperty.Register<SettingsCard, IDataTemplate?>(nameof(TrailingTemplate));

    /// <summary>
    /// Gets or sets the template used to render <see cref="Trailing"/>.
    /// </summary>
    public IDataTemplate? TrailingTemplate
    {
        get => GetValue(TrailingTemplateProperty);
        set => SetValue(TrailingTemplateProperty, value);
    }

    /// <summary>
    /// Defines the <see cref="Detail"/> styled property.
    /// </summary>
    public static readonly StyledProperty<object?> DetailProperty =
        AvaloniaProperty.Register<SettingsCard, object?>(nameof(Detail));

    /// <summary>
    /// Gets or sets the inline detail settings. When not null, the row shows a chevron and
    /// clicking the row expands or collapses the detail area.
    /// </summary>
    public object? Detail
    {
        get => GetValue(DetailProperty);
        set => SetValue(DetailProperty, value);
    }

    /// <summary>
    /// Defines the <see cref="DetailTemplate"/> styled property.
    /// </summary>
    public static readonly StyledProperty<IDataTemplate?> DetailTemplateProperty =
        AvaloniaProperty.Register<SettingsCard, IDataTemplate?>(nameof(DetailTemplate));

    /// <summary>
    /// Gets or sets the template used to render <see cref="Detail"/>.
    /// </summary>
    public IDataTemplate? DetailTemplate
    {
        get => GetValue(DetailTemplateProperty);
        set => SetValue(DetailTemplateProperty, value);
    }

    /// <summary>
    /// Defines the <see cref="ShowChevron"/> styled property.
    /// </summary>
    public static readonly StyledProperty<bool> ShowChevronProperty =
        AvaloniaProperty.Register<SettingsCard, bool>(nameof(ShowChevron));

    /// <summary>
    /// Gets or sets a value indicating whether the trailing chevron is always shown.
    /// Used for navigable rows bound to <see cref="Button.Command"/>; detail rows show
    /// the chevron automatically.
    /// </summary>
    public bool ShowChevron
    {
        get => GetValue(ShowChevronProperty);
        set => SetValue(ShowChevronProperty, value);
    }

    /// <summary>
    /// Defines the <see cref="IsExpanded"/> styled property.
    /// </summary>
    public static readonly StyledProperty<bool> IsExpandedProperty =
        AvaloniaProperty.Register<SettingsCard, bool>(nameof(IsExpanded), defaultBindingMode: BindingMode.TwoWay);

    /// <summary>
    /// Gets or sets a value indicating whether <see cref="Detail"/> is expanded.
    /// Binds two-way.
    /// </summary>
    public bool IsExpanded
    {
        get => GetValue(IsExpandedProperty);
        set => SetValue(IsExpandedProperty, value);
    }

    protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
    {
        base.OnPropertyChanged(change);
        if (change.Property == DetailProperty)
        {
            // SetCurrentValue keeps a two-way binding on IsExpanded alive.
            if (change.NewValue is null) SetCurrentValue(IsExpandedProperty, false);
            PseudoClasses.Set(PC_HasDetail, change.NewValue is not null);
            UpdateInteractive();
        }
        else if (change.Property == IsExpandedProperty)
        {
            PseudoClasses.Set(PC_Expanded, IsExpanded);
            // Once collapsed the detail is no longer hoverable; a stale :over-detail would
            // suppress the row highlight on the next hover of the row header.
            if (!IsExpanded) PseudoClasses.Set(PC_OverDetail, false);
        }
        else if (change.Property == CommandProperty || change.Property == ShowChevronProperty)
        {
            UpdateInteractive();
        }
    }

    /// <summary>
    /// Only interactive rows (expandable or navigable) get hover/press feedback and focus;
    /// plain rows and rows whose right side hosts an input control stay quiet, avoiding the
    /// impression that the whole row is clickable.
    /// </summary>
    private void UpdateInteractive()
    {
        var interactive = Detail is not null || Command is not null || ShowChevron;
        PseudoClasses.Set(PC_Interactive, interactive);
        Focusable = interactive;
    }
}