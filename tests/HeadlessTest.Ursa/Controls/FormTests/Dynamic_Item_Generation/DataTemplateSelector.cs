using Avalonia.Controls;
using Avalonia.Controls.Templates;
using Avalonia.Metadata;

namespace HeadlessTest.Ursa.Controls.FormTests.Dynamic_Item_Generation;

internal class DataTemplateSelector : IDataTemplate
{
    [Content] public Dictionary<Type, IDataTemplate?> Templates { get; } = new();

    public Control? Build(object? param)
    {
        if (param is null) return null;
        var type = param.GetType();
        return Templates.TryGetValue(type, out var template) ? template?.Build(param) : null;
    }

    public bool Match(object? data)
    {
        return data is IFromItemViewModel;
    }
}