using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Ursa.Controls;

namespace Test.Ursa.AnchorTests;

public class AnchorUnitTests
{
    [Fact]
    public void Anchor_Properties_Should_Have_Correct_Default_Values()
    {
        var anchor = new Anchor();
        
        Assert.Null(anchor.TargetContainer);
        Assert.Equal(0.0, anchor.TopOffset);
    }

    [Fact]
    public void Anchor_Should_Set_And_Get_TargetContainer()
    {
        var anchor = new Anchor();
        var scrollViewer = new ScrollViewer();
        
        anchor.TargetContainer = scrollViewer;
        Assert.Equal(scrollViewer, anchor.TargetContainer);
        
        anchor.TargetContainer = null;
        Assert.Null(anchor.TargetContainer);
    }

    [Fact]
    public void Anchor_Should_Set_And_Get_TopOffset()
    {
        var anchor = new Anchor();
        
        anchor.TopOffset = 25.5;
        Assert.Equal(25.5, anchor.TopOffset);
        
        anchor.TopOffset = -10.0;
        Assert.Equal(-10.0, anchor.TopOffset);
        
        anchor.TopOffset = 0.0;
        Assert.Equal(0.0, anchor.TopOffset);
    }

    [Fact]
    public void Anchor_Id_AttachedProperty_Should_Work_With_Different_Visual_Types()
    {
        var border = new Border();
        var textBlock = new TextBlock();
        var stackPanel = new StackPanel();
        
        // Test with Border
        Anchor.SetId(border, "border-id");
        Assert.Equal("border-id", Anchor.GetId(border));
        
        // Test with TextBlock
        Anchor.SetId(textBlock, "text-id");
        Assert.Equal("text-id", Anchor.GetId(textBlock));
        
        // Test with StackPanel
        Anchor.SetId(stackPanel, "panel-id");
        Assert.Equal("panel-id", Anchor.GetId(stackPanel));
        
        // Test setting to null
        Anchor.SetId(border, null);
        Assert.Null(Anchor.GetId(border));
    }

    [Fact]
    public void AnchorItem_Properties_Should_Have_Correct_Default_Values()
    {
        var anchorItem = new AnchorItem();
        
        Assert.Null(anchorItem.AnchorId);
        Assert.False(anchorItem.IsSelected);
        Assert.Equal(0, anchorItem.Level); // Default level before attachment
    }

    [Fact]
    public void AnchorItem_Should_Set_And_Get_AnchorId()
    {
        var anchorItem = new AnchorItem();
        
        anchorItem.AnchorId = "test-anchor";
        Assert.Equal("test-anchor", anchorItem.AnchorId);
        
        anchorItem.AnchorId = null;
        Assert.Null(anchorItem.AnchorId);
        
        anchorItem.AnchorId = "";
        Assert.Equal("", anchorItem.AnchorId);
    }

    [Fact]
    public void AnchorItem_Should_Set_And_Get_IsSelected()
    {
        var anchorItem = new AnchorItem();
        
        anchorItem.IsSelected = true;
        Assert.True(anchorItem.IsSelected);
        
        anchorItem.IsSelected = false;
        Assert.False(anchorItem.IsSelected);
    }

    [Fact]
    public void Anchor_Should_Create_AnchorItem_Containers()
    {
        var anchor = new Anchor();
        
        // Test NeedsContainer
        var needsContainer = anchor.NeedsContainerOverrideInternal("test-item", 0, out var recycleKey);
        Assert.True(needsContainer);
        
        // Test CreateContainer
        var container = anchor.CreateContainerForItemOverrideInternal("test-item", 0, recycleKey);
        Assert.IsType<AnchorItem>(container);
    }
}