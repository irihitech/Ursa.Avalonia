using System.Collections.ObjectModel;
using Avalonia.Controls;
using Avalonia.Headless.XUnit;
using Avalonia.LogicalTree;
using Avalonia.VisualTree;
using Ursa.Controls;

namespace HeadlessTest.Ursa.Controls;

public class TimelineTests
{
    [AvaloniaFact]
    public void TimelineItem_Should_Have_Correct_PseudoClasses()
    {
        var window = new Window();
        var timeline = new Timeline();
        window.Content = timeline;
        window.Show();
        var item1 = new TimelineItem();
        var item2 = new TimelineItem();
        timeline.Items.Add(item1);
        Assert.Contains(":first", item1.Classes);
        Assert.Contains(":last", item1.Classes);
        timeline.Items.Add(item2);
        Assert.Contains(":first", item1.Classes);
        Assert.DoesNotContain(":last", item1.Classes);
        Assert.DoesNotContain(":first", item2.Classes);
        Assert.Contains(":last", item2.Classes);
    }
    
    [AvaloniaFact]
    public void TimelineItem_Should_Have_Correct_PseudoClasses_When_Insert()
    {
        var window = new Window();
        var timeline = new Timeline();
        window.Content = timeline;
        window.Show();
        var item1 = new TimelineItem();
        var item2 = new TimelineItem();
        timeline.Items.Add(item1);
        Assert.Contains(":first", item1.Classes);
        Assert.Contains(":last", item1.Classes);
        timeline.Items.Insert(0, item2);
        Assert.DoesNotContain(":first", item1.Classes);
        Assert.Contains(":last", item1.Classes);
        Assert.Contains(":first", item2.Classes);
        Assert.DoesNotContain(":last", item2.Classes);
    }
    
    [AvaloniaFact]
    public void Generated_TimelineItem_Should_Have_Correct_PseudoClasses()
    {
        var window = new Window();
        var timeline = new Timeline();
        window.Content = timeline;
        window.Show();
        var item1 = new TextBlock();
        var item2 = new TextBlock();
        timeline.Items.Add(item1);
        var firstItem = timeline.GetVisualDescendants().OfType<TimelineItem>().FirstOrDefault();
        Assert.NotNull(firstItem);
        Assert.Contains(":first", firstItem.Classes);
        Assert.Contains(":last", firstItem.Classes);
        timeline.Items.Add(item2);
        var lastItem = timeline.GetVisualDescendants().OfType<TimelineItem>().LastOrDefault();
        Assert.NotNull(lastItem);
        Assert.DoesNotContain(":first", lastItem.Classes);
        Assert.Contains(":last", lastItem.Classes);
        Assert.Contains(":first", firstItem.Classes);
        Assert.DoesNotContain(":last", firstItem.Classes);
    }
    
    [AvaloniaFact]
    public void Generated_TimelineItem_Should_Have_Correct_PseudoClasses_When_Insert()
    {
        var window = new Window();
        var timeline = new Timeline();
        window.Content = timeline;
        window.Show();
        var item1 = new TextBlock();
        var item2 = new TextBlock();
        timeline.Items.Add(item1);
        var firstItem = timeline.GetVisualDescendants().OfType<TimelineItem>().FirstOrDefault();
        Assert.NotNull(firstItem);
        Assert.Contains(":first", firstItem.Classes);
        Assert.Contains(":last", firstItem.Classes);
        timeline.Items.Insert(0, item2);
        var lastItem = timeline.GetVisualDescendants().OfType<TimelineItem>().LastOrDefault();
        Assert.NotNull(lastItem);
        Assert.DoesNotContain(":first", lastItem.Classes);
        Assert.Contains(":last", lastItem.Classes);
        firstItem = timeline.GetVisualDescendants().OfType<TimelineItem>().FirstOrDefault();
        Assert.NotNull(firstItem);
        Assert.Contains(":first", firstItem.Classes);
        Assert.DoesNotContain(":last", firstItem.Classes);
    }
    
    [AvaloniaFact]
    public void TimelineItem_FromItemsSource_Should_Have_Correct_PseudoClasses()
    {
        var window = new Window();
        var timeline = new Timeline();
        window.Content = timeline;
        window.Show();
        var items = new ObservableCollection<string>();
        timeline.ItemsSource = items;
        items.Add("Item 1");
        var item1 = timeline.GetVisualDescendants().OfType<TimelineItem>().FirstOrDefault();
        Assert.NotNull(item1);
        Assert.Contains(":first", item1.Classes);
        Assert.Contains(":last", item1.Classes);
        items.Add("Item 2");
        var item2 = timeline.GetVisualDescendants().OfType<TimelineItem>().LastOrDefault();
        Assert.NotNull(item2);
        Assert.DoesNotContain(":first", item2.Classes);
        Assert.Contains(":last", item2.Classes);
        Assert.Contains(":first", item1.Classes);
        Assert.DoesNotContain(":last", item1.Classes);
    }
    
    [AvaloniaFact]
    public void TimelineItem_FromItemsSource_Should_Have_Correct_PseudoClasses_When_Insert() 
    {
        var window = new Window();
        var timeline = new Timeline();
        window.Content = timeline;
        window.Show();
        var items = new ObservableCollection<string>();
        timeline.ItemsSource = items;
        items.Add("Item 1");
        var item1 = timeline.GetVisualDescendants().OfType<TimelineItem>().FirstOrDefault();
        Assert.NotNull(item1);
        Assert.Contains(":first", item1.Classes);
        Assert.Contains(":last", item1.Classes);
        items.Insert(0, "Item 2");
        var item2 = timeline.GetVisualDescendants().OfType<TimelineItem>().LastOrDefault();
        Assert.NotNull(item2);
        Assert.DoesNotContain(":first", item2.Classes);
        Assert.Contains(":last", item2.Classes);
        var newItem1 = timeline.GetVisualDescendants().OfType<TimelineItem>().FirstOrDefault();
        Assert.NotNull(newItem1);
        Assert.Contains(":first", newItem1.Classes);
        Assert.DoesNotContain(":last", newItem1.Classes);
    }
    
    [AvaloniaFact]
    public void Bulk_Added_TimelineItem_Should_Have_Correct_PseudoClasses()
    {
        var window = new Window();
        var timeline = new Timeline();
        window.Content = timeline;
        window.Show();
        var items = new ObservableCollection<string>
        {
            "Item 1",
            "Item 2"
        };
        timeline.ItemsSource = items;
        var item1 = timeline.GetVisualDescendants().OfType<TimelineItem>().FirstOrDefault();
        var item2 = timeline.GetVisualDescendants().OfType<TimelineItem>().LastOrDefault();
        Assert.NotNull(item1);
        Assert.NotNull(item2);
        Assert.Contains(":first", item1.Classes);
        Assert.DoesNotContain(":last", item1.Classes);
        Assert.DoesNotContain(":first", item2.Classes);
        Assert.Contains(":last", item2.Classes);
    }
}
