using System.Collections.Generic;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Primitives;
using Avalonia.Input;
using Avalonia.Input.Platform;
using Avalonia.Layout;
using Avalonia.Media;
using Irihi.Avalonia.Shared.Contracts;

namespace Ursa.Controls;

[TemplatePart(PART_Placeholder, typeof(Visual))]
[TemplatePart(PART_FocusedPlaceholder, typeof(Visual))]
[PseudoClasses(PC_Empty)]
public class KeyGestureInput: TemplatedControl, IClearControl, IInnerContentControl
{
    public const string PC_Empty = ":empty";
    public const string PART_Placeholder = "PART_Placeholder";
    public const string PART_FocusedPlaceholder = "PART_FocusedPlaceholder";

    // Modifier flags cannot distinguish LeftCtrl from RightCtrl, so pending-gesture
    // clearing tracks the physical modifier keys that are still held.
    private readonly HashSet<Key> _heldModifierKeys = [];

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
            x.PseudoClasses.Set(PC_Empty, e.NewValue.Value is null);
            x.UpdateKeys();
        });
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
    /// Gets or sets whether a pending modifier-only recording is cleared when the
    /// modifiers are released without a regular key completing the gesture.
    /// </summary>
    public bool ClearOnModifierRelease
    {
        get => GetValue(ClearOnModifierReleaseProperty);
        set => SetValue(ClearOnModifierReleaseProperty, value);
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
    /// by default. The ClearButton/clearButton style class additionally keeps the
    /// button always visible while a gesture is set.
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
        PseudoClasses.Set(PC_Empty, Gesture is null);
        UpdateKeys();
    }

    protected override void OnKeyDown(KeyEventArgs e)
    {
        // base.OnKeyDown(e);
        if (IsReadOnly)
        {
            return;
        }

        // Escape clears the gesture unless the user explicitly wants to bind Escape itself.
        if (e.Key == Key.Escape && (AcceptableKeys is null || !AcceptableKeys.Contains(Key.Escape)))
        {
            Clear();
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
            SetCurrentValue(GestureProperty, new KeyGesture(Key.None, modifiers));
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
        var gesture = Gesture;
        // Only a pending modifier-only recording is cleared; completed gestures persist.
        if (gesture is null || gesture.Key is not Key.None || gesture.KeyModifiers == KeyModifiers.None)
        {
            return;
        }
        // Clear only after the last physically held modifier is released: the flag check
        // alone cannot distinguish LeftCtrl from RightCtrl, while the physical set alone
        // misses modifiers that were already held before the control got focus. Some
        // platforms also still report the released modifier in the keyup event.
        if (_heldModifierKeys.Count == 0 &&
            (e.KeyModifiers & ~released & gesture.KeyModifiers) == KeyModifiers.None)
        {
            Clear();
            e.Handled = true;
        }
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
        var gesture = Gesture;
        if (gesture is not null && gesture.Key == Key.None && gesture.KeyModifiers != KeyModifiers.None)
        {
            Clear();
        }
    }

    public void Clear()
    {
        SetCurrentValue(GestureProperty, null);
    }

    private void UpdateKeys()
    {
        var gesture = Gesture;
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
