using System.Linq;
using Avalonia.Controls;
using Avalonia.Controls.Presenters;
using Avalonia.Headless.XUnit;
using Avalonia.Input;
using Avalonia.VisualTree;
using Ursa.Controls;

namespace HeadlessTest.Ursa.Controls.KeyGestureInputTests;

public class KeyGestureInputBaseTests
{
    [AvaloniaFact]
    public void Keys_Should_Be_Empty_When_Gesture_Is_Null()
    {
        var window = new Window();
        var input = new KeyGestureInput();
        window.Content = input;
        window.Show();

        Assert.Null(input.Gesture);
        Assert.Empty(input.Keys);
    }

    [AvaloniaFact]
    public void Keys_Should_Split_Gesture_Into_Platform_Key_Displays()
    {
        var window = new Window();
        var input = new KeyGestureInput
        {
            Gesture = new KeyGesture(Key.A, KeyModifiers.Control | KeyModifiers.Shift),
        };
        window.Content = input;
        window.Show();

        Assert.Equal(new[] { "Ctrl", "Shift", "A" }, input.Keys);
        // The joined display should stay consistent with the gesture's own platform string.
        Assert.Equal(input.Gesture!.ToString("p", null), string.Join("+", input.Keys));
    }

    [AvaloniaFact]
    public void KeyDown_Should_Record_Gesture_And_Update_Keys()
    {
        var window = new Window();
        var input = new KeyGestureInput();
        window.Content = input;
        window.Show();

        input.RaiseEvent(new KeyEventArgs
        {
            RoutedEvent = InputElement.KeyDownEvent,
            Key = Key.K,
            KeyModifiers = KeyModifiers.Control | KeyModifiers.Alt,
        });

        Assert.Equal(new KeyGesture(Key.K, KeyModifiers.Control | KeyModifiers.Alt), input.Gesture);
        Assert.Equal(new[] { "Ctrl", "Alt", "K" }, input.Keys);
    }

    [AvaloniaFact]
    public void KeyDown_Should_Ignore_Modifiers_When_ConsiderKeyModifiers_Is_False()
    {
        var window = new Window();
        var input = new KeyGestureInput { ConsiderKeyModifiers = false };
        window.Content = input;
        window.Show();

        input.RaiseEvent(new KeyEventArgs
        {
            RoutedEvent = InputElement.KeyDownEvent,
            Key = Key.K,
            KeyModifiers = KeyModifiers.Control | KeyModifiers.Shift,
        });

        Assert.Equal(new KeyGesture(Key.K), input.Gesture);
        Assert.Equal(new[] { "K" }, input.Keys);
    }

    [AvaloniaFact]
    public void Keys_Should_Display_Modifier_Key_As_Modifier_String_Without_Duplicates()
    {
        var window = new Window();
        var input = new KeyGestureInput();
        window.Content = input;
        window.Show();

        // e.g. macOS records Shift as KeyGesture(LeftShift, Shift); it must display
        // only the modifier (⇧ on macOS, "Shift" here), never "LeftShift".
        input.Gesture = new KeyGesture(Key.LeftShift, KeyModifiers.Shift);
        Assert.Equal(new[] { "Shift" }, input.Keys);

        input.Gesture = new KeyGesture(Key.LWin, KeyModifiers.Meta);
        Assert.Equal(new[] { "Cmd" }, input.Keys);

        // A modifier-only gesture (accumulated modifier presses) displays every modifier.
        input.Gesture = new KeyGesture(Key.None, KeyModifiers.Control | KeyModifiers.Shift);
        Assert.Equal(new[] { "Ctrl", "Shift" }, input.Keys);

        // A bare modifier key gesture displays as the modifier alone.
        input.Gesture = new KeyGesture(Key.LeftShift);
        Assert.Equal(new[] { "Shift" }, input.Keys);

        // A modifier key plus other flags deduplicates and keeps ToString's order.
        input.Gesture = new KeyGesture(Key.RWin, KeyModifiers.Shift | KeyModifiers.Meta);
        Assert.Equal(new[] { "Shift", "Cmd" }, input.Keys);
    }

