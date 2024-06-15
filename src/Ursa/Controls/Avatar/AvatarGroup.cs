using Avalonia;
using Avalonia.Controls;

namespace Ursa.Controls;

public class AvatarGroup : ItemsControl
{
    public const string PART_RenderMore = "PART_RenderMore";

    public static readonly StyledProperty<int> MaxCountProperty = AvaloniaProperty.Register<AvatarGroup, int>(
        nameof(MaxCount));

    public static readonly StyledProperty<OverlapFromType> OverlapFromProperty = AvaloniaProperty.Register<AvatarGroup, OverlapFromType>(
        nameof(OverlapFrom));

    public int MaxCount
    {
        get => GetValue(MaxCountProperty);
        set => SetValue(MaxCountProperty, value);
    }

    public OverlapFromType OverlapFrom
    {
        get => GetValue(OverlapFromProperty);
        set => SetValue(OverlapFromProperty, value);
    }
}

public enum OverlapFromType
{
    Start,
    End
}