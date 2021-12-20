//HintName: EnumExtensions.g.cs

namespace NetEscapades.EnumGenerators
{
    public static partial class ColourExtensions
    {
        public static string ToStringFast(this Colour value)
            => value switch
            {
                Colour.Red => nameof(Colour.Red),
                Colour.Blue => nameof(Colour.Blue),
                _ => value.ToString(),
            };
    }

    public static partial class DirectionExtensions
    {
        public static string ToStringFast(this Direction value)
            => value switch
            {
                Direction.Left => nameof(Direction.Left),
                Direction.Right => nameof(Direction.Right),
                Direction.Up => nameof(Direction.Up),
                Direction.Down => nameof(Direction.Down),
                _ => value.ToString(),
            };
    }
}