    [AvaloniaFact]
    public void KeyDown_Of_Modifier_Keys_Should_Accumulate_Held_Modifiers()
    {
        var window = new Window();
        var input = new KeyGestureInput();
        window.Content = input;
        window.Show();

        // Holding modifiers accumulates instead of overwriting with the last one.
        input.RaiseEvent(new KeyEventArgs
        {
            RoutedEvent = InputElement.KeyDownEvent,
            Key = Key.LeftCtrl,
            KeyModifiers = KeyModifiers.Control,
        });
        Assert.Equal(new KeyGesture(Key.None, KeyModifiers.Control), input.Gesture);
        Assert.Equal(new[] { "Ctrl" }, input.Keys);

        input.RaiseEvent(new KeyEventArgs
        {
            RoutedEvent = InputElement.KeyDownEvent,
            Key = Key.LeftShift,
            KeyModifiers = KeyModifiers.Control | KeyModifiers.Shift,
        });
        Assert.Equal(new KeyGesture(Key.None, KeyModifiers.Control | KeyModifiers.Shift), input.Gesture);
        Assert.Equal(new[] { "Ctrl", "Shift" }, input.Keys);

        // A modifier keydown without flags still shows at least its own modifier.
        input.RaiseEvent(new KeyEventArgs
        {
            RoutedEvent = InputElement.KeyDownEvent,
            Key = Key.LWin,
            KeyModifiers = KeyModifiers.None,
        });
        Assert.Equal(new KeyGesture(Key.None, KeyModifiers.Meta), input.Gesture);
        Assert.Equal(new[] { "Cmd" }, input.Keys);

        // A regular key freezes the full combination.
        input.RaiseEvent(new KeyEventArgs
        {
            RoutedEvent = InputElement.KeyDownEvent,
            Key = Key.A,
            KeyModifiers = KeyModifiers.Control | KeyModifiers.Shift | KeyModifiers.Meta,
        });
        Assert.Equal(new KeyGesture(Key.A, KeyModifiers.Control | KeyModifiers.Shift | KeyModifiers.Meta), input.Gesture);
        Assert.Equal(new[] { "Ctrl", "Shift", "Cmd", "A" }, input.Keys);
    }

    [AvaloniaFact]
    public void Modifier_Release_Should_Clear_Pending_Modifier_Only_Gesture()
    {
        var window = new Window();
        var input = new KeyGestureInput();
        window.Content = input;
        window.Show();

        input.RaiseEvent(new KeyEventArgs
        {
            RoutedEvent = InputElement.KeyDownEvent,
            Key = Key.LeftCtrl,
            KeyModifiers = KeyModifiers.Control,
        });
        Assert.Equal(new KeyGesture(Key.None, KeyModifiers.Control), input.Gesture);

        input.RaiseEvent(new KeyEventArgs
        {
            RoutedEvent = InputElement.KeyUpEvent,
            Key = Key.LeftCtrl,
            KeyModifiers = KeyModifiers.None,
        });
        Assert.Null(input.Gesture);
        Assert.Empty(input.Keys);
    }

    [AvaloniaFact]
    public void Modifier_Release_Should_Update_Pending_Gesture_Live()
    {
        var window = new Window();
        var input = new KeyGestureInput();
        window.Content = input;
        window.Show();

        input.RaiseEvent(new KeyEventArgs
        {
            RoutedEvent = InputElement.KeyDownEvent,
            Key = Key.LeftShift,
            KeyModifiers = KeyModifiers.Control | KeyModifiers.Shift,
        });

        // Releasing Shift while Ctrl is still held drops only Shift from the display.
        input.RaiseEvent(new KeyEventArgs
        {
            RoutedEvent = InputElement.KeyUpEvent,
            Key = Key.LeftShift,
            KeyModifiers = KeyModifiers.Control,
        });
        Assert.Equal(new KeyGesture(Key.None, KeyModifiers.Control), input.Gesture);
        Assert.Equal(new[] { "Ctrl" }, input.Keys);

        input.RaiseEvent(new KeyEventArgs
        {
            RoutedEvent = InputElement.KeyUpEvent,
            Key = Key.LeftCtrl,
            KeyModifiers = KeyModifiers.None,
        });
        Assert.Null(input.Gesture);
    }

