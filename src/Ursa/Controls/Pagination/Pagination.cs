using Avalonia;
using Avalonia.Collections;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Primitives;
using Avalonia.Interactivity;
using Avalonia.Styling;

namespace Ursa.Controls;

/// <summary>
/// Pagination is a control that displays a series of buttons that can be used to navigate to pages.
/// CurrentPage starts from 1.
/// Pagination only stores an approximate index internally.
/// </summary>
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
    private readonly PaginationButton[] _buttons = new PaginationButton[7];
    private ComboBox? _sizeChangerComboBox;

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
        InitializePanelButtons();
        UpdateButtons(0);

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
        nameof(PageSize), defaultValue: 10);
    
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

    protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
    {
        base.OnPropertyChanged(change);
        // When page size is updated, the selected current page must be updated. 
        if (change.Property == PageSizeProperty)
        {
            int oldPageSize = change.GetOldValue<int>();
            int index = oldPageSize * CurrentPage;
            UpdateButtons(index);
        }
        else if (change.Property == TotalCountProperty || change.Property == CurrentPageProperty)
        {
            int index = PageSize * CurrentPage;
            UpdateButtons(index);
        }
    }

    private void OnButtonClick(object? sender, RoutedEventArgs e)
    {
        if (Equals(sender, _previousButton))
        {
            AddCurrentPage(-1);
        }
        else
        {
            AddCurrentPage(1);
        }
    }

    private void InitializePanelButtons()
    {
        if (_buttonPanel is null) return;
        _buttonPanel.Children.Clear();
        for (int i = 1; i <= 7; i++)
        {
            var button = new PaginationButton() { Page = i, IsVisible = true };
            _buttonPanel.Children.Add(button);
            _buttons![i - 1] = button;
            button.Click+= OnPageButtonClick;
        }
    }

    private void OnPageButtonClick(object sender, RoutedEventArgs args)
    {
        if (sender is PaginationButton pageButton)
        {
            if (pageButton.IsLeftForward)
            {
                AddCurrentPage(-5);
            }
            else if (pageButton.IsRightForward)
            {
                AddCurrentPage(5);
            }
            else
            {
                CurrentPage = pageButton.Page;
            }
        }
    }

    private void AddCurrentPage(int pageChange)
    {
        int newValue = CurrentPage + pageChange;
        if (newValue <= 0) newValue = 1;
        else if(newValue>=PageCount) newValue = PageCount;
        CurrentPage = newValue;
    }

    private void UpdateButtons(int index)
    {
        if (_buttonPanel is null) return;
        if (PageSize == 0) return;

        int currentIndex = index;
        
        int pageCount = TotalCount / PageSize;
        int residue = TotalCount % PageSize;
        if (residue > 0)
        {
            pageCount++;
        }
        
        PageCount = pageCount;
        
        int currentPage = currentIndex/ PageSize;
        if (currentPage == 0) currentPage++;

        if (pageCount <= 7)
        {
            for (int i = 0; i < 7; i++)
            {
                if (i < pageCount)
                {
                    _buttons[i].IsVisible = true;
                    _buttons[i].SetStatus(i + 1, i+1 == CurrentPage, false, false);
                }
                else
                {
                    _buttons[i].IsVisible = false;
                }
            }
        }
        else
        {
            for (int i = 0; i < 7; i++)
            {
                _buttons[i].IsVisible = true;
            }
            int mid = currentPage;
            if (mid < 4) mid = 4;
            else if (mid > pageCount - 3) mid = pageCount - 3;
            _buttons[3].Page = mid;
            _buttons[2].Page = mid - 1;
            _buttons[4].Page = mid + 1;
            _buttons[0].Page = 1;
            _buttons[6].Page = pageCount;
            if(mid>4)
            {
                _buttons[1].SetStatus(0, false, true, false);
            }
            else
            {
                _buttons[1].SetStatus(mid-2, false, false, false);
            }
            if(mid<pageCount-3)
            {
                _buttons[5].SetStatus(0, false, false, true);
            }
            else
            {
                _buttons[5].SetStatus(mid+2, false, false, false);
            }

            foreach (var button in _buttons)
            {
                if (button.Page == currentPage)
                {
                    button.SetSelected(true);
                }
                else
                {
                    button.SetSelected(false);
                }
            }
        }

        CurrentPage = currentPage;
    }
}