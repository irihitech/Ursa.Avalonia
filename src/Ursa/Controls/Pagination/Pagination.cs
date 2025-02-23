using System.Windows.Input;
using Avalonia;
using Avalonia.Collections;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Primitives;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Styling;
using Irihi.Avalonia.Shared.Helpers;

namespace Ursa.Controls;

/// <summary>
///     Pagination is a control that displays a series of buttons that can be used to navigate to pages.
///     CurrentPage starts from 1.
///     Pagination only stores an approximate index internally.
/// </summary>
[TemplatePart(PART_PreviousButton, typeof(PaginationButton))]
[TemplatePart(PART_NextButton, typeof(PaginationButton))]
[TemplatePart(PART_ButtonPanel, typeof(StackPanel))]
[TemplatePart(PART_QuickJumpInput, typeof(NumericIntUpDown))]
public class Pagination : TemplatedControl
{
    public const string PART_PreviousButton = "PART_PreviousButton";
    public const string PART_NextButton = "PART_NextButton";
    public const string PART_ButtonPanel = "PART_ButtonPanel";
    public const string PART_QuickJumpInput = "PART_QuickJumpInput";

    public static readonly StyledProperty<int?> CurrentPageProperty = AvaloniaProperty.Register<Pagination, int?>(
        nameof(CurrentPage), coerce: CoerceCurrentPage);

    public static readonly RoutedEvent<ValueChangedEventArgs<int>> CurrentPageChangedEvent =
        RoutedEvent.Register<Pagination, ValueChangedEventArgs<int>>(nameof(CurrentPageChanged),
            RoutingStrategies.Bubble);

    public static readonly StyledProperty<ICommand?> CommandProperty = AvaloniaProperty.Register<Pagination, ICommand?>(
        nameof(Command));

    public static readonly StyledProperty<object?> CommandParameterProperty =
        AvaloniaProperty.Register<Pagination, object?>(nameof(CommandParameter));

    public static readonly StyledProperty<int> TotalCountProperty = AvaloniaProperty.Register<Pagination, int>(
        nameof(TotalCount));

    public static readonly StyledProperty<int> PageSizeProperty = AvaloniaProperty.Register<Pagination, int>(
        nameof(PageSize), 10);

    public static readonly DirectProperty<Pagination, int> PageCountProperty =
        AvaloniaProperty.RegisterDirect<Pagination, int>(
            nameof(PageCount), o => o.PageCount, (o, e) => o.PageCount = e);

    public static readonly StyledProperty<AvaloniaList<int>> PageSizeOptionsProperty =
        AvaloniaProperty.Register<Pagination, AvaloniaList<int>>(
            nameof(PageSizeOptions));

    public static readonly StyledProperty<ControlTheme> PageButtonThemeProperty =
        AvaloniaProperty.Register<Pagination, ControlTheme>(
            nameof(PageButtonTheme));

    public static readonly StyledProperty<bool> ShowPageSizeSelectorProperty =
        AvaloniaProperty.Register<Pagination, bool>(
            nameof(ShowPageSizeSelector));

    public static readonly StyledProperty<bool> ShowQuickJumpProperty = AvaloniaProperty.Register<Pagination, bool>(
        nameof(ShowQuickJump));

    private readonly PaginationButton[] _buttons = new PaginationButton[7];
    private StackPanel? _buttonPanel;
    private PaginationButton? _nextButton;

    private int _pageCount;
    private PaginationButton? _previousButton;
    private NumericIntUpDown? _quickJumpInput;

    static Pagination()
    {
        PageSizeProperty.Changed.AddClassHandler<Pagination, int>((pagination, args) =>
            pagination.OnPageSizeChanged(args));
        CurrentPageProperty.Changed.AddClassHandler<Pagination, int?>((pagination, args) =>
            pagination.UpdateButtonsByCurrentPage(args.NewValue.Value));
        CurrentPageProperty.Changed.AddClassHandler<Pagination, int?>((pagination, args) =>
            pagination.OnCurrentPageChanged(args));
        TotalCountProperty.Changed.AddClassHandler<Pagination, int>((pagination, _) =>
            pagination.UpdateButtonsByCurrentPage(pagination.CurrentPage));
    }

    public int? CurrentPage
    {
        get => GetValue(CurrentPageProperty);
        set => SetValue(CurrentPageProperty, value);
    }

    public ICommand? Command
    {
        get => GetValue(CommandProperty);
        set => SetValue(CommandProperty, value);
    }

    public object? CommandParameter
    {
        get => GetValue(CommandParameterProperty);
        set => SetValue(CommandParameterProperty, value);
    }