    [AvaloniaFact]
    public void ClearOnModifierRelease_False_Should_Keep_Pending_Gesture()
    {
        var window = new Window();
        var input = new KeyGestureInput { ClearOnModifierRelease = false };
        window.Content = input;
        window.Show();

        input.RaiseEvent(new KeyEventArgs
        {
            RoutedEvent = InputElement.KeyDownEvent,
            Key = Key.LeftCtrl,
            KeyModifiers = KeyModifiers.Control,
        });
        input.RaiseEvent(new KeyEventArgs
        {
            RoutedEvent = InputElement.KeyUpEvent,
            Key = Key.LeftCtrl,
            KeyModifiers = KeyModifiers.None,
        });
        Assert.Equal(new KeyGesture(Key.None, KeyModifiers.Control), input.Gesture);
    }

    [AvaloniaFact]
    public void Modifier_Release_Should_Not_Clear_Completed_Gesture()
    {
        var window = new Window();
        var input = new KeyGestureInput
        {
            Gesture = new KeyGesture(Key.A, KeyModifiers.Control),
        };
        window.Content = input;
        window.Show();

        input.RaiseEvent(new KeyEventArgs
        {
            RoutedEvent = InputElement.KeyUpEvent,
            Key = Key.LeftCtrl,
            KeyModifiers = KeyModifiers.None,
        });
        Assert.Equal(new KeyGesture(Key.A, KeyModifiers.Control), input.Gesture);
    }

    [AvaloniaFact]
    public void Placeholder_Should_Swap_Between_Unfocused_And_Focused()
    {
        var window = new Window();
        var input = new KeyGestureInput();
        window.Content = input;
        window.Show();
        window.Focus();

        var placeholder = input.GetVisualDescendants().OfType<TextBlock>()
            .Single(t => t.Name == "PART_Placeholder");
        var focusedPlaceholder = input.GetVisualDescendants().OfType<TextBlock>()
            .Single(t => t.Name == "PART_FocusedPlaceholder");

        // Unfocused: the default hint is visible, the focused hint is not.
        Assert.True(placeholder.IsVisible);
        Assert.False(focusedPlaceholder.IsVisible);

        // Focused: hints swap.
        input.Focus();
        Assert.False(placeholder.IsVisible);
        Assert.True(focusedPlaceholder.IsVisible);

        // A gesture hides both.
        input.Gesture = new KeyGesture(Key.A, KeyModifiers.Control);
        Assert.False(placeholder.IsVisible);
        Assert.False(focusedPlaceholder.IsVisible);

        // Clearing while focused returns to the focused hint.
        input.Clear();
        Assert.True(focusedPlaceholder.IsVisible);
    }

    [AvaloniaFact]
    public void IsReadOnly_Should_Ignore_All_Key_Input()
    {
        var window = new Window();
        var input = new KeyGestureInput
        {
            IsReadOnly = true,
            Gesture = new KeyGesture(Key.A, KeyModifiers.Control),
        };
        window.Content = input;
        window.Show();

        input.RaiseEvent(new KeyEventArgs
        {
            RoutedEvent = InputElement.KeyDownEvent,
            Key = Key.B,
            KeyModifiers = KeyModifiers.Control,
        });
        Assert.Equal(new KeyGesture(Key.A, KeyModifiers.Control), input.Gesture);

        // Escape and modifier-release clearing are also disabled in read-only mode.
        input.RaiseEvent(new KeyEventArgs
        {
            RoutedEvent = InputElement.KeyDownEvent,
            Key = Key.Escape,
        });
        Assert.Equal(new KeyGesture(Key.A, KeyModifiers.Control), input.Gesture);

        input.Gesture = new KeyGesture(Key.None, KeyModifiers.Control);
        input.RaiseEvent(new KeyEventArgs
        {
            RoutedEvent = InputElement.KeyUpEvent,
            Key = Key.LeftCtrl,
            KeyModifiers = KeyModifiers.None,
        });
        Assert.Equal(new KeyGesture(Key.None, KeyModifiers.Control), input.Gesture);
    }

