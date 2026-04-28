using Avalonia;
using Avalonia.Collections;
using Avalonia.Controls;
using Avalonia.Styling;
using Avalonia.VisualTree;

namespace Ursa.Controls;

public class ThemeVariantMapper : ThemeVariantScope
{
    protected override Type StyleKeyOverride { get; } =  typeof(ThemeVariantScope);
    private Visual? _subscribedParent;
    private readonly AvaloniaList<ThemeVariantMapping> _mappings;

    // ── Constructor ──────────────────────────────────────────────────────────

    /// <summary>Initialises a new <see cref="ThemeVariantMapper"/> instance.</summary>
    public ThemeVariantMapper()
    {
        _mappings = new AvaloniaList<ThemeVariantMapping>();
        _mappings.CollectionChanged += (_, _) => ApplyMapping();
    }

    // ── Public API ───────────────────────────────────────────────────────────

    /// <summary>
    /// Gets the collection of <see cref="ThemeVariantMapping"/> entries.
    /// Each entry maps a parent <c>ActualThemeVariant</c> value to a target theme variant
    /// that this scope will apply to its descendants.
    /// </summary>
    /// <remarks>
    /// The collection is evaluated in order; the first matching <see cref="ThemeVariantMapping.Source"/>
    /// wins.  If no entry matches, <see cref="ThemeVariantScope.RequestedThemeVariant"/> is
    /// reset to <see cref="ThemeVariant.Default"/> so the parent's theme is inherited unchanged.
    /// </remarks>
    public AvaloniaList<ThemeVariantMapping> Mappings => _mappings;

    // ── Visual-tree attachment ───────────────────────────────────────────────

    /// <inheritdoc/>
    protected override void OnAttachedToVisualTree(VisualTreeAttachmentEventArgs e)
    {
        base.OnAttachedToVisualTree(e);
        SubscribeToParent();
    }

    /// <inheritdoc/>
    protected override void OnDetachedFromVisualTree(VisualTreeAttachmentEventArgs e)
    {
        base.OnDetachedFromVisualTree(e);
        UnsubscribeFromParent();
    }

    // ── Private helpers ──────────────────────────────────────────────────────

    /// <summary>
    /// Subscribes to <see cref="ThemeVariantScope.ActualThemeVariantProperty"/> changes on the visual parent
    /// so that any theme change in the parent scope is immediately reflected here.
    /// </summary>
    private void SubscribeToParent()
    {
        UnsubscribeFromParent();

        var parent = this.GetVisualParent();
        if (parent is not null)
        {
            parent.PropertyChanged += OnParentPropertyChanged;
            _subscribedParent = parent;
        }

        ApplyMapping();
    }

    /// <summary>Removes the subscription from the previously observed parent, if any.</summary>
    private void UnsubscribeFromParent()
    {
        if (_subscribedParent is null) return;
        _subscribedParent.PropertyChanged -= OnParentPropertyChanged;
        _subscribedParent = null;
    }

    /// <summary>
    /// Handles <c>PropertyChanged</c> on the subscribed visual parent.
    /// Re-applies the mapping whenever the parent's <c>ActualThemeVariant</c> changes.
    /// </summary>
    private void OnParentPropertyChanged(object? sender, AvaloniaPropertyChangedEventArgs e)
    {
        if (e.Property == ActualThemeVariantProperty)
            ApplyMapping();
    }

    /// <summary>
    /// Reads the parent's current <c>ActualThemeVariant</c>, looks it up in
    /// <see cref="Mappings"/>, and sets <see cref="ThemeVariantScope.RequestedThemeVariant"/>
    /// accordingly.
    /// </summary>
    private void ApplyMapping()
    {
        var parent = this.GetVisualParent();
        if (parent is null)
            return;

        var parentTheme = parent.GetValue(ActualThemeVariantProperty);

        foreach (var mapping in _mappings)
        {
            if (mapping.Source == parentTheme)
            {
                SetCurrentValue(RequestedThemeVariantProperty, mapping.Target ?? ThemeVariant.Default);
                return;
            }
        }

        // No match — reset so this scope transparently inherits the parent theme.
        SetCurrentValue(RequestedThemeVariantProperty, ThemeVariant.Default);
    }
}
