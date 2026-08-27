using System.Collections.ObjectModel;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Headless.XUnit;
using Ursa.Controls;

namespace HeadlessTest.Ursa.Controls.ClassSelectorTests;

public class ClassSelectorTests
{
    [AvaloniaFact]
    public void Selecting_Items_In_Different_Groups_Should_Keep_Both_Classes()
    {
        var (_, selector, small, _, left, _) = CreateSelector();

        small.IsSelected = true;
        left.IsSelected = true;

        Assert.Equal(["Small", "Left"], selector.SelectedClasses!.Cast<string>());
        Assert.True(small.IsSelected);
        Assert.True(left.IsSelected);
    }

    [AvaloniaFact]
    public void Selecting_Another_Item_In_Same_Group_Should_Replace_Previous_Class()
    {
        var (_, selector, small, large, _, _) = CreateSelector();

        small.IsSelected = true;
        large.IsSelected = true;

        Assert.Equal(["Large"], selector.SelectedClasses!.Cast<string>());
        Assert.False(small.IsSelected);
        Assert.True(large.IsSelected);
    }

    [AvaloniaFact]
    public void Deselecting_Item_Should_Remove_Its_Class()
    {
        var (_, selector, small, _, _, _) = CreateSelector();

        small.IsSelected = true;
        small.IsSelected = false;

        Assert.Empty(selector.SelectedClasses!);
    }

    [AvaloniaFact]
    public void Initial_SelectedClasses_Should_Select_Items_When_Attached()
    {
        var selectedClasses = new ObservableCollection<string> { "Large", "Left" };
        var (_, _, small, large, left, _) = CreateSelector(selectedClasses);

        Assert.False(small.IsSelected);
        Assert.True(large.IsSelected);
        Assert.True(left.IsSelected);
    }

    [AvaloniaFact]
    public void Initial_Selection_Should_Keep_Only_Last_Class_In_Each_Group()
    {
        var selectedClasses = new ObservableCollection<string> { "Small", "Large" };
        var (_, selector, small, large, _, _) = CreateSelector(selectedClasses);

        Assert.Equal(["Large"], selector.SelectedClasses.Cast<string>());
        Assert.False(small.IsSelected);
        Assert.True(large.IsSelected);
    }

    [AvaloniaFact]
    public void Declaratively_Selected_Item_Should_Populate_SelectedClasses()
    {
        var selectedItem = new ClassSelectorItem
        {
            ClassName = "Small",
            Content = "Small",
            IsSelected = true
        };
        var group = new ClassSelectorGroup { Header = "Size" };
        group.Items.Add(selectedItem);
        var selector = new ClassSelector();
        selector.Items.Add(group);
        var window = new Window { Content = selector };

        window.Show();

        Assert.Contains("Small", selector.SelectedClasses!.Cast<string>());
        Assert.True(selectedItem.IsSelected);
    }

    [AvaloniaFact]
    public void SelectedClasses_Should_Alias_SelectedItems()
    {
        var selector = new ClassSelector();
        var selectedClasses = new ObservableCollection<string> { "Small" };

        selector.SelectedClasses = selectedClasses;

        Assert.Same(selectedClasses, selector.SelectedItems);
    }

    [AvaloniaFact]
    public void Remove_Should_Deselect_Nested_Class_Item()
    {
        var (_, selector, small, _, left, _) = CreateSelector();
        small.IsSelected = true;
        left.IsSelected = true;

        selector.Remove(new Border { DataContext = "Small" });

        Assert.False(small.IsSelected);
        Assert.True(left.IsSelected);
        Assert.Equal(["Left"], selector.SelectedClasses!.Cast<string>());
    }

    [AvaloniaFact]
    public void Clear_Should_Deselect_All_Nested_Class_Items()
    {
        var (_, selector, small, _, left, _) = CreateSelector();
        small.IsSelected = true;
        left.IsSelected = true;

        selector.Clear();

        Assert.False(small.IsSelected);
        Assert.False(left.IsSelected);
        Assert.Empty(selector.SelectedClasses!);
    }

    [AvaloniaFact]
    public void Target_Should_Receive_Selected_Classes()
    {
        var (_, selector, small, large, left, _) = CreateSelector();
        var target = new Button { Classes = { "Existing" } };
        selector.Target = target;

        small.IsSelected = true;
        left.IsSelected = true;
        large.IsSelected = true;

        Assert.Equal(["Left", "Large"], GetStyleClasses(target));
    }

    [AvaloniaFact]
    public void Source_Should_Update_Attached_Targets()
    {
        var (_, selector, small, _, left, _) = CreateSelector();
        var firstTarget = new Button();
        var secondTarget = new TextBox();
        ClassSelector.SetSource(firstTarget, selector);
        ClassSelector.SetSource(secondTarget, selector);

        small.IsSelected = true;
        left.IsSelected = true;

        Assert.Equal(["Small", "Left"], GetStyleClasses(firstTarget));
        Assert.Equal(["Small", "Left"], GetStyleClasses(secondTarget));
    }

    [AvaloniaFact]
    public void Changing_Source_Should_Stop_Updating_Previous_Selector_Target()
    {
        var (_, firstSelector, small, _, _, _) = CreateSelector();
        var (_, secondSelector, _, _, left, _) = CreateSelector();
        var target = new Button();
        ClassSelector.SetSource(target, firstSelector);
        small.IsSelected = true;

        ClassSelector.SetSource(target, secondSelector);
        left.IsSelected = true;
        small.IsSelected = false;

        Assert.Equal(["Left"], GetStyleClasses(target));
    }

    [AvaloniaFact]
    public void External_SelectedClasses_Changes_Should_Update_Target()
    {
        var selectedClasses = new ObservableCollection<string>();
        var (_, selector, _, _, _, _) = CreateSelector(selectedClasses);
        var target = new Button();
        selector.Target = target;

        selectedClasses.Add("Small");
        selectedClasses.Add("Left");

        Assert.Equal(["Small", "Left"], GetStyleClasses(target));
    }

    private static IEnumerable<string> GetStyleClasses(StyledElement element) =>
        element.Classes.Where(className => !className.StartsWith(':'));

    private static (
        Window Window,
        ClassSelector Selector,
        ClassSelectorItem Small,
        ClassSelectorItem Large,
        ClassSelectorItem Left,
        ClassSelectorItem Right) CreateSelector(ObservableCollection<string>? selectedClasses = null)
    {
        var small = new ClassSelectorItem { ClassName = "Small", Content = "Small" };
        var large = new ClassSelectorItem { ClassName = "Large", Content = "Large" };
        var left = new ClassSelectorItem { ClassName = "Left", Content = "Left" };
        var right = new ClassSelectorItem { ClassName = "Right", Content = "Right" };

        var sizeGroup = new ClassSelectorGroup { Header = "Size" };
        sizeGroup.Items.Add(small);
        sizeGroup.Items.Add(large);

        var alignmentGroup = new ClassSelectorGroup { Header = "Alignment" };
        alignmentGroup.Items.Add(left);
        alignmentGroup.Items.Add(right);

        var selector = new ClassSelector
        {
            SelectedClasses = selectedClasses ?? new ObservableCollection<string>()
        };
        selector.Items.Add(sizeGroup);
        selector.Items.Add(alignmentGroup);

        var window = new Window { Content = selector };
        window.Show();

        return (window, selector, small, large, left, right);
    }
}
