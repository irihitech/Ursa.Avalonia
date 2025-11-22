using Avalonia.Controls;
using Avalonia.Controls.Templates;
using Ursa.Common;

namespace Ursa.Controls;

internal interface IIconButton
{
    object? Icon { get; set; }
    IDataTemplate? IconTemplate { get; set; }
    Position IconPlacement { get; set; }
    IPseudoClasses PseudoClasses { get; }
}