    /// <summary>
    ///     Total count of items.
    /// </summary>
    public int TotalCount
    {
        get => GetValue(TotalCountProperty);
        set => SetValue(TotalCountProperty, value);
    }

    /// <summary>
    ///     Page size.
    /// </summary>
    public int PageSize
    {
        get => GetValue(PageSizeProperty);
        set => SetValue(PageSizeProperty, value);
    }

    /// <summary>
    ///     Page count.
    /// </summary>
    public int PageCount
    {
        get => _pageCount;
        private set => SetAndRaise(PageCountProperty, ref _pageCount, value);
    }

    public AvaloniaList<int> PageSizeOptions
    {
        get => GetValue(PageSizeOptionsProperty);
        set => SetValue(PageSizeOptionsProperty, value);
    }

    public ControlTheme PageButtonTheme
    {
        get => GetValue(PageButtonThemeProperty);
        set => SetValue(PageButtonThemeProperty, value);
    }

    public bool ShowPageSizeSelector
    {
        get => GetValue(ShowPageSizeSelectorProperty);
        set => SetValue(ShowPageSizeSelectorProperty, value);
    }

    public bool ShowQuickJump
    {
        get => GetValue(ShowQuickJumpProperty);
        set => SetValue(ShowQuickJumpProperty, value);
    }

    public static readonly StyledProperty<bool> DisplayCurrentPageInQuickJumperProperty = AvaloniaProperty.Register<Pagination, bool>(
        nameof(DisplayCurrentPageInQuickJumper));

    public bool DisplayCurrentPageInQuickJumper
    {
        get => GetValue(DisplayCurrentPageInQuickJumperProperty);
        set => SetValue(DisplayCurrentPageInQuickJumperProperty, value);
    }

    private static int? CoerceCurrentPage(AvaloniaObject arg1, int? arg2)
    {
        if (arg2 is null) return null;
        // Only coerce the value if the pagination is initialized. Otherwise the value will be coerced to default because PageCount is not yet determined.
        if (arg1 is Pagination { IsInitialized: true } p)
        {
            arg2 = MathHelpers.SafeClamp(arg2.Value, 1, p.PageCount);
        }
        return arg2;
    }

    private void OnCurrentPageChanged(AvaloniaPropertyChangedEventArgs<int?> args)
    {
        var oldValue = args.GetOldValue<int?>();
        var newValue = args.GetNewValue<int?>();
        var e = new ValueChangedEventArgs<int>(CurrentPageChangedEvent, oldValue, newValue);
        if (DisplayCurrentPageInQuickJumper)
        {
            _quickJumpInput?.SetCurrentValue(NumericIntUpDown.ValueProperty, newValue);
        }
        RaiseEvent(e);
    }

    /// <summary>
    ///     Raised when the <see cref="CurrentPage" /> changes.
    /// </summary>
    public event EventHandler<ValueChangedEventArgs<int>>? CurrentPageChanged
    {
        add => AddHandler(CurrentPageChangedEvent, value);
        remove => RemoveHandler(CurrentPageChangedEvent, value);
    }