    [AvaloniaFact]
    public void RequireModifier_Should_Reject_Plain_Keys()
    {
        var window = new Window();
        var input = new KeyGestureInput { RequireModifier = true };
        window.Content = input;
        window.Show();

        input.RaiseEvent(new KeyEventArgs
        {
            RoutedEvent = InputElement.KeyDownEvent,
            Key = Key.A,
        });
        Assert.Null(input.Gesture);

        // Modifier accumulation is still allowed.
        input.RaiseEvent(new KeyEventArgs
        {
            RoutedEvent = InputElement.KeyDownEvent,
            Key = Key.LeftCtrl,
            KeyModifiers = KeyModifiers.Control,
        });
        Assert.Equal(new KeyGesture(Key.None, KeyModifiers.Control), input.Gesture);

        // A completed gesture with modifiers is accepted.
        input.RaiseEvent(new KeyEventArgs
        {
            RoutedEvent = InputElement.KeyDownEvent,
            Key = Key.A,
            KeyModifiers = KeyModifiers.Control,
        });
        Assert.Equal(new KeyGesture(Key.A, KeyModifiers.Control), input.Gesture);
    }

    [AvaloniaFact]
    public void Escape_Should_Clear_Gesture()
    {
        var window = new Window();
        var input = new KeyGestureInput
        {
            Gesture = new KeyGesture(Key.A, KeyModifiers.Control),
        };
        window.Content = input;
        window.Show();

        input.RaiseEvent(new KeyEventArgs
        {
            RoutedEvent = InputElement.KeyDownEvent,
            Key = Key.Escape,
        });
        Assert.Null(input.Gesture);
        Assert.Empty(input.Keys);
    }

    [AvaloniaFact]
    public void Escape_Should_Record_When_AcceptableKeys_Contains_It()
    {
        var window = new Window();
        var input = new KeyGestureInput
        {
            AcceptableKeys = new List<Key> { Key.Escape },
        };
        window.Content = input;
        window.Show();

        input.RaiseEvent(new KeyEventArgs
        {
            RoutedEvent = InputElement.KeyDownEvent,
            Key = Key.Escape,
        });
        Assert.Equal(new KeyGesture(Key.Escape), input.Gesture);
    }

    [AvaloniaFact]
    public void Theme_Should_Provide_Localized_Default_Placeholder()
    {
        var window = new Window();
        var input = new KeyGestureInput();
        window.Content = input;
        window.Show();

        Assert.False(string.IsNullOrEmpty(input.PlaceholderText));

        // A local value overrides the theme default.
        input.PlaceholderText = "Custom";
        Assert.Equal("Custom", input.PlaceholderText);
    }

    [AvaloniaFact]
    public void Small_And_Large_Classes_Should_Change_MinHeight()
    {
        var window = new Window();
        var input = new KeyGestureInput { Gesture = new KeyGesture(Key.A, KeyModifiers.Control) };
        window.Content = input;
        window.Show();

        var height = input.MinHeight;
        input.Classes.Add("Small");
        var small = input.MinHeight;
        input.Classes.Remove("Small");
        input.Classes.Add("Large");
        var large = input.MinHeight;

        Assert.True(small < height);
        Assert.True(large > height);
    }

    [AvaloniaFact]
    public void Focus_Loss_Should_Clear_Pending_Modifier_Only_Gesture()
    {
        var window = new Window();
        var input = new KeyGestureInput();
        var other = new Button();
        window.Content = new StackPanel { Children = { input, other } };
        window.Show();

        input.Focus();
        input.RaiseEvent(new KeyEventArgs
        {
            RoutedEvent = InputElement.KeyDownEvent,
            Key = Key.LeftCtrl,
            KeyModifiers = KeyModifiers.Control,
        });
        Assert.Equal(new KeyGesture(Key.None, KeyModifiers.Control), input.Gesture);

        // After focus moves elsewhere the control no longer sees the modifier keyup,
        // so the pending recording is cancelled on focus loss instead.
        other.Focus();
        Assert.Null(input.Gesture);

        // A completed gesture survives focus loss.
        input.Focus();
        input.Gesture = new KeyGesture(Key.A, KeyModifiers.Control);
        other.Focus();
        Assert.Equal(new KeyGesture(Key.A, KeyModifiers.Control), input.Gesture);
    }

