using System.Windows.Input;
using Avalonia;
using Avalonia.Controls;

namespace Ursa.Controls;

public class MultiComboBoxSelectedItemList: ItemsControl
{
    public static readonly StyledProperty<ICommand?> RemoveCommandProperty = AvaloniaProperty.Register<MultiComboBoxSelectedItemList, ICommand?>(
        nameof(RemoveCommand));

    public ICommand? RemoveCommand
    {
        get => GetValue(RemoveCommandProperty);
        set => SetValue(RemoveCommandProperty, value);
    }
    
    protected override bool NeedsContainerOverride(object? item, int index, out object? recycleKey)
    {
        return NeedsContainer<ClosableTag>(item, out recycleKey);
    }

    protected override Control CreateContainerForItemOverride(object? item, int index, object? recycleKey)
    {
        return new ClosableTag();
    }

    protected override void PrepareContainerForItemOverride(Control container, object? item, int index)
    {
        base.PrepareContainerForItemOverride(container, item, index);
        if (container is ClosableTag tag)
        {
            tag.Command = RemoveCommand;
        }
    }
}