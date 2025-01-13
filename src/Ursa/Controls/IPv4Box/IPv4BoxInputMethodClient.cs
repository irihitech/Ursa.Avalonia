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
    public void SetPresenter(TextPresenter? presenter, IPv4Box? parent)
    {
        if (this._parent != null)
            this._parent.PropertyChanged -= new EventHandler<AvaloniaPropertyChangedEventArgs>(this.OnParentPropertyChanged);
        this._parent = parent;
        if (this._parent != null)
            this._parent.PropertyChanged += new EventHandler<AvaloniaPropertyChangedEventArgs>(this.OnParentPropertyChanged);
        TextPresenter presenter1 = this._presenter;
        if (presenter1 != null)
        {
            presenter1.CaretBoundsChanged -= (EventHandler) ((s, e) => this.RaiseCursorRectangleChanged());
        }
        this._presenter = presenter;
        if (this._presenter != null)
            this._presenter.CaretBoundsChanged += (EventHandler) ((s, e) => this.RaiseCursorRectangleChanged());
        this.RaiseTextViewVisualChanged();
        this.RaiseCursorRectangleChanged();
    }
    
    private void OnParentPropertyChanged(object? sender, AvaloniaPropertyChangedEventArgs e)
    {
        this.RaiseSelectionChanged();
    }
}