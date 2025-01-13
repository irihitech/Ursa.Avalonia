using Avalonia;
using Avalonia.Controls.Presenters;
using Avalonia.Input.TextInput;

namespace Ursa.Controls;

public class IPv4BoxInputMethodClient:TextInputMethodClient
{
    private TextPresenter? _presenter;
    public override Visual TextViewVisual => _presenter;
    public override bool SupportsPreedit => false;
    public override bool SupportsSurroundingText => true;

    public override string SurroundingText
    {
        get;
    }
    public override Rect CursorRectangle { get; }
    public override TextSelection Selection { get; set; }
    private IPv4Box? _parent;
    public void SetPresenter(TextPresenter? presenter)
    {
        this.RaiseTextViewVisualChanged();
        this.RaiseCursorRectangleChanged();
    }
    
    private void OnParentPropertyChanged(object? sender, AvaloniaPropertyChangedEventArgs e)
    {
        this.RaiseSelectionChanged();
    }
}