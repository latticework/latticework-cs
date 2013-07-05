using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Reflection;

namespace Lw
{
    public static class Extensions
    {
        #region Enum Extensions
        [DebuggerStepThrough]
        public static bool IsDefined(this Enum reference)
        {
            return Enum.IsDefined(reference.GetType(), reference);
        }
        
        [DebuggerStepThrough]
        public static string GetName(this Enum reference)
        {
            return EquivalentEnumValueSelectorAttribute.SelectName(
                reference.GetType(), Enum.GetName(reference.GetType(), reference));
        }

        [DebuggerStepThrough]
        public static Type GetUnderlyingType(this Enum reference)
        {
            return Enum.GetUnderlyingType(reference.GetType());
        }

        [DebuggerStepThrough]
        public static bool In(this Enum reference, params Enum[] values)
        {
            return values.FirstOrDefault(e => e == reference) != null;
        }

        /// <summary>
        ///     Calculates whether the specified <see cref="FlagsAttribute"/> enumeration value matches the 
        ///     specified comparison values.
        /// </summary>
        /// <param name="reference">
        ///     The <see cref="FlagsAttribute"/> enumeration value to test.
        /// </param>
        /// <param name="values">
        ///     The values to test against.
        /// </param>
        /// <returns>
        ///     <see langword="true"/> if the value includes the tested values; otherwise, 
        ///     <see langword="false"/>.
        /// </returns>
        [DebuggerStepThrough]
        public static bool Includes(this Enum reference, Enum values)
        {
            return Match(reference, values, values);
        }

        [DebuggerStepThrough]
        public static TDestination Map<TDestination>(this Enum reference)
        {
            var mappers = reference.GetType().GetTypeInfo().GetCustomAttributes<EnumMappingAttribute>(false);

            EnumMappingAttribute mapper = mappers
                .SingleOrDefault(ema => ema.MappedType == typeof(TDestination));

            if (mapper == null)
            {
                ExceptionOperations.ThrowArgumentException("value");
            }

            return mapper.Map<TDestination>(reference);
        }


        /// <summary>
        ///     Calculates whether the specified <see cref="FlagsAttribute"/> enumeration value matches the 
        ///     specified comparison values when the specified mask is applied.
        /// </summary>
        /// <param name="reference">
        ///     The <see cref="FlagsAttribute"/> enumeration value to test.
        /// </param>
        /// <param name="mask">
        ///     The mask to apply.
        /// </param>
        /// <param name="values">
        ///     The values to test against.
        /// </param>
        /// <returns>
        ///     <see langword="true"/> if the value includes the tested values; otherwise, 
        ///     <see langword="false"/>.
        /// </returns>
        [DebuggerStepThrough]
        public static bool Match(this Enum reference, Enum mask, Enum values)
        {
            Type valueType = reference.GetType();
            if (mask.GetType() != valueType)
            {
                ExceptionOperations.ThrowArgumentException(() => mask);
            }

            if (values.GetType() != valueType)
            {
                ExceptionOperations.ThrowArgumentException(() => values);
            }

            Type underlyingType = reference.GetUnderlyingType();

            if (underlyingType == typeof(byte))
            {
                return ((byte)(object)reference & (byte)(object)mask) == (byte)(object)values;
            }

            if (underlyingType == typeof(int))
            {
                return ((int)(object)reference & (int)(object)mask) == (int)(object)values;
            }

            if (underlyingType == typeof(long))
            {
                return ((long)(object)reference & (long)(object)mask) == (long)(object)values;
            }

            if (underlyingType == typeof(uint))
            {
                return ((uint)(object)reference & (uint)(object)mask) == (uint)(object)values;
            }

            if (underlyingType == typeof(ulong))
            {
                return ((ulong)(object)reference & (ulong)(object)mask) == (ulong)(object)values;
            }

            // Never reached.
            return false;
        }
        #endregion Enum Extensions

        #region EventHandler Extensions
        // TODO: Iim.Extensions.Raise -- Verify that these can be executed successfuly.
        [DebuggerStepThrough]
        public static void Raise(this EventHandler reference, object sender)
        {
            if (reference != null)
            {
                reference(sender, EventArgs.Empty);
            }
        }

        [DebuggerStepThrough]
        public static void Raise<TEventArgs>(this EventHandler<TEventArgs> reference, object sender, TEventArgs args)
            where TEventArgs : EventArgs
        {
            if (reference != null)
            {
                reference(sender, args);
            }
        }
        #endregion EventHandler Extensions

