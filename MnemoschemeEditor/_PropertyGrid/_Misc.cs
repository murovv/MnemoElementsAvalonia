using System;
using System.Collections.Generic;

namespace MnemoschemeEditor._PropertyGrid
{
    public enum PropertyValueType
    {
        Unsupported,

        Bool,

        String,

        Enum,
        
        Integer,
        Float,

        DetailSettings,
        
        
    }

    public interface IPropertyContractResolver
    {
        T? GetDataAnnotation<T>(Type targetType, string propertyName)
            where T : Attribute;

        IEnumerable<Attribute> GetDataAnnotations(Type targetType, string propertyName);
    }

    public class DetailSettingsAttribute : Attribute
    {
        public DetailSettingsAttribute()
        {

        }
    }
}
