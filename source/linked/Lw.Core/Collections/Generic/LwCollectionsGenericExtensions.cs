using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Lw.Collections.Generic
{
    /// <summary>
    ///     Extension methods for <c>System.Collections.Generic</c> types.
    /// </summary>
    public static class Extensions
    {
        #region ICollection<T> Extensions
        [DebuggerStepThrough]
        public static void AddRange<T>(this ICollection<T> collection, IEnumerable<T> sequence)
        {
            ExceptionOperations.VerifyNonNull(collection, () => collection);
            ExceptionOperations.VerifyNonNull(sequence, () => sequence);

            sequence.ForEach(item => collection.Add(item));
        }

        [DebuggerNonUserCode]
        public static T GetOrAdd<T>(this ICollection<T> collection, Func<T, bool> predicate, Func<T> initializer)
        {
            return GetOrAdd(collection, predicate, initializer, EqualityComparer<T>.Default);
        }

        [DebuggerNonUserCode]
        public static T GetOrAdd<T>(this ICollection<T> collection, 
            Func<T, bool> predicate, Func<T> initializer, IEqualityComparer<T> comparer)
        {
            T value = collection.FirstOrDefault(predicate);

            if (value == null)
            {
                value = initializer();
                collection.Add(value);
            }

            return value;
        }

        [DebuggerStepThrough]
        public static void Replace<T>(this ICollection<T> collection, IEnumerable<T> sequence)
        {
            collection.Clear();
            collection.AddRange(sequence);
        }

        /// <summary>
        ///     Determines whether a sequence is <see langword="null"/> or has no elements.
        /// </summary>
        /// <typeparam name="TSource">
        ///     The element type.
        /// </typeparam>
        /// <param name="source">
        ///     An <see cref="ICollection{TSource}"/> to test.
        /// </param>
        /// <returns>
        ///     <see langword="true"/> if the specified sequence is <see langword="null"/> or has no elements;
        ///     otherwise, <see langword="false"/>.
        /// </returns>
        [DebuggerStepThrough]
        public static bool IsNullOrEmpty<TSource>(this ICollection<TSource> source)
        {
            return (source == null) ? true : source.Count == 0;
        }
        #endregion ICollection<T> Extensions

        #region IDictionary<TKey, TValue> Extensions
        [DebuggerStepThrough]
        public static void AddRange<TKey, TValue>(
            this IDictionary<TKey, TValue> dictionary, IDictionary<TKey, TValue> source)
        {
            ExceptionOperations.VerifyNonNull(dictionary, () => dictionary);
            ExceptionOperations.VerifyNonNull(source, () => source);

            source.ForEach(kvp => dictionary.Add(kvp));
        }

        [DebuggerStepThrough]
        public static void AddRange<TKey, TValue>(
            this IDictionary<TKey, TValue> dictionary, IEnumerable<KeyValuePair<TKey, TValue>> source)
        {
            ExceptionOperations.VerifyNonNull(dictionary, () => dictionary);
            ExceptionOperations.VerifyNonNull(source, () => source);

            source.ForEach(kvp => dictionary.Add(kvp));
        }

        [DebuggerStepThrough]
        public static void AddRange<TKey, TValue>(
            this IDictionary<TKey, TValue> dictionary, IEnumerable<Tuple<TKey, TValue>> source)
        {
            ExceptionOperations.VerifyNonNull(dictionary, () => dictionary);
            ExceptionOperations.VerifyNonNull(source, () => source);

            source.ForEach(tuple => dictionary.Add(new KeyValuePair<TKey, TValue>(tuple.Item1, tuple.Item2)));
        }

        [DebuggerStepThrough]
        public static TValue GetOrCreateValue<TKey, TValue>(
            this IDictionary<TKey, TValue> dictionary, TKey key)
            where TValue : new()
        {
            return GetOrCreateValue(dictionary, key, () => new TValue());
        }

        [DebuggerStepThrough]
        public static TValue GetOrCreateValue<TKey, TValue>(
            this IDictionary<TKey, TValue> dictionary, TKey key, Func<TValue> valueCreator)
        {
            ExceptionOperations.VerifyNonNull(dictionary, () => dictionary);

            TValue value;
            bool found = dictionary.TryGetValue(key, out value);

            if (!found)
            {
                value = valueCreator();
                dictionary.Add(key, value);
            }

            return value;
        }

        [DebuggerStepThrough]
        public static TValue GetValue<TKey, TValue>(
            this IDictionary<TKey, TValue> dictionary, TKey key, Action failureAction)
        {
            TValue value = default(TValue);
            bool found = dictionary.TryGetValue(key, out value);

            if (!found)
            {
                if (failureAction != null)
                {
                    failureAction();
                }
            }

            return value;
        }

        /// <summary>
        ///     Returns the value associated with the specified key in the current dictionary or a default value.
        /// </summary>
        /// <typeparam name="TKey">
        ///     The dictionary key type.
        /// </typeparam>
        /// <typeparam name="TValue">
        ///     The dictionary value type.
        /// </typeparam>
        /// <param name="dictionary">
        ///     A <see cref="IDictionary{TKey, TValue}"/>.
        /// </param>
        /// <param name="key">
        ///     A <typeparamref name="TKey"/> instance.
        /// </param>
        /// <returns>
        ///     The value associated with <paramref name="key"/> in the current dictionary, if <paramref name="key"/> 
        ///     is found; otherwise, the default value for <typeparamref name="TValue"/>.
        /// </returns>
        [DebuggerStepThrough]
        public static TValue GetValueOrDefault<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key)
        {
            return GetValueOrDefault(dictionary, key, default(TValue));
        }

        /// <summary>
        ///     Returns the value associated with the specified key in the current dictionary or a default value.
        /// </summary>
        /// <typeparam name="TKey">
        ///     The dictionary key type.
        /// </typeparam>
        /// <typeparam name="TValue">
        ///     The dictionary value type.
        /// </typeparam>
        /// <param name="dictionary">
        ///     A <see cref="IDictionary{TKey, TValue}"/>.
        /// </param>
        /// <param name="key">
        ///     A <typeparamref name="TKey"/> instance.
        /// </param>
        /// <param name="defaultValue">
        ///     A <typeparamref name="TValue"/> instance.
        /// </param>
        /// <returns>
        ///     The value associated with <paramref name="key"/> in the current dictionary, if <paramref name="key"/> 
        ///     is found; otherwise, <paramref name="defaultValue"/>.
        /// </returns>
        [DebuggerStepThrough]
        public static TValue GetValueOrDefault<TKey, TValue>(
            this IDictionary<TKey, TValue> dictionary, TKey key, TValue defaultValue)
        {
            TValue value = defaultValue;
            bool found = dictionary != null && dictionary.TryGetValue(key, out value);

            return (found) ? value : defaultValue;
        }
        #endregion IDictionary<TKey, TValue> Extensions

        #region IEnumerable<T> Extensions
        [DebuggerStepThrough]
        public static IEnumerable<T> Append<T>(this IEnumerable<T> sequence, T value)
        {
            return Enumerable.Concat(sequence, new[] { value });
        }

        /// <summary>
        ///     Executes the specified action for each element in the specified sequence.
        /// </summary>
        /// <typeparam name="T">
        ///     The element type.
        /// </typeparam>
        /// <param name="sequence">
        ///     An <see cref="IEnumerable{T}"/>.
        /// </param>
        /// <param name="action">
        ///     An <see cref="Action{T}"/>.
        /// </param>
        /// <remarks>
        ///     <see cref="ForEach{T}(IEnumerable{T}, Action{T})"/> transforms a <c>foreach</c> statement into an expression.
        /// </remarks>
        [DebuggerStepThrough]
        public static void ForEach<T>(this IEnumerable<T> sequence, Action<T> action)
        {
            foreach (T value in sequence)
            {
                action(value);
            }
        }

        /// <summary>
        ///     Executes the specified action for each element in the specified sequence.
        /// </summary>
        /// <typeparam name="T">
        ///     The element type.
        /// </typeparam>
        /// <param name="sequence">
        ///     An <see cref="IEnumerable{T}"/>.
        /// </param>
        /// <param name="action">
        ///     An <see cref="System.Action{t1, t2}"/> that recieves each element and element index in the sequence.
        /// </param>
        /// <remarks>
        ///     <see cref="ForEach{T}(IEnumerable{T}, Action{T, int})"/> transforms a <c>foreach</c> statement into an expression.
        /// </remarks>
        [DebuggerStepThrough]
        public static void ForEach<T>(this IEnumerable<T> sequence, Action<T, int> action)
        {
            int index = 0;
            foreach (T value in sequence)
            {
                action(value, index++);
            }
        }

        [DebuggerStepThrough]
        public static void Do<T>(
            this IEnumerable<T> sequence, Action<T> action, Func<bool> predicate)
        {
            if (!predicate())
            {
                return;
            }

            foreach (T value in sequence)
            {
                action(value);

                if (!predicate())
                {
                    break;
                }
            }
        }

        [DebuggerStepThrough]
        public static void DoWhile<T>(
            this IEnumerable<T> sequence, Action<T> action, Func<T, bool> predicate)
        {
            foreach (T value in sequence)
            {
                action(value);

                if (!predicate(value))
                {
                    break;
                }
            }
        }


        [DebuggerStepThrough]
        public static T ThrowIfNullOrEmpty<T>(this T value)
            where T : class
        {
            //return ThrowIfNullOrEmpty(value, () => new InternalErrorException());
            return ThrowIfNullOrEmpty(value, () => new InvalidOperationException());
        }

        [DebuggerStepThrough]
        public static T ThrowIfNullOrEmpty<T, E>(this T value, Func<E> createException)
            where T : class
            where E : Exception
        {
            if (value == null)
            {
                throw createException();
            }

            return value;
        }


        [DebuggerStepThrough]
        public static void While<T>(
            this IEnumerable<T> sequence, Action<T> action, Func<T, bool> predicate)
        {
            foreach (T value in sequence)
            {
                if (!predicate(value))
                {
                    break;
                }

                action(value);
            }
        }

        /// <summary>
        ///     Determines whether a sequence is <see langword="null"/> or has no elements.
        /// </summary>
        /// <typeparam name="TSource">
        ///     The element type.
        /// </typeparam>
        /// <param name="source">
        ///     An <see cref="IEnumerable{TSource}"/> to test.
        /// </param>
        /// <returns>
        ///     <see langword="true"/> if the specified sequence is <see langword="null"/> or has no elements;
        ///     otherwise, <see langword="false"/>.
        /// </returns>
        [DebuggerStepThrough]
        public static bool IsNullOrEmpty<TSource>(this IEnumerable<TSource> source)
        {
            return (source == null) ? true : source.Count() == 0;
        }

        /// <summary>
        ///     Creates a <see cref="List{TSource}"/> from an <see cref="IEnumerable{TSource}"/> or 
        ///     <see langword="null"/>.
        /// </summary>
        /// <typeparam name="TSource">
        ///     The type of the elements of <paramref name="source"/>. 
        /// </typeparam>
        /// <param name="source">
        ///     The <see cref="IEnumerable{TSource}"/> to create a <see cref="List{TSource}"/> from.
        /// </param>
        /// <returns>
        ///     If <paramref name="source"/> is not <see langword="null"/>, a <see cref="List{TSource}"/> 
        ///     that contains elements from the input sequence; otherwise, an empty <see cref="List{TSource}"/>.
        /// </returns>
        [DebuggerStepThrough]
        public static List<TSource> ToListOrEmpty<TSource>(this IEnumerable<TSource> source)
        {
            if (source == null)
            {
                return new List<TSource>();
            }

            return source.ToList();
        }

        [DebuggerStepThrough]
        public static UniqueObject<T>[] ToUniqueArray<T>(this IEnumerable<T> sequence)
        {
            return sequence.Select(item => new UniqueObject<T>(item)).ToArray();
        }

        [DebuggerStepThrough]
        public static List<UniqueObject<T>> ToUniqueObjectList<T>(this IEnumerable<T> sequence)
        {
            return sequence.Select(item => new UniqueObject<T>(item)).ToList();
        }
        #endregion IEnumerable<T> Extensions
    }
}
