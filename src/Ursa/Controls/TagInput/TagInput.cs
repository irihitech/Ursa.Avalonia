using System.Collections;
using Avalonia;
using Avalonia.Collections;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Controls.Templates;
using Avalonia.Layout;
using Avalonia.Styling;

namespace Ursa.Controls;

public class TagInput: TemplatedControl
{
    public static readonly StyledProperty<IList<string>> TagsProperty = AvaloniaProperty.Register<TagInput, IList<string>>(
        nameof(Tags));
    
    private TextBox _textBox;

    public IList<string> Tags
    {
        get => GetValue(TagsProperty);
        set => SetValue(TagsProperty, value);
    }

    public static readonly StyledProperty<IList> ItemsProperty = AvaloniaProperty.Register<TagInput, IList>(
        nameof(Items));

    public IList Items
    {
        get => GetValue(ItemsProperty);
        set => SetValue(ItemsProperty, value);
    }

    public TagInput()
    {
        _textBox = new TextBox();
    }

    public static readonly StyledProperty<ControlTheme> InputThemeProperty = AvaloniaProperty.Register<TagInput, ControlTheme>(
        nameof(InputTheme));

    public ControlTheme InputTheme
    {
        get => GetValue(InputThemeProperty);
        set => SetValue(InputThemeProperty, value);
    }

    public static readonly StyledProperty<IDataTemplate?> ItemTemplateProperty = AvaloniaProperty.Register<TagInput, IDataTemplate?>(
        nameof(ItemTemplate));

    public IDataTemplate? ItemTemplate
    {
        get => GetValue(ItemTemplateProperty);
        set => SetValue(ItemTemplateProperty, value);
    }

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);
        Items = new AvaloniaList<object>();
        if (IsSet(InputThemeProperty) && InputTheme.TargetType == typeof(TextBox))
        {
            _textBox.Theme = InputTheme;
        }
        _textBox.KeyDown += (sender, args) =>
        {
            if (args.Key == Avalonia.Input.Key.Enter)
            {
                Items.Insert(Items.Count - 1, _textBox.Text);
                // Tags.Insert(Items.Count - 1, _textBox.Text ?? string.Empty);
                _textBox.Text = "";
            }
        };
        Items.Add(_textBox);
    }
}