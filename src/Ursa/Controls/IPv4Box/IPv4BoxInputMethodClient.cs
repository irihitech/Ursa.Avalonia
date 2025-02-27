using Avalonia;
using Avalonia.Controls.Presenters;
using Avalonia.Input.TextInput;

namespace Ursa.Controls;

public class IPv4BoxInputMethodClient: TextInputMethodClient
{
    private TextPresenter? _presenter;
    public override Visual TextViewVisual => _presenter!;
    public override bool SupportsPreedit => false;
    public override bool SupportsSurroundingText => true;

    public override string SurroundingText { get; } = null!;

    public override Rect CursorRectangle { get; } = new();
    public override TextSelection Selection { get; set; }
    public void SetPresenter(TextPresenter? presenter)
    {
        _presenter = presenter;
        this.RaiseTextViewVisualChanged();
        this.RaiseCursorRectangleChanged();
    }
}