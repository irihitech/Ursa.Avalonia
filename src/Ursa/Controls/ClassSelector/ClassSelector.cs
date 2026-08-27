using System.Collections;
using System.Collections.Specialized;
using Avalonia;
using Avalonia.Controls;
using Avalonia.LogicalTree;
using Irihi.Avalonia.Shared.Helpers;

namespace Ursa.Controls;

public class ClassSelector : MultiComboBox
{
    private readonly List<StyledElement> _targets = [];

    public static readonly StyledProperty<IList?> SelectedClassesProperty = SelectedItemsProperty;

    public static readonly StyledProperty<Control?> TargetProperty =
        AvaloniaProperty.Register<ClassSelector, Control?>(nameof(Target));

    public static readonly AttachedProperty<ClassSelector?> SourceProperty =
        AvaloniaProperty.RegisterAttached<ClassSelector, StyledElement, ClassSelector?>("Source");

    static ClassSelector()
    {
        TargetProperty.Changed.AddClassHandler<ClassSelector>((selector, _) => selector.ApplyClassesToTargets());
        SelectedClassesProperty.Changed.AddClassHandler<ClassSelector, IList?>((selector, args) =>
            selector.OnSelectedClassesChanged(args));
        SourceProperty.Changed.AddClassHandler<StyledElement, ClassSelector?>(OnSourceChanged);
    }

    public IList? SelectedClasses
    {
        get => GetValue(SelectedClassesProperty);
        set => SetValue(SelectedClassesProperty, value);
    }

    public Control? Target
    {
        get => GetValue(TargetProperty);
        set => SetValue(TargetProperty, value);
    }

    public static void SetSource(StyledElement element, ClassSelector? source) =>
        element.SetValue(SourceProperty, source);

    public static ClassSelector? GetSource(StyledElement element) =>
        element.GetValue(SourceProperty);

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
            ApplyClassesToTargets();
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

        ApplyClassesToTargets();
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

        ApplyClassesToTargets();
    }

    public override void Clear()
    {
        SelectedClasses?.Clear();
        foreach (var item in this.GetLogicalDescendants().OfType<ClassSelectorItem>())
        {
            item.SetSelectionFromSelector(false);
        }

        ApplyClassesToTargets();
    }

    private static void OnSourceChanged(
        StyledElement target,
        AvaloniaPropertyChangedEventArgs<ClassSelector?> args)
    {
        args.OldValue.Value?._targets.Remove(target);

        var source = args.NewValue.Value;
        if (source is null)
        {
            return;
        }

        if (!source._targets.Contains(target))
        {
            source._targets.Add(target);
        }

        source.ApplyClasses(target);
    }

    private void OnSelectedClassesChanged(AvaloniaPropertyChangedEventArgs<IList?> args)
    {
        if (args.OldValue.Value is INotifyCollectionChanged oldCollection)
        {
            oldCollection.CollectionChanged -= OnSelectedClassesCollectionChanged;
        }

        if (args.NewValue.Value is INotifyCollectionChanged newCollection)
        {
            newCollection.CollectionChanged += OnSelectedClassesCollectionChanged;
        }

        ApplyClassesToTargets();
    }

    private void OnSelectedClassesCollectionChanged(object? sender, NotifyCollectionChangedEventArgs args)
    {
        ApplyClassesToTargets();
    }

    private void ApplyClassesToTargets()
    {
        if (Target is not null)
        {
            ApplyClasses(Target);
        }

        foreach (var target in _targets)
        {
            ApplyClasses(target);
        }
    }

    private void ApplyClasses(StyledElement target)
    {
        target.Classes.Replace(SelectedClasses?.OfType<string>().ToList() ?? []);
    }

}
