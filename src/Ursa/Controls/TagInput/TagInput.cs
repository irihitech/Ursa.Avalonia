using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Diagnostics.CodeAnalysis;
using Avalonia;
using Avalonia.Collections;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Presenters;
using Avalonia.Controls.Primitives;
using Avalonia.Controls.Templates;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Media;
using Avalonia.Metadata;
using Irihi.Avalonia.Shared.Common;
using Irihi.Avalonia.Shared.Helpers;

namespace Ursa.Controls;

[TemplatePart(PART_ItemsControl, typeof(ItemsControl))]
[TemplatePart(PART_Placeholder, typeof(Visual))]
[PseudoClasses(PseudoClassName.PC_Empty)]
public class TagInput : TemplatedControl
{
    public const string PART_ItemsControl = "PART_ItemsControl";
    public const string PART_Placeholder = "PART_Placeholder";

    public static readonly StyledProperty<IList<string>?> TagsProperty =
        AvaloniaProperty.Register<TagInput, IList<string>?>(
            nameof(Tags));

    [SuppressMessage("AvaloniaProperty", "AVP1013",
        Justification = "Obsolete property alias for backward compatibility.")]
    public static readonly StyledProperty<string?> PlaceholderTextProperty =
        TextBox.PlaceholderTextProperty.AddOwner<TagInput>();

    public static readonly StyledProperty<IBrush?> PlaceholderForegroundProperty =
        TextBox.PlaceholderForegroundProperty.AddOwner<TagInput>();

    [Obsolete("Use PlaceholderTextProperty instead.")]
    public static readonly StyledProperty<string?> WatermarkProperty = PlaceholderTextProperty;

    public static readonly StyledProperty<bool> AcceptsReturnProperty =
        TextBox.AcceptsReturnProperty.AddOwner<TagInput>();

    public static readonly StyledProperty<int> MaxCountProperty =
        AvaloniaProperty.Register<TagInput, int>(nameof(MaxCount), int.MaxValue);

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

    private ItemsControl? _itemsControl;
    private TextPresenter? _presenter;
    private IDisposable? _tagsSubscription;

    internal TextBox? InputTextBox;

    public TagInput()
    {
        SetCurrentValue(TagsProperty, new ObservableCollection<string>());
    }

    public string? PlaceholderText
    {
        get => GetValue(PlaceholderTextProperty);
        set => SetValue(PlaceholderTextProperty, value);
    }

    public IBrush? PlaceholderForeground
    {
        get => GetValue(PlaceholderForegroundProperty);
        set => SetValue(PlaceholderForegroundProperty, value);
    }

    public bool AcceptsReturn
    {
        get => GetValue(AcceptsReturnProperty);
        set => SetValue(AcceptsReturnProperty, value);
    }

    [Obsolete("Use PlaceholderText property instead.")]
    [SuppressMessage("AvaloniaProperty", "AVP1012",
        Justification = "Obsolete property alias for backward compatibility.")]
    public string? Watermark
    {
        get => PlaceholderText;
        set => PlaceholderText = value;
    }

    public IList<string>? Tags
    {
        get => GetValue(TagsProperty);
        set => SetValue(TagsProperty, value);
    }

    public int MaxCount
    {
        get => GetValue(MaxCountProperty);
        set => SetValue(MaxCountProperty, value);
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
                AddTags(InputTextBox?.Text);
                break;
            case LostFocusBehavior.Clear:
                if (InputTextBox is not null) InputTextBox.Text = string.Empty;
                break;
        }
    }

    protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
    {
        base.OnPropertyChanged(change);
        if (change.Property == TagsProperty)
        {
            _tagsSubscription?.Dispose();
            _tagsSubscription = null;
            if (Tags is INotifyCollectionChanged observable)
            {
                _tagsSubscription = observable.GetWeakCollectionChangedObservable().Subscribe(_ => CheckEmpty());
            }

            CheckEmpty();
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
        InputTextBox = (_itemsControl?.ItemsPanelRoot as WrapPanelWithTrailingItem)?.TrailingItem as TextBox;
        if (InputTextBox is null) return;

        InputTextBox.AddHandler(KeyDownEvent, OnTextBoxKeyDown, RoutingStrategies.Tunnel);
        InputTextBox.AddHandler(LostFocusEvent, OnTextBox_LostFocus, RoutingStrategies.Bubble);

        InputTextBox[~AcceptsReturnProperty] = this[~AcceptsReturnProperty];
        InputTextBox.GetObservable(TextBox.TextProperty).Subscribe(_ => CheckEmpty());

        _presenter = InputTextBox.GetTemplateChildren().OfType<TextPresenter>().FirstOrDefault();
        _presenter?.GetObservable(TextPresenter.PreeditTextProperty).Subscribe(_ => CheckEmpty());
    }

    private void CheckEmpty()
    {
        if (string.IsNullOrWhiteSpace(_presenter?.PreeditText) && string.IsNullOrEmpty(InputTextBox?.Text) &&
            (Tags is null || Tags.Count == 0))
            PseudoClasses.Set(PseudoClassName.PC_Empty, true);
        else
            PseudoClasses.Set(PseudoClassName.PC_Empty, false);
    }

    private void OnTextBoxKeyDown(object? sender, KeyEventArgs args)
    {
        if (!AcceptsReturn && args.Key == Key.Enter)
        {
            AddTags(InputTextBox?.Text);
        }
        else if (AcceptsReturn && args.Key == Key.Enter)
        {
            var texts = InputTextBox?.Text?.Split(["\r", "\n"], StringSplitOptions.RemoveEmptyEntries) ?? [];
            foreach (var text in texts)
            {
                AddTags(text);
            }

            args.Handled = true;
        }
        else if (args.Key == Key.Delete || args.Key == Key.Back)
        {
            if (string.IsNullOrEmpty(InputTextBox?.Text))
            {
                if (Tags is null || Tags.Count == 0) return;
                Tags.RemoveAt(Tags.Count - 1);
            }
        }
    }

    private void AddTags(string? text)
    {
        if (!(text?.Length > 0)) return;
        if (Tags is null) return;
        if (Tags.Count >= MaxCount) return;
        string[] values;
        if (!string.IsNullOrEmpty(Separator))
        {
            values = text.Split([Separator], StringSplitOptions.RemoveEmptyEntries);
        }
        else
        {
            values = [text];
        }

        if (!AllowDuplicates)
            values = values.Distinct().Except(Tags).ToArray();

        foreach (var value in values) Tags.Add(value);
        InputTextBox?.Clear();
    }

    public void Close(object o)
    {
        if (o is not ClosableTag presenter) return;
        if (Tags is null) return;
        var index = _itemsControl?.IndexFromContainer(presenter);
        if (index is >= 0 && index < Tags.Count)
            Tags.RemoveAt(index.Value);
    }
}
