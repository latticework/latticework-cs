using System;
using System.Data;
using System.Diagnostics;

namespace Lw.Data
{
    public static class Extensions
    {
        #region DataRow Extensions
        [DebuggerStepThrough]
        public static T GetValue<T>(this DataRow row, string columnName)
        {
            ExceptionOperations.VerifyNonNull(row, () => row);

            object value = row[columnName];

            return (value == DBNull.Value)
                ? default(T)
                : (T)value;
        }

        [DebuggerStepThrough]
        public static T GetValue<T>(this DataRow row, int columnIndex)
        {
            ExceptionOperations.VerifyNonNull(row, () => row);

            object value = row[columnIndex];

            return (value == DBNull.Value)
                ? default(T)
                : (T)value;
        }
        #endregion DataRow Extensions

        #region Object Extensions
        /// <summary>
        ///     Returns a value of the specified type converted from the specifed data value taking into account a 
        ///     possible value of <see cref="DBNull"/>.
        /// </summary>
        /// <typeparam name="T">
        ///     The target value type.
        /// </typeparam>
        /// <param name="reference">
        ///     The value to convert.
        /// </param>
        /// <returns>
        ///     The converted value.
        /// </returns>
        /// <exception cref="InvalidCastException">
        ///     The reference value cannot cannot be cast to the specified type.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        ///     <typeparamref name="T"/> is a non-nullable <see cref="ValueType"/> and <paramref name="reference"/> is
        ///     <see cref="DBNull"/>.
        /// </exception>
        public static T FromDataValue<T>(this object reference)
        {
            if (reference != System.DBNull.Value && !typeof(T).IsValueType)
            {
                return (T)reference;
            }

            // http://geekswithblogs.net/WillSmith/archive/2008/03/04/generic-conversion-for-dbnull-and-nullable-types.aspx
            if (!typeof(T).IsValueType || typeof(T).GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                return default(T);
            }

            throw new InvalidOperationException(nullValueExceptionMessage);
        }

        /// <summary>
        ///     Converts the referenced value to a data value that replaces any <see langword="null"/> value with 
        ///     <see cref="DBNull"/>.
        /// </summary>
        /// <param name="reference">
        ///     The value to convert.
        /// </param>
        /// <returns>
        ///     A data value.
        /// </returns>
        public static object ToDataValue(this object reference)
        {
            return (object.Equals(null, reference)) ? DBNull.Value : reference;
        }
        #endregion Object Extensions


        #region Private Fields
        private static readonly string nullValueExceptionMessage = GetNullValueExceptionMessage();
        #endregion Private Fields

        #region Private Methods
        private static string GetNullValueExceptionMessage()
        {
            InvalidOperationException result = null;

            try { int dummy = (int)(int?)null; }
            catch (InvalidOperationException exception) { result = exception; }

            return result.Message;
        }
        #endregion Private Methods
    }
}
