namespace Ursa.Controls;

public interface IDialogContext
{
    object Close();
    T? Close<T>();
}