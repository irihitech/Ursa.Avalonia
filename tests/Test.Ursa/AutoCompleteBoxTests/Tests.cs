using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Input;
using Avalonia.Remote.Protocol.Input;
using AutoCompleteBox = Ursa.Controls.AutoCompleteBox;

namespace Test.Ursa.AutoCompleteBoxTests;

public class Tests
{
    [Fact]
    public void Clear_SetsSelectedItemToNull()
    {
        var autoCompleteBox = new AutoCompleteBox();
        autoCompleteBox.SelectedItem = "TestItem";
        autoCompleteBox.Clear();
        Assert.Null(autoCompleteBox.SelectedItem);
    }
}