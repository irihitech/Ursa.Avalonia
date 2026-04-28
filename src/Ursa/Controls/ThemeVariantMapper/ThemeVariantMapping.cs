using Avalonia.Styling;

namespace Ursa.Controls;

/// <summary>
/// Describes a single theme-variant mapping entry used by <see cref="ThemeVariantMapper"/>.
/// When the parent's <c>ActualThemeVariant</c> matches <see cref="Source"/>, the mapper
/// applies <see cref="Target"/> as its own requested theme variant.
/// </summary>
public class ThemeVariantMapping
{
    /// <summary>
    /// Gets or sets the parent theme variant to match against.
    /// Use <see cref="ThemeVariant.Default"/>, <see cref="ThemeVariant.Light"/>,
    /// or <see cref="ThemeVariant.Dark"/> (or any custom <see cref="ThemeVariant"/>).
    /// </summary>
    public ThemeVariant? Source { get; set; }

    /// <summary>
    /// Gets or sets the theme variant to apply when the parent theme matches <see cref="Source"/>.
    /// Setting this to <see cref="ThemeVariant.Default"/> means the mapper will inherit
    /// the parent's theme (i.e., no override).
    /// </summary>
    public ThemeVariant? Target { get; set; }
}
