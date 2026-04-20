using System.ComponentModel;
using Ursa.Controls;

namespace Test.Ursa.DateTimePicker;

public class OffsetDefinitionTests
{
    // -------------------------------------------------------------------------
    // OffsetValue struct  static factories
    // -------------------------------------------------------------------------

    [Fact]
    public void OffsetValue_Utc_FlagsAreCorrect()
    {
        Assert.True(OffsetValue.Utc.IsUtc);
        Assert.False(OffsetValue.Utc.IsLocal);
        Assert.False(OffsetValue.Utc.IsFixed);
    }

    [Fact]
    public void OffsetValue_Local_FlagsAreCorrect()
    {
        Assert.True(OffsetValue.Local.IsLocal);
        Assert.False(OffsetValue.Local.IsUtc);
        Assert.False(OffsetValue.Local.IsFixed);
    }

    [Fact]
    public void OffsetValue_Fixed_FlagsAndOffsetAreCorrect()
    {
        var val = OffsetValue.Fixed(TimeSpan.FromHours(8));
        Assert.True(val.IsFixed);
        Assert.False(val.IsUtc);
        Assert.False(val.IsLocal);
        Assert.Equal(TimeSpan.FromHours(8), val.FixedOffset);
    }

    // -------------------------------------------------------------------------
    // OffsetValue  equality
    // -------------------------------------------------------------------------

    [Fact]
    public void OffsetValue_EqualityOperator_SameValues_AreEqual()
    {
        Assert.True(OffsetValue.Utc == OffsetValue.Utc);
        Assert.True(OffsetValue.Local == OffsetValue.Local);
        Assert.True(OffsetValue.Fixed(TimeSpan.FromHours(8)) == OffsetValue.Fixed(TimeSpan.FromHours(8)));
    }

    [Fact]
    public void OffsetValue_InequalityOperator_DifferentKinds_AreNotEqual()
    {
        Assert.True(OffsetValue.Utc != OffsetValue.Local);
        Assert.True(OffsetValue.Utc != OffsetValue.Fixed(TimeSpan.Zero));
    }

    [Fact]
    public void OffsetValue_InequalityOperator_DifferentFixedOffsets_AreNotEqual()
    {
        Assert.True(OffsetValue.Fixed(TimeSpan.FromHours(8)) != OffsetValue.Fixed(TimeSpan.FromHours(9)));
    }

    // -------------------------------------------------------------------------
    // OffsetValue.Parse  named keywords
    // -------------------------------------------------------------------------

    [Theory]
    [InlineData("UTC")]
    [InlineData("utc")]
    [InlineData("Utc")]
    public void OffsetValue_Parse_UtcVariants_IsUtc(string input)
    {
        Assert.True(OffsetValue.Parse(input).IsUtc);
    }

    [Theory]
    [InlineData("Local")]
    [InlineData("local")]
    [InlineData("LOCAL")]
    public void OffsetValue_Parse_LocalVariants_IsLocal(string input)
    {
        Assert.True(OffsetValue.Parse(input).IsLocal);
    }

    // -------------------------------------------------------------------------
    // OffsetValue.Parse  fixed offset strings
    // -------------------------------------------------------------------------

    [Theory]
    [InlineData("+08:00", 8, 0)]
    [InlineData("+00:00", 0, 0)]
    [InlineData("+05:30", 5, 30)]
    [InlineData("+14:00", 14, 0)]
    public void OffsetValue_Parse_PositiveOffset_ReturnsCorrectFixed(string input, int hours, int minutes)
    {
        var val = OffsetValue.Parse(input);
        Assert.True(val.IsFixed);
        Assert.Equal(TimeSpan.FromHours(hours) + TimeSpan.FromMinutes(minutes), val.FixedOffset);
    }

    [Theory]
    [InlineData("-05:00", -5, 0)]
    [InlineData("-05:30", -5, -30)]
    [InlineData("-12:00", -12, 0)]
    public void OffsetValue_Parse_NegativeOffset_ReturnsCorrectFixed(string input, int hours, int minutes)
    {
        var val = OffsetValue.Parse(input);
        Assert.True(val.IsFixed);
        Assert.Equal(TimeSpan.FromHours(hours) + TimeSpan.FromMinutes(minutes), val.FixedOffset);
    }

