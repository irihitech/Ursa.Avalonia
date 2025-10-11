namespace Ursa.Controls;

/// <summary>
/// Describes the alignment of items with header and content in a collection control.
/// </summary>
public enum ItemAlignment
{
    /// <summary>
    /// The separate line of header and content is aligned. Header right aligned and Content left aligned.
    /// </summary>
    Center,
    /// <summary>
    /// The separate line of header and content is aligned. Header left aligned and Content left aligned.
    /// </summary>
    Left, 
    /// <summary>
    /// Content docks to the right of Header.
    /// </summary>
    PLain,
    /// <summary>
    /// The separate line of header and content is aligned. Header left aligned and Content right aligned.
    /// </summary>
    Justify,
}