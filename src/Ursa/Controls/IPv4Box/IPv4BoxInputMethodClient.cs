using Avalonia;
using Avalonia.Controls.Presenters;
using Avalonia.Input.TextInput;
using Avalonia.Media.TextFormatting;
using System.Text;

namespace Ursa.Controls;

public class IPv4BoxInputMethodClient: TextInputMethodClient
{
    private TextPresenter? _presenter;
    public override Visual TextViewVisual => _presenter!;
    public override bool SupportsPreedit => false;
    public override bool SupportsSurroundingText => true;

    public override string SurroundingText
    {
		get
		{
			if (_presenter is null)
			{
				return "";
			}

			var lineIndex = _presenter.TextLayout.GetLineIndexFromCharacterIndex(_presenter.CaretIndex, false);

			var textLine = _presenter.TextLayout.TextLines[lineIndex];

			var lineText = GetTextLineText(textLine);

			return lineText;
		}
	}

	private static string GetTextLineText(TextLine textLine)
	{
		if (textLine.Length == 0)
		{
			return string.Empty;
		}

		var builder = new StringBuilder();

		foreach (var run in textLine.TextRuns)
		{
			if (run.Length > 0)
			{
#if NET6_0_OR_GREATER
				builder.Append(run.Text.Span);
#else
                    builder.Append(run.Text.Span.ToArray());
#endif
			}
		}

		var lineText = builder.ToString();

		return lineText;
	}

	public override Rect CursorRectangle { get; } = new();
	public override TextSelection Selection
	{
		get
		{
			if (_presenter is null)
			{
				return default;
			}

			var lineIndex = _presenter.TextLayout.GetLineIndexFromCharacterIndex(_presenter.CaretIndex, false);

			var textLine = _presenter.TextLayout.TextLines[lineIndex];

			var lineStart = textLine.FirstTextSourceIndex;

			var selectionStart = Math.Max(0, _presenter.SelectionStart - lineStart);

			var selectionEnd = Math.Max(0, _presenter.SelectionEnd - lineStart);

			return new TextSelection(selectionStart, selectionEnd);
		}
		set
		{
			if (_presenter is null)
			{
				return;
			}

			var lineIndex = _presenter.TextLayout.GetLineIndexFromCharacterIndex(_presenter.CaretIndex, false);

			var textLine = _presenter.TextLayout.TextLines[lineIndex];

			var lineStart = textLine.FirstTextSourceIndex;

			var selectionStart = lineStart + value.Start;
			var selectionEnd = lineStart + value.End;

			_presenter.SelectionStart = selectionStart;
			_presenter.SelectionEnd = selectionEnd;

			RaiseSelectionChanged();
		}
	}
	public void SetPresenter(TextPresenter? presenter)
    {
        _presenter = presenter;
        this.RaiseTextViewVisualChanged();
        this.RaiseCursorRectangleChanged();
    }
}