        #region IEnumMappingStrategy Extensions
        [DebuggerStepThrough]
        public static TDestination Map<TDestination>(this IEnumMappingStrategy reference, Enum value)
        {
            return (TDestination)reference.Map(typeof(TDestination), value);
        }
        #endregion IEnumMappingStrategy Extensions

        #region Int32 Extensions
        [DebuggerStepThrough]
        public static int CompareMasked(this int reference, int mask, int maskedValue)
        {
            return (reference & mask).CompareTo(maskedValue);
        }

        [DebuggerStepThrough]
        public static bool EqualsMasked(this int reference, int mask, int maskedValue)
        {
            return (reference & mask).Equals(maskedValue);
        }

        [DebuggerStepThrough]
        public static int GetMaskedValue(this int reference, int mask)
        {
            int shiftCount = 0;
            int shiftedValue = reference;
            int nextShiftedValue;
            while ((nextShiftedValue = shiftedValue % 2) == 0)
            {
                ++shiftCount;
                shiftedValue = nextShiftedValue;
            }

            return GetMaskedValue(reference, mask, shiftCount);
        }

        [DebuggerStepThrough]
        public static int GetMaskedValue(this int reference, int mask, int shiftCount)
        {
            return (reference & mask) >> shiftCount;
        }
        #endregion Int32 ExtensionsB

        #region Int64 Extensions
        [DebuggerStepThrough]
        public static long CompareMasked(this long reference, Enum mask, Enum maskedValue)
        {
            EnumOperations.VerifyEnumUnderlyingType<long>(mask, "mask");
            EnumOperations.VerifyEnumUnderlyingType<long>(maskedValue, "maskedValue");

            return CompareMasked(reference, (long)(object)mask, (long)(object)maskedValue);
        }

        [DebuggerStepThrough]
        public static long CompareMasked(this long reference, long mask, long maskedValue)
        {
            return (reference & mask).CompareTo(maskedValue);
        }

        [DebuggerStepThrough]
        public static bool EqualsMasked(this long reference, Enum mask, Enum maskedValue)
        {
            EnumOperations.VerifyEnumUnderlyingType<long>(mask, "mask");
            EnumOperations.VerifyEnumUnderlyingType<long>(maskedValue, "maskedValue");

            return EqualsMasked(reference, (long)(object)mask, (long)(object)maskedValue);
        }

        [DebuggerStepThrough]
        public static bool EqualsMasked(this long reference, long mask, long maskedValue)
        {
            return (reference & mask).Equals(maskedValue);
        }

        [DebuggerStepThrough]
        public static long GetMaskedValue(this long reference, Enum mask)
        {
            EnumOperations.VerifyEnumUnderlyingType<long>(mask, "mask");

            return GetMaskedValue(reference, (long)(object)mask);
        }

        [DebuggerStepThrough]
        public static long GetMaskedValue(this long reference, long mask)
        {
            int shiftCount = 0;
            long shiftedValue = reference;

            while ((shiftedValue % 2) == 0)
            {
                ++shiftCount;
                shiftedValue = shiftedValue / 2;
            }

            return GetMaskedValue(reference, mask, shiftCount);
        }

        [DebuggerStepThrough]
        public static long GetMaskedValue(this long reference, Enum mask, int shiftCount)
        {
            EnumOperations.VerifyEnumUnderlyingType<long>(mask, "mask");

            return GetMaskedValue(reference, (long)(object)mask, shiftCount);
        }

        [DebuggerStepThrough]
        public static long GetMaskedValue(this long reference, long mask, int shiftCount)
        {
            return (reference & mask) >> shiftCount;
        }
        #endregion Int64 Extensions

        #region Object Extensions
        [DebuggerStepThrough]
        public static T GetValueIfNotEqualOrDefault<T>(this T reference, T otherValue)
        {
            return GetValueIfOrDefault(reference, v => !v.Equals(otherValue), default(T));
        }

        [DebuggerStepThrough]
        public static T GetValueIfNotEqualOrDefault<T>(this T reference, T otherValue, T defaultValue)
        {
            return GetValueIfOrDefault(reference, v => !v.Equals(otherValue), defaultValue);
        }

