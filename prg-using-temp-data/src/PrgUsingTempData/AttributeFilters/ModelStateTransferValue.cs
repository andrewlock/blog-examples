using System.Collections.Generic;

namespace PrgUsingTempData.AttributeFilters
{
public class ModelStateTransferValue
{
    public string Key { get; set; }
    public string AttemptedValue { get; set; }
    public object RawValue { get; set; }
    public ICollection<string> ErrorMessages { get; set; } = new List<string>();
}
}