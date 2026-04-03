namespace Ursa.Controls;

internal record RangePickerStatus
{
    public Status Previous { get; private set; }
    public Status Current { get; private set; }

    public void Reset()
    {
        Previous = Status.None;
        Current = Status.None;
    }

    public void Push(Status status)
    {
        if (Current == status) return;
        Previous = Current;
        Current = status;
    }
}