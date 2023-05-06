using Avalonia;
using Avalonia.Collections;
using Avalonia.Controls.Primitives;

namespace Ursa.Controls;

public class PaginationExpandButton: TemplatedControl
{
    public static readonly StyledProperty<AvaloniaList<int>> PagesProperty = AvaloniaProperty.Register<PaginationExpandButton, AvaloniaList<int>>(
        nameof(Pages));

    public AvaloniaList<int> Pages
    {
        get => GetValue(PagesProperty);
        set => SetValue(PagesProperty, value);
    }
    
    
}