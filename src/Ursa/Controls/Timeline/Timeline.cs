using System.Collections.Specialized;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.ComTypes;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Generators;
using Avalonia.Controls.Presenters;
using Avalonia.Controls.Primitives;
using Avalonia.Controls.Templates;

namespace Ursa.Controls;

public class Timeline: ItemsControl
{
    
    public static readonly StyledProperty<IDataTemplate?> ItemDescriptionTemplateProperty = AvaloniaProperty.Register<Timeline, IDataTemplate?>(
        nameof(ItemDescriptionTemplate));

    public IDataTemplate? ItemDescriptionTemplate
    {
        get => GetValue(ItemDescriptionTemplateProperty);
        set => SetValue(ItemDescriptionTemplateProperty, value);
    }

    public Timeline()
    {
        ItemsView.CollectionChanged+=ItemsViewOnCollectionChanged;
    }

    private void ItemsViewOnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
    {
        RefreshTimelineItems();
    }

    protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
    {
        base.OnPropertyChanged(change);
        RefreshTimelineItems();
    }

    private void RefreshTimelineItems()
    {
        for (int i = 0; i < this.LogicalChildren.Count; i++)
        {
            if (this.LogicalChildren[i] is TimelineItem t)
            {
                t.SetPosition(i == 0, i == this.LogicalChildren.Count - 1);
            }
            else if (this.LogicalChildren[i] is ContentPresenter { Child: TimelineItem t2 })
            {
                t2.SetPosition(i == 0, i == this.LogicalChildren.Count - 1);
            }
        }
    }
}