using System;
using System.ComponentModel;
using System.Reflection;

namespace Lw.ComponentModel.DataAnnotations
{
    public interface IDataAnnotationProvider
    {
        PropertyDescriptor GetPropertyDescriptor(PropertyInfo propertyInfo);
        ICustomTypeDescriptor GetTypeDescriptor(Type type);
    }
}
