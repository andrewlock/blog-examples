//HintName: EnumExtensions.g.cs

namespace NetEscapades.EnumGenerators
{
    public static partial class EnumExtensions
    {
        public static string ToStringFast(this MyTestEnums.Direction value)
            => value switch
            {
                MyTestEnums.Direction.Left => nameof(MyTestEnums.Direction.Left),
                MyTestEnums.Direction.Right => nameof(MyTestEnums.Direction.Right),
                MyTestEnums.Direction.Up => nameof(MyTestEnums.Direction.Up),
                MyTestEnums.Direction.Down => nameof(MyTestEnums.Direction.Down),
                _ => value.ToString(),
            };

    }
}