using Avalonia.Controls;
using Avalonia.Controls.Shapes;
using Avalonia.Controls.Templates;
using Avalonia.Media;
using Ursa.Controls;

namespace Ursa.Demo.TemplateSelectors;

public class TimelineIconTemplateSelector: ResourceDictionary, IDataTemplate
{
    
    public Control? Build(object? param)
    {
        if (param is TimelineItemType t)
        {
            string s = t.ToString();
            if (ContainsKey(s))
            {
                object? o = this[s];
                if (o is Control c) return c;
            }
        }
        return null;
    }

    public bool Match(object? data)
    {
        return data is TimelineItemType;
    }
}