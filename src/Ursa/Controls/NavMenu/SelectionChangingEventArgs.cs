using System.Collections;
using Avalonia.Interactivity;

namespace Ursa.Controls;

public class SelectionChangingEventArgs: RoutedEventArgs
{
    /// <summary>Gets the items that were added to the selection.</summary>
    public IList AddedItems { get; }

    /// <summary>Gets the items that were removed from the selection.</summary>
    public IList RemovedItems { get; }
    
    /// <summary>
    /// Gets or sets a value indicating whether the selection can be changed. If set to <c>false</c>, the selection will not change.
    /// </summary>
    public bool CanSelect { get; set; } = true;
    
    public SelectionChangingEventArgs(RoutedEvent routedEvent, IList removedItems, IList addedItems): base(routedEvent)
    {
        RemovedItems = removedItems;
        AddedItems = addedItems;
    }
}