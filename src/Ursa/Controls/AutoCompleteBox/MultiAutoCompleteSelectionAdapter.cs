using System.Collections;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Controls.Utils;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.LogicalTree;

namespace Ursa.Controls;

public class MultiAutoCompleteSelectionAdapter : ISelectionAdapter
{
    /// <summary>
    ///     The SelectingItemsControl instance.
    /// </summary>
    private SelectingItemsControl? _selector;

    private object? _previewSelectedItem;
    private object? _selectedItem;

    /// <summary>
    ///     Initializes a new instance of the
    ///     <see cref="T:Avalonia.Controls.Utils.SelectingItemsControlSelectionAdapter" />
    ///     class.
    /// </summary>
    public MultiAutoCompleteSelectionAdapter()
    {
    }

    /// <summary>
    ///     Initializes a new instance of the
    ///     <see cref="T:Avalonia.Controls.Utils.SelectingItemsControlSelectionAdapterr" />
    ///     class with the specified
    ///     <see cref="T:Avalonia.Controls.Primitives.SelectingItemsControl" />
    ///     control.
    /// </summary>
    /// <param name="selector">
    ///     The
    ///     <see cref="T:Avalonia.Controls.Primitives.SelectingItemsControl" /> control
    ///     to wrap as a
    ///     <see cref="T:Avalonia.Controls.Utils.SelectingItemsControlSelectionAdapter" />.
    /// </param>
    public MultiAutoCompleteSelectionAdapter(SelectingItemsControl selector)
    {
        SelectorControl = selector;
    }


    /// <summary>
    ///     Gets or sets a value indicating whether the selection change event
    ///     should not be fired.
    /// </summary>
    private bool IgnoringSelectionChanged { get; set; }

    /// <summary>
    ///     Gets or sets the underlying
    ///     <see cref="T:Avalonia.Controls.Primitives.SelectingItemsControl" />
    ///     control.
    /// </summary>
    /// <value>
    ///     The underlying
    ///     <see cref="T:Avalonia.Controls.Primitives.SelectingItemsControl" />
    ///     control.
    /// </value>
    public SelectingItemsControl? SelectorControl
    {
        get => _selector;

        set
        {
            if (_selector != null)
            {
                _selector.SelectionChanged -= OnSelectionChanged;
                _selector.PointerReleased -= OnSelectorPointerReleased;
            }

            _selector = value;

            if (_selector != null)
            {
                _selector.SelectionChanged += OnSelectionChanged;
                _selector.PointerReleased += OnSelectorPointerReleased;
            }
        }
    }

    /// <summary>
    ///     Occurs when the
    ///     <see cref="P:Avalonia.Controls.Utils.SelectingItemsControlSelectionAdapter.SelectedItem" />
    ///     property value changes.
    /// </summary>
    public event EventHandler<SelectionChangedEventArgs>? SelectionChanged;

    /// <summary>
    ///     Occurs when an item is selected and is committed to the underlying
    ///     <see cref="T:Avalonia.Controls.Primitives.SelectingItemsControl" />
    ///     control.
    /// </summary>
    public event EventHandler<RoutedEventArgs>? Commit;

    /// <summary>
    ///     Occurs when a selection is canceled before it is committed.
    /// </summary>
    public event EventHandler<RoutedEventArgs>? Cancel;

    /// <summary>
    ///     Gets or sets the selected item of the selection adapter.
    /// </summary>
    /// <value>The selected item of the underlying selection adapter.</value>
    public object? SelectedItem
    {
        get => _selectedItem;

        set
        {
            IgnoringSelectionChanged = true;
            if (SelectorControl != null)
            {
                _selectedItem = value;
                SelectorControl.SelectedItem = value;
            }
            // Attempt to reset the scroll viewer's position
            if (value == null) ResetScrollViewer();
            IgnoringSelectionChanged = false;
        }
    }

    /// <summary>
    ///     Gets or sets a collection that is used to generate the content of
    ///     the selection adapter.
    /// </summary>
    /// <value>
    ///     The collection used to generate content for the selection
    ///     adapter.
    /// </value>
    public IEnumerable? ItemsSource
    {
        get => SelectorControl?.ItemsSource;
        set
        {
            if (SelectorControl != null) SelectorControl.ItemsSource = value;
        }
    }

