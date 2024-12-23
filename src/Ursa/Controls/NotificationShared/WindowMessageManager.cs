using System.Collections;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Primitives;
using Avalonia.Layout;
using Avalonia.VisualTree;

namespace Ursa.Controls;

/// <summary>
/// An <see cref="WindowMessageManager"/> that displays messages in a <see cref="Window"/>.
/// </summary>
[TemplatePart(PART_Items, typeof(Panel))]
public abstract class WindowMessageManager : TemplatedControl
{
    public const string PART_Items = "PART_Items";

    protected IList? _items;

    /// <summary>
    /// Defines the <see cref="MaxItems"/> property.
    /// </summary>
    public static readonly StyledProperty<int> MaxItemsProperty =
        AvaloniaProperty.Register<WindowMessageManager, int>(nameof(MaxItems), 5);

    /// <summary>
    /// Defines the maximum number of messages visible at once.
    /// </summary>
    public int MaxItems
    {
        get => GetValue(MaxItemsProperty);
        set => SetValue(MaxItemsProperty, value);
    }

    static WindowMessageManager()
    {
        HorizontalAlignmentProperty.OverrideDefaultValue<WindowMessageManager>(HorizontalAlignment.Stretch);
        VerticalAlignmentProperty.OverrideDefaultValue<WindowMessageManager>(VerticalAlignment.Stretch);
    }

    public WindowMessageManager()
    {
    }

    public WindowMessageManager(VisualLayerManager? visualLayerManager) : this()
    {
        if (visualLayerManager is null) return;
        visualLayerManager.AdornerLayer.Children.Add(this);
        AdornerLayer.SetAdornedElement(this, visualLayerManager.AdornerLayer);
    }

    /// <inheritdoc/>
    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);

        var itemsControl = e.NameScope.Find<Panel>(PART_Items);
        _items = itemsControl?.Children;
    }

    public abstract void Show(object content);

    /// <summary>
    /// Installs the <see cref="WindowMessageManager"/> within the <see cref="AdornerLayer"/>
    /// </summary>
    protected void InstallFromTopLevel(TopLevel topLevel)
    {
        topLevel.TemplateApplied += TopLevelOnTemplateApplied;
        var adorner = topLevel.FindDescendantOfType<VisualLayerManager>()?.AdornerLayer;
        if (adorner is not null)
        {
            adorner.Children.Add(this);
            AdornerLayer.SetAdornedElement(this, adorner);
        }
    }

    public virtual void Uninstall()
    {
        if (Parent is AdornerLayer adornerLayer)
        {
            adornerLayer.Children.Remove(this);
            AdornerLayer.SetAdornedElement(this, null);
        }
    }

    protected void TopLevelOnTemplateApplied(object? sender, TemplateAppliedEventArgs e)
    {
        if (Parent is AdornerLayer adornerLayer)
        {
            adornerLayer.Children.Remove(this);
            AdornerLayer.SetAdornedElement(this, null);
        }

        // Reinstall message manager on template reapplied.
        var topLevel = (TopLevel)sender!;
        topLevel.TemplateApplied -= TopLevelOnTemplateApplied;
        InstallFromTopLevel(topLevel);
    }
}