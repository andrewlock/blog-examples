using System;

namespace NetEscapades.EnumGenerators.IntegrationTests;

[EnumExtensions]
[Flags]
public enum Colour
{
    Red = 1,
    Blue = 2,
    Green = 4,
}