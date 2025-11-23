using System.Collections.Specialized;
using Avalonia;
using Avalonia.Controls;

namespace Ursa.Common;

/// <summary>
/// Utility class with attached properties allowing binding to and from <see cref="StyledElement.Classes"/> property.
/// </summary>
public class BindableClasses
{
    /// <summary>
    /// Enables or disables updates to <see cref="ClassesProperty"/> from <see cref="StyledElement.Classes"/>,
    /// which allows using a <see cref="StyledElement"/> as a source for binding.
    /// </summary>
    /// <remarks>
    /// Do not set both <see cref="IsClassesSourceProperty"/> and <see cref="ClassesProperty"/> on a control.
    /// A control can be either a source or a target of <see cref="Classes"/>, but not both at the same time.
    /// </remarks>
    public static readonly AttachedProperty<bool> IsClassesSourceProperty =
        AvaloniaProperty.RegisterAttached<BindableClasses, StyledElement, bool>(
            "IsClassesSource", defaultValue: false);

    public static void SetIsClassesSource(StyledElement obj, bool value) => obj.SetValue(IsClassesSourceProperty, value);
    public static bool GetIsClassesSource(StyledElement obj) => obj.GetValue(IsClassesSourceProperty);

    /// <summary>
    /// When set, replaces all non-pseudo classes of a control with classes from the space-separated value.
    /// To enable getting the classes as a string, use <see cref="IsClassesSourceProperty"/>.
    /// </summary>
    public static readonly AttachedProperty<string> ClassesProperty =
        AvaloniaProperty.RegisterAttached<BindableClasses, StyledElement, string>(
            "Classes", defaultValue: "", coerce: (_, v) => v ?? "");

    public static void SetClasses(StyledElement obj, string value) => obj.SetValue(ClassesProperty, value);
    public static string GetClasses(StyledElement obj) => obj.GetValue(ClassesProperty);

    static BindableClasses()
    {
        IsClassesSourceProperty.Changed.AddClassHandler<StyledElement>(IsClassesSourceProperty_OnChanged);
        ClassesProperty.Changed.AddClassHandler<StyledElement>(ClassesProperty_OnChanged);
    }

    private static void IsClassesSourceProperty_OnChanged(StyledElement element, AvaloniaPropertyChangedEventArgs e)
    {
        if (e.GetNewValue<bool>())
        {
            element.Classes.CollectionChanged += Classes_OnCollectionChanged;
            Classes_OnCollectionChanged(null, null);
        }
        else
        {
            element.Classes.CollectionChanged -= Classes_OnCollectionChanged;
        }

        void Classes_OnCollectionChanged(object? o, NotifyCollectionChangedEventArgs? args)
        {
            SetClasses(element, ClassesToString(element.Classes));
        }
    }

    private static void ClassesProperty_OnChanged(StyledElement element, AvaloniaPropertyChangedEventArgs e)
    {
        if (GetIsClassesSource(element))
            return;

        var oldClasses = ClassesToStrings(element.Classes);
        var newClasses = StringsToClasses(e.GetNewValue<string?>() ?? "");
        if (newClasses.SequenceEqual(oldClasses))
            return;

        element.Classes.Replace(newClasses);
    }

    private static string[] ClassesToStrings(Classes classes) =>
        classes.Where(c => !c.StartsWith(":", StringComparison.Ordinal)).ToArray();

    private static string ClassesToString(Classes classes) =>
        string.Join(" ", ClassesToStrings(classes));

    private static string[] StringsToClasses(string classes) =>
        classes.Split(' ');
}