        [DebuggerStepThrough]
        public static T GetValueIfOrDefault<T>(this T reference, Func<T, bool> func)
        {
            Contract.Requires<ArgumentNullException>(func != null, "func");

            return (func(reference))
                ? reference
                : default(T);
        }

        [DebuggerStepThrough]
        public static T GetValueIfOrDefault<T>(this T reference, Func<T, bool> func, T defaultValue)
        {
            Contract.Requires<ArgumentNullException>(func != null, "func");

            return (func(reference))
                ? reference
                : defaultValue;
        }

        /// <summary>
        ///     Executes the specified action if the specified value satisifes the specified condition.
        /// </summary>
        /// <typeparam name="T">
        ///     The value type.
        /// </typeparam>
        /// <param name="reference">
        ///     The value to test.
        /// </param>
        /// <param name="predicate">
        ///     Specifies whether the action will execute.
        /// </param>
        /// <param name="action">
        ///     The action to execute.
        /// </param>
        /// <remarks>
        ///     <see cref="DoIf{T}"/> transforms an <c>if</c> statement into an expression
        /// </remarks>
        [DebuggerStepThrough]
        public static void DoIf<T>(this T reference, Func<T, bool> predicate, Action<T> action)
        {
            Contract.Requires<ArgumentNullException>(reference != null, "reference");
            Contract.Requires<ArgumentNullException>(predicate != null, "predicate");
            Contract.Requires<ArgumentNullException>(action != null, "action");

            if (predicate(reference))
            {
                action(reference);
            }
        }

        /// <summary>
        ///     Executes the specified action if the specified value is not the default value for that type.
        /// </summary>
        /// <typeparam name="T">
        ///     The value type.
        /// </typeparam>
        /// <param name="reference">
        ///     The value to test.
        /// </param>
        /// <param name="action">
        ///     The action to execute.
        /// </param>
        /// <remarks>
        ///     <see cref="DoIfNotDefault{T}"/> streamlines testing a value for <c>default(<typeparamref name="T"/>)</c> 
        ///     (usually <see langword="null"/>) before executing an action on the value.
        /// </remarks>
        [DebuggerStepThrough]
        public static void DoIfNotDefault<T>(this T reference, Action<T> action)
        {
            Contract.Requires<ArgumentNullException>(reference != null, "reference");
            Contract.Requires<ArgumentNullException>(action != null, "action");

            if (!object.Equals(reference, default(T)))
            {
                action(reference);
            }
        }


        /// <summary>
        ///     Returns the result of the specified selector if the specified source value is not the default
        ///     value for the source type; otherwise, returns the default value for the destination type.
        /// </summary>
        /// <typeparam name="TSource">The source type.</typeparam>
        /// <typeparam name="TDestination">The destination type.</typeparam>
        /// <param name="reference">
        ///     The source value.
        /// </param>
        /// <param name="selector">
        ///     Converts the source value to a destination type value.
        /// </param>
        /// <returns>
        ///     The result of the specified selector if the specified source value is not the default
        ///     value for the source type; otherwise, returns the default value for the destination type.
        /// </returns>
        [DebuggerStepThrough]
        public static TDestination GetValueOrDefault<TSource, TDestination>(
            this TSource reference, Func<TSource, TDestination> selector)
        {
            Contract.Requires<ArgumentNullException>(selector != null, "selector");

            if (object.Equals(reference, default(TSource)))
            {
                return default(TDestination);
            }

            return selector(reference);
        }

        /// <summary>
        ///     Returns the result of the specified selector if the specified source value is not the default
        ///     value for the source type; otherwise, returns the default value for the destination type.
        /// </summary>
        /// <typeparam name="TSource">The source type.</typeparam>
        /// <typeparam name="TDestination">The destination type.</typeparam>
        /// <param name="reference">
        ///     The source value.
        /// </param>
        /// <param name="selector">
        ///     Converts the source value to a destination type value.
        /// </param>
        /// <param name="defaultValue">
        ///     The destination type default value.
        /// </param>
        /// <returns>
        ///     The result of the specified selector if the specified source value is not the default
        ///     value for the source type; otherwise, returns the specified default value for the destination type.
        /// </returns>
        [DebuggerStepThrough]
        public static TDestination GetValueOrDefault<TSource, TDestination>(
            this TSource reference, Func<TSource, TDestination> selector, TDestination defaultValue)
        {
            Contract.Requires<ArgumentNullException>(selector != null, "selector");

            if (object.Equals(reference, default(TSource)))
            {
                return defaultValue;
            }

            return selector(reference);
        }

