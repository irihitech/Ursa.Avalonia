using Avalonia.Controls;
using Avalonia.Interactivity;
using Irihi.Avalonia.Shared.Contracts;
using Ursa.Controls;

namespace HeadlessTest.Ursa.Controls.OverlayShared.Case_Close_Dialog_Clear_Content_Parent;

public partial class TestWindow : UrsaWindow
{
    public TestWindow()
    {
        InitializeComponent();
    }

    internal TextBox TextBox = new TextBox();
    internal DialogViewModel DialogViewModel = new DialogViewModel();
    
    private void Button_OnClick(object? sender, RoutedEventArgs e)
    {
        OverlayDialog.Show(TextBox, DialogViewModel);
    }
}

class DialogViewModel : IDialogContext
{
    public void Close()
    {
        RequestClose?.Invoke(this, null);
    }

    public event EventHandler<object?>? RequestClose;
}