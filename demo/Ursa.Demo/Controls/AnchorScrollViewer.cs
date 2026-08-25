using System.Collections;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Templates;

namespace Ursa.Demo.Controls;

/// <summary>
/// A demo ScrollViewer with an integrated Anchor navigation panel on the right side.
/// </summary>
public class AnchorScrollViewer : ScrollViewer
{
    public static readonly StyledProperty<IEnumerable?> AnchorItemsProperty =
        AvaloniaProperty.Register<AnchorScrollViewer, IEnumerable?>(
            nameof(AnchorItems));

    public static readonly StyledProperty<IDataTemplate?> AnchorItemTemplateProperty =
        AvaloniaProperty.Register<AnchorScrollViewer, IDataTemplate?>(
            nameof(AnchorItemTemplate));

    public static readonly StyledProperty<double> AnchorTopOffsetProperty =
        AvaloniaProperty.Register<AnchorScrollViewer, double>(
            nameof(AnchorTopOffset), 0.0);

    public static readonly StyledProperty<double> MaxContentWidthProperty =
        AvaloniaProperty.Register<AnchorScrollViewer, double>(
            nameof(MaxContentWidth), double.PositiveInfinity);

    public IEnumerable? AnchorItems
    {
        get => GetValue(AnchorItemsProperty);
        set => SetValue(AnchorItemsProperty, value);
    }

    public IDataTemplate? AnchorItemTemplate
    {
        get => GetValue(AnchorItemTemplateProperty);
        set => SetValue(AnchorItemTemplateProperty, value);
    }

    public double AnchorTopOffset
    {
        get => GetValue(AnchorTopOffsetProperty);
        set => SetValue(AnchorTopOffsetProperty, value);
    }

    public double MaxContentWidth
    {
        get => GetValue(MaxContentWidthProperty);
        set => SetValue(MaxContentWidthProperty, value);
    }
}
