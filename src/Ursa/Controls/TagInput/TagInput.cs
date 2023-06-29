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
using Avalonia.Interactivity;
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
    
    public static readonly DirectProperty<TagInput, IList> ItemsProperty = AvaloniaProperty.RegisterDirect<TagInput, IList>(
        nameof(Items), o => o.Items);
    private IList _items;
    public IList Items
    {
        get => _items;
        private set => SetAndRaise(ItemsProperty, ref _items, value);
    }

    public TagInput()
    {
        _textBox = new TextBox();
        _textBox.AddHandler(InputElement.KeyDownEvent, OnTextBoxKeyDown, RoutingStrategies.Tunnel);
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

    public static readonly StyledProperty<string> SeparatorProperty = AvaloniaProperty.Register<TagInput, string>(
        nameof(Separator));

    public string Separator
    {
        get => GetValue(SeparatorProperty);
        set => SetValue(SeparatorProperty, value);
    }

    public static readonly StyledProperty<bool> AllowDuplicatesProperty = AvaloniaProperty.Register<TagInput, bool>(
        nameof(AllowDuplicates), defaultValue: true);

    public bool AllowDuplicates
    {
        get => GetValue(AllowDuplicatesProperty);
        set => SetValue(AllowDuplicatesProperty, value);
    }

    static TagInput()
    {
        InputThemeProperty.Changed.AddClassHandler<TagInput>((o, e) => o.OnInputThemePropertyChanged(e));
        TagsProperty.Changed.AddClassHandler<TagInput>((o, e) => o.OnTagsPropertyChanged(e));
    }

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);
        _itemsControl =  e.NameScope.Find<ItemsControl>(PART_ItemsControl);
        Items.Add(_textBox);
    }

    private void OnInputThemePropertyChanged(AvaloniaPropertyChangedEventArgs args)
    {
        var newTheme = args.GetNewValue<ControlTheme>();
        if (newTheme.TargetType == typeof(TextBox))
        {
            _textBox.Theme = newTheme;
        }
    }
    
    private void OnTagsPropertyChanged(AvaloniaPropertyChangedEventArgs args)
    {
        var newTags = args.GetNewValue<IList<string>>();
        for (int i = 0; i < Items.Count - 1; i++)
        {
            Items.RemoveAt(Items.Count-1);
        }
        for (int i = 0; i < newTags.Count; i++)
        {
            Items.Insert(Items.Count - 1, newTags[i]);
        }
    }

    private void OnTextBoxKeyDown(object? sender, KeyEventArgs args)
    {
        if (args.Key == Key.Enter)
        {
            if (_textBox.Text?.Length > 0)
            {
                string[] values;
                if (!string.IsNullOrEmpty(Separator))
                {
                    values = _textBox.Text.Split(new string[] { Separator },
                        StringSplitOptions.RemoveEmptyEntries);
                }
                else
                {
                    values = new[] { _textBox.Text };
                }

                if (!AllowDuplicates)
                {
                    values = values.Distinct().Except(Tags).ToArray();
                }
                for(int i = 0; i < values.Length; i++)
                {
                    Items.Insert(Items.Count - 1, values[i]);
                    Tags.Insert(Items.Count - 2, values[i]);
                }
                _textBox.Text = "";
            }
        }
        else if (args.Key == Key.Delete || args.Key == Key.Back)
        {
            if (_textBox.Text?.Length == 0)
            {
                if (Tags.Count == 0)
                {
                    return;
                }
                Items.RemoveAt(Items.Count - 2);
                Tags.RemoveAt(Items.Count - 1);
            }
        }
    }

    public void Close(object o)
    {
        if (o is Control t)
        {
            if (t.Parent is ContentPresenter presenter)
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