using System.Collections;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using Avalonia;
using Avalonia.Collections;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Presenters;
using Avalonia.Controls.Primitives;
using Avalonia.Controls.Templates;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Styling;
using Irihi.Avalonia.Shared.Common;
using Irihi.Avalonia.Shared.Helpers;

namespace Ursa.Controls;

[TemplatePart(PART_ItemsControl, typeof(ItemsControl))]
[TemplatePart(PART_Watermark, typeof(Visual))]
[PseudoClasses(PseudoClassName.PC_Empty)]
public class TagInput : TemplatedControl
{
    public const string PART_ItemsControl = "PART_ItemsControl";
    public const string PART_Watermark = "PART_Watermark";

    public static readonly StyledProperty<IList<string>> TagsProperty =
        AvaloniaProperty.Register<TagInput, IList<string>>(
            nameof(Tags));

    public static readonly StyledProperty<string?> WatermarkProperty = TextBox.WatermarkProperty.AddOwner<TagInput>();

    public static readonly StyledProperty<int> MaxCountProperty = AvaloniaProperty.Register<TagInput, int>(
        nameof(MaxCount), int.MaxValue);

    public static readonly DirectProperty<TagInput, IList> ItemsProperty =
        AvaloniaProperty.RegisterDirect<TagInput, IList>(
            nameof(Items), o => o.Items);

    public static readonly StyledProperty<ControlTheme> InputThemeProperty =
        AvaloniaProperty.Register<TagInput, ControlTheme>(
            nameof(InputTheme));

    public static readonly StyledProperty<IDataTemplate?> ItemTemplateProperty =
        AvaloniaProperty.Register<TagInput, IDataTemplate?>(
            nameof(ItemTemplate));

    public static readonly StyledProperty<string> SeparatorProperty = AvaloniaProperty.Register<TagInput, string>(
        nameof(Separator));

    public static readonly StyledProperty<LostFocusBehavior> LostFocusBehaviorProperty =
        AvaloniaProperty.Register<TagInput, LostFocusBehavior>(
            nameof(LostFocusBehavior));


    public static readonly StyledProperty<bool> AllowDuplicatesProperty = AvaloniaProperty.Register<TagInput, bool>(
        nameof(AllowDuplicates), true);

    public static readonly StyledProperty<object?> InnerLeftContentProperty =
        AvaloniaProperty.Register<TagInput, object?>(
            nameof(InnerLeftContent));

    public static readonly StyledProperty<object?> InnerRightContentProperty =
        AvaloniaProperty.Register<TagInput, object?>(
            nameof(InnerRightContent));

    private readonly TextBox _textBox;

    private IList _items = null!;
    private ItemsControl? _itemsControl;

    private TextPresenter? _presenter;
    private Visual? _watermark;


    static TagInput()
    {
        InputThemeProperty.Changed.AddClassHandler<TagInput>((o, e) => o.OnInputThemePropertyChanged(e));
        TagsProperty.Changed.AddClassHandler<TagInput>((o, e) => o.OnTagsPropertyChanged(e));
    }

    public TagInput()
    {
        _textBox = new TextBox();
        _textBox.AddHandler(KeyDownEvent, OnTextBoxKeyDown, RoutingStrategies.Tunnel);
        _textBox.AddHandler(LostFocusEvent, OnTextBox_LostFocus, RoutingStrategies.Bubble);
        Items = new AvaloniaList<object>
        {
            _textBox
        };
        Tags = new ObservableCollection<string>();
    }

    public string? Watermark
    {
        get => GetValue(WatermarkProperty);
        set => SetValue(WatermarkProperty, value);
    }

    public IList<string> Tags
    {
        get => GetValue(TagsProperty);
        set => SetValue(TagsProperty, value);
    }

    public int MaxCount
    {
        get => GetValue(MaxCountProperty);
        set => SetValue(MaxCountProperty, value);
    }

    public IList Items
    {
        get => _items;
        private set => SetAndRaise(ItemsProperty, ref _items, value);
    }

    public ControlTheme InputTheme
    {
        get => GetValue(InputThemeProperty);
        set => SetValue(InputThemeProperty, value);
    }

    public IDataTemplate? ItemTemplate
    {
        get => GetValue(ItemTemplateProperty);
        set => SetValue(ItemTemplateProperty, value);
    }

    public string Separator
    {
        get => GetValue(SeparatorProperty);
        set => SetValue(SeparatorProperty, value);
    }

    public LostFocusBehavior LostFocusBehavior
    {
        get => GetValue(LostFocusBehaviorProperty);
        set => SetValue(LostFocusBehaviorProperty, value);
    }

    public bool AllowDuplicates
    {
        get => GetValue(AllowDuplicatesProperty);
        set => SetValue(AllowDuplicatesProperty, value);
    }

    public object? InnerLeftContent
    {
        get => GetValue(InnerLeftContentProperty);
        set => SetValue(InnerLeftContentProperty, value);
    }

