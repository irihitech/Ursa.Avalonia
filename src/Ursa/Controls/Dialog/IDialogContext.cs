namespace Ursa.Controls;

public interface IDialogContext
{
    object? DefaultCloseResult { get; set; }
    event EventHandler<object?> Closed;
}