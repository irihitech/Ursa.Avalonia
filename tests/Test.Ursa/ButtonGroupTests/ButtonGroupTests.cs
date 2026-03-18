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
}