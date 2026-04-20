using System.ComponentModel;
using Ursa.Controls;

namespace Test.Ursa.DateTimePicker;

public class OffsetDefinitionTests
{
    // -------------------------------------------------------------------------
    // Static singletons
    // -------------------------------------------------------------------------

    [Fact]
    public void Utc_Singleton_HasUtcKind()
    {
        Assert.Equal(OffsetDefinitionKind.Utc, OffsetDefinition.Utc.Kind);
    }

    [Fact]
    public void Local_Singleton_HasLocalKind()
    {
        Assert.Equal(OffsetDefinitionKind.Local, OffsetDefinition.Local.Kind);
    }

    [Fact]
    public void Fixed_Factory_SetsOffsetCorrectly()
    {
        var def = OffsetDefinition.Fixed(TimeSpan.FromHours(8));
        Assert.Equal(OffsetDefinitionKind.Fixed, def.Kind);
        Assert.Equal(TimeSpan.FromHours(8), def.Offset);
    }

    // -------------------------------------------------------------------------
    // Parse — named keywords
    // -------------------------------------------------------------------------

    [Theory]
    [InlineData("UTC")]
    [InlineData("utc")]
    [InlineData("Utc")]
    public void Parse_UtcVariants_ReturnsUtcKind(string input)
    {
        var def = OffsetDefinition.Parse(input);
        Assert.Equal(OffsetDefinitionKind.Utc, def.Kind);
    }

    [Theory]
    [InlineData("Local")]
    [InlineData("local")]
    [InlineData("LOCAL")]
    public void Parse_LocalVariants_ReturnsLocalKind(string input)
    {
        var def = OffsetDefinition.Parse(input);
        Assert.Equal(OffsetDefinitionKind.Local, def.Kind);
    }

    // -------------------------------------------------------------------------
    // Parse — fixed offset strings
    // -------------------------------------------------------------------------

    [Theory]
    [InlineData("+08:00", 8, 0)]
    [InlineData("+00:00", 0, 0)]
    [InlineData("+05:30", 5, 30)]
    [InlineData("+14:00", 14, 0)]
    public void Parse_PositiveOffsetString_ReturnsCorrectFixedOffset(string input, int hours, int minutes)
    {
        var def = OffsetDefinition.Parse(input);
        Assert.Equal(OffsetDefinitionKind.Fixed, def.Kind);
        Assert.Equal(TimeSpan.FromHours(hours) + TimeSpan.FromMinutes(minutes), def.Offset);
    }

    [Theory]
    [InlineData("-05:00", -5, 0)]
    [InlineData("-05:30", -5, -30)]
    [InlineData("-12:00", -12, 0)]
    public void Parse_NegativeOffsetString_ReturnsCorrectFixedOffset(string input, int hours, int minutes)
    {
        var def = OffsetDefinition.Parse(input);
        Assert.Equal(OffsetDefinitionKind.Fixed, def.Kind);
        Assert.Equal(TimeSpan.FromHours(hours) + TimeSpan.FromMinutes(minutes), def.Offset);
    }

    [Theory]
    [InlineData("08:00", 8, 0)]
    [InlineData("5:30", 5, 30)]
    public void Parse_NoSignOffsetString_ReturnsPositiveFixedOffset(string input, int hours, int minutes)
    {
        var def = OffsetDefinition.Parse(input);
        Assert.Equal(OffsetDefinitionKind.Fixed, def.Kind);
        Assert.Equal(TimeSpan.FromHours(hours) + TimeSpan.FromMinutes(minutes), def.Offset);
    }

    [Theory]
    [InlineData("0", 0)]
    [InlineData("8", 8)]
    [InlineData("14", 14)]
    [InlineData("+8", 8)]
    public void Parse_IntegerHours_ReturnsCorrectFixedOffset(string input, int expectedHours)
    {
        var def = OffsetDefinition.Parse(input);
        Assert.Equal(OffsetDefinitionKind.Fixed, def.Kind);
        Assert.Equal(TimeSpan.FromHours(expectedHours), def.Offset);
    }

    [Theory]
    [InlineData("-8", -8)]
    [InlineData("-12", -12)]
    public void Parse_NegativeIntegerHours_ReturnsCorrectFixedOffset(string input, int expectedHours)
    {
        var def = OffsetDefinition.Parse(input);
        Assert.Equal(OffsetDefinitionKind.Fixed, def.Kind);
        Assert.Equal(TimeSpan.FromHours(expectedHours), def.Offset);
    }

    [Theory]
    [InlineData("  +08:00  ")]
    [InlineData("  UTC  ")]
    [InlineData("  local  ")]
    public void Parse_WithSurroundingWhitespace_ParsesSuccessfully(string input)
    {
        var def = OffsetDefinition.Parse(input);
        Assert.NotNull(def);
    }

    [Theory]
    [InlineData("invalid")]
    [InlineData("GMT+8")]
    [InlineData("")]
    public void Parse_InvalidInput_ThrowsFormatException(string input)
    {
        Assert.Throws<FormatException>(() => OffsetDefinition.Parse(input));
    }

    // -------------------------------------------------------------------------
    // Resolve
    // -------------------------------------------------------------------------

