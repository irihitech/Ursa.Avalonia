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
using Avalonia.Metadata;
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

    public static readonly StyledProperty<string?> WatermarkProperty = 
        TextBox.WatermarkProperty.AddOwner<TagInput>();
    
    public static readonly StyledProperty<bool> AcceptsReturnProperty =
        TextBox.AcceptsReturnProperty.AddOwner<TagInput>();

    public bool AcceptsReturn
    {
        get => GetValue(AcceptsReturnProperty);
        set => SetValue(AcceptsReturnProperty, value);
    }

    public static readonly StyledProperty<int> MaxCountProperty = 
        AvaloniaProperty.Register<TagInput, int>(nameof(MaxCount), int.MaxValue);

    public static readonly StyledProperty<ControlTheme> InputThemeProperty =
        AvaloniaProperty.Register<TagInput, ControlTheme>(nameof(InputTheme));

    public static readonly StyledProperty<IDataTemplate?> ItemTemplateProperty =
        AvaloniaProperty.Register<TagInput, IDataTemplate?>(nameof(ItemTemplate));

    public static readonly StyledProperty<string> SeparatorProperty = 
        AvaloniaProperty.Register<TagInput, string>(nameof(Separator));

    public static readonly StyledProperty<LostFocusBehavior> LostFocusBehaviorProperty =
        AvaloniaProperty.Register<TagInput, LostFocusBehavior>(nameof(LostFocusBehavior));


    public static readonly StyledProperty<bool> AllowDuplicatesProperty = 
        AvaloniaProperty.Register<TagInput, bool>(nameof(AllowDuplicates), true);

    public static readonly StyledProperty<object?> InnerLeftContentProperty =
        AvaloniaProperty.Register<TagInput, object?>(nameof(InnerLeftContent));

    public static readonly StyledProperty<object?> InnerRightContentProperty =
        AvaloniaProperty.Register<TagInput, object?>(nameof(InnerRightContent));

    private TextBox? _textBox;
    private ItemsControl? _itemsControl;
    private TextPresenter? _presenter;

    public TagInput()
    {
        var tags = new ObservableCollection<string>();
        Tags = tags;
        tags.GetWeakCollectionChangedObservable().Subscribe(_ => CheckEmpty());
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

    public ControlTheme InputTheme
    {
        get => GetValue(InputThemeProperty);
        set => SetValue(InputThemeProperty, value);
    }

    [InheritDataTypeFromItems(nameof(Tags))]
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
                AddTags(_textBox?.Text);
                break;
            case LostFocusBehavior.Clear:
                _textBox?.SetValue(TextBox.TextProperty, string.Empty);
                break;
        }
    }

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);
        _itemsControl = e.NameScope.Find<ItemsControl>(PART_ItemsControl);
    }

    protected override void OnLoaded(RoutedEventArgs e)
    {
        base.OnLoaded(e);
        _textBox = (_itemsControl?.ItemsPanelRoot as WrapPanelWithTrailingItem)?.TrailingItem as TextBox;
        _textBox?.AddHandler(KeyDownEvent, OnTextBoxKeyDown, RoutingStrategies.Tunnel);
        _textBox?.AddHandler(LostFocusEvent, OnTextBox_LostFocus, RoutingStrategies.Bubble);
        if (_textBox != null)
        {
            _textBox[!AcceptsReturnProperty] = this[!AcceptsReturnProperty];
        }
        _textBox?.GetObservable(TextBox.TextProperty).Subscribe(_ => CheckEmpty());
        _presenter = _textBox?.GetTemplateChildren().OfType<TextPresenter>().FirstOrDefault();
        _presenter?.GetObservable(TextPresenter.PreeditTextProperty).Subscribe(_ => CheckEmpty());
    }

    private void CheckEmpty()
    {
        if (string.IsNullOrWhiteSpace(_presenter?.PreeditText) && string.IsNullOrEmpty(_textBox?.Text) &&
            Tags.Count == 0)
            PseudoClasses.Set(PseudoClassName.PC_Empty, true);
        else
            PseudoClasses.Set(PseudoClassName.PC_Empty, false);
    }
    
    private void OnTextBoxKeyDown(object? sender, KeyEventArgs args)
    {
        if (!AcceptsReturn && args.Key == Key.Enter)
        {
            AddTags(_textBox?.Text);
        }
        else if (AcceptsReturn && args.Key==Key.Enter)
        {
            var texts = _textBox?.Text?.Split(["\r", "\n"], StringSplitOptions.RemoveEmptyEntries) ?? [];
            foreach (var text in texts)
            {
                AddTags(text);
            }
            args.Handled = true;
        }
        else if (args.Key == Key.Delete || args.Key == Key.Back)
            if (string.IsNullOrEmpty(_textBox?.Text) || _textBox.Text?.Length == 0)
            {
                if (Tags.Count == 0) return;
                Tags.RemoveAt(Tags.Count - 1);
            }
    }

    private void AddTags(string? text)
    {
        if (!(text?.Length > 0)) return;
        if (Tags.Count >= MaxCount) return;
        string[] values = [];
        if (!string.IsNullOrEmpty(Separator))
            values = text.Split([Separator], StringSplitOptions.RemoveEmptyEntries);
        else if(_textBox?.Text is not null)
            values = [_textBox.Text];

        if (!AllowDuplicates)
            values = values.Distinct().Except(Tags).ToArray();

        foreach (var value in values)
        {
            Tags.Add(value);
        }
        _textBox?.Clear();
    }

    public void Close(object o)
    {
        if (o is not Control { Parent: ContentPresenter presenter }) return;
        var index = _itemsControl?.IndexFromContainer(presenter);
        if (index is >= 0 && index < Tags.Count)
            Tags.RemoveAt(index.Value);
    }
}
