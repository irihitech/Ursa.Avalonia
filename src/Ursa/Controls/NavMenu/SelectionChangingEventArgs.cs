using System.Collections;
using Avalonia.Interactivity;

namespace Ursa.Controls;

public class SelectionChangingEventArgs: RoutedEventArgs
{
    /// <summary>Gets the items that were added to the selection.</summary>
    public IList NewItems { get; }

    /// <summary>Gets the items that were removed from the selection.</summary>
    public IList OldItems { get; }
    
    /// <summary>
    /// Gets or sets a value indicating whether the selection can be changed. If set to <c>false</c>, the selection will not change.
    /// </summary>
    public bool CanSelect { get; set; } = true;
    
    public SelectionChangingEventArgs(RoutedEvent routedEvent, IList oldItems, IList newItems): base(routedEvent)
    {
        OldItems = oldItems;
        NewItems = newItems;
    }
}