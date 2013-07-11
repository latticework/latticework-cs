using System;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
using Lw.Linq.Expressions;
using System.Reflection;
using System.Linq;
using Lw.Reflection;
using System.Collections.Generic;

namespace Lw.ComponentModel.DataAnnotations
{
    public static class DataAnnotationOperations
    {
        public static ValidationContext CreatePropertyContext<T>(
            ValidationContext objectContext, Expression<Func<T>> lambdaExpression)
        {
            ExceptionOperations.VerifyNonNull(lambdaExpression, () => lambdaExpression);
            return CreatePropertyContext(objectContext, LambdaExpressionOperations.FindMemberName(lambdaExpression));
        }

        public static ValidationContext CreatePropertyContext(ValidationContext objectContext, string propertyName)
        {
            ExceptionOperations.VerifyNonNull(objectContext, () => objectContext);
            ExceptionOperations.VerifyNonNull(propertyName, () => propertyName);


            return new ValidationContext(objectContext.ObjectInstance, objectContext, objectContext.Items)
            {
                MemberName = propertyName,
            };
        }

        public static PropertyInfo FindTransientKeyProperty(Type containerType)
        {
            return containerType.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                   .FirstOrDefault(p => p.GetCustomAttribute<TransientKeyAttribute>(true) != null);
        }


        public static IList<Attribute> GetMetadataAttributes<TObject>()
        {
            LwComponentModelDataAnnotationExtensions.EnsureCashedType(typeof(TObject));

            return typeof(TObject).GetCustomAttributes<Attribute>(true).ToList();
        }

        public static IList<Attribute> GetMetadataAttributes<TObject, TProperty>(Expression<Func<TObject, TProperty>> propertyExpression)
        {
            LwComponentModelDataAnnotationExtensions.EnsureCashedType(typeof(TObject));

            var propertyInfo = LambdaExpressionOperations.FindPropertyInfo<TObject, TProperty>(propertyExpression);

            return propertyInfo.GetCustomAttributes<Attribute>(true).ToList();
        }

        
        public static ValidationResult ValidateEntityProperty<TEntity, TProperty>(
            TProperty value, 
            ValidationContext context, 
            ValidationAttribute optionalValidator, 
            Expression<Func<TEntity, TProperty>> lambdaExpression, 
            Func<TEntity, bool> predicate)
        {
            var entity = (TEntity)context.ObjectInstance;
            var propertyName = LambdaExpressionOperations.FindMemberName(lambdaExpression);

            return (!predicate(entity))
                ? ValidationResult.Success
                : optionalValidator.GetValidationResult(value, CreatePropertyContext(context, propertyName));
        }

        public static ValidationResult ValidateEntityProperty<TEntity, TProperty>(
            TProperty value,
            ValidationContext context,
            Func<TEntity, TProperty, string> validator)
        {
            return ValidateEntityProperty(value, context, context.MemberName, validator);
        }

        public static ValidationResult ValidateEntityProperty<TEntity, TProperty>(
            TProperty value, 
            ValidationContext context,
            Expression<Func<TEntity, TProperty>> lambdaExpression,
            Func<TEntity, TProperty, string> validator)
        {
            var propertyName = LambdaExpressionOperations.FindMemberName(lambdaExpression);
            return ValidateEntityProperty(value, context, propertyName, validator);
        }

        public static ValidationResult ValidateEntityProperty<TEntity, TProperty>(
            TProperty value,
            ValidationContext context,
            string propertyName,
            Func<TEntity, TProperty, string> validator)
        {
            var entity = (TEntity)context.ObjectInstance;
            string errorMessage = validator(entity, value);

            return (errorMessage == null)
                ? ValidationResult.Success
                : new ValidationResult(errorMessage, new[] { propertyName });
        }
    }
}