        /// <summary>
        ///     Returns whether the specified value is found in a sequence of values.
        /// </summary>
        /// <typeparam name="T">
        ///     The value type.
        /// </typeparam>
        /// <param name="value">
        ///     The value to search for.
        /// </param>
        /// <param name="values">
        ///     The sequence of values to search.
        /// </param>
        /// <returns>
        ///     Returns <see langword="true"/> if the default equality comparer finds that <paramref name="value"/>
        ///     matches any value in the sequence; otherwise, <see langword="false"/>.
        /// </returns>
        [DebuggerStepThrough]
        public static bool In<T>(this T value, params T[] values)
        {
            return value.In((IEnumerable<T>)values);
        }

        /// <summary>
        ///     Returns whether the specified value is found in a sequence of values.
        /// </summary>
        /// <typeparam name="T">
        ///     The value type.
        /// </typeparam>
        /// <param name="value">
        ///     The value to search for.
        /// </param>
        /// <param name="comparer">
        ///     The equality comparer.
        /// </param>
        /// <param name="values">
        ///     The sequence of values to search.
        /// </param>
        /// <returns>
        ///     Returns <see langword="true"/> if <paramref name="comparer"/>  finds that <paramref name="value"/>
        ///     matches any value in the sequence; otherwise, <see langword="false"/>.
        /// </returns>
        [DebuggerStepThrough]
        public static bool In<T>(this T value, IEqualityComparer<T> comparer, params T[] values)
        {
            return value.In(comparer, (IEnumerable<T>)values);
        }

        /// <summary>
        ///     Returns whether the specified value is found in a sequence of values.
        /// </summary>
        /// <typeparam name="T">
        ///     The value type.
        /// </typeparam>
        /// <param name="value">
        ///     The value to search for.
        /// </param>
        /// <param name="values">
        ///     The sequence of values to search.
        /// </param>
        /// <returns>
        ///     Returns <see langword="true"/> if the default equality comparer finds that <paramref name="value"/>
        ///     matches any value in the sequence; otherwise, <see langword="false"/>.
        /// </returns>
        [DebuggerStepThrough]
        public static bool In<T>(this T value, IEnumerable<T> values)
        {
            return value.In(EqualityComparer<T>.Default, values);
        }

        /// <summary>
        ///     Returns whether the specified value is found in a sequence of values.
        /// </summary>
        /// <typeparam name="T">
        ///     The value type.
        /// </typeparam>
        /// <param name="value">
        ///     The value to search for.
        /// </param>
        /// <param name="comparer">
        ///     The equality comparer.
        /// </param>
        /// <param name="values">
        ///     The sequence of values to search.
        /// </param>
        /// <returns>
        ///     Returns <see langword="true"/> if <paramref name="comparer"/> finds that <paramref name="value"/>
        ///     matches any value in the sequence; otherwise, <see langword="false"/>.
        /// </returns>

        [DebuggerStepThrough]
        public static bool In<T>(this T value, IEqualityComparer<T> comparer, IEnumerable<T> values)
        {
            return values.Any(v => comparer.Equals(value, v));
        }


        [DebuggerStepThrough]
        public static bool Same<T>(this T reference, T otherValue)
            where T : class
        {
            return Object.ReferenceEquals(reference, otherValue);
        }

        [DebuggerStepThrough]
        public static T ThrowIfNull<T>(this T reference)
            where T : class
        {
            //return ThrowIfNull(value, () => new InternalErrorException());
            return ThrowIfNull(reference, () => new InvalidOperationException());
        }

        [DebuggerStepThrough]
        public static T ThrowIfNull<T, TException>(this T reference, Func<TException> createException)
            where T : class
            where TException : Exception
        {
            return ThrowIf(reference, reference == null, createException);
        }

        [DebuggerStepThrough]
        public static T ThrowIf<T>(this T reference, bool predicate)
            where T : class
        {
            //return ThrowIf(value, predicate, () => new InternalErrorException());
            return ThrowIf(reference, predicate, () => new InvalidOperationException());
        }

        [DebuggerStepThrough]
        public static T ThrowIf<T, E>(this T reference, bool predicate, Func<E> createException)
            where T : class
            where E : Exception
        {
            if (predicate)
            {
                var exception = createException();
                //var messageException = exception as ApplicationMessageException;

                //if (messageException != null)
                //{
                //    messageException.Throw();
                //}

                throw exception;
            }

            return reference;
        }
        #endregion Object Extensions

