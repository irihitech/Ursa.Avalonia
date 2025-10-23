using Ursa.Controls;

namespace Test.Ursa.PathPickerTests;

public class PathPickerTests
{
    [Theory]
    [InlineData(".123")]
    [InlineData(".txt")]
    [InlineData(".png")]
    [InlineData("[.txt]")]
    [InlineData("[]")]
    [InlineData("[.]")]
    [InlineData("[a,]")]
    [InlineData("[,a]")]
    [InlineData("[a,b,]")]
    [InlineData("[a,,b]")]
    [InlineData("[a*b]")]
    [InlineData("[a.b]")]
    public void ParseFileTypes_InvalidInputs_ThrowsArgumentException(string input)
    {
        var exception = Assert.Throws<ArgumentException>(() => PathPicker.ParseFileTypes(input));
        Assert.Contains("Invalid parameter, please refer to the following content", exception.Message);
    }

    [Theory]
    [InlineData("[Name,Pattern]")]
    [InlineData("[123,.exe,.pdb]")]
    [InlineData("[All]")]
    [InlineData("[ImageAll]")]
    [InlineData("[11,.txt]")]
    [InlineData("[a]")]
    [InlineData("[a,b]")]
    [InlineData("[a,b,c]")]
    [InlineData("[a,.]")]
    public void ParseFileTypes_ValidInputs_DoesNotThrow(string input)
    {
        try
        {
            PathPicker.ParseFileTypes(input);
        }
        catch (Exception ex)
        {
            Assert.Fail($"It is expected not to throw an exception, but actually throws one :{ex.Message}");
        }
    }
}