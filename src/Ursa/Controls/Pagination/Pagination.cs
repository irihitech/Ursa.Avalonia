using Avalonia;
using Avalonia.Collections;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Primitives;
using Avalonia.Interactivity;
using Avalonia.Styling;

namespace Ursa.Controls;

[TemplatePart(PART_PreviousButton, typeof(Button))]
[TemplatePart(PART_NextButton, typeof(Button))]
[TemplatePart(PART_ButtonPanel, typeof(StackPanel))]
[TemplatePart(PART_SizeChangerComboBox, typeof(ComboBox))]
public class Pagination: TemplatedControl
{
    public const string PART_PreviousButton = "PART_PreviousButton";
    public const string PART_NextButton = "PART_NextButton";
    public const string PART_ButtonPanel = "PART_ButtonPanel";
    public const string PART_SizeChangerComboBox = "PART_SizeChangerComboBox";
    private Button? _previousButton;
    private Button? _nextButton;
    private StackPanel? _buttonPanel;
    private ComboBox? _sizeChangerComboBox;

    /// <summary>
    /// To reduce allocation, there are maximum of 7 buttons and 2 selection controls. it will be reused.
    /// </summary>
    private PaginationExpandButton? _leftSelection;
    private PaginationExpandButton? _rightSelection;

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);
        if (_previousButton != null) _previousButton.Click -= OnButtonClick;
        if (_nextButton != null) _nextButton.Click -= OnButtonClick;
        _previousButton = e.NameScope.Find<Button>(PART_PreviousButton);
        _nextButton = e.NameScope.Find<Button>(PART_NextButton);
        _buttonPanel = e.NameScope.Find<StackPanel>(PART_ButtonPanel);
        _sizeChangerComboBox = e.NameScope.Find<ComboBox>(PART_SizeChangerComboBox);
        if (_previousButton != null) _previousButton.Click += OnButtonClick;
        if (_nextButton != null) _nextButton.Click += OnButtonClick;
        _leftSelection = new PaginationExpandButton();
        _rightSelection = new PaginationExpandButton();
        UpdateButtons();

    }
    
    public static readonly StyledProperty<int> CurrentPageProperty = AvaloniaProperty.Register<Pagination, int>(
        nameof(CurrentPage));
    
    public int CurrentPage
    {
        get => GetValue(CurrentPageProperty);
        set => SetValue(CurrentPageProperty, value);
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
        nameof(PageSize), defaultValue: 50);
    
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

    public static readonly StyledProperty<AvaloniaList<int>> PageSizeOptionsProperty = AvaloniaProperty.Register<Pagination, AvaloniaList<int>>(
        nameof(PageSizeOptions));

    public AvaloniaList<int> PageSizeOptions
    {
        get => GetValue(PageSizeOptionsProperty);
        set => SetValue(PageSizeOptionsProperty, value);
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

    protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
    {
        base.OnPropertyChanged(change);
        if (change.Property == PageSizeProperty || change.Property == TotalCountProperty)
        {
            UpdateButtons();
        }
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

    private void UpdateButtons()
    {
        if (PageSize == 0) return;
        int currentPage = CurrentPage;
        
        int pageCount = TotalCount / PageSize;
        int residue = TotalCount % PageSize;
        if (residue > 0)
        {
            pageCount++;
        }
        _buttonPanel?.Children.Clear();
        for (int i = 0; i < pageCount; i++)
        {
            if (i == 1 && _leftSelection is not null)
            {
                _leftSelection.Pages = new AvaloniaList<int>() { PageSize + 1, PageSize + 2, PageSize + 3 };
                _buttonPanel?.Children.Add(_leftSelection);
            }
            else
            {
                _buttonPanel?.Children.Add(new Button { Content = i + 1 });
            }
        }
    }
}