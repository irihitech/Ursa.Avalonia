using Irihi.Avalonia.Shared.Contracts;

namespace HeadlessTest.Ursa.Controls;

public class DrawerCloseTestPopupControlVM : IDialogContext
{
    public void Close()
    {
        RequestClose?.Invoke(this, Result);
    }
    
#if NET8_0_OR_GREATER
    public int Result { get; } = Random.Shared.Next();
#else
    private static Random r = new Random();
    public int Result { get; } = r.Next();
#endif    
    public event EventHandler<object?>? RequestClose;
}