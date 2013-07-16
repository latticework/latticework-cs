using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;

using Lw.Collections.Generic;

namespace Lw.ComponentModel.DataAnnotations
{
    public class TypeDescriptionDataAnnotationProvider : IDataAnnotationProvider
    {
        #region Public Constructors
        public TypeDescriptionDataAnnotationProvider(TypeDescriptionProvider typeDescriptionProvider)
        {
            TypeDescriptionProvider = typeDescriptionProvider;
        }
        #endregion Public Constructors

        #region Public Methods
        public PropertyDescriptor GetPropertyDescriptor(PropertyInfo propertyInfo)
        {
            ExceptionOperations.VerifyNonNull(propertyInfo, () => propertyInfo);

            return GetDescriptors(propertyInfo.DeclaringType).Item2[propertyInfo.Name];
        }

        public ICustomTypeDescriptor GetTypeDescriptor(Type type)
        {
            return GetDescriptors(type).Item1;
        }
        #endregion Public Methods

        #region Public Properties
        public TypeDescriptionProvider TypeDescriptionProvider { get; private set; }
        #endregion Public Properties


        #region Private Fields
        private Dictionary<Type, Tuple<ICustomTypeDescriptor, PropertyDescriptorCollection>> descriptors =
            new Dictionary<Type, Tuple<ICustomTypeDescriptor, PropertyDescriptorCollection>>();
        #endregion Private Fields

        #region Private Methods
        private Tuple<ICustomTypeDescriptor, PropertyDescriptorCollection> GetDescriptors(Type type)
        {
            return descriptors.GetOrCreateValue(type, () =>
            {
                var typeDescriptor = TypeDescriptionProvider.GetTypeDescriptor(type);
                return Tuple.Create(typeDescriptor, typeDescriptor.GetProperties());
            });
        }
        #endregion Private Methods
    }
}