    [Theory]
    [InlineData("08:00", 8, 0)]
    [InlineData("5:30", 5, 30)]
    public void OffsetValue_Parse_NoSignOffset_ReturnsPositiveFixed(string input, int hours, int minutes)
    {
        var val = OffsetValue.Parse(input);
        Assert.True(val.IsFixed);
        Assert.Equal(TimeSpan.FromHours(hours) + TimeSpan.FromMinutes(minutes), val.FixedOffset);
    }

    [Theory]
    [InlineData("0", 0)]
    [InlineData("8", 8)]
    [InlineData("14", 14)]
    [InlineData("+8", 8)]
    public void OffsetValue_Parse_IntegerHours_ReturnsCorrectFixed(string input, int expectedHours)
    {
        var val = OffsetValue.Parse(input);
        Assert.True(val.IsFixed);
        Assert.Equal(TimeSpan.FromHours(expectedHours), val.FixedOffset);
    }

    [Theory]
    [InlineData("-8", -8)]
    [InlineData("-12", -12)]
    public void OffsetValue_Parse_NegativeIntegerHours_ReturnsCorrectFixed(string input, int expectedHours)
    {
        var val = OffsetValue.Parse(input);
        Assert.True(val.IsFixed);
        Assert.Equal(TimeSpan.FromHours(expectedHours), val.FixedOffset);
    }

    [Theory]
    [InlineData("  +08:00  ", false, false, true)]
    [InlineData("  UTC  ", true, false, false)]
    [InlineData("  local  ", false, true, false)]
    public void OffsetValue_Parse_WithSurroundingWhitespace_ParsesSuccessfully(
        string input, bool isUtc, bool isLocal, bool isFixed)
    {
        var val = OffsetValue.Parse(input);
        Assert.Equal(isUtc, val.IsUtc);
        Assert.Equal(isLocal, val.IsLocal);
        Assert.Equal(isFixed, val.IsFixed);
    }

    [Theory]
    [InlineData("invalid")]
    [InlineData("GMT+8")]
    [InlineData("")]
    public void OffsetValue_Parse_InvalidInput_ThrowsFormatException(string input)
    {
        Assert.Throws<FormatException>(() => OffsetValue.Parse(input));
    }

    // -------------------------------------------------------------------------
    // OffsetValue.Resolve
    // -------------------------------------------------------------------------

    [Fact]
    public void OffsetValue_Resolve_Utc_ReturnsZero()
    {
        Assert.Equal(TimeSpan.Zero, OffsetValue.Utc.Resolve());
    }

    [Fact]
    public void OffsetValue_Resolve_Fixed_ReturnsConfiguredOffset()
    {
        var offset = TimeSpan.FromHours(9);
        Assert.Equal(offset, OffsetValue.Fixed(offset).Resolve());
    }

    [Fact]
    public void OffsetValue_Resolve_Local_ReturnsCurrentLocalOffset()
    {
        var expected = TimeZoneInfo.Local.GetUtcOffset(DateTime.Now);
        Assert.Equal(expected, OffsetValue.Local.Resolve());
    }

    // -------------------------------------------------------------------------
    // OffsetValue.ToString
    // -------------------------------------------------------------------------

    [Fact]
    public void OffsetValue_ToString_Utc_ReturnsUTC()
    {
        Assert.Equal("UTC", OffsetValue.Utc.ToString());
    }

    [Theory]
    [InlineData(8, 0, "+08:00")]
    [InlineData(5, 30, "+05:30")]
    [InlineData(0, 0, "+00:00")]
    [InlineData(-5, -30, "-05:30")]
    [InlineData(-12, 0, "-12:00")]
    public void OffsetValue_ToString_Fixed_ReturnsFormattedOffset(int hours, int minutes, string expected)
    {
        var val = OffsetValue.Fixed(TimeSpan.FromHours(hours) + TimeSpan.FromMinutes(minutes));
        Assert.Equal(expected, val.ToString());
    }

