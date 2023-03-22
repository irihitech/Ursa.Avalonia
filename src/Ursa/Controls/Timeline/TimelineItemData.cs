namespace Ursa.Controls;

public interface ITimelineItemData
{
    public DateTime Time { get; set; }
    public object Content { get; set; }
    public object Description { get; set; }
    public TimelineItemType ItemType { get; set; }
}