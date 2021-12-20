using VerifyXunit;
using Xunit;

namespace NetEscapades.EnumGenerators.Tests;

[UsesVerify]
public class EnumGeneratorSnapshotTests
{
    [Fact]
    public Task GeneratesEnumExtensionsCorrectly()
    {
        var source = @"
using NetEscapades.EnumGenerators;

[EnumExtensions]
public enum Colour // Yes, I'm British
{
    Red = 0,
    Blue = 1,
}";

        return TestHelper.Verify(source);
    }

    [Fact]
    public Task NoAttribute()
    {
        var source = @"
public enum Colour // Yes, I'm British
{
    Red = 0,
    Blue = 1,
}";

        return TestHelper.Verify(source);
    }

    [Fact]
    public Task NoNamespace()
    {
        var source = @"
[EnumExtensions]
public enum Colour // Yes, I'm British
{
    Red = 0,
    Blue = 1,
}";

        return TestHelper.Verify(source);
    }

    [Fact]
    public Task TwoEnums()
    {
        var source = @"
using NetEscapades.EnumGenerators;

namespace MyTestEnums;

[EnumExtensions]
public enum Colour
{
    Red = 0,
    Blue = 1,
}

[EnumExtensions]
public enum Direction
{
    Left,
    Right,
    Up,
    Down,
}";

        return TestHelper.Verify(source);
    }

    [Fact]
    public Task TwoEnums_OneAttribute()
    {
        var source = @"
using NetEscapades.EnumGenerators;

namespace MyTestEnums;

public enum Colour
{
    Red = 0,
    Blue = 1,
}

[EnumExtensions]
public enum Direction
{
    Left,
    Right,
    Up,
    Down,
}";

        return TestHelper.Verify(source);
    }

    [Fact]
    public Task GeneratesCustomEnumNameCorrectly()
    {
        var source = @"
using NetEscapades.EnumGenerators;

[EnumExtensions(ExtensionClassName = ""ColourExtensions"")]
public enum Colour // Yes, I'm British
{
    Red = 0,
    Blue = 1,
}";

        return TestHelper.Verify(source);
    }

    [Fact]
    public Task GeneratesCustomEnumNameCorrectly_TwoAttributes()
    {
        var source = @"
using NetEscapades.EnumGenerators;

[EnumExtensions(ExtensionClassName = ""ColourExtensions"")]
public enum Colour
{
    Red = 0,
    Blue = 1,
}


[EnumExtensions(ExtensionClassName = ""DirectionExtensions"")]
public enum Direction
{
    Left,
    Right,
    Up,
    Down,
}";

        return TestHelper.Verify(source);
    }
}