    [Fact]
    public void OffsetValue_ToString_Local_MatchesPattern()
    {
        Assert.Matches(@"^Local \([+-]\d{2}:\d{2}\)$", OffsetValue.Local.ToString());
    }

    // -------------------------------------------------------------------------
    // OffsetDefinition class  static singletons and factory
    // -------------------------------------------------------------------------

    [Fact]
    public void OffsetDefinition_Utc_Singleton_HasUtcOffset()
    {
        Assert.True(OffsetDefinition.Utc.Offset.IsUtc);
    }

    [Fact]
    public void OffsetDefinition_Local_Singleton_HasLocalOffset()
    {
        Assert.True(OffsetDefinition.Local.Offset.IsLocal);
    }

    [Fact]
    public void OffsetDefinition_Fixed_Factory_HasFixedOffsetValue()
    {
        var def = OffsetDefinition.Fixed(TimeSpan.FromHours(8));
        Assert.True(def.Offset.IsFixed);
        Assert.Equal(TimeSpan.FromHours(8), def.Offset.FixedOffset);
    }

    [Fact]
    public void OffsetDefinition_DefaultConstructor_OffsetIsDefaultUtc()
    {
        var def = new OffsetDefinition();
        Assert.True(def.Offset.IsUtc);
    }

    // -------------------------------------------------------------------------
    // OffsetDefinition.Resolve (delegates to OffsetValue)
    // -------------------------------------------------------------------------

    [Fact]
    public void OffsetDefinition_Resolve_Utc_ReturnsZero()
    {
        Assert.Equal(TimeSpan.Zero, OffsetDefinition.Utc.Resolve());
    }

    [Fact]
    public void OffsetDefinition_Resolve_Fixed_ReturnsConfiguredOffset()
    {
        var offset = TimeSpan.FromHours(9);
        Assert.Equal(offset, OffsetDefinition.Fixed(offset).Resolve());
    }

    [Fact]
    public void OffsetDefinition_Resolve_Local_ReturnsCurrentLocalOffset()
    {
        var expected = TimeZoneInfo.Local.GetUtcOffset(DateTime.Now);
        Assert.Equal(expected, OffsetDefinition.Local.Resolve());
    }

    // -------------------------------------------------------------------------
    // OffsetDefinition.ToString (delegates to OffsetValue, or DisplayName)
    // -------------------------------------------------------------------------

    [Fact]
    public void OffsetDefinition_ToString_Utc_ReturnsUTC()
    {
        Assert.Equal("UTC", OffsetDefinition.Utc.ToString());
    }

    [Fact]
    public void OffsetDefinition_ToString_Local_MatchesPattern()
    {
        Assert.Matches(@"^Local \([+-]\d{2}:\d{2}\)$", OffsetDefinition.Local.ToString());
    }

    [Fact]
    public void OffsetDefinition_ToString_WithDisplayName_ReturnsDisplayName()
    {
        var def = new OffsetDefinition { Offset = OffsetValue.Fixed(TimeSpan.FromHours(8)), DisplayName = "Beijing" };
        Assert.Equal("Beijing", def.ToString());
    }

    [Fact]
    public void OffsetDefinition_ToString_NullDisplayName_FallsBackToOffset()
    {
        var def = new OffsetDefinition { Offset = OffsetValue.Fixed(TimeSpan.FromHours(8)), DisplayName = null };
        Assert.Equal("+08:00", def.ToString());
    }

    // -------------------------------------------------------------------------
    // OffsetValueConverter
    // -------------------------------------------------------------------------

    [Fact]
    public void OffsetValueConverter_CanConvertFromString_ReturnsTrue()
    {
        var converter = TypeDescriptor.GetConverter(typeof(OffsetValue));
        Assert.True(converter.CanConvertFrom(typeof(string)));
    }

