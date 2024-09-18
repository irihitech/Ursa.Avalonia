namespace Ursa.Controls;

[Flags]
public enum ResizeDirection
{
    Top = 1,
    Bottom = 2,
    Left = 4,
    Right = 8,
    TopLeft = 16,
    TopRight = 32,
    BottomLeft = 64,
    BottomRight = 128,
    
    Sides = Top | Bottom | Left | Right,
    Corners = TopLeft | TopRight | BottomLeft | BottomRight,
    All = Sides | Corners,
}