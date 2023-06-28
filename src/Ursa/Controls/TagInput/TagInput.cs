using System.Collections;
using System.Collections.ObjectModel;
using Avalonia;
using Avalonia.Collections;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Presenters;
using Avalonia.Controls.Primitives;
using Avalonia.Controls.Templates;
using Avalonia.Input;
using Avalonia.Layout;
using Avalonia.Styling;

namespace Ursa.Controls;

[TemplatePart (PART_ItemsControl, typeof (ItemsControl))]
public class TagInput: TemplatedControl
{
    public const string PART_ItemsControl = "PART_ItemsControl";
    
    private readonly TextBox _textBox;
    private ItemsControl? _itemsControl;
    
    
    public static readonly StyledProperty<IList<string>> TagsProperty = AvaloniaProperty.Register<TagInput, IList<string>>(
        nameof(Tags));
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
        _textBox.KeyDown += OnTextBoxKeyDown;
        Items = new AvaloniaList<object>();
        Tags = new ObservableCollection<string>();
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
        _itemsControl =  e.NameScope.Find<ItemsControl>(PART_ItemsControl);
        if (IsSet(InputThemeProperty) && InputTheme.TargetType == typeof(TextBox))
        {
            _textBox.Theme = InputTheme;
        }
        Items.Add(_textBox);
    }

    private void OnTextBoxKeyDown(object? sender, KeyEventArgs args)
    {
        if (args.Key == Avalonia.Input.Key.Enter)
        {
            if (_textBox.Text?.Length > 0)
            {
                Items.Insert(Items.Count - 1, _textBox.Text);
                Tags.Insert(Items.Count - 2, _textBox.Text ?? string.Empty);
                _textBox.Text = "";
            }
            
        }
    }

    public void Close(object o)
    {
        if (o is ClosableTag t)
        {
            var presenter = t.Parent as ContentPresenter;
            if (presenter != null)
            {
                int? index = _itemsControl?.IndexFromContainer(presenter);
                if (index is >= 0 && index < Items.Count - 1)
                {
                    Items.RemoveAt(index.Value);
                    Tags.RemoveAt(index.Value);
                }
            }
        }
    }
}