    private void OnPageSizeChanged(AvaloniaPropertyChangedEventArgs<int> args)
    {
        if (!IsInitialized) return;
        var pageCount = TotalCount / args.NewValue.Value;
        var residue = TotalCount % args.NewValue.Value;
        if (residue > 0) pageCount++;
        PageCount = pageCount;
        if (CurrentPage > PageCount) CurrentPage = null;
        UpdateButtonsByCurrentPage(CurrentPage);
    }

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);

        Button.ClickEvent.RemoveHandler(OnButtonClick, _previousButton, _nextButton);
        _previousButton = e.NameScope.Find<PaginationButton>(PART_PreviousButton);
        _nextButton = e.NameScope.Find<PaginationButton>(PART_NextButton);
        _buttonPanel = e.NameScope.Find<StackPanel>(PART_ButtonPanel);
        Button.ClickEvent.AddHandler(OnButtonClick, _previousButton, _nextButton);

        KeyDownEvent.RemoveHandler(OnQuickJumpInputKeyDown, _quickJumpInput);
        LostFocusEvent.RemoveHandler(OnQuickJumpInputLostFocus, _quickJumpInput);
        _quickJumpInput = e.NameScope.Find<NumericIntUpDown>(PART_QuickJumpInput);
        KeyDownEvent.AddHandler(OnQuickJumpInputKeyDown, _quickJumpInput);
        LostFocusEvent.AddHandler(OnQuickJumpInputLostFocus, _quickJumpInput);

        InitializePanelButtons();
        CurrentPage = MathHelpers.SafeClamp(CurrentPage ?? 1, 1, PageCount);
        UpdateButtonsByCurrentPage(CurrentPage);
    }

    private void OnQuickJumpInputKeyDown(object? sender, KeyEventArgs e)
    {
        if (e.Key is Key.Enter or Key.Return) SyncQuickJumperValue();
    }

    private void OnQuickJumpInputLostFocus(object? sender, RoutedEventArgs e)
    {
        SyncQuickJumperValue();
    }

    private void SyncQuickJumperValue()
    {
        if (_quickJumpInput is null) return;
        var value = _quickJumpInput?.Value;
        if (value is null) return;
        value = Clamp(value.Value, 1, PageCount);
        SetCurrentValue(CurrentPageProperty, value);
        if (!DisplayCurrentPageInQuickJumper)
        {
            _quickJumpInput?.SetCurrentValue(NumericIntUpDown.ValueProperty, null);
        }
        InvokeCommand();
    }

    private void OnButtonClick(object? sender, RoutedEventArgs e)
    {
        var diff = Equals(sender, _previousButton) ? -1 : 1;
        AddCurrentPage(diff);
        InvokeCommand();
    }

    private void InitializePanelButtons()
    {
        if (_buttonPanel is null) return;
        _buttonPanel.Children.Clear();
        for (var i = 1; i <= 7; i++)
        {
            var button = new PaginationButton { Page = i, IsVisible = true };
            _buttonPanel.Children.Add(button);
            _buttons[i - 1] = button;
            Button.ClickEvent.AddHandler(OnPageButtonClick, button);
        }
    }

    private void OnPageButtonClick(object? sender, RoutedEventArgs args)
    {
        if (sender is PaginationButton pageButton)
        {
            if (pageButton.IsFastForward)
                AddCurrentPage(-5);
            else if (pageButton.IsFastBackward)
                AddCurrentPage(5);
            else
                CurrentPage = pageButton.Page;
        }

        InvokeCommand();
    }

    private void AddCurrentPage(int pageChange)
    {
        var newValue = (CurrentPage ?? 0) + pageChange;
        newValue = Clamp(newValue, 1, PageCount);
        SetCurrentValue(CurrentPageProperty, newValue);
    }

    private int Clamp(int value, int min, int max)
    {
        return value < min ? min : value > max ? max : value;
    }

    /// <summary>
    ///     Update Button Content and Visibility by current page.
    /// </summary>
    /// <param name="page"></param>
    private void UpdateButtonsByCurrentPage(int? page)
    {
        if (PageSize == 0) return;
        var pageCount = TotalCount / PageSize;
        var currentPage = CurrentPage;
        var residue = TotalCount % PageSize;
        if (residue > 0) pageCount++;
        if (_buttonPanel is null)
        {
            SetCurrentValue(PageCountProperty, pageCount);
            return;
        }
        
        if (pageCount <= 7)
        {
            for (var i = 0; i < 7; i++)
                if (i < pageCount)
                {
                    _buttons[i].IsVisible = true;
                    _buttons[i].SetStatus(i + 1, i + 1 == CurrentPage, false, false);
                }
                else
                {
                    _buttons[i].IsVisible = false;
                }
        }
        else
        {
            for (var i = 0; i < 7; i++) _buttons[i].IsVisible = true;
            var mid = currentPage ?? 0;
            mid = Clamp(mid, 4, pageCount - 3);
            _buttons[3].Page = mid;
            _buttons[2].Page = mid - 1;
            _buttons[4].Page = mid + 1;
            _buttons[0].Page = 1;
            _buttons[6].Page = pageCount;
            if (mid > 4)
                _buttons[1].SetStatus(-1, false, true, false);
            else
                _buttons[1].SetStatus(mid - 2, false, false, false);
            if (mid < pageCount - 3)
                _buttons[5].SetStatus(-1, false, false, true);
            else
                _buttons[5].SetStatus(mid + 2, false, false, false);

            foreach (var button in _buttons)
                if (button.Page == currentPage)
                    button.SetSelected(true);
                else
                    button.SetSelected(false);
        }

        SetCurrentValue(PageCountProperty, pageCount);
        SetCurrentValue(CurrentPageProperty, currentPage);
        if (_previousButton != null) _previousButton.IsEnabled = (CurrentPage ?? int.MaxValue) > 1;
        if (_nextButton != null) _nextButton.IsEnabled = (CurrentPage ?? 0) < PageCount;
    }

    private void InvokeCommand()
    {
        if (Command != null && Command.CanExecute(CommandParameter)) Command.Execute(CommandParameter);
    }
}