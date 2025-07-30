using Avalonia.Data;
using Ursa.Controls;

namespace Test.Ursa.ButtonGroupTests;

public class ButtonGroupTests
{
    [Fact]
    public void ButtonGroup_Should_Initialize_With_Default_Values()
    {
        var buttonGroup = new ButtonGroup();
        
        Assert.Null(buttonGroup.CommandBinding);
        Assert.Null(buttonGroup.CommandParameterBinding);
        Assert.Null(buttonGroup.ContentBinding);
        Assert.NotNull(buttonGroup.Items);
        Assert.Empty(buttonGroup.Items);
    }

    [Fact]
    public void ButtonGroup_Should_Set_And_Get_CommandBinding()
    {
        var buttonGroup = new ButtonGroup();
        var binding = new Binding("TestCommand");
        
        buttonGroup.CommandBinding = binding;
        
        Assert.Equal(binding, buttonGroup.CommandBinding);
    }

    [Fact]
    public void ButtonGroup_Should_Set_And_Get_CommandParameterBinding()
    {
        var buttonGroup = new ButtonGroup();
        var binding = new Binding("TestParameter");
        
        buttonGroup.CommandParameterBinding = binding;
        
        Assert.Equal(binding, buttonGroup.CommandParameterBinding);
    }

    [Fact]
    public void ButtonGroup_Should_Set_And_Get_ContentBinding()
    {
        var buttonGroup = new ButtonGroup();
        var binding = new Binding("TestContent");
        
        buttonGroup.ContentBinding = binding;
        
        Assert.Equal(binding, buttonGroup.ContentBinding);
    }

    [Fact]
    public void ButtonGroup_Should_Accept_Items_In_Collection()
    {
        var buttonGroup = new ButtonGroup();
        var item1 = "Item 1";
        var item2 = new Avalonia.Controls.Button { Content = "Button Item" };
        
        buttonGroup.Items.Add(item1);
        buttonGroup.Items.Add(item2);
        
        Assert.Equal(2, buttonGroup.Items.Count);
        Assert.Contains(item1, buttonGroup.Items);
        Assert.Contains(item2, buttonGroup.Items);
    }
}