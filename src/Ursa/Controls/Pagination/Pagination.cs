using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Primitives;
using Avalonia.Interactivity;
using Avalonia.Styling;

namespace Ursa.Controls;

[TemplatePart(PART_PreviousButton, typeof(Button))]
[TemplatePart(PART_NextButton, typeof(Button))]
[TemplatePart(PART_ButtonPanel, typeof(StackPanel))]
public class Pagination: TemplatedControl
{
    public const string PART_PreviousButton = "PART_PreviousButton";
    public const string PART_NextButton = "PART_NextButton";
    public const string PART_ButtonPanel = "PART_ButtonPanel";
    private Button? _previousButton;
    private Button? _nextButton;
    private StackPanel? _buttonPanel;

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);
        if (_previousButton != null) _previousButton.Click -= OnButtonClick;
        if (_nextButton != null) _nextButton.Click -= OnButtonClick;
        _previousButton = e.NameScope.Find<Button>(PART_PreviousButton);
        _nextButton = e.NameScope.Find<Button>(PART_NextButton);
        _buttonPanel = e.NameScope.Find<StackPanel>(PART_ButtonPanel);
        if (_previousButton != null) _previousButton.Click += OnButtonClick;
        if (_nextButton != null) _nextButton.Click += OnButtonClick;
    }
    
    public static readonly StyledProperty<int> CurrentPageProperty = AvaloniaProperty.Register<Pagination, int>(
        nameof(CurrentPage));
    
    public int CurrentPage
    {
        get => GetValue(CurrentPageProperty);
        set {
            int actualValue = CoercePage(value);
            SetValue(CurrentPageProperty, actualValue);
        }
    }

    public static readonly StyledProperty<int> TotalCountProperty = AvaloniaProperty.Register<Pagination, int>(
        nameof(TotalCount));

    /// <summary>
    /// Total count of items. 
    /// </summary>
    public int TotalCount
    {
        get => GetValue(TotalCountProperty);
        set => SetValue(TotalCountProperty, value);
    }
    
    public static readonly StyledProperty<int> PageSizeProperty = AvaloniaProperty.Register<Pagination, int>(
        nameof(PageSize));
    
    /// <summary>
    /// Page size.
    /// </summary>
    public int PageSize
    {
        get => GetValue(PageSizeProperty);
        set => SetValue(PageSizeProperty, value);
    }

    private int _pageCount;

    public static readonly DirectProperty<Pagination, int> PageCountProperty = AvaloniaProperty.RegisterDirect<Pagination, int>(
        nameof(PageCount), o => o.PageCount);

    /// <summary>
    /// Page count.
    /// </summary>
    public int PageCount
    {
        get => _pageCount;
        private set => SetAndRaise(PageCountProperty, ref _pageCount, value);
    }

    private int _startIndex;

    public static readonly DirectProperty<Pagination, int> StartIndexProperty = AvaloniaProperty.RegisterDirect<Pagination, int>(
        nameof(StartIndex), o => o.StartIndex);

    public int StartIndex
    {
        get => _startIndex;
        private set => SetAndRaise(StartIndexProperty, ref _startIndex, value);
    }

    private int _endIndex;

    public static readonly DirectProperty<Pagination, int> EndIndexProperty = AvaloniaProperty.RegisterDirect<Pagination, int>(
        nameof(EndIndex), o => o.EndIndex);

    public int EndIndex
    {
        get => _endIndex;
        private set => SetAndRaise(EndIndexProperty, ref _endIndex, value);
    }

    public static readonly StyledProperty<ControlTheme> PageButtonThemeProperty = AvaloniaProperty.Register<Pagination, ControlTheme>(
        nameof(PageButtonTheme));

    public ControlTheme PageButtonTheme
    {
        get => GetValue(PageButtonThemeProperty);
        set => SetValue(PageButtonThemeProperty, value);
    }

    public static readonly StyledProperty<ControlTheme> ExpandButtonThemeProperty = AvaloniaProperty.Register<Pagination, ControlTheme>(
        nameof(ExpandButtonTheme));

    public ControlTheme ExpandButtonTheme
    {
        get => GetValue(ExpandButtonThemeProperty);
        set => SetValue(ExpandButtonThemeProperty, value);
    }
    
    private void OnButtonClick(object? sender, RoutedEventArgs e)
    {
        if (Equals(sender, _previousButton))
        {
            CurrentPage--;
        }
        else
        {
            CurrentPage++;
        }
    }
    
    private int CoercePage(int page)
    {
        if (page < 1) return 1;
        if (page > PageCount) return PageCount;
        return page;
    }

}