    /// <summary>
    ///     Provides handling for the
    ///     <see cref="E:Avalonia.Input.InputElement.KeyDown" /> event that occurs
    ///     when a key is pressed while the drop-down portion of the
    ///     <see cref="T:Avalonia.Controls.AutoCompleteBox" /> has focus.
    /// </summary>
    /// <param name="e">
    ///     A <see cref="T:Avalonia.Input.KeyEventArgs" />
    ///     that contains data about the
    ///     <see cref="E:Avalonia.Input.InputElement.KeyDown" /> event.
    /// </param>
    public void HandleKeyDown(KeyEventArgs e)
    {
        switch (e.Key)
        {
            case Key.Enter:
                OnCommit();
                e.Handled = true;
                break;

            case Key.Up:
                SelectedIndexDecrement();
                e.Handled = true;
                break;

            case Key.Down:
                if ((e.KeyModifiers & KeyModifiers.Alt) == KeyModifiers.None)
                {
                    SelectedIndexIncrement();
                    e.Handled = true;
                }

                break;

            case Key.Escape:
                OnCancel();
                e.Handled = true;
                break;
        }
    }

    /// <summary>
    ///     If the control contains a ScrollViewer, this will reset the viewer
    ///     to be scrolled to the top.
    /// </summary>
    private void ResetScrollViewer()
    {
        if (SelectorControl != null)
        {
            var sv = SelectorControl.GetLogicalDescendants().OfType<ScrollViewer>().FirstOrDefault();
            if (sv != null) sv.Offset = new Vector(0, 0);
        }
    }

    /// <summary>
    ///     Handles the mouse left button up event on the selector control.
    /// </summary>
    /// <param name="sender">The source object.</param>
    /// <param name="e">The event data.</param>
    private void OnSelectorPointerReleased(object? sender, PointerReleasedEventArgs e)
    {
        if (e.InitialPressMouseButton == MouseButton.Left) OnCommit();
    }

    /// <summary>
    ///     Handles the SelectionChanged event on the SelectingItemsControl control.
    /// </summary>
    /// <param name="sender">The source object.</param>
    /// <param name="e">The selection changed event data.</param>
    private void OnSelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        if (IgnoringSelectionChanged) return;
        _previewSelectedItem = SelectorControl?.SelectedItem;
    }

    /// <summary>
    ///     Increments the
    ///     <see cref="P:Avalonia.Controls.Primitives.SelectingItemsControl.SelectedIndex" />
    ///     property of the underlying
    ///     <see cref="T:Avalonia.Controls.Primitives.SelectingItemsControl" />
    ///     control.
    /// </summary>
    protected void SelectedIndexIncrement()
    {
        if (SelectorControl != null)
            SelectorControl.SelectedIndex = SelectorControl.SelectedIndex + 1 >= SelectorControl.ItemCount
                ? -1
                : SelectorControl.SelectedIndex + 1;
    }

    /// <summary>
    ///     Decrements the
    ///     <see cref="P:Avalonia.Controls.Primitives.SelectingItemsControl.SelectedIndex" />
    ///     property of the underlying
    ///     <see cref="T:Avalonia.Controls.Primitives.SelectingItemsControl" />
    ///     control.
    /// </summary>
    protected void SelectedIndexDecrement()
    {
        if (SelectorControl != null)
        {
            var index = SelectorControl.SelectedIndex;
            if (index >= 0)
                SelectorControl.SelectedIndex--;
            else if (index == -1) SelectorControl.SelectedIndex = SelectorControl.ItemCount - 1;
        }
    }

    /// <summary>
    ///     Raises the
    ///     <see cref="E:Avalonia.Controls.Utils.SelectingItemsControlSelectionAdapter.Commit" />
    ///     event.
    /// </summary>
    internal void OnCommit()
    {
        if (_previewSelectedItem is null) return;
        SelectedItem = _previewSelectedItem;
        SelectionChanged?.Invoke(this,
            new SelectionChangedEventArgs(
                SelectingItemsControl.SelectionChangedEvent,
                new List<object?>(),
                new List<object?> { SelectedItem }
            )
        );
        Commit?.Invoke(this, new RoutedEventArgs());
        AfterAdapterAction();
    }

    /// <summary>
    ///     Raises the
    ///     <see cref="E:Avalonia.Controls.Utils.SelectingItemsControlSelectionAdapter.Cancel" />
    ///     event.
    /// </summary>
    private void OnCancel()
    {
        Cancel?.Invoke(this, new RoutedEventArgs());
        AfterAdapterAction();
    }
    
    /// <summary>
    ///     Change the selection after the actions are complete.
    /// </summary>
    private void AfterAdapterAction()
    {
        IgnoringSelectionChanged = true;
        if (SelectorControl != null)
        {
            SelectorControl.SelectedItem = null;
            SelectorControl.SelectedIndex = -1;
            _previewSelectedItem = null;
        }
        IgnoringSelectionChanged = false;
    }
}