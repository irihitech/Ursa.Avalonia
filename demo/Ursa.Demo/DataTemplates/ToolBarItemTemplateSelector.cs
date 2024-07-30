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
        if (param is ToolBarSeparatorViewModel)
        {
            return new ToolBarSeparator();
        }
        if (param is ToolBarButtonItemViewModel)
        {
            return new Button()
            {
                [!ContentControl.ContentProperty] = new Binding() { Path = "Content" },
                [!Button.CommandProperty] = new Binding() { Path = "Command" },
                [!ToolBar.OverflowModeProperty] = new Binding(){Path = nameof(ToolBarItemViewModel.OverflowMode)},
            };
        }
        if (param is ToolBarCheckBoxItemViweModel)
        {
            return new CheckBox()
            {
                [!ContentControl.ContentProperty] = new Binding() { Path = "Content" },
                [!ToggleButton.IsCheckedProperty] = new Binding() { Path = "IsChecked" },
                [!ToolBar.OverflowModeProperty] = new Binding(){Path = nameof(ToolBarItemViewModel.OverflowMode)},
            };
        }
        if (param is ToolBarComboBoxItemViewModel)
        {
            return new ComboBox()
            {
                [!ContentControl.ContentProperty] = new Binding() { Path = "Content" },
                [!SelectingItemsControl.SelectedItemProperty] = new Binding() { Path = "SelectedItem" },
                [!ItemsControl.ItemsSourceProperty] = new Binding() { Path = "Items" },
                [!ToolBar.OverflowModeProperty] = new Binding(){Path = nameof(ToolBarItemViewModel.OverflowMode)},
            };
        }
        return new Button() { Content = "Undefined Item" };
    }

    public bool Match(object? data)
    {
        return data is ToolBarItemViewModel;
    }
}