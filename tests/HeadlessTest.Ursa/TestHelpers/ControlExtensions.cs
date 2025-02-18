using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Controls.Templates;

namespace HeadlessTest.Ursa.TestHelpers;

public static class ControlExtensions
{
    public static IEnumerable<T> GetTemplateChildrenOfType<T>(this TemplatedControl control) where T: Control
    {
        return control.GetTemplateChildren().OfType<T>();
    }
    public static T? GetTemplateChildOfType<T>(this TemplatedControl control, string name) where T : Control
    {
        return control.GetTemplateChildren().OfType<T>().FirstOrDefault(a => a.Name == name);
    }
}