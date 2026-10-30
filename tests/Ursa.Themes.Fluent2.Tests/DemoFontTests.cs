using Avalonia.Headless.XUnit;
using Avalonia.Media;
using Avalonia.Platform;
using SkiaSharp;
using Ursa.Fluent2.Demo;
using Xunit;

namespace Ursa.Themes.Fluent2.Tests;

public class DemoFontTests
{
    [AvaloniaTheory]
    [InlineData("Regular")]
    [InlineData("Bold")]
    public void Demo_embeds_cjk_font_faces(string weight)
    {
        var uri = new Uri($"avares://Ursa.Fluent2.Demo/Assets/Fonts/SourceHanSansCN-{weight}.otf");
        Assert.True(AssetLoader.Exists(uri));
        using var stream = AssetLoader.Open(uri);
        Span<byte> header = stackalloc byte[4];
        stream.ReadExactly(header);
        Assert.Equal("OTTO"u8.ToArray(), header.ToArray());
    }

    [AvaloniaFact]
    public void Cjk_font_family_resolves_from_the_demo_assembly()
    {
        var family = new FontFamily(DemoFonts.CjkFamily);
        Assert.Equal("Source Han Sans CN", family.Name);
        Assert.NotNull(family.Key);
        Assert.Equal(new Uri("avares://Ursa.Fluent2.Demo/Assets/Fonts"), family.Key.Source);
    }

    [AvaloniaTheory]
    [InlineData("Regular", 400)]
    [InlineData("Bold", 700)]
    public void Bundled_font_faces_contain_chinese_ui_glyphs(string weight, int fontWeight)
    {
        using var stream = AssetLoader.Open(
            new Uri($"avares://Ursa.Fluent2.Demo/Assets/Fonts/SourceHanSansCN-{weight}.otf"));
        using var typeface = SKTypeface.FromStream(stream);
        Assert.NotNull(typeface);
        Assert.Equal("Source Han Sans CN", typeface.FamilyName);
        Assert.Equal(fontWeight, typeface.FontWeight);
        foreach (var character in "控件总览输入选择日期时间外观设置")
            Assert.True(typeface.ContainsGlyph(character), $"Missing glyph: {character}");
    }
}
