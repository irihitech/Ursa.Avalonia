using Irihi.Avalonia.Shared.Contracts;

namespace HeadlessTest.Ursa.Controls;

public class DrawerCloseTestPopupControlVM : IDialogContext
{
    public void Close()
    {
        RequestClose?.Invoke(this, Result);
    }
    
#if NET8_0 
    public int Result { get; } = Random.Shared.Next();
#endif
    
#if NETSTANDARD2_0
    private static Random r = new Random();
    public int Result { get; } = r.Next();
#endif    
    public event EventHandler<object?>? RequestClose;
}