        #region String Extensions
        /// <summary>
        ///     Determines whether two specified <see cref="string"/> objects have the same value according to culture 
        ///     invariant <see cref="StringComparison.Ordinal"/> comparison rules, used for nonlinguistic strings.
        /// </summary>
        /// <param name="reference">
        ///     A <see cref="string"/> or <see langword="null"/>.
        /// </param>
        /// <param name="b">
        ///     A <see cref="string"/> or <see langword="null"/>.
        /// </param>
        /// <returns>
        ///     <see langword="true"/> if the value of the <paramref name="reference"/> parameter is equal to the value of the 
        ///     <paramref name="b"/> parameter; otherwise, <see langword="false"/>.
        /// </returns>
        /// <remarks>
        ///     <see cref="EqualsOrdinal"/> facilitates meeting Microsoft best practice of using 
        ///     <see cref="StringComparison.Ordinal"/> for nonlinguistic string comparisons.
        /// </remarks>
        /// <seealso href="http://msdn.microsoft.com/en-us/library/bb385972.aspx">CA1309: Use ordinal StringComparison</seealso>
        [DebuggerStepThrough]
        public static bool EqualsOrdinal(this string reference, string b)
        {
            return string.Equals(reference, b, StringComparison.Ordinal);
        }

        [DebuggerStepThrough]
        public static bool EqualsOrdinalIgnoreCase(this string reference, string b)
        {
            return string.Equals(reference, b, StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        ///     Replaces the format item in a specified string with the string representation
        ///     of a corresponding object in a specified array. 
        /// </summary>
        /// <param name="reference">
        ///     A composite format string.
        /// </param>
        /// <param name="args">
        ///     An object array that contains zero or more objects to format.
        /// </param>
        /// <returns>
        ///     A copy of format in which the format items have been replaced by the string
        ///     representation of the corresponding objects in args.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="reference"/> or <paramref name="args"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="FormatException">
        ///     <para>
        ///     <paramref name="reference"/> is invalid.
        ///     </para><para>
        ///     -or-
        ///     </para><para>
        ///     The index of a format item is less than zero, or greater
        ///     than or equal to the length of the args array
        ///     </para>
        /// </exception>
        [DebuggerStepThrough]
        public static string DoFormat(this string reference, params object[] args)
        {
            return string.Format(reference, args);
        }

        /// <summary>
        ///     Replaces the format item in a specified string with the string representation
        ///     of a corresponding object in a specified array. 
        /// </summary>
        /// <param name="reference">
        ///     A composite format string.
        /// </param>
        /// <param name="provider">
        ///     An <see cref="IFormatProvider"/> implementation that supplies culture-specific formatting information.
        /// </param>
        /// <param name="args">
        ///     An <see cref="object"/> array that contains zero or more objects to format.
        /// </param>
        /// <returns>
        ///     A copy of format in which the format items have been replaced by the string
        ///     representation of the corresponding objects in args.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="reference"/> or <paramref name="args"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="FormatException">
        ///     <para>
        ///     <paramref name="reference"/> is invalid.
        ///     </para><para>
        ///     -or-
        ///     </para><para>
        ///     The index of a format item is less than zero, or greater
        ///     than or equal to the length of the args array
        ///     </para>
        /// </exception>        [DebuggerStepThrough]
        public static string DoFormat(this string reference, IFormatProvider provider, params object[] args)
        {
            return string.Format(provider, reference, args);
        }

        /// <summary>
        ///     Indicates whether the current <see cref="string"/> object is <see langword="null"/> or an 
        ///     <see cref="String.Empty"/> string.
        /// </summary>
        /// <param name="reference">
        ///     A <see cref="string"/>.
        /// </param>
        /// <returns>
        ///     <see langword="true"/> if the <paramref name="reference"/> parameter is <see langword="null"/> or an empty 
        ///     string (""); otherwise, <see langword="false"/>.
        /// </returns>
        /// <remarks>
        ///     <see cref="IsNullOrEmpty"/> transforms the static <see cref="string.IsNullOrEmpty"/> method into an
        ///     instance method.
        /// </remarks>
        [DebuggerStepThrough]
        public static bool IsNullOrEmpty(this string reference)
        {
            return String.IsNullOrEmpty(reference);
        }

        /// <summary>
        ///     Concatenates a specified separator <see cref="string"/> between each element of the current 
        ///     <see cref="string"/> sequence, yielding a single concatenated string.
        /// </summary>
        /// <param name="reference">
        ///     A sequense of <see cref="string"/> values.
        /// </param>
        /// <param name="separator">
        ///     A <see cref="string"/>.
        /// </param>
        /// <returns>
        ///     A <see cref="string"/> consisting of the elements of <paramref name="reference"/> interspersed with the
        ///     separator string.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="reference"/> is <see langword="null"/>.
        /// </exception>
        [DebuggerStepThrough]
        public static string Join(this IEnumerable<string> reference, string separator)
        {
            return string.Join(separator, reference.ToArray());
        }

        /// <summary>
        ///     Concatenates a specified separator <see cref="string"/> between each element of the current 
        ///     <see cref="string"/> sequence, yielding a single concatenated string. Parameters specify the first 
        ///     sequence element and number of elements to use.
        /// </summary>
        /// <param name="reference">
        ///     A sequense of <see cref="string"/> values.
        /// </param>
        /// <param name="separator">
        ///     A <see cref="string"/>.
        /// </param>
        /// <param name="startIndex">
        ///     The first array element in value to use.
        /// </param>
        /// <param name="count">
        ///     The number of elements of value to use.
        /// </param>
        /// <returns>
        ///     A <see cref="string"/> consisting of the elements of <paramref name="reference"/> interspersed with the
        ///     separator string. Or, <see cref="string.Empty"/> if <paramref name="count"/> is zero, 
        ///     <paramref name="reference"/> has no elements, or <paramref name="separator"/> and all the elements of 
        ///     <paramref name="reference"/> are <see cref="string.Empty"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="reference"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <para>
        ///     <paramref name="startIndex"/> or <paramref name="count"/> is less than 0.
        ///     </para><para>
        ///     -or- 
        ///     </para><para>
        ///     <paramref name="startIndex"/> plus <paramref name="count"/> is greater than the number of elements in 
        ///     <paramref name="reference"/>.
        ///     </para>
        /// </exception>
        /// <exception cref="OutOfMemoryException">
        ///     Out of memory.
        /// </exception>
        [DebuggerStepThrough]
        public static string Join(
            this IEnumerable<string> reference, string separator, int startIndex, int count)
        {
            return string.Join(separator, reference.ToArray(), startIndex, count);
        }
        #endregion String Extensions

        #region IServiceProvider Extensions
        /// <summary>
        ///     Gets the service object of the specified type.
        /// </summary>
        /// <typeparam name="T">
        ///     the type of service object to get.
        /// </typeparam>
        /// <param name="provider">
        ///     The <see cref="IServiceProvider"/>.
        /// </param>
        /// <returns>
        ///     <para>
        ///     A service object of type <typeparamref name="T"/>.
        ///     </para><para>
        ///     -or-
        ///     </para><para>
        ///     <see langword="null"/> if there is no service object of type <typeparamref name="T"/>.
        ///     </para>
        /// </returns>
        [DebuggerStepThrough]
        public static T GetService<T>(this IServiceProvider provider)
        {
            return (T)provider.GetService(typeof(T));
        }
        #endregion IServiceProvider Extensions

        #region String Extensions
        /// <summary>
        ///     Indicates whether the referenced string is null, empty, or consists only of white-space characters.
        /// </summary>
        /// <param name="value">
        ///     The string to test.
        /// </param>
        /// <returns>
        ///     <see langword="true"/> if the value parameter is <see langword="null"/> or <see cref="String.Empty"/>, 
        ///     or if value consists exclusively of white-space characters; otherwise, <see langword="false"/>.
        /// </returns>
        /// <remarks>
        ///     <see cref="IsNullOrWhiteSpace"/> transforms <see cref="String.IsNullOrWhiteSpace"/> to an instance 
        ///     method.
        /// </remarks>
        [DebuggerStepThrough]
        public static bool IsNullOrWhiteSpace(this string value)
        {
            return String.IsNullOrWhiteSpace(value);
        }
        #endregion String Extensions


        #region Private Methods
        private static U GetValueOrDefaultCore<T, U>(this T value, Func<T, U> selector, Func<U> defaultValueExpr)
        {
            if (object.Equals(value, default(T)))
            {
                return defaultValueExpr();
            }

            return selector(value);
        }
        #endregion Private Methods
    }
}
