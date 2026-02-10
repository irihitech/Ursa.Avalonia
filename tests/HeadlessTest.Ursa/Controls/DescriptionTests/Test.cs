using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Data;
using Avalonia.Headless.XUnit;
using Avalonia.LogicalTree;
using Avalonia.Threading;
using HeadlessTest.Ursa.TestHelpers;
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
            LabelMemberBinding = new Binding("Label"),
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
            LabelMemberBinding = new Binding("Label"),
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
            LabelMemberBinding = new Binding("Label"),
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

    [AvaloniaFact]
    public void DescriptionItem_Colon_Visibility_When_ItemAlignment_Is_Plain()
    {
        // Test case 1: ItemAlignment=Plain with non-empty Label - colon should be visible
        var itemWithLabel = new DescriptionsItem
        {
            Label = "Name",
            Content = "John Doe",
            ItemAlignment = ItemAlignment.Plain,
            LabelPosition = Position.Left
        };
        var window1 = new Window { Content = itemWithLabel };
        window1.Show();
        Dispatcher.UIThread.RunJobs();

        var colon1 = itemWithLabel.GetTemplateChildOfType<TextBlock>("PART_Colon");
        Assert.NotNull(colon1);
        Assert.True(colon1.IsVisible, "Colon should be visible when ItemAlignment=Plain and Label is not empty");

        window1.Close();

        // Test case 2: ItemAlignment=Plain with null Label - colon should be hidden
        var itemWithoutLabel = new DescriptionsItem
        {
            Label = null,
            Content = "John Doe",
            ItemAlignment = ItemAlignment.Plain,
            LabelPosition = Position.Left
        };
        var window2 = new Window { Content = itemWithoutLabel };
        window2.Show();
        Dispatcher.UIThread.RunJobs();

        var colon2 = itemWithoutLabel.GetTemplateChildOfType<TextBlock>("PART_Colon");
        Assert.NotNull(colon2);
        Assert.False(colon2.IsVisible, "Colon should be hidden when ItemAlignment=Plain and Label is null");

        window2.Close();

        // Test case 3: ItemAlignment=Plain with empty string Label - colon should be hidden
        var itemWithEmptyLabel = new DescriptionsItem
        {
            Label = string.Empty,
            Content = "John Doe",
            ItemAlignment = ItemAlignment.Plain,
            LabelPosition = Position.Left
        };
        var window3 = new Window { Content = itemWithEmptyLabel };
        window3.Show();
        Dispatcher.UIThread.RunJobs();

        var colon3 = itemWithEmptyLabel.GetTemplateChildOfType<TextBlock>("PART_Colon");
        Assert.NotNull(colon3);
        Assert.False(colon3.IsVisible, "Colon should be hidden when ItemAlignment=Plain and Label is empty string");

        window3.Close();

        // Test case 4: ItemAlignment=Center with Label - colon should be hidden (per existing style)
        var itemWithCenterAlignment = new DescriptionsItem
        {
            Label = "Age",
            Content = "30",
            ItemAlignment = ItemAlignment.Center,
            LabelPosition = Position.Left
        };
        var window4 = new Window { Content = itemWithCenterAlignment };
        window4.Show();
        Dispatcher.UIThread.RunJobs();

        var colon4 = itemWithCenterAlignment.GetTemplateChildOfType<TextBlock>("PART_Colon");
        Assert.NotNull(colon4);
        Assert.False(colon4.IsVisible, "Colon should be hidden when ItemAlignment=Center (regardless of Label)");

        window4.Close();
    }
}