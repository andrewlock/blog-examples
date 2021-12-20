namespace NetEscapades.EnumGenerators;

public readonly struct EnumToGenerate
{
    public readonly string ExtensionName;
    public readonly string Name;
    public readonly List<string> Values;

    public EnumToGenerate(string extensionName, string name, List<string> values)
    {
        Name = name;
        Values = values;
        ExtensionName = extensionName;
    }
}