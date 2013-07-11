using System;
using System.Linq.Expressions;
using Lw.ComponentModel.DataAnnotations;
using Lw.Linq.Expressions;

namespace Lw.BusinessEntities
{
    public static class Extensions
    {
        /// <summary>
        ///     Returns the key token string for the specified entity.
        /// </summary>
        /// <param name="entity">
        ///     An <see cref="IBusinessEntity"/>.
        /// </param>
        /// <returns>
        ///     The result of 
        ///     <see cref="IBusinessEntity.GetKey"/>().<see cref="IBusinessEntityKey.ToToken"/>.
        /// </returns>
        public static string GetKeyToken(this IBusinessEntity entity)
        {
            ExceptionOperations.VerifyNonNull(entity, () => entity);

            return entity.GetKey().ToToken();
        }

        //public static ValidationErrorCollection TryValidateProperty<T>(this IBusinessEntity entity, Expression<Func<T>> expr)
        //{
        //    ExceptionOperations.VerifyNonNull(entity, () => entity);
        //    ExceptionOperations.VerifyNonNull(expr, () => expr);

        //    string propertyName = LambdaExpressionOperations.FindMemberName(expr);

        //    return entity.TryValidateProperty(propertyName);
        //}

        ///// <summary>
        /////     Validates the entity.
        ///// </summary>
        ///// <param name="entity">
        /////     An <see cref="IBusinessEntity"/>.
        ///// </param>
        ///// <exception cref="ValidationException">
        /////     One or more <see cref="ValidationError"/> instances were created from the validation
        /////     process.
        ///// </exception>
        //public static void Validate(this IBusinessEntity entity)
        //{
        //    ExceptionOperations.VerifyNonNull(entity, () => entity);

        //    var errors = entity.TryValidate();

        //    TestErrors(errors);
        //}

        ///// <summary>
        /////     Validates the entity property specified by the lambda expression.
        ///// </summary>
        ///// <typeparam name="T">
        /////     The property type.
        ///// </typeparam>
        ///// <param name="entity">
        /////     The entity to test.
        ///// </param>
        ///// <param name="expr">
        /////     A lambda expression specifying the property to test. Use the form () => entity.PropertyName.
        ///// </param>
        ///// <exception cref="ValidationException">
        /////     One or more <see cref="ValidationError"/> instances were created from the validation
        /////     process.
        ///// </exception>
        //public static void ValidateProperty<T>(this IBusinessEntity entity, Expression<Func<T>> expr)
        //{
        //    var errors = TryValidateProperty(entity, expr);

        //    TestErrors(errors);
        //}

        ///// <summary>
        /////     Validates the entity property specified by the property name.
        ///// </summary>
        ///// <param name="entity">
        /////     The entity to test.
        ///// </param>
        ///// <param name="propertyName">
        /////     The property of the entity to test.
        ///// </param>
        ///// <exception cref="ValidationException">
        /////     One or more <see cref="ValidationError"/> instances were created from the validation
        /////     process.
        ///// </exception>
        //public static void ValidateProperty(this IBusinessEntity entity, string propertyName)
        //{
        //    ExceptionOperations.VerifyNonNull(entity, () => entity);
        //    ExceptionOperations.VerifyNonNull(propertyName, () => propertyName);

        //    var errors = entity.TryValidateProperty(propertyName);

        //    TestErrors(errors);
        //}

        //private static void TestErrors(ValidationErrorCollection errors)
        //{
        //    if (errors.Count > 0)
        //    {
        //        throw new ValidationException(errors);
        //    }
        //}
    }
}
