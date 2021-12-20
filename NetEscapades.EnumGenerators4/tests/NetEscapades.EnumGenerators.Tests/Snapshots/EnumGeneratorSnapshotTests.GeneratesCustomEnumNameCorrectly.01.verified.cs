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
}