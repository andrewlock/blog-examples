using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.Encodings.Web;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace injecting_services_when_creating_mvcoptions
{
   public class HtmlEncodeContractResolver : DefaultContractResolver
    {
        protected override IList<JsonProperty> CreateProperties(Type type, MemberSerialization memberSerialization)
        {
            var properties = base.CreateProperties(type, memberSerialization);
 
            foreach (var property in properties.Where(p => p.PropertyType == typeof(string)))
            {
                var propertyInfo = type.GetProperty(property.UnderlyingName);
                if (propertyInfo != null)
                {
                    property.ValueProvider = new HtmlEncodingValueProvider(propertyInfo);
                }
            }
 
            return properties;
        }
 
        protected class HtmlEncodingValueProvider : IValueProvider
        {
            private readonly PropertyInfo _targetProperty;
 
            public HtmlEncodingValueProvider(PropertyInfo targetProperty)
            {
                this._targetProperty = targetProperty;
            }
             
            public void SetValue(object target, object value)
            {
                var s = value as string;
                if (s != null)
                {
                    var encodedString = HtmlEncoder.Default.Encode(s);
                    _targetProperty.SetValue(target, encodedString);
                }
                else
                {
                    // Shouldn't get here as we checked for string properties before setting this value provider
                    _targetProperty.SetValue(target, value);
                }
            }
 
            public object GetValue(object target)
            {
                return _targetProperty.GetValue(target);
            }
        }
    }
}