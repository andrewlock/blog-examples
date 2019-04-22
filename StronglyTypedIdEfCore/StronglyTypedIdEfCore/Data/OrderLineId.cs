using System;
using System.ComponentModel;
using System.Globalization;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace StronglyTypedId.Shop.Data
{
    [TypeConverter(typeof(OrderLineIdTypeConverter))]
    public readonly struct OrderLineId : IComparable<OrderLineId>, IEquatable<OrderLineId>
    {
        public Guid Value { get; }

        public OrderLineId(Guid value)
        {
            Value = value;
        }

        public static OrderLineId New() => new OrderLineId(Guid.NewGuid());
        public static OrderLineId Empty { get; } = new OrderLineId(Guid.Empty);

        public bool Equals(OrderLineId other) => this.Value.Equals(other.Value);
        public int CompareTo(OrderLineId other) => Value.CompareTo(other.Value);

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is OrderLineId other && Equals(other);
        }

        public override int GetHashCode() => Value.GetHashCode();

        public override string ToString() => Value.ToString();
        public static bool operator ==(OrderLineId a, OrderLineId b) => a.CompareTo(b) == 0;
        public static bool operator !=(OrderLineId a, OrderLineId b) => !(a == b);

        class OrderLineIdTypeConverter : TypeConverter
        {
            public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
            {
                return sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);
            }

            public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
            {
                var stringValue = value as string;
                if (!string.IsNullOrEmpty(stringValue)
                    && Guid.TryParse(stringValue, out var guid))
                {
                    return new OrderLineId(guid);
                }

                return base.ConvertFrom(context, culture, value);

            }
        }

        public class EfCoreValueConverter : ValueConverter<OrderLineId, Guid>
        {
            public EfCoreValueConverter(ConverterMappingHints mappingHints = null) : base(
                id => id.Value,
                value => new OrderLineId(value),
                mappingHints
            )
            { }
        }
    }
}