using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using StronglyTypedId.Shop.Data;

namespace Microsoft.EntityFrameworkCore
{
    public static class StronglyTypedIdRegistrationExtensions
    {
        public static DbContextOptionsBuilder UseStronglyTypedIdValueConverters(this DbContextOptionsBuilder options)
        {
            return options
                .ReplaceService<IValueConverterSelector, CustomValueConverterSelector>();
        }
    }
}