    [Fact]
    public void OffsetValueConverter_ConvertFrom_UTC_IsUtc()
    {
        var converter = TypeDescriptor.GetConverter(typeof(OffsetValue));
        var result = Assert.IsType<OffsetValue>(converter.ConvertFrom("UTC"));
        Assert.True(result.IsUtc);
    }

    [Fact]
    public void OffsetValueConverter_ConvertFrom_Local_IsLocal()
    {
        var converter = TypeDescriptor.GetConverter(typeof(OffsetValue));
        var result = Assert.IsType<OffsetValue>(converter.ConvertFrom("Local"));
        Assert.True(result.IsLocal);
    }

    [Fact]
    public void OffsetValueConverter_ConvertFrom_FixedOffset_IsFixed()
    {
        var converter = TypeDescriptor.GetConverter(typeof(OffsetValue));
        var result = Assert.IsType<OffsetValue>(converter.ConvertFrom("+08:00"));
        Assert.True(result.IsFixed);
        Assert.Equal(TimeSpan.FromHours(8), result.FixedOffset);
    }

    // -------------------------------------------------------------------------
    // OffsetDefinitionConverter (single item TypeConverter)
    // -------------------------------------------------------------------------

    [Fact]
    public void OffsetDefinitionConverter_CanConvertFromString_ReturnsTrue()
    {
        var converter = TypeDescriptor.GetConverter(typeof(OffsetDefinition));
        Assert.True(converter.CanConvertFrom(typeof(string)));
    }

    [Fact]
    public void OffsetDefinitionConverter_ConvertFrom_UTC_IsUtc()
    {
        var converter = TypeDescriptor.GetConverter(typeof(OffsetDefinition));
        var def = Assert.IsType<OffsetDefinition>(converter.ConvertFrom("UTC"));
        Assert.True(def.Offset.IsUtc);
    }

    [Fact]
    public void OffsetDefinitionConverter_ConvertFrom_Local_IsLocal()
    {
        var converter = TypeDescriptor.GetConverter(typeof(OffsetDefinition));
        var def = Assert.IsType<OffsetDefinition>(converter.ConvertFrom("Local"));
        Assert.True(def.Offset.IsLocal);
    }

    [Fact]
    public void OffsetDefinitionConverter_ConvertFrom_FixedOffset_IsFixed()
    {
        var converter = TypeDescriptor.GetConverter(typeof(OffsetDefinition));
        var def = Assert.IsType<OffsetDefinition>(converter.ConvertFrom("+08:00"));
        Assert.True(def.Offset.IsFixed);
        Assert.Equal(TimeSpan.FromHours(8), def.Offset.FixedOffset);
    }

    // -------------------------------------------------------------------------
    // OffsetDefinitionsConverter (collection TypeConverter)
    // -------------------------------------------------------------------------

    [Fact]
    public void OffsetDefinitionsConverter_CanConvertFromString_ReturnsTrue()
    {
        var converter = TypeDescriptor.GetConverter(typeof(OffsetDefinitions));
        Assert.True(converter.CanConvertFrom(typeof(string)));
    }

    [Fact]
    public void OffsetDefinitionsConverter_SingleItem_ReturnsSingleDefinition()
    {
        var converter = TypeDescriptor.GetConverter(typeof(OffsetDefinitions));
        var result = Assert.IsType<OffsetDefinitions>(converter.ConvertFrom("UTC"));
        Assert.Single(result);
        Assert.True(result[0].Offset.IsUtc);
    }

    [Fact]
    public void OffsetDefinitionsConverter_CommaSeparated_ReturnsAllItems()
    {
        var converter = TypeDescriptor.GetConverter(typeof(OffsetDefinitions));
        var result = Assert.IsType<OffsetDefinitions>(converter.ConvertFrom("UTC, Local, +08:00"));
        Assert.Equal(3, result.Count);
        Assert.True(result[0].Offset.IsUtc);
        Assert.True(result[1].Offset.IsLocal);
        Assert.True(result[2].Offset.IsFixed);
        Assert.Equal(TimeSpan.FromHours(8), result[2].Offset.FixedOffset);
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
