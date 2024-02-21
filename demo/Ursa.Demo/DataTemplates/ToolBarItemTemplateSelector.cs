using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Controls.Templates;
using Avalonia.Data;
using Ursa.Controls;
using Ursa.Demo.ViewModels;

namespace Ursa.Demo.Converters;

public class ToolBarItemTemplateSelector: IDataTemplate
{
    public static ToolBarItemTemplateSelector Instance { get; } = new();
    public Control? Build(object? param)
    {
        if (param is null) return null;
        if (param is ToolBarButtonItemViewModel vm)
        {
            return new Button()
            {
                [!ContentControl.ContentProperty] = new Binding() { Path = "Content" },
                [!Button.CommandProperty] = new Binding() { Path = "Command" },
                [!ToolBar.OverflowModeProperty] = new Binding(){ Path = "OverflowMode" }
            };
        }
        if (param is ToolBarCheckBoxItemViweModel cb)
        {
            return new CheckBox()
            {
                [!ContentControl.ContentProperty] = new Binding() { Path = "Content" },
                [!ToggleButton.IsCheckedProperty] = new Binding() { Path = "IsChecked" },
                [!ToolBar.OverflowModeProperty] = new Binding(){ Path = "OverflowMode" }
            };
        }
        if (param is ToolBarComboBoxItemViewModel combo)
        {
            return new ComboBox()
            {
                [!ContentControl.ContentProperty] = new Binding() { Path = "Content" },
                [!SelectingItemsControl.SelectedItemProperty] = new Binding() { Path = "SelectedItem" },
                [!ItemsControl.ItemsSourceProperty] = new Binding() { Path = "Items" },
                [ToolBar.OverflowModeProperty] = OverflowMode.Always,
            };
        }
        return new Button() { Content = "Undefined Item" };
    }

    public bool Match(object? data)
    {
        return data is ToolBarItemViewModel;
    }
}