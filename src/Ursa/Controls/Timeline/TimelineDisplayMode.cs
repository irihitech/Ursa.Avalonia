namespace Ursa.Controls;

public enum TimelineDisplayMode
{
    Left,
    Center,
    Right,
    Alternate,
}

/// <summary>
/// Placement of timeline.
/// Left means line is placed left to TimelineItem content.
/// Right means line is placed right to TimelineItem content.
/// Separate means line is placed between TimelineItem content and time.
/// </summary>
public enum TimelineItemPosition
{
    Left,
    Right,
    Separate,
}