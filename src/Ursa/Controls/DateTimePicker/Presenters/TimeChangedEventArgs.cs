using Avalonia.Interactivity;

namespace Ursa.Controls;

public class TimeChangedEventArgs:RoutedEventArgs
{
    public TimeOnly? OldTime { get; }

    public TimeOnly? NewTime { get; }

    public TimeChangedEventArgs(TimeOnly? oldTime, TimeOnly? newTime)
    {
        this.OldTime = oldTime;
        this.NewTime = newTime;
    }
}
