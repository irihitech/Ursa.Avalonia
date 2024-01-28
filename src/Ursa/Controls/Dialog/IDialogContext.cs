namespace Ursa.Controls;

public interface IDialogContext
{
    void Close();
    event EventHandler<object?> RequestClose;
}