    [Fact]
    public void Resolve_Utc_ReturnsZero()
    {
        Assert.Equal(TimeSpan.Zero, OffsetDefinition.Utc.Resolve());
    }

    [Fact]
    public void Resolve_Fixed_ReturnsConfiguredOffset()
    {
        var offset = TimeSpan.FromHours(9);
        var def = OffsetDefinition.Fixed(offset);
        Assert.Equal(offset, def.Resolve());
    }

    [Fact]
    public void Resolve_Local_ReturnsCurrentLocalOffset()
    {
        var expected = TimeZoneInfo.Local.GetUtcOffset(DateTime.Now);
        Assert.Equal(expected, OffsetDefinition.Local.Resolve());
    }

    // -------------------------------------------------------------------------
    // ToString
    // -------------------------------------------------------------------------

    [Fact]
    public void ToString_Utc_ReturnsUTC()
    {
        Assert.Equal("UTC", OffsetDefinition.Utc.ToString());
    }

    [Theory]
    [InlineData(8, 0, "+08:00")]
    [InlineData(5, 30, "+05:30")]
    [InlineData(0, 0, "+00:00")]
    [InlineData(-5, -30, "-05:30")]
    [InlineData(-12, 0, "-12:00")]
    public void ToString_Fixed_ReturnsFormattedOffset(int hours, int minutes, string expected)
    {
        var def = OffsetDefinition.Fixed(TimeSpan.FromHours(hours) + TimeSpan.FromMinutes(minutes));
        Assert.Equal(expected, def.ToString());
    }

    [Fact]
    public void ToString_Local_StartsWithLocal()
    {
        Assert.StartsWith("Local", OffsetDefinition.Local.ToString());
    }

    [Fact]
    public void ToString_Local_ContainsOffsetInParentheses()
    {
        var result = OffsetDefinition.Local.ToString();
        Assert.Matches(@"^Local \([+-]\d{2}:\d{2}\)$", result);
    }

    // -------------------------------------------------------------------------
    // OffsetDefinitionConverter (single item TypeConverter)
    // -------------------------------------------------------------------------

    [Theory]
    [InlineData("UTC", OffsetDefinitionKind.Utc)]
    [InlineData("Local", OffsetDefinitionKind.Local)]
    [InlineData("+08:00", OffsetDefinitionKind.Fixed)]
    public void OffsetDefinitionConverter_ConvertFrom_ReturnsCorrectKind(string input, OffsetDefinitionKind expectedKind)
    {
        var converter = TypeDescriptor.GetConverter(typeof(OffsetDefinition));
        var result = converter.ConvertFrom(input);
        var def = Assert.IsType<OffsetDefinition>(result);
        Assert.Equal(expectedKind, def.Kind);
    }

    [Fact]
    public void OffsetDefinitionConverter_CanConvertFromString_ReturnsTrue()
    {
        var converter = TypeDescriptor.GetConverter(typeof(OffsetDefinition));
        Assert.True(converter.CanConvertFrom(typeof(string)));
    }

    // -------------------------------------------------------------------------
    // OffsetDefinitionsConverter (collection TypeConverter)
    // -------------------------------------------------------------------------

    [Fact]
    public void OffsetDefinitionsConverter_SingleItem_ReturnsSingleDefinition()
    {
        var converter = TypeDescriptor.GetConverter(typeof(OffsetDefinitions));
        var result = Assert.IsType<OffsetDefinitions>(converter.ConvertFrom("UTC"));
        Assert.Single(result);
        Assert.Equal(OffsetDefinitionKind.Utc, result[0].Kind);
    }

    [Fact]
    public void OffsetDefinitionsConverter_CommaSeparated_ReturnsAllItems()
    {
        var converter = TypeDescriptor.GetConverter(typeof(OffsetDefinitions));
        var result = Assert.IsType<OffsetDefinitions>(converter.ConvertFrom("UTC, Local, +08:00"));
        Assert.Equal(3, result.Count);
        Assert.Equal(OffsetDefinitionKind.Utc, result[0].Kind);
        Assert.Equal(OffsetDefinitionKind.Local, result[1].Kind);
        Assert.Equal(OffsetDefinitionKind.Fixed, result[2].Kind);
        Assert.Equal(TimeSpan.FromHours(8), result[2].Offset);
    }

    [Fact]
    public void OffsetDefinitionsConverter_CanConvertFromString_ReturnsTrue()
    {
        var converter = TypeDescriptor.GetConverter(typeof(OffsetDefinitions));
        Assert.True(converter.CanConvertFrom(typeof(string)));
    }

    // -------------------------------------------------------------------------
    // OffsetDefinitions collection
    // -------------------------------------------------------------------------

    [Fact]
    public void OffsetDefinitions_DefaultConstructor_IsEmpty()
    {
        var defs = new OffsetDefinitions();
        Assert.Empty(defs);
    }

    [Fact]
    public void OffsetDefinitions_FromEnumerable_ContainsItems()
    {
        var defs = new OffsetDefinitions([OffsetDefinition.Utc, OffsetDefinition.Local]);
        Assert.Equal(2, defs.Count);
    }
}
