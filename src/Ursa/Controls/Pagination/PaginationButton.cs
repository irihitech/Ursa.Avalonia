using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;

namespace Ursa.Controls;

[PseudoClasses(PC_Left, PC_Right, PC_Selected)]
public class PaginationButton: RepeatButton
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
    internal bool IsFastForward { get; private set; }
    internal bool IsFastBackward { get; private set; }
    
    internal void SetStatus(int page, bool isSelected, bool isLeft, bool isRight)
    {
        PseudoClasses.Set(PC_Selected, isSelected);
        PseudoClasses.Set(PC_Left, isLeft);
        PseudoClasses.Set(PC_Right, isRight);
        IsFastForward = isLeft;
        IsFastBackward = isRight;
        Page = page;
    }

    internal void SetSelected(bool isSelected)
    {
        PseudoClasses.Set(PC_Selected, isSelected);
    }
}