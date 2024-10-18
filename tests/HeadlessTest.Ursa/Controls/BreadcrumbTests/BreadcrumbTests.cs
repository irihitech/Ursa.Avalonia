using System.Collections.ObjectModel;
using Avalonia.Controls;
using Avalonia.Headless.XUnit;
using Avalonia.LogicalTree;
using Ursa.Controls;

namespace HeadlessTest.Ursa.Controls;

public class BreadcrumbTests
{
    [AvaloniaFact]
    public void BreadcrumbItem_Should_Have_Correct_PseudoClasses()
    {
        var window = new Window();
        var breadcrumb = new Breadcrumb();
        window.Content = breadcrumb;
        window.Show();
        var item1 = new BreadcrumbItem();
        var item2 = new BreadcrumbItem();
        breadcrumb.Items.Add(item1);
        Assert.Contains(":last", item1.Classes);
        breadcrumb.Items.Add(item2);
        Assert.Contains(":last", item2.Classes);
        Assert.DoesNotContain(":last", item1.Classes);
    }
    
    [AvaloniaFact]
    public void Generated_BreadcrumbItem_Should_Have_Correct_PseudoClasses()
    {
        var window = new Window();
        var breadcrumb = new Breadcrumb();
        window.Content = breadcrumb;
        window.Show();
        var item1 = new TextBlock();
        var item2 = new TextBlock();
        breadcrumb.Items.Add(item1);
        var firstItem = breadcrumb.GetLogicalChildren().OfType<BreadcrumbItem>().FirstOrDefault();
        Assert.NotNull(firstItem);
        Assert.Contains(":last", firstItem.Classes);
        breadcrumb.Items.Add(item2);
        var lastItem = breadcrumb.GetLogicalChildren().OfType<BreadcrumbItem>().LastOrDefault();
        Assert.NotNull(lastItem);
        Assert.Contains(":last", lastItem.Classes);
        Assert.DoesNotContain(":last", firstItem.Classes);
    }
    
    [AvaloniaFact]
    public void BreadcrumbItem_FromItemsSource_Should_Have_Correct_PseudoClasses()
    {
        var window = new Window();
        var breadcrumb = new Breadcrumb();
        window.Content = breadcrumb;
        window.Show();
        var items = new ObservableCollection<string>();
        breadcrumb.ItemsSource = items;
        items.Add("Item 1");
        var item1 = breadcrumb.GetLogicalChildren().OfType<BreadcrumbItem>().FirstOrDefault();
        Assert.NotNull(item1);
        Assert.Contains(":last", item1.Classes);
        items.Add("Item 2");
        var item2 = breadcrumb.GetLogicalChildren().OfType<BreadcrumbItem>().LastOrDefault();
        Assert.NotNull(item2);
        Assert.Contains(":last", item2.Classes);
        Assert.DoesNotContain(":last", item1.Classes);
    }
    
    [AvaloniaFact]
    public void Bulk_Added_BreadcrumbItem_Should_Have_Correct_PseudoClasses()
    {
        var window = new Window();
        var breadcrumb = new Breadcrumb();
        window.Content = breadcrumb;
        window.Show();
        var items = new ObservableCollection<string>
        {
            "Item 1",
            "Item 2"
        };
        breadcrumb.ItemsSource = items;
        var item1 = breadcrumb.GetLogicalChildren().OfType<BreadcrumbItem>().FirstOrDefault();
        var item2 = breadcrumb.GetLogicalChildren().OfType<BreadcrumbItem>().LastOrDefault();
        Assert.NotNull(item1);
        Assert.NotNull(item2);
        Assert.Contains(":last", item2.Classes);
        Assert.DoesNotContain(":last", item1.Classes);
    }
}