    [AvaloniaFact]
    public void Releasing_One_Side_Of_A_Modifier_Should_Keep_Pending_Gesture()
    {
        var window = new Window();
        var input = new KeyGestureInput();
        window.Content = input;
        window.Show();

        input.RaiseEvent(new KeyEventArgs
        {
            RoutedEvent = InputElement.KeyDownEvent,
            Key = Key.LeftCtrl,
            KeyModifiers = KeyModifiers.Control,
        });
        input.RaiseEvent(new KeyEventArgs
        {
            RoutedEvent = InputElement.KeyDownEvent,
            Key = Key.RightCtrl,
            KeyModifiers = KeyModifiers.Control,
        });
        Assert.Equal(new KeyGesture(Key.None, KeyModifiers.Control), input.Gesture);

        // RightCtrl is still physically held, so the pending gesture is kept.
        input.RaiseEvent(new KeyEventArgs
        {
            RoutedEvent = InputElement.KeyUpEvent,
            Key = Key.LeftCtrl,
            KeyModifiers = KeyModifiers.Control,
        });
        Assert.Equal(new KeyGesture(Key.None, KeyModifiers.Control), input.Gesture);

        input.RaiseEvent(new KeyEventArgs
        {
            RoutedEvent = InputElement.KeyUpEvent,
            Key = Key.RightCtrl,
            KeyModifiers = KeyModifiers.None,
        });
        Assert.Null(input.Gesture);
    }

    [AvaloniaFact]
    public void Clear_Should_Reset_Gesture_And_Keys()
    {
        var window = new Window();
        var input = new KeyGestureInput
        {
            Gesture = new KeyGesture(Key.A, KeyModifiers.Control),
        };
        window.Content = input;
        window.Show();

        input.Clear();

        Assert.Null(input.Gesture);
        Assert.Empty(input.Keys);
    }

    [AvaloniaFact]
    public void Template_Should_Render_Each_Key_As_Tag()
    {
        var window = new Window();
        var input = new KeyGestureInput
        {
            Gesture = new KeyGesture(Key.A, KeyModifiers.Control | KeyModifiers.Shift),
        };
        window.Content = input;
        window.Show();

        var itemsControl = input.GetVisualDescendants().OfType<ItemsControl>().Single();
        var itemTexts = itemsControl.GetVisualDescendants()
            .OfType<TextBlock>()
            .Select(t => t.Text)
            .ToList();
        // One keycap tag per key, without separators between them.
        Assert.Equal(new[] { "Ctrl", "Shift", "A" }, itemTexts);
    }

    [AvaloniaFact]
    public void ClearButton_Should_Be_Visible_When_Not_Empty_And_PointerOver()
    {
        var window = new Window();
        var input = new KeyGestureInput
        {
            Gesture = new KeyGesture(Key.A, KeyModifiers.Control),
        };
        window.Content = input;
        window.Show();

        var button = input.GetVisualDescendants().OfType<Button>()
            .Single(b => b.Name == "PART_ClearButton");
        Assert.False(button.IsVisible);

        ((IPseudoClasses)input.Classes).Set(":pointerover", true);
        Assert.True(button.IsVisible);

        button.Command?.Execute(null);
        Assert.Null(input.Gesture);
        Assert.Empty(input.Keys);
        Assert.False(button.IsVisible);
    }

    [AvaloniaFact]
    public void AllowClear_False_Should_Hide_ClearButton()
    {
        var window = new Window();
        var input = new KeyGestureInput
        {
            Gesture = new KeyGesture(Key.A, KeyModifiers.Control),
            AllowClear = false,
        };
        window.Content = input;
        window.Show();

        var button = input.GetVisualDescendants().OfType<Button>()
            .Single(b => b.Name == "PART_ClearButton");
        Assert.False(button.IsVisible);

        ((IPseudoClasses)input.Classes).Set(":pointerover", true);
        Assert.False(button.IsVisible);
    }
}
