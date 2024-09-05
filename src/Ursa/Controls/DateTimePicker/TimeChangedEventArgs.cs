using Avalonia.Interactivity;

namespace Ursa.Controls;

public class TimeChangedEventArgs:RoutedEventArgs
{
    public TimeSpan? OldTime { get; }

    public TimeSpan? NewTime { get; }

    public TimeChangedEventArgs(TimeSpan? oldTime, TimeSpan? newTime)
    {
        this.OldTime = oldTime;
        this.NewTime = newTime;
    }
}