namespace Ursa.Controls;

/// <summary>
/// Describes the origin of a gesture clear on <see cref="KeyGestureInput"/>.
/// </summary>
public enum KeyGestureInputClearReason
{
    /// <summary>
    /// The gesture was cleared by a call to <see cref="KeyGestureInput.Clear"/>.
    /// </summary>
    Programmatic,

    /// <summary>
    /// The gesture was cleared by clicking the built-in clear button.
    /// </summary>
    ClearButton,

    /// <summary>
    /// The gesture was cleared by pressing Escape while focused.
    /// </summary>
    Escape,

    /// <summary>
    /// The gesture was cleared because a modifier-only recording was abandoned:
    /// the last held modifier key was released without a regular key completing the gesture.
    /// </summary>
    ModifierRelease,

    /// <summary>
    /// The gesture was cleared because focus moved away while a modifier-only
    /// recording was in progress.
    /// </summary>
    LostFocus
}
