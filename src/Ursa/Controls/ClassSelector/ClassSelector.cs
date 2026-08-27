using System.Collections;
using Avalonia;
using Avalonia.Controls;
using Avalonia.LogicalTree;

namespace Ursa.Controls;

public class ClassSelector : MultiComboBox
{
    public static readonly StyledProperty<IList?> SelectedClassesProperty = SelectedItemsProperty;

    public IList? SelectedClasses
    {
        get => GetValue(SelectedClassesProperty);
        set => SetValue(SelectedClassesProperty, value);
    }

    protected override bool NeedsContainerOverride(object? item, int index, out object? recycleKey)
    {
        return NeedsContainer<ClassSelectorGroup>(item, out recycleKey);
    }

    protected override Control CreateContainerForItemOverride(object? item, int index, object? recycleKey)
    {
        return new ClassSelectorGroup();
    }

    protected override void PrepareContainerForItemOverride(Control container, object? item, int index)
    {
        if (item is ClassSelectorGroup)
        {
            return;
        }

        base.PrepareContainerForItemOverride(container, item, index);
    }

    internal void UpdateSelection(ClassSelectorItem selectedItem, bool isSelected)
    {
        var className = selectedItem.GetClassName();
        if (string.IsNullOrWhiteSpace(className))
        {
            return;
        }

        if (!isSelected)
        {
            SelectedClasses?.Remove(className);
            return;
        }

        selectedItem.SetSelectionFromSelector(true);
        var group = selectedItem.FindLogicalAncestorOfType<ClassSelectorGroup>();
        if (group is not null)
        {
            foreach (var sibling in group.GetLogicalDescendants().OfType<ClassSelectorItem>())
            {
                if (ReferenceEquals(sibling, selectedItem) || !sibling.IsSelected)
                {
                    continue;
                }

                sibling.SetSelectionFromSelector(false);
                var siblingClass = sibling.GetClassName();
                if (siblingClass is not null)
                {
                    SelectedClasses?.Remove(siblingClass);
                }
            }
        }

        if (SelectedClasses?.Contains(className) == false)
        {
            SelectedClasses.Add(className);
        }
    }

    public override void Remove(object? value)
    {
        if (value is not StyledElement element || element.DataContext?.ToString() is not { } className)
        {
            return;
        }

        SelectedClasses?.Remove(className);
        foreach (var item in this.GetLogicalDescendants().OfType<ClassSelectorItem>())
        {
            if (item.GetClassName() == className)
            {
                item.SetSelectionFromSelector(false);
            }
        }
    }

    public override void Clear()
    {
        SelectedClasses?.Clear();
        foreach (var item in this.GetLogicalDescendants().OfType<ClassSelectorItem>())
        {
            item.SetSelectionFromSelector(false);
        }
    }

}
