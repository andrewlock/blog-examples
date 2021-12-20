//HintName: EnumExtensionsAttribute.g.cs


namespace NetEscapades.EnumGenerators
{
    [System.AttributeUsage(System.AttributeTargets.Enum)]
    public class EnumExtensionsAttribute : System.Attribute
    {
        public string ExtensionClassName { get; set; }
    }
}