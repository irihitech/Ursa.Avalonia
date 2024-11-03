using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Irihi.Avalonia.Shared.Contracts;
using Ursa.Controls;

namespace HeadlessTest.Ursa.Controls.UrsaWindowTests;

public partial class UrsaWindowWithCloseInference : UrsaWindow
{
    public UrsaWindowWithCloseInference()
    {
        InitializeComponent();
    }
    
    internal TextBox TextBox = new TextBox();
    internal DialogViewModel DialogViewModel = new DialogViewModel();

    protected override async Task<bool> CanClose()
    {
        var result = await OverlayDialog.ShowModal(TextBox, DialogViewModel);
        return result == DialogResult.Yes;
    }
}

class DialogViewModel : IDialogContext
{
    public void Close()
    {
        RequestClose?.Invoke(this, DialogResult.No);
    }
    
    public void CloseYes()
    {
        RequestClose?.Invoke(this, DialogResult.Yes);
    }

    public event EventHandler<object?>? RequestClose;
}