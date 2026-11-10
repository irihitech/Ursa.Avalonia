using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Primitives;
using Avalonia.Input;
using Avalonia.Input.Platform;
using Avalonia.Interactivity;
using Avalonia.Layout;
using Avalonia.Media;
using Irihi.Avalonia.Shared.Contracts;

namespace Ursa.Controls;

[TemplatePart(PART_Placeholder, typeof(Visual))]
[TemplatePart(PART_FocusedPlaceholder, typeof(Visual))]
[TemplatePart(PART_ClearButton, typeof(Button))]
[PseudoClasses(PC_Empty)]
public class KeyGestureInput: TemplatedControl, IClearControl, IInnerContentControl
{
    public const string PC_Empty = ":empty";
    public const string PART_Placeholder = "PART_Placeholder";
    public const string PART_FocusedPlaceholder = "PART_FocusedPlaceholder";
    public const string PART_ClearButton = "PART_ClearButton";

    public static readonly RoutedEvent<KeyGestureInputClearedEventArgs> ClearedEvent =
        RoutedEvent.Register<KeyGestureInput, KeyGestureInputClearedEventArgs>(
            nameof(Cleared), RoutingStrategies.Bubble);

    /// <summary>
    /// Raised when the gesture is cleared. The event args carry the origin of the clear
    /// (see <see cref="KeyGestureInputClearReason"/>).
    /// </summary>
    public event EventHandler<KeyGestureInputClearedEventArgs>? Cleared
    {
        add => AddHandler(ClearedEvent, value);
        remove => RemoveHandler(ClearedEvent, value);
    }

    // Modifier flags cannot distinguish LeftCtrl from RightCtrl, so pending-gesture
    // clearing tracks the physical modifier keys that are still held.
    private readonly HashSet<Key> _heldModifierKeys = [];

    // A modifier-only recording in progress; display-only, never written to Gesture.
    private KeyModifiers _pendingModifiers;

    private Button? _clearButton;

    private static readonly IReadOnlyDictionary<Key, KeyModifiers> ModifierKeyMap =
        new Dictionary<Key, KeyModifiers>
        {
            { Key.LeftCtrl, KeyModifiers.Control }, { Key.RightCtrl, KeyModifiers.Control },
            { Key.LeftAlt, KeyModifiers.Alt }, { Key.RightAlt, KeyModifiers.Alt },
            { Key.LeftShift, KeyModifiers.Shift }, { Key.RightShift, KeyModifiers.Shift },
            { Key.LWin, KeyModifiers.Meta }, { Key.RWin, KeyModifiers.Meta },
        };

    // Same modifier order as KeyGesture.ToString("p").
    private static readonly KeyModifiers[] ModifierOrder =
        [KeyModifiers.Control, KeyModifiers.Shift, KeyModifiers.Alt, KeyModifiers.Meta];

    static KeyGestureInput()
    {
        FocusableProperty.OverrideDefaultValue<KeyGestureInput>(true);
        GestureProperty.Changed.AddClassHandler<KeyGestureInput, KeyGesture?>((x, e) =>
        {
            // An external value replaces any pending recording; a completed recording
            // clears its own pending state before committing.
            x._pendingModifiers = KeyModifiers.None;
            x.UpdateDisplay();
        });
    }

    protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
    {
        base.OnPropertyChanged(change);
        // Mode switches must not leave a pending display stuck with no key events to update it.
        if ((change.Property == IsReadOnlyProperty || change.Property == ClearOnModifierReleaseProperty)
            && _pendingModifiers != KeyModifiers.None)
        {
            _pendingModifiers = KeyModifiers.None;
            UpdateDisplay();
        }
    }

    public static readonly StyledProperty<KeyGesture?> GestureProperty = AvaloniaProperty.Register<KeyGestureInput, KeyGesture?>(
        nameof(Gesture));

    public KeyGesture? Gesture
    {
        get => GetValue(GestureProperty);
        set => SetValue(GestureProperty, value);
    }

    public static readonly DirectProperty<KeyGestureInput, IReadOnlyList<string>> KeysProperty =
        AvaloniaProperty.RegisterDirect<KeyGestureInput, IReadOnlyList<string>>(
            nameof(Keys), o => o.Keys, (o, v) => o.Keys = v);

