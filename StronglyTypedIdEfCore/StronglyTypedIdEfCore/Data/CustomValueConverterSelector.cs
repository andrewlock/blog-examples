using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace StronglyTypedId.Shop.Data
{
    public class CustomValueConverterSelector : ValueConverterSelector
    {
        private readonly ConcurrentDictionary<(Type ModelClrType, Type ProviderClrType), ValueConverterInfo> _converters
            = new ConcurrentDictionary<(Type ModelClrType, Type ProviderClrType), ValueConverterInfo>();

        public CustomValueConverterSelector(ValueConverterSelectorDependencies dependencies) : base(dependencies)
        {
        }

        public override IEnumerable<ValueConverterInfo> Select(Type modelClrType, Type providerClrType = null)
        {
            foreach (var converter in base.Select(modelClrType, providerClrType))
            {
                yield return converter;
            }

            // Extract the "real" type T from Nullable<T> if required
            var underlyingModelType = UnwrapNullableType(modelClrType);
            var underlyingProviderType = UnwrapNullableType(providerClrType);

            // 'null' means 'get any value converters for the modelClrType'
            if (underlyingProviderType is null || underlyingProviderType == typeof(Guid))
            {
                // Try and get a nested class with the expected name. 
                var converterType = underlyingModelType.GetNestedType("EfCoreValueConverter");

                if (converterType != null)
                {
                    yield return _converters.GetOrAdd(
                        (underlyingModelType, typeof(Guid)),
                        k =>
                        {
                            // Create an instance of the converter whenever it's requested.
                            Func<ValueConverterInfo, ValueConverter> factory =
                                info => (ValueConverter) Activator.CreateInstance(converterType, info.MappingHints);

                            // Build the info for our strongly-typed ID => Guid converter
                            return new ValueConverterInfo(modelClrType, typeof(Guid), factory);
                        }
                    );
                }
            }
        }

        private static Type UnwrapNullableType(Type providerClrType)
        {
            if (providerClrType is null)
            {
                return null;
            }

            return Nullable.GetUnderlyingType(providerClrType) ?? providerClrType;
        }
    }
}