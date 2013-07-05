using Lw.Linq.Expressions;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq.Expressions;
using System.Reflection;

namespace Lw
{
    public static class EnumOperations
    {
        #region Public Methods
        public static IEnumerable<string> GetNames<TEnum>() 
            where TEnum : struct
        {
            VerifyEnumType<TEnum>();

            return Enum.GetNames(typeof(TEnum));
        }

        public static IEnumerable<TEnum> GetValues<TEnum>()
            where TEnum : struct

        {
            return (TEnum[])Enum.GetValues(typeof(TEnum));
        }

        public static TEnum Parse<TEnum>(string value, bool ignoreCase = false)
            where TEnum : struct

        {
            Contract.Requires<ArgumentNullException>(value != null, "value");

            return (TEnum)Enum.Parse(typeof(TEnum), value, ignoreCase);
        }

        public static bool TryParse<TEnum>(string value, out TEnum result, bool ignoreCase = false)
            where TEnum : struct

        {
            Contract.Requires<ArgumentNullException>(value != null, "value");

            return Enum.TryParse<TEnum>(value, ignoreCase, out result);
        }
        #endregion Public Methods


        #region Internal Methods
        internal static void VerifyEnumType<T>(string typeParameterName)
        {
            if (!typeof(T).GetTypeInfo().IsEnum)
            {
                ExceptionOperations.ThrowArgumentException(typeParameterName);
            }
        }

        internal static void VerifyEnumType<T>(Type enumType, Expression<Func<T>> paramExpr)
        {
            if (!enumType.GetTypeInfo().IsSubclassOf(typeof(Enum)))
            {
                string name = LambdaExpressionOperations.FindMemberName(paramExpr);

                ExceptionOperations.ThrowArgumentException(name);
            }
        }

        internal static void VerifyEnumType(Type enumType, string paramName)
        {
            if (!enumType.GetTypeInfo().IsSubclassOf(typeof(Enum)))
            {
                ExceptionOperations.ThrowArgumentException(paramName);
            }
        }

        internal static void VerifyEnumUnderlyingType<TUnderlying>(Enum value, string paramName) 
            where TUnderlying : struct
        {
            if (typeof(TUnderlying) != Enum.GetUnderlyingType(value.GetType()))
            {
                ExceptionOperations.ThrowArgumentException(paramName);
            }
        }
        #endregion Internal Methods


        #region Private Methods
        private static void VerifyEnumType<T>()
        {
            VerifyEnumType<T>("TEnum");
        }
        #endregion Private Methods
    }
}