    /// <summary>
    /// Gets the platform-styled display strings of the current gesture, one per key, in
    /// the same order as <see cref="KeyGesture.ToString(string,System.IFormatProvider)"/> ("p" format).
    /// Modifier keys are always displayed as their platform modifier strings (e.g. ⇧ on macOS,
    /// "Shift" elsewhere) instead of raw key names like "LeftShift".
    /// While a modifier-only recording is in progress, the pending modifiers are shown
    /// (see <see cref="ClearOnModifierRelease"/>).
    /// </summary>
    public IReadOnlyList<string> Keys
    {
        get;
        private set => SetAndRaise(KeysProperty, ref field, value);
    } = [];

    public static readonly StyledProperty<IList<Key>?> AcceptableKeysProperty = AvaloniaProperty.Register<KeyGestureInput, IList<Key>?>(
        nameof(AcceptableKeys));

    public IList<Key>? AcceptableKeys
    {
        get => GetValue(AcceptableKeysProperty);
        set => SetValue(AcceptableKeysProperty, value);
    }

    public static readonly StyledProperty<bool> ConsiderKeyModifiersProperty = AvaloniaProperty.Register<KeyGestureInput, bool>(
        nameof(ConsiderKeyModifiers), true);

    public bool ConsiderKeyModifiers
    {
        get => GetValue(ConsiderKeyModifiersProperty);
        set => SetValue(ConsiderKeyModifiersProperty, value);
    }

    public static readonly StyledProperty<bool> IsReadOnlyProperty = AvaloniaProperty.Register<KeyGestureInput, bool>(
        nameof(IsReadOnly));

    /// <summary>
    /// Gets or sets whether the control only displays the current gesture and ignores key input.
    /// </summary>
    public bool IsReadOnly
    {
        get => GetValue(IsReadOnlyProperty);
        set => SetValue(IsReadOnlyProperty, value);
    }

    public static readonly StyledProperty<bool> RequireModifierProperty = AvaloniaProperty.Register<KeyGestureInput, bool>(
        nameof(RequireModifier));

    /// <summary>
    /// Gets or sets whether a gesture is only accepted when at least one modifier key is held,
    /// so plain keys like single letters are not recorded.
    /// </summary>
    public bool RequireModifier
    {
        get => GetValue(RequireModifierProperty);
        set => SetValue(RequireModifierProperty, value);
    }

    public static readonly StyledProperty<bool> ClearOnModifierReleaseProperty = AvaloniaProperty.Register<KeyGestureInput, bool>(
        nameof(ClearOnModifierRelease), true);

    /// <summary>
    /// Gets or sets whether a pending modifier-only recording follows the modifiers as
    /// they are released, dropping each one from the display and clearing the gesture
    /// once none is held anymore. A pending recording is display-only; it is never
    /// written to <see cref="Gesture"/>. Turn off to record modifier-only gestures.
    /// </summary>
    public bool ClearOnModifierRelease
    {
        get => GetValue(ClearOnModifierReleaseProperty);
        set => SetValue(ClearOnModifierReleaseProperty, value);
    }

    public static readonly StyledProperty<bool> ClearOnEscapeProperty = AvaloniaProperty.Register<KeyGestureInput, bool>(
        nameof(ClearOnEscape), true);

    /// <summary>
    /// Gets or sets whether pressing Escape clears the recorded gesture. When false,
    /// Escape is recorded like any other key (still subject to AcceptableKeys).
    /// </summary>
    public bool ClearOnEscape
    {
        get => GetValue(ClearOnEscapeProperty);
        set => SetValue(ClearOnEscapeProperty, value);
    }

    public static readonly StyledProperty<string?> PlaceholderTextProperty =
        TextBox.PlaceholderTextProperty.AddOwner<KeyGestureInput>();

    public string? PlaceholderText
    {
        get => GetValue(PlaceholderTextProperty);
        set => SetValue(PlaceholderTextProperty, value);
    }

    public static readonly StyledProperty<bool> AllowClearProperty = AvaloniaProperty.Register<KeyGestureInput, bool>(
        nameof(AllowClear), true);

    /// <summary>
    /// Gets or sets whether the built-in clear button is available on focus/hover.
    /// Unlike text inputs, a gesture cannot be removed by backspace, so clearing is on
    /// by default.
    /// </summary>
    public bool AllowClear
    {
        get => GetValue(AllowClearProperty);
        set => SetValue(AllowClearProperty, value);
    }

    public static readonly StyledProperty<string?> FocusedPlaceholderTextProperty =
        AvaloniaProperty.Register<KeyGestureInput, string?>(nameof(FocusedPlaceholderText));

