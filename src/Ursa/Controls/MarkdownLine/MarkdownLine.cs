using System.Text;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Documents;
using Avalonia.Media;

namespace Ursa.Controls;

/// <summary>
/// A lightweight <see cref="TextBlock"/> subclass that interprets a limited subset of
/// Markdown and renders the result through <see cref="TextBlock.Inlines"/>.
/// </summary>
/// <remarks>
/// <para>Supported inline formatting:</para>
/// <list type="bullet">
/// <item><c>**text**</c> or <c>__text__</c> — bold</item>
/// <item><c>*text*</c> or <c>_text_</c> — italic</item>
/// <item><c>~~text~~</c> — strikethrough</item>
/// <item><c>`code`</c> — inline code (monospace + optional background)</item>
/// <item>Nesting works wherever it makes sense.</item>
/// </list>
/// </remarks>
public class MarkdownLine : SelectableTextBlock
{
    protected override Type StyleKeyOverride { get; } = typeof(MarkdownLine);

    /// <summary>Defines the <see cref="Markdown"/> property.</summary>
    public static readonly StyledProperty<string?> MarkdownProperty =
        AvaloniaProperty.Register<MarkdownLine, string?>(nameof(Markdown));

    /// <summary>Defines the <see cref="CodeBackground"/> property.</summary>
    public static readonly StyledProperty<IBrush?> CodeBackgroundProperty =
        AvaloniaProperty.Register<MarkdownLine, IBrush?>(nameof(CodeBackground));

    /// <summary>Defines the <see cref="CodeFontFamily"/> property.</summary>
    public static readonly StyledProperty<FontFamily?> CodeFontFamilyProperty =
        AvaloniaProperty.Register<MarkdownLine, FontFamily?>(nameof(CodeFontFamily));

    private static readonly FontFamily s_defaultCodeFontFamily =
        new("Consolas, Menlo, monospace");

    static MarkdownLine()
    {
        MarkdownProperty.Changed.AddClassHandler<MarkdownLine>((x, e) => x.OnMarkdownChanged(e));
        CodeBackgroundProperty.Changed.AddClassHandler<MarkdownLine>((x, e) => x.OnMarkdownChanged(e));
        CodeFontFamilyProperty.Changed.AddClassHandler<MarkdownLine>((x, e) => x.OnMarkdownChanged(e));
        TextWrappingProperty.OverrideDefaultValue<MarkdownLine>(TextWrapping.Wrap);
    }

    /// <summary>
    /// Gets or sets the Markdown source text.  When set the text is parsed and
    /// the resulting inline elements replace <see cref="TextBlock.Inlines"/>.
    /// </summary>
    public string? Markdown
    {
        get => GetValue(MarkdownProperty);
        set => SetValue(MarkdownProperty, value);
    }

    /// <summary>
    /// Gets or sets the background brush applied to inline code spans
    /// (<c>`code`</c>).  Defaults to <see langword="null"/> (no background).
    /// </summary>
    public IBrush? CodeBackground
    {
        get => GetValue(CodeBackgroundProperty);
        set => SetValue(CodeBackgroundProperty, value);
    }

    /// <summary>
    /// Gets or sets the font family applied to inline code spans
    /// (<c>`code`</c>).  Defaults to <c>Consolas, Menlo, monospace</c>.
    /// </summary>
    public FontFamily? CodeFontFamily
    {
        get => GetValue(CodeFontFamilyProperty);
        set => SetValue(CodeFontFamilyProperty, value);
    }

    private void OnMarkdownChanged(AvaloniaPropertyChangedEventArgs _)
    {
        var text = Markdown;

        if (Inlines is not { } inlines)
            return;

        inlines.Clear();

        if (string.IsNullOrEmpty(text))
            return;

        var runs = Parse(text, CodeBackground, CodeFontFamily ?? s_defaultCodeFontFamily);
        inlines.AddRange(runs);
    }

    // ----------------------------------------------------------------
    // Parser
    // ----------------------------------------------------------------

    /// <summary>
    /// Scans <paramref name="text"/> character by character, toggling styles on
    /// delimiter tokens and emitting <see cref="Run"/> elements.
    /// </summary>
    private static List<Run> Parse(string text, IBrush? codeBackground, FontFamily codeFontFamily)
    {
        var runs = new List<Run>();
        var sb = new StringBuilder();
        bool bold = false, italic = false, strike = false, code = false;

        int i = 0;
        while (i < text.Length)
        {
            char c = text[i];

            // Inside a code span: only look for the closing backtick.
            if (code)
            {
                if (c == '`')
                {
                    Flush();
                    code = false;
                    i++;
                    continue;
                }

                sb.Append(c);
                i++;
                continue;
            }

            // Code toggle.
            if (c == '`')
            {
                Flush();
                code = true;
                i++;
                continue;
            }

            // Strikethrough: ~~
            if (c == '~' && i + 1 < text.Length && text[i + 1] == '~')
            {
                Flush();
                strike = !strike;
                i += 2;
                continue;
            }

            // Bold: ** / __   and   Italic: * / _
            if (c is '*' or '_')
            {
                if (i + 1 < text.Length && text[i + 1] == c)
                {
                    Flush();
                    bold = !bold;
                    i += 2;
                    continue;
                }

                Flush();
                italic = !italic;
                i++;
                continue;
            }

            sb.Append(c);
            i++;
        }

        Flush();
        return runs;

        void Flush()
        {
            if (sb.Length == 0)
                return;

            var run = new Run { Text = sb.ToString() };
            sb.Clear();

            if (bold)
                run.FontWeight = FontWeight.Bold;

            if (italic)
                run.FontStyle = FontStyle.Italic;

            if (strike)
                run.TextDecorations = [new TextDecoration { Location = TextDecorationLocation.Strikethrough }];

            if (code)
            {
                run.FontFamily = codeFontFamily;
                if (codeBackground is not null)
                    run.Background = codeBackground;
            }

            runs.Add(run);
        }
    }
}
