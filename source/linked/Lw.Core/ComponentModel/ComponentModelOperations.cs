using System;
using System.ComponentModel;

namespace Lw.ComponentModel
{
    public static class ComponentModelOperations
    {
#if !NETFX_CORE
        public static TypeConverter GetTypeConverter<T>(T obj)
        {
            return GetTypeConverter(typeof(T));
        }

        public static TypeConverter GetTypeConverter<T>()
        {
            return GetTypeConverter(typeof(T));
        }

        // http://lostechies.com/jimmybogard/2010/02/19/automapper-for-silverlight-3-0-alpha/
        //http://stackoverflow.com/questions/2962780/silverlight-typedescriptor-getconverter-substitute
        public static TypeConverter GetTypeConverter(Type type)
        {
#if !SILVERLIGHT
            return TypeDescriptor.GetConverter(type);
#else
            var attribute = type.GetCustomAttribute<TypeConverterAttribute>(false);

            if (attribute == null) { return new TypeConverter(); }
                

            var converterType = Type.GetType(attribute.ConverterTypeName, throwOnError: false);

            if (converterType == null) { return new TypeConverter(); }


            return Activator.CreateInstance(converterType) as TypeConverter;
#endif
        }
#endif
    }
}