    /// <summary>
    /// Gets or sets the hint shown while the control is focused and empty, e.g. an
    /// "enter a shortcut" prompt. When null, the unfocused <see cref="PlaceholderText"/>
    /// is hidden on focus without a replacement.
    /// </summary>
    public string? FocusedPlaceholderText
    {
        get => GetValue(FocusedPlaceholderTextProperty);
        set => SetValue(FocusedPlaceholderTextProperty, value);
    }

    public static readonly StyledProperty<IBrush?> PlaceholderForegroundProperty =
        TextBox.PlaceholderForegroundProperty.AddOwner<KeyGestureInput>();

    public IBrush? PlaceholderForeground
    {
        get => GetValue(PlaceholderForegroundProperty);
        set => SetValue(PlaceholderForegroundProperty, value);
    }

    public static readonly StyledProperty<HorizontalAlignment> HorizontalContentAlignmentProperty =
        ContentControl.HorizontalContentAlignmentProperty.AddOwner<KeyGestureInput>(
            new StyledPropertyMetadata<HorizontalAlignment>(HorizontalAlignment.Center));

    public HorizontalAlignment HorizontalContentAlignment
    {
        get => GetValue(HorizontalContentAlignmentProperty);
        set => SetValue(HorizontalContentAlignmentProperty, value);
    }

    public static readonly StyledProperty<VerticalAlignment> VerticalContentAlignmentProperty =
        ContentControl.VerticalContentAlignmentProperty.AddOwner<KeyGestureInput>(
            new StyledPropertyMetadata<VerticalAlignment>(VerticalAlignment.Center));

    public VerticalAlignment VerticalContentAlignment
    {
        get => GetValue(VerticalContentAlignmentProperty);
        set => SetValue(VerticalContentAlignmentProperty, value);
    }

    public static readonly StyledProperty<object?> InnerLeftContentProperty = AvaloniaProperty.Register<KeyGestureInput, object?>(
        nameof(InnerLeftContent));

    public object? InnerLeftContent
    {
        get => GetValue(InnerLeftContentProperty);
        set => SetValue(InnerLeftContentProperty, value);
    }

    public static readonly StyledProperty<object?> InnerRightContentProperty = AvaloniaProperty.Register<KeyGestureInput, object?>(
        nameof(InnerRightContent));

