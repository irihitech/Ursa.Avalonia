using Irihi.Avalonia.Shared.Contracts;

namespace HeadlessTest.Ursa.Controls;

public class DrawerCloseTestPopupControlVM : IDialogContext
{
    public void Close()
    {
        RequestClose?.Invoke(this, Result);
    }

    public int Result { get; } = Random.Shared.Next();

    public event EventHandler<object?>? RequestClose;
}