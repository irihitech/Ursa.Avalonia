using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Primitives;
using Avalonia.Styling;

namespace Ursa.Controls;

[PseudoClasses(PC_Left, PC_Right, PC_Selected)]
public class PaginationButton: Button, IStyleable
{
    public const string PC_Left = ":left";
    public const string PC_Right = ":right";
    public const string PC_Selected = ":selected";

    public static readonly StyledProperty<int> PageProperty = AvaloniaProperty.Register<PaginationButton, int>(
        nameof(Page));

    public int Page
    {
        get => GetValue(PageProperty);
        set => SetValue(PageProperty, value);
    }
    public bool IsLeftForward { get; private set; }
    public bool IsRightForward { get; private set; }
    
    internal void SetStatus(int page, bool isSelected, bool isLeft, bool isRight)
    {
        PseudoClasses.Set(PC_Selected, isSelected);
        PseudoClasses.Set(PC_Left, isLeft);
        PseudoClasses.Set(PC_Right, isRight);
        IsLeftForward = isLeft;
        IsRightForward = isRight;
        Page = page;
    }

    internal void SetSelected(bool isSelected)
    {
        PseudoClasses.Set(PC_Selected, isSelected);
    }
}