    public object? InnerRightContent
    {
        get => GetValue(InnerRightContentProperty);
        set => SetValue(InnerRightContentProperty, value);
    }

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);
        if (_clearButton is not null)
        {
            _clearButton.Click -= OnClearButtonClick;
            _clearButton = null;
        }
        _clearButton = e.NameScope.Find(PART_ClearButton) as Button;
        if (_clearButton is not null)
        {
            _clearButton.Click += OnClearButtonClick;
        }
        UpdateDisplay();
    }

    protected override void OnKeyDown(KeyEventArgs e)
    {
        // base.OnKeyDown(e);
        if (IsReadOnly)
        {
            return;
        }

        // Escape clears the gesture unless the user explicitly wants to bind Escape itself.
        if (e.Key == Key.Escape && ClearOnEscape && (AcceptableKeys is null || !AcceptableKeys.Contains(Key.Escape)))
        {
            ClearCore(KeyGestureInputClearReason.Escape);
            e.Handled = true;
            return;
        }

        if (AcceptableKeys is not null && !AcceptableKeys.Contains(e.Key))
        {
            return;
        }

        var isModifierKey = ModifierKeyMap.ContainsKey(e.Key);

        if (RequireModifier && !isModifierKey && e.KeyModifiers == KeyModifiers.None)
        {
            return;
        }

        if (!ConsiderKeyModifiers)
        {
            if (isModifierKey)
            {
                return;
            }
            SetCurrentValue(GestureProperty, new KeyGesture(e.Key));
            e.Handled = true;
            return;
        }

        if (isModifierKey)
        {
            // Modifier key presses accumulate: show every modifier currently held
            // (at least the one just pressed) until a regular key completes the gesture.
            _heldModifierKeys.Add(e.Key);
            var modifiers = e.KeyModifiers | ModifierKeyMap[e.Key];
            if (ClearOnModifierRelease)
            {
                _pendingModifiers = modifiers;
                UpdateDisplay();
            }
            else
            {
                SetCurrentValue(GestureProperty, new KeyGesture(Key.None, modifiers));
            }
            e.Handled = true;
            return;
        }
        SetCurrentValue(GestureProperty, new KeyGesture(e.Key, e.KeyModifiers));
        e.Handled = true;
    }

    protected override void OnKeyUp(KeyEventArgs e)
    {
        base.OnKeyUp(e);
        if (!ModifierKeyMap.TryGetValue(e.Key, out var released))
        {
            return;
        }
        _heldModifierKeys.Remove(e.Key);
        if (IsReadOnly || !ClearOnModifierRelease)
        {
            return;
        }
        if (_pendingModifiers == KeyModifiers.None)
        {
            // Only a pending modifier-only recording tracks live; completed gestures persist.
            return;
        }
        // The pending recording follows the physically held modifiers: releasing one drops
        // it from the display, unless its other side (e.g. RightCtrl after releasing
        // LeftCtrl) is still held. Modifier flags alone cannot distinguish the two sides,
        // and some platforms still report the released modifier in the keyup event.
        var sameSideHeld = false;
        foreach (var key in _heldModifierKeys)
        {
            if (ModifierKeyMap[key] == released)
            {
                sameSideHeld = true;
                break;
            }
        }
        var remaining = sameSideHeld ? _pendingModifiers : _pendingModifiers & ~released;
        if (remaining == KeyModifiers.None)
        {
            _pendingModifiers = KeyModifiers.None;
            ClearCore(KeyGestureInputClearReason.ModifierRelease);
        }
        else
        {
            _pendingModifiers = remaining;
            UpdateDisplay();
        }
        e.Handled = true;
    }

    protected override void OnLostFocus(FocusChangedEventArgs e)
    {
        base.OnLostFocus(e);
        // The control no longer sees keyups once focus moves elsewhere, so a pending
        // modifier-only recording and the held-key tracking would both go stale.
        _heldModifierKeys.Clear();
        if (IsReadOnly || !ClearOnModifierRelease)
        {
            return;
        }
        if (_pendingModifiers != KeyModifiers.None)
        {
            _pendingModifiers = KeyModifiers.None;
            ClearCore(KeyGestureInputClearReason.LostFocus);
        }
    }

    public void Clear()
    {
        ClearCore(KeyGestureInputClearReason.Programmatic);
    }

    private void ClearCore(KeyGestureInputClearReason reason)
    {
        _pendingModifiers = KeyModifiers.None;
        if (Gesture is null)
        {
            // Nothing committed; only the pending display needs updating.
            UpdateDisplay();
            return;
        }
        SetCurrentValue(GestureProperty, null);
        RaiseEvent(new KeyGestureInputClearedEventArgs(reason) { RoutedEvent = ClearedEvent });
    }

    private void OnClearButtonClick(object? sender, RoutedEventArgs e)
    {
        ClearCore(KeyGestureInputClearReason.ClearButton);
    }

    private void UpdateDisplay()
    {
        PseudoClasses.Set(PC_Empty, Gesture is null && _pendingModifiers == KeyModifiers.None);
        UpdateKeys();
    }

    private void UpdateKeys()
    {
        var gesture = _pendingModifiers != KeyModifiers.None
            ? new KeyGesture(Key.None, _pendingModifiers)
            : Gesture;
        if (gesture is null)
        {
            if (Keys.Count > 0)
            {
                Keys = [];
            }
            return;
        }
        var formatInfo = KeyGestureFormatInfo.GetInstance(null);
        // A modifier key is displayed as its platform modifier string (⇧ on macOS,
        // "Shift" elsewhere), merged with the gesture's modifier flags so it never
        // shows twice and keeps the platform's modifier order.
        var modifiers = gesture.KeyModifiers;
        var isModifierKey = ModifierKeyMap.TryGetValue(gesture.Key, out var keyModifier);
        if (isModifierKey)
        {
            modifiers |= keyModifier;
        }
        var keys = new List<string>(5);
        foreach (var modifier in ModifierOrder)
        {
            if (modifiers.HasFlag(modifier))
            {
                keys.Add(GetModifierDisplay(formatInfo, modifier));
            }
        }
        if (!isModifierKey && (gesture.Key is not Key.None || modifiers == KeyModifiers.None))
        {
            keys.Add(formatInfo.FormatKey(gesture.Key));
        }
        Keys = keys;
    }

    private static string GetModifierDisplay(KeyGestureFormatInfo formatInfo, KeyModifiers modifier)
    {
        return modifier switch
        {
            KeyModifiers.Control => formatInfo.Ctrl,
            KeyModifiers.Shift => formatInfo.Shift,
            KeyModifiers.Alt => formatInfo.Alt,
            KeyModifiers.Meta => formatInfo.Meta,
            _ => string.Empty,
        };
    }
}
