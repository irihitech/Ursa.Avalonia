using System;
using Avalonia.Controls;
using Avalonia.Controls.Templates;
using Ursa.Demo.Pages;

namespace Ursa.Demo.Converters;

public class ViewLocator: IDataTemplate
{
    public Control? Build(object? param)
    {
        if (param is null) return null;
        var name = param.GetType().Name!.Replace("ViewModel", "");
        var type = Type.GetType("Ursa.Demo.Pages."+name);
        if (type != null)
        {
            return (Control)Activator.CreateInstance(type)!;
        }
        else
        {
            return new TextBlock { Text = "Not Found: " + name };
        }
    }

    public bool Match(object? data)
    {
        return true;
    }
}