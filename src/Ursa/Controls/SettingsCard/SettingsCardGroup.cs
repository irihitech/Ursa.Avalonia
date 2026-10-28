using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Presenters;
using Avalonia.Controls.Primitives;
using Avalonia.Controls.Templates;


namespace Ursa.Controls;

/// <summary>
/// A card container for <see cref="SettingsCard"/> rows: below a <see cref="HeaderedItemsControl.Header"/>
/// and an optional <see cref="Description"/>, rows are laid out inside one rounded card, and
/// separator lines are drawn between adjacent rows (except after the last one). The card corner
/// radius is controlled by the inherited <see cref="TemplatedControl.CornerRadius"/>; row
/// hover/press backgrounds inherit the radius on the first/last row sides (see the theme styles).
/// Items can be <see cref="SettingsCard"/> instances directly, or arbitrary models — without an
/// ItemTemplate, a model is automatically wrapped in a <see cref="SettingsCard"/> and shown as
/// its title.
/// </summary>
public class SettingsCardGroup : HeaderedItemsControl
{
    /// <summary>
    /// Defines the <see cref="Description"/> styled property.
    /// </summary>
    public static readonly StyledProperty<object?> DescriptionProperty =
        AvaloniaProperty.Register<SettingsCardGroup, object?>(nameof(Description));

    /// <summary>
    /// Gets or sets the group description, shown between the header and the card in a muted color.
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
        AvaloniaProperty.Register<SettingsCardGroup, IDataTemplate?>(nameof(DescriptionTemplate));

    /// <summary>
    /// Gets or sets the template used to render <see cref="Description"/>.
    /// </summary>
    public IDataTemplate? DescriptionTemplate
    {
        get => GetValue(DescriptionTemplateProperty);
        set => SetValue(DescriptionTemplateProperty, value);
    }

    protected override bool NeedsContainerOverride(object? item, int index, out object? recycleKey)
    {
        recycleKey = null;
        return item is not Control;
    }

    protected override Control CreateContainerForItemOverride(object? item, int index, object? recycleKey)
    {
        return ItemTemplate is null ? new SettingsCard() : new ContentPresenter();
    }

    protected override void PrepareContainerForItemOverride(Control container, object? item, int index)
    {
        base.PrepareContainerForItemOverride(container, item, index);
        if (container is ContentPresenter presenter)
        {
            presenter.Content = item;
            presenter.ContentTemplate = ItemTemplate;
        }
        else if (container is SettingsCard card && !card.IsSet(ContentControl.ContentTemplateProperty))
        {
            card.ContentTemplate = ItemTemplate;
        }
    }
}
