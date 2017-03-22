using Microsoft.AspNetCore.Mvc;

namespace preventing_over_posting
{
    [ModelMetadataType(typeof(UserModel))]
    public class DistinctBindingModel
    {
        public string Name { get; set; }
    }
}
