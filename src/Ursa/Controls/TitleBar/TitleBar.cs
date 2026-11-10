using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;

namespace Ursa.Controls;

public class TitleBar : TemplatedControl
{
    public const string PART_TitleBarGrid = "PART_TitleBarGrid";

    private ColumnDefinition? _rightInsetColumn;

    public static readonly StyledProperty<object?> LeftContentProperty = AvaloniaProperty.Register<TitleBar, object?>(
        nameof(LeftContent));

    public object? LeftContent
    {
        get => GetValue(LeftContentProperty);
        set => SetValue(LeftContentProperty, value);
    }

    public static readonly StyledProperty<object?> CenterContentProperty = AvaloniaProperty.Register<TitleBar, object?>(
        nameof(CenterContent));

    public object? CenterContent
    {
        get => GetValue(CenterContentProperty);
        set => SetValue(CenterContentProperty, value);
    }

    public static readonly StyledProperty<object?> RightContentProperty = AvaloniaProperty.Register<TitleBar, object?>(
        nameof(RightContent));

    public object? RightContent
    {
        get => GetValue(RightContentProperty);
        set => SetValue(RightContentProperty, value);
    }

    /// <summary>
    ///     Defines the space reserved to the right of <see cref="RightContent"/>, e.g.
    ///     for the window's caption buttons.
    /// </summary>
    public static readonly StyledProperty<double> RightInsetProperty = AvaloniaProperty.Register<TitleBar, double>(
        nameof(RightInset));

    /// <summary>
    ///     Gets or sets the space reserved to the right of <see cref="RightContent"/>, e.g.
    ///     for the window's caption buttons.
    /// </summary>
    public double RightInset
    {
        get => GetValue(RightInsetProperty);
        set => SetValue(RightInsetProperty, value);
    }

    /// <inheritdoc/>
    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);
        var grid = e.NameScope.Find<Grid>(PART_TitleBarGrid);
        _rightInsetColumn = grid?.ColumnDefinitions.Count == 4 ? grid.ColumnDefinitions[3] : null;
        SyncRightInsetColumn();
    }

    protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
    {
        base.OnPropertyChanged(change);
        if (change.Property == RightInsetProperty)
        {
            SyncRightInsetColumn();
        }
    }

    private void SyncRightInsetColumn()
    {
        if (_rightInsetColumn is not null)
        {
            // Runtime width changes on bound template elements do not reliably
            // re-run the container layout, so the control drives the column width.
            _rightInsetColumn.Width = new GridLength(RightInset, GridUnitType.Pixel);
        }
    }
}