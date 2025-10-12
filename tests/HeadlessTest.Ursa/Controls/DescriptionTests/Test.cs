using Avalonia.Controls;
using Avalonia.Headless.XUnit;
using Avalonia.LogicalTree;
using Avalonia.VisualTree;
using Ursa.Common;
using Ursa.Controls;

namespace HeadlessTest.Ursa.Controls.DescriptionTests;

public class Test
{
    [AvaloniaFact]
    public void Descriptions_LabelPosition_Propagates_To_DescriptionItems()
    {
        var descriptions = new Descriptions
        {
            LabelPosition = Position.Left,
            ItemsSource = new[]
            {
                new { Label = "Name", Content = "John Doe" },
                new { Label = "Age", Content = "30" }
            },
            LabelBinding = new Avalonia.Data.Binding("Label"),
        };
        var window = new Window
        {
            Content = descriptions
        };
        
        window.Show();
        
        var items = descriptions.GetLogicalChildren().OfType<DescriptionsItem>().ToList();
        foreach (var item in items)
        {
            Assert.Equal(Position.Left, item.LabelPosition);
        }

        descriptions.LabelPosition = Position.Top;
        foreach (var item in items)
        {
            Assert.Equal(Position.Top, item.LabelPosition);
        }
    }
    
    [AvaloniaFact]
    public void Inline_Descriptions_LabelPosition_Overrides_Parent_Descriptions()
    {
        var descriptions = new Descriptions
        {
            LabelPosition = Position.Left,
            Items =
            {
                new DescriptionsItem()
                {
                    Label = "Name",
                    Content = "John Doe",
                    LabelPosition = Position.Top
                },
                new DescriptionsItem()
                {
                    Label = "Age",
                    Content = "30",
                }
            }
        };
        var window = new Window
        {
            Content = descriptions
        };
        
        window.Show();
        
        var items = descriptions.GetLogicalChildren().OfType<DescriptionsItem>().ToList();
        Assert.Equal(Position.Top, items[0].LabelPosition);
        Assert.Equal(Position.Left, items[1].LabelPosition);
        
        descriptions.LabelPosition = Position.Top;
        Assert.Equal(Position.Top, items[0].LabelPosition);
        Assert.Equal(Position.Top, items[1].LabelPosition);
    }
    
    [AvaloniaFact]
    public void Descriptions_ItemAlignment_Propagates_To_DescriptionItems()
    {
        var descriptions = new Descriptions
        {
            LabelPosition = Position.Left,
            ItemsSource = new[]
            {
                new { Label = "Name", Content = "John Doe" },
                new { Label = "Age", Content = "30" }
            },
            ItemAlignment = ItemAlignment.Center,
            LabelBinding = new Avalonia.Data.Binding("Label"),
        };
        var window = new Window
        {
            Content = descriptions
        };
        
        window.Show();
        
        var items = descriptions.GetLogicalChildren().OfType<DescriptionsItem>().ToList();
        foreach (var item in items)
        {
            Assert.Equal(ItemAlignment.Center, item.ItemAlignment);
        }

        descriptions.ItemAlignment = ItemAlignment.Justify;
        foreach (var item in items)
        {
            Assert.Equal(ItemAlignment.Justify, item.ItemAlignment);
        }
    }
    
    [AvaloniaFact]
    public void Inline_Descriptions_ItemAlignment_Overrides_Parent_Descriptions()
    {
        var descriptions = new Descriptions
        {
            ItemAlignment = ItemAlignment.Center,
            Items =
            {
                new DescriptionsItem()
                {
                    Label = "Name",
                    Content = "John Doe",
                    ItemAlignment = ItemAlignment.Justify
                },
                new DescriptionsItem()
                {
                    Label = "Age",
                    Content = "30",
                }
            }
        };
        var window = new Window
        {
            Content = descriptions
        };
        
        window.Show();
        
        var items = descriptions.GetLogicalChildren().OfType<DescriptionsItem>().ToList();
        Assert.Equal(ItemAlignment.Justify, items[0].ItemAlignment);
        Assert.Equal(ItemAlignment.Center, items[1].ItemAlignment);

        descriptions.ItemAlignment = ItemAlignment.Left;
        Assert.Equal(ItemAlignment.Justify, items[0].ItemAlignment);
        Assert.Equal(ItemAlignment.Left, items[1].ItemAlignment);
    }

    [AvaloniaFact]
    public void Descriptions_LabelWidth_Propagates_To_DescriptionItems()
    {
        var descriptions = new Descriptions
        {
            LabelWidth = new GridLength(100),
            ItemsSource = new[]
            {
                new { Label = "Name", Content = "John Doe" },
                new { Label = "Age", Content = "30" }
            },
            LabelBinding = new Avalonia.Data.Binding("Label"),
        };
        var window = new Window
        {
            Content = descriptions
        };
        window.Show();
        var items = descriptions.GetLogicalChildren().OfType<DescriptionsItem>().ToList();
        foreach (var item in items)
        {
            Assert.Equal(100, item.LabelWidth);
        }
        
        descriptions.LabelWidth = new GridLength(150);
        foreach (var item in items)
        {
            Assert.Equal(150, item.LabelWidth);
        }
    }
}