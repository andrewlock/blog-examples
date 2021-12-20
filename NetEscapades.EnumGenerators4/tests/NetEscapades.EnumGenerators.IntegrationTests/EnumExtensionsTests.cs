using Xunit;

namespace NetEscapades.EnumGenerators.IntegrationTests;

public class EnumExtensionsTests
{
    [Theory]
    [InlineData(Colour.Red)]
    [InlineData(Colour.Green)]
    [InlineData(Colour.Green | Colour.Blue)]
    [InlineData((Colour)15)]
    [InlineData((Colour)0)]
    public void FastToStringIsSameAsToString(Colour value)
    {
        var expected = value.ToString();
        var actual = value.ToStringFast();

        Assert.Equal(expected, actual);
    }

    [Theory]
    [InlineData(Direction.Up)]
    [InlineData((Direction)15)]
    [InlineData((Direction)0)]
    public void CustomExtensionNameToStringFast(Direction value)
    {
        var expected = value.ToString();
        var actual = value.ToStringFast();

        Assert.Equal(expected, actual);
    }
}