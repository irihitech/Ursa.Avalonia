using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Styling;

namespace Ursa.Controls;

public class PaginationButton: Button, IStyleable
{
    Type IStyleable.StyleKey => typeof(Button);
    
    public int Page { get; set; }
    
    
}