using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace Lw
{
    /// <summary>
    ///     Specifies that this is a TransietKey property of the parent Type.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited=true)]
    public sealed class TransientKeyAttribute : Attribute
    {
        public static Guid? GetTransientKey(object obj)
        {
            Guid? uid = null;

            var keyProperties = obj
                .GetType()
                .GetTypeInfo()
                .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(pi => pi.GetCustomAttribute<TransientKeyAttribute>() != null);

            if (keyProperties.Count() > 1)
            {
                throw new InternalErrorException(
                    "Cannot have more than one property tagged with the '{0}'. You have '{2}'.".DoFormat(
                        typeof(TransientKeyAttribute).Name, keyProperties.Select(pi => pi.Name).Join(",")));
            }

            var keyProperty = keyProperties.FirstOrDefault();

            if (keyProperty != null)
            {
                var propertyType = keyProperty.PropertyType;
                if (propertyType != typeof(Guid))
                {
                    throw new InternalErrorException(
                        "Property tagged with the '{0}' must be of type Guid. Yours, '{1}', is of type '{2}'.".DoFormat(
                            typeof(TransientKeyAttribute).Name, keyProperty.Name, propertyType.FullName));
                }

                uid = (Guid)keyProperty.GetValue(obj);
            }

            return uid;
        }
    }
}
