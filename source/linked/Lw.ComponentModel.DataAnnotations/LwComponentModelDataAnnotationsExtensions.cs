using Lw.ApplicationMessages;
using Lw.Linq.Expressions;
using Lw.Reflection;
using System;
using System.Reflection;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
using System.Linq;
using System.ComponentModel;

namespace Lw.ComponentModel.DataAnnotations
{
    public static class LwComponentModelDataAnnotationExtensions
    {
        #region Object Extensions
        public static IEnumerable<Attribute> GetMetadataAttributes(this object reference)
        {
            var type = reference.GetType();
            LwComponentModelDataAnnotationExtensions.EnsureCashedType(type);

            return type.GetCustomAttributes<Attribute>(true);
        }

        public static IList<Attribute> GetMetadataAttributes<TObject, TProperty>(this TObject reference,
            Expression<Func<TObject, TProperty>> propertyExpression)
        {
            LwComponentModelDataAnnotationExtensions.EnsureCashedType(typeof(TObject));

            var propertyInfo = LambdaExpressionOperations.FindPropertyInfo<TObject, TProperty>(propertyExpression);

            return propertyInfo.GetCustomAttributes<Attribute>(true).ToList();
        }

        public static Guid? GetTransientKey(this object reference)
        {
            if (reference == null) { return null; }


            var keyProperty = DataAnnotationOperations.FindTransientKeyProperty(reference.GetType());

            return keyProperty.GetValueOrDefault(p => (Guid?)p.GetValue(reference, null));
        }
        #endregion Object Extensions

        #region ValidationResult Extensions
        public static ApplicationMessage ToApplicationMessage(this ValidationResult reference, object obj)
        {
            return LwComponentModelDataAnnotationsMessages.CreateMessage(
                obj, null, LwComponentModelDataAnnotationsMessages.ErrorMessageCodeValidationError, reference.ErrorMessage);
        }
        #endregion ValidationResult Extensions


        #region Internal Methods
        internal static void EnsureCashedType(Type type)
        {
            if (!LwComponentModelDataAnnotationExtensions.visitedTypes.ContainsKey(type))
            {
                LwComponentModelDataAnnotationExtensions.visitedTypes[type] = null;

                var attributes = type.GetCustomAttributes<MetadataTypeAttribute>(true);

                foreach (var attribute in attributes)
                {
                    TypeDescriptor.AddProviderTransparent(new AssociatedMetadataTypeTypeDescriptionProvider(
                        type, attribute.MetadataClassType), type);
                }
            }
        }
        #endregion Internal Methods


        #region Private Constructors
        static LwComponentModelDataAnnotationExtensions()
        {
            LwComponentModelDataAnnotationExtensions.visitedTypes = new ConcurrentDictionary<Type, object>();
        }
        #endregion Private Constructors

        #region Private Fields
        private static ConcurrentDictionary<Type, object> visitedTypes;
        #endregion Private Fields
    }
}