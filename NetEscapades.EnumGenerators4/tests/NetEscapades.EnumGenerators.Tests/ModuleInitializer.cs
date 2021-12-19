using System.Runtime.CompilerServices;
using VerifyTests;

namespace NetEscapades.EnumGenerators.Tests;

public static class ModuleInitializer
{
    [ModuleInitializer]
    public static void Init()
    {
        VerifySourceGenerators.Enable();
    }
}