    public object? InnerRightContent
    {
        get => GetValue(InnerRightContentProperty);
        set => SetValue(InnerRightContentProperty, value);
    }

    private void OnTextBox_LostFocus(object? sender, RoutedEventArgs e)
    {
        switch (LostFocusBehavior)
        {
            case LostFocusBehavior.Add:
                AddTags();
                break;
            case LostFocusBehavior.Clear:
                _textBox.Text = "";
                break;
        }
    }

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);
        _itemsControl = e.NameScope.Find<ItemsControl>(PART_ItemsControl);
        _watermark = e.NameScope.Find<Visual>(PART_Watermark);
    }

    protected override void OnLoaded(RoutedEventArgs e)
    {
        base.OnLoaded(e);
        if (_watermark is null) return;
        _presenter = _textBox.GetTemplateChildren().OfType<TextPresenter>().FirstOrDefault();
        _presenter?.GetObservable(TextPresenter.PreeditTextProperty).Subscribe(_ => CheckEmpty());
        _textBox.GetObservable(TextBox.TextProperty).Subscribe(_ => CheckEmpty());
        if (Tags is INotifyCollectionChanged incc)
            incc.GetWeakCollectionChangedObservable().Subscribe(_ => CheckEmpty());
    }

    private void OnInputThemePropertyChanged(AvaloniaPropertyChangedEventArgs args)
    {
        var newTheme = args.GetNewValue<ControlTheme?>();
        if (newTheme?.TargetType == typeof(TextBox)) _textBox.Theme = newTheme;
    }

    private void CheckEmpty()
    {
        if (string.IsNullOrWhiteSpace(_presenter?.PreeditText) && string.IsNullOrEmpty(_textBox.Text) &&
            Tags.Count == 0)
            PseudoClasses.Set(PseudoClassName.PC_Empty, true);
        else
            PseudoClasses.Set(PseudoClassName.PC_Empty, false);
    }

    private void OnTagsPropertyChanged(AvaloniaPropertyChangedEventArgs args)
    {
        var newTags = args.GetNewValue<IList<string>?>();
        var oldTags = args.GetOldValue<IList<string>?>();

        if (Items is AvaloniaList<object> avaloniaList)
        {
            avaloniaList.RemoveRange(0, avaloniaList.Count - 1);
        }
        else if (Items.Count != 0)
        {
            Items.Clear();
            Items.Add(_textBox);
        }

        if (newTags != null)
            for (var i = 0; i < newTags.Count; i++)
                Items.Insert(Items.Count - 1, newTags[i]);
        if (oldTags is INotifyCollectionChanged inccold) inccold.CollectionChanged -= OnCollectionChanged;

        if (Tags is INotifyCollectionChanged incc) incc.CollectionChanged += OnCollectionChanged;
    }

    private void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
    {
        if (e.Action == NotifyCollectionChangedAction.Add)
        {
            var items = e.NewItems;
            var index = e.NewStartingIndex;
            foreach (var item in items)
                if (item is string s)
                {
                    Items.Insert(index, s);
                    index++;
                }
        }
        else if (e.Action == NotifyCollectionChangedAction.Remove)
        {
            var items = e.OldItems;
            var index = e.OldStartingIndex;
            foreach (var item in items)
                if (item is string)
                    Items.RemoveAt(index);
        }
        else if (e.Action == NotifyCollectionChangedAction.Reset)
        {
            Items.Clear();
            Items.Add(_textBox);
            InvalidateVisual();
        }
    }

    private void OnTextBoxKeyDown(object? sender, KeyEventArgs args)
    {
        if (args.Key == Key.Enter)
            AddTags();
        else if (args.Key == Key.Delete || args.Key == Key.Back)
            if (string.IsNullOrEmpty(_textBox.Text) || _textBox.Text?.Length == 0)
            {
                if (Tags.Count == 0) return;
                var index = Items.Count - 2;
                // Items.RemoveAt(index);
                Tags.RemoveAt(index);
            }
    }

    private void AddTags()
    {
        if (!(_textBox.Text?.Length > 0)) return;
        if (Tags.Count >= MaxCount) return;
        string[] values;
        if (!string.IsNullOrEmpty(Separator))
            values = _textBox.Text.Split(new[] { Separator },
                StringSplitOptions.RemoveEmptyEntries);
        else
            values = new[] { _textBox.Text };

        if (!AllowDuplicates)
            values = values.Distinct().Except(Tags).ToArray();

        for (var i = 0; i < values.Length; i++)
        {
            var index = Items.Count - 1;
            // Items.Insert(index, values[i]);
            Tags?.Insert(index, values[i]);
        }

        _textBox.Text = "";
    }

    public void Close(object o)
    {
        if (o is Control t)
            if (t.Parent is ContentPresenter presenter)
            {
                var index = _itemsControl?.IndexFromContainer(presenter);
                if (index is >= 0 && index < Items.Count - 1)
                    // Items.RemoveAt(index.Value);
                    Tags.RemoveAt(index.Value);
            }
    }
}