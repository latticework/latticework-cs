using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Linq;


namespace Lw.Linq
{
    public static class Extensions
    {
        #region IEnumerable<T> Extensions
        /// <summary>
        ///     Returns whether all elements in the sequence satisfies the specified predicate.
        /// </summary>
        /// <typeparam name="T">
        ///     The sequence type.
        /// </typeparam>
        /// <param name="source">
        ///     The sequence being evaluated.
        /// </param>
        /// <param name="predicate">
        ///     An evaluation function.
        /// </param>
        /// <returns>
        ///     <see langword="true"/> if <paramref name="predicate"/> evaluates to <see langword="true"/> for all 
        ///     element of the sequence; otherwise, <see langword="false"/>.
        /// </returns>
        [DebuggerStepThrough]
        public static bool And<T>(this IEnumerable<T> source, Func<T, bool> predicate)
        {
            Contract.Requires<ArgumentNullException>(predicate != null, "predicate");

            return And(source.Select(t => predicate(t)));
        }

        /// <summary>
        ///     Returns whether all elements in the sequence is <see langword="true"/>.
        /// </summary>
        /// <param name="source">
        ///     The sequence being evaluated.
        /// </param>
        /// <returns>
        ///     <see langword="true"/> if all elements of the sequence is <see langword="true"/>; otherwise, 
        ///     <see langword="false"/>.
        /// </returns>
        [DebuggerStepThrough]
        public static bool And(this IEnumerable<bool> source)
        {
            if (!source.Any()) { return true; }

            foreach (bool truth in source)
            {
                if (!truth) { return false; }
            }

            return true;
        }

        [DebuggerStepThrough]
        public static TSource FirstOrDefault<TSource>(this IEnumerable<TSource> source, TSource defaultValue)
        {
            var value = source.FirstOrDefault();

            return (object.Equals(value, default(TSource)))
                ? value
                : defaultValue;
        }

        [DebuggerStepThrough]
        public static TSource FirstOrDefault<TSource>(this IEnumerable<TSource> source, 
            Func<TSource, bool> predicate, TSource defaultValue)
        {
            var value = source.FirstOrDefault(predicate);

            return (object.Equals(value, default(TSource)))
                ? value
                : defaultValue;
        }

        /// <summary>
        ///     Performs a full outer join of two sequences.
        /// </summary>
        /// <typeparam name="T">
        ///     The type of the first sequence.
        /// </typeparam>
        /// <typeparam name="U">
        ///     The type of the second sequence.
        /// </typeparam>
        /// <param name="sequence1">
        ///     The first seqence.
        /// </param>
        /// <param name="sequence2">
        ///     The second sequence.
        /// </param>
        /// <param name="predicate">
        ///     Determines of an element from each sequence match.
        /// </param>
        /// <returns>
        ///     Returns the result of a full outer join of the two seqences.
        /// </returns>
        /// <seealso href="http://blogs.geniuscode.net/RyanDHatch/?p=116">Full Outer Join : LINQ Extension</seealso>
        [DebuggerStepThrough]
        public static IEnumerable<Tuple<T, U>> FullOuterJoin<T, U>(
            this IEnumerable<T> sequence1, IEnumerable<U> sequence2, Func<T, U, bool> predicate)
        {
            var left = from a in sequence1
                       from b in sequence2.Where((b) => predicate(a, b)).DefaultIfEmpty()
                       select new Tuple<T, U>(a, b);

            var right = from b in sequence2
                        from a in sequence1.Where((a) => predicate(a, b)).DefaultIfEmpty()
                        select new Tuple<T, U>(a, b);

            return left.Concat(right).Distinct();
        }

        ///// <summary>
        /////     Performs a full outer join of two sequences.
        ///// </summary>
        ///// <typeparam name="T">
        /////     The type of the first sequence.
        ///// </typeparam>
        ///// <typeparam name="U">
        /////     The type of the second sequence.
        ///// </typeparam>
        ///// <param name="sequence1">
        /////     The first seqence.
        ///// </param>
        ///// <param name="sequence2">
        /////     The second sequence.
        ///// </param>
        ///// <param name="predicate">
        /////     Determines of an element from each sequence match.
        ///// </param>
        ///// <returns>
        /////     Returns the result of a full outer join of the two seqences.
        ///// </returns>
        ///// <seealso href="http://blogs.geniuscode.net/RyanDHatch/?p=116">Full Outer Join : LINQ Extension</seealso>
        //public static IEnumerable<Tuple<T, U>> FullOuterJoin<T, U>(
        //    this IEnumerable<T> sequence1, 
        //    IEnumerable<U> sequence2, 
        //    Func<T, U, bool> predicate,
        //    IEqualityComparer<T> sequence1Comparer,
        //    IEqualityComparer<U> sequence2Comparer)
        //{
        //    var left = from a in sequence1
        //               from b in sequence2.Where((b) => predicate(a, b)).DefaultIfEmpty()
        //               select new Tuple<T, U>(a, b);

        //    var right = from b in sequence2
        //                from a in sequence1.Where((a) => predicate(a, b)).DefaultIfEmpty()
        //                select new Tuple<T, U>(a, b);

        //    var comparer = new DelegateEqualityComparer<Tuple<T, U>>(
        //        (x, y) => sequence1Comparer.Equals(x.Item1, y.Item1) && sequence2Comparer.Equals(x.Item2, y.Item2),
        //        x => sequence1Comparer.GetHashCode(x.Item1) ^ sequence2Comparer.GetHashCode(x.Item2));

        //    return left.Concat(right).Distinct(comparer);
        //}

        /// <summary>
        ///     Returns the index of the element matching the provided value or -1 if the value was not found.
        /// </summary>
        /// <typeparam name="T">
        ///     The seqence type.
        /// </typeparam>
        /// <param name="sequence">
        ///     The sequence being evaluated.
        /// </param>
        /// <param name="value">
        ///     The value being tested for equality.
        /// </param>
        /// <returns>
        ///     The index of the element matching the provided value or -1 if the value was not found.
        /// </returns>
        [DebuggerStepThrough]
        public static int IndexOf<T>(this IEnumerable<T> sequence, T value)
        {
            return sequence.IndexOf(value, (T x, T y) => EqualityComparer<T>.Default.Equals(x, y));
        }

        /// <summary>
        ///     Returns the index of the element matching the provided value or -1 if the value was not found.
        /// </summary>
        /// <typeparam name="T">
        ///     The seqence type.
        /// </typeparam>
        /// <param name="sequence">
        ///     The sequence being evaluated.
        /// </param>
        /// <param name="value">
        ///     The value being tested for equality.
        /// </param>
        /// <param name="comparison">
        ///     The equality test.
        /// </param>
        /// <returns>
        ///     The index of the element matching the provided value or -1 if the value was not found.
        /// </returns>
        [DebuggerStepThrough]
        public static int IndexOf<T>(this IEnumerable<T> sequence, T value, Func<T, T, bool> comparison)
        {
            return sequence.IndexOf(e => comparison(value, e));
        }

        /// <summary>
        ///     Returns the index of the element matching the provided predicate or -1 if the predicate was not 
        ///     satisfied.
        /// </summary>
        /// <typeparam name="T">
        ///     The seqence type.
        /// </typeparam>
        /// <param name="sequence">
        ///     The sequence being evaluated.
        /// </param>
        /// <param name="predicate">
        ///     A function to test each element for a condition.
        /// </param>
        /// <returns>
        ///     The index of the element matching the provided predicate or -1 if the predicate was not satisfied.
        /// </returns>
        [DebuggerStepThrough]
        public static int IndexOf<T>(this IEnumerable<T> sequence, Func<T, bool> predicate)
        {
            sequence.First();
            int elementIndex = 0;

            int result = -1;
            foreach (var e in sequence)
            {
                if (predicate(e))
                {
                    result = elementIndex;
                    break;
                }

                ++elementIndex;
            }

            return result;
        }

        [DebuggerStepThrough]
        public static TSource Max<TSource>(
            this IEnumerable<TSource> sequence, IComparer<TSource> comparer)
        {
            Contract.Requires<ArgumentNullException>(comparer != null, "comparer");

            TSource result = default(TSource);
            if (result == null)
            {
                foreach (TSource element in sequence)
                {
                    if ((element != null) && ((result == null) || (comparer.Compare(element, result) > 0)))
                    {
                        result = element;
                    }
                }
                return result;
            }

            bool first = false;
            foreach (TSource element in sequence)
            {
                if (first)
                {
                    if (comparer.Compare(element, result) > 0)
                    {
                        result = element;
                    }
                }
                else
                {
                    result = element;
                    first = true;
                }
            }
            if (!first)
            {
                throw new InvalidOperationException();
            }
            return result;
        }

        [DebuggerStepThrough]
        public static TSource Min<TSource>(
            this IEnumerable<TSource> sequence, IComparer<TSource> comparer)
        {
            Contract.Requires<ArgumentNullException>(comparer != null, "comparer");



            TSource result = default(TSource);
            if (result == null)
            {
                foreach (TSource element in sequence)
                {
                    if ((element != null) && ((result == null) || (comparer.Compare(element, result) < 0)))
                    {
                        result = element;
                    }
                }
                return result;
            }
            bool first = false;
            foreach (TSource element in sequence)
            {
                if (first)
                {
                    if (comparer.Compare(element, result) < 0)
                    {
                        result = element;
                    }
                }
                else
                {
                    result = element;
                    first = true;
                }
            }
            if (!first)
            {
                throw new InvalidOperationException();
            }
            return result;
        }

        /// <summary>
        ///     Returns whether any element in the sequence satisfies the specified predicate.
        /// </summary>
        /// <typeparam name="T">
        ///     The sequence type.
        /// </typeparam>
        /// <param name="source">
        ///     The sequence being evaluated.
        /// </param>
        /// <param name="predicate">
        ///     An evaluation function.
        /// </param>
        /// <returns>
        ///     <see langword="true"/> if <paramref name="predicate"/> evaluates to <see langword="true"/> for any 
        ///     element of the sequence; otherwise, <see langword="false"/>.
        /// </returns>
        [DebuggerStepThrough]
        public static bool Or<T>(this IEnumerable<T> source, Func<T, bool> predicate)
        {
            Contract.Requires<ArgumentNullException>(predicate != null, "predicate");

            return Or(source.Select(t => predicate(t)));
        }

        /// <summary>
        ///     Returns whether any element in the sequence is <see langword="true"/>.
        /// </summary>
        /// <param name="source">
        ///     The sequence being evaluated.
        /// </param>
        /// <returns>
        ///     <see langword="true"/> if any element of the sequence is <see langword="true"/>; otherwise, 
        ///     <see langword="false"/>.
        /// </returns>
        [DebuggerStepThrough]
        public static bool Or(this IEnumerable<bool> source)
        {
            if (!source.Any()) { return true; }

            foreach (bool truth in source)
            {
                if (truth) { return true; }
            }

            return false;
        }

        [DebuggerStepThrough]
        public static TResult SelectFirst<TSource, TResult>(
            this IEnumerable<TSource> sequence, Func<TSource, TResult> selector)
        {
            Contract.Requires<ArgumentNullException>(selector != null, "selector");

            return SelectFirst(sequence, selector, t => !t.Equals(default(TSource)));
        }

        [DebuggerStepThrough]
        public static TResult SelectFirst<TSource, TResult>(
            this IEnumerable<TSource> sequence,
            Func<TSource, TResult> selector,
            Func<TResult, bool> predicate)
        {
            Contract.Requires<ArgumentNullException>(selector != null, "selector");
            Contract.Requires<ArgumentNullException>(predicate != null, "predicate");

            foreach (TSource element in sequence)
            {
                TResult selection = selector(element);
                if (predicate(selection))
                {
                    return selection;
                }
            }

            throw new InvalidOperationException(Properties.Resources.Exception_InvalidOperation_NoMatch);
        }

        [DebuggerStepThrough]
        public static TResult SelectFirstOrDefault<TSource, TResult>(
            this IEnumerable<TSource> sequence, Func<TSource, TResult> selector)
        {
            Contract.Requires<ArgumentNullException>(selector != null, "selector");

            return SelectFirstOrDefault(sequence, selector, t => !t.Equals(default(TSource)), default(TResult));
        }

        [DebuggerStepThrough]
        public static TResult SelectFirstOrDefault<TSource, TResult>(
            this IEnumerable<TSource> sequence, Func<TSource, TResult> selector, TResult defaultValue)
        {
            Contract.Requires<ArgumentNullException>(selector != null, "selector");

            return SelectFirstOrDefault(sequence, selector, t => !t.Equals(default(TSource)), defaultValue);
        }

        [DebuggerStepThrough]
        public static TResult SelectFirstOrDefault<TSource, TResult>(
            this IEnumerable<TSource> sequence,
            Func<TSource, TResult> selector,
            Func<TResult, bool> predicate)
        {
            Contract.Requires<ArgumentNullException>(selector != null, "selector");
            Contract.Requires<ArgumentNullException>(predicate != null, "predicate");

            return SelectFirstOrDefault(sequence, selector, predicate, default(TResult));
        }

        [DebuggerStepThrough]
        public static TResult SelectFirstOrDefault<TSource, TResult>(
            this IEnumerable<TSource> sequence,
            Func<TSource, TResult> selector,
            Func<TResult, bool> predicate,
            TResult defaultValue)
        {
            Contract.Requires<ArgumentNullException>(selector != null, "selector");
            Contract.Requires<ArgumentNullException>(predicate != null, "predicate");

            return Extensions.FirstOrDefault(sequence.Select(selector), predicate, defaultValue);
        }

        /// <summary>
        ///     Wraps the specified sequence so that the underlying collection is not accessible.
        /// </summary>
        /// <typeparam name="T">
        ///     The sequence item type.
        /// </typeparam>
        /// <param name="sequence">
        ///     The sequence to wrap.
        /// </param>
        /// <returns>
        ///     An internal implementation of <see cref="IEnumerable{TSource}"/>.
        /// </returns>
        [DebuggerStepThrough]
        public static IEnumerable<T> ToEnumerable<T>(this IEnumerable<T> sequence)
        {
            return new EnumerableWrapper<T>(sequence);
        }
        #endregion IEnumerable<T> Extensions


        #region IDictionary<TKey, TValue> Extensions
        /// <summary>
        ///     Casts the specified .NET 1.1 dictionary interface to the generic 
        ///     <see cref="IDictionary{TKey, TValue}"/>.
        /// </summary>
        /// <typeparam name="TKey">
        ///     The key type.
        /// </typeparam>
        /// <typeparam name="TValue">
        ///     The value type.
        /// </typeparam>
        /// <param name="dictionary">
        ///     The dictionary to convert.
        /// </param>
        /// <returns>
        ///     An internal implementation of <see cref="IDictionary{TKey, TValue}"/>.
        /// </returns>
        [DebuggerStepThrough]
        public static IDictionary<TKey, TValue> Cast<TKey, TValue>(this IDictionary dictionary)
        {
            return new DictionaryWrapper<TKey, TValue>(dictionary);
        }

        /// <summary>
        ///     Casts the specified <see cref="IDictionary{TKey, TValue}"/> interface to the generic 
        ///     <see cref="IDictionary{TKey, TValue}"/>.
        /// </summary>
        /// <typeparam name="TKey">
        ///     The key type.
        /// </typeparam>
        /// <typeparam name="TValue">
        ///     The value type.
        /// </typeparam>
        /// <param name="dictionary">
        ///     The dictionary to convert.
        /// </param>
        /// <returns>
        ///     An internal implementation of <see cref="IDictionary{TKey, TValue}"/>.
        /// </returns>
        [DebuggerStepThrough]
        public static IDictionary<TKey, TValue> Cast<TKey, TValue>(this IDictionary<object, object> dictionary)
        {
            return new GenericDictionaryWrapper<TKey, TValue>(dictionary);
        }
        #endregion IDictionary<TKey, TValue> Extensions
    }

#if !NETFX_CORE
    [Serializable]
#endif
    internal sealed class DictionaryWrapper<TKey, TValue> : IDictionary<TKey, TValue>
    {
        public DictionaryWrapper(IDictionary innerDictionary)
        {
            this.InnerDictionary = innerDictionary;
        }

        public IDictionary InnerDictionary { get; set; }


        public void Add(TKey key, TValue value)
        {
            InnerDictionary.Add(key, value);
        }

        public bool ContainsKey(TKey key)
        {
            return InnerDictionary.Contains(key);
        }

        public ICollection<TKey> Keys
        {
            get { return InnerDictionary.Keys.Cast<TKey>().ToArray(); }
        }

        public bool Remove(TKey key)
        {
            if (!InnerDictionary.Contains(key))
            {
                return false;
            }

            InnerDictionary.Remove(key);
            return true;
        }

        public bool TryGetValue(TKey key, out TValue value)
        {
            value = default(TValue);

            if (!InnerDictionary.Contains(key))
            {
                return false;
            }

            value = (TValue)InnerDictionary[key];
            return true;
        }

        public ICollection<TValue> Values
        {
            get { return InnerDictionary.Values.Cast<TValue>().ToArray(); }
        }

        public TValue this[TKey key]
        {
            get
            {
                TValue value = (TValue)InnerDictionary[key];

                if (object.ReferenceEquals(value, null) && !InnerDictionary.Contains(key))
                {
                    throw new KeyNotFoundException();
                }

                return value;
            }
            set
            {
                InnerDictionary[key] = value;
            }
        }

        public void Add(KeyValuePair<TKey, TValue> item)
        {
            InnerDictionary.Add(item.Key, item.Value);
        }

        public void Clear()
        {
            InnerDictionary.Clear();
        }

        public bool Contains(KeyValuePair<TKey, TValue> item)
        {
            if (!InnerDictionary.Contains(item.Key))
            {
                return false;
            }

            var item2 = new KeyValuePair<TKey, TValue>(item.Key, (TValue)InnerDictionary[item.Key]);

            return item2.Equals(item);
        }

        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            InnerDictionary.CopyTo(array, arrayIndex);
        }

        public int Count
        {
            get { return InnerDictionary.Count; }
        }

        public bool IsReadOnly
        {
            get { return InnerDictionary.IsReadOnly; }
        }

        public bool Remove(KeyValuePair<TKey, TValue> item)
        {
            if (!InnerDictionary.Contains(item.Key))
            {
                return false;
            }

            var item2 = new KeyValuePair<TKey, TValue>(item.Key, (TValue)InnerDictionary[item.Key]);

            if (item2.Equals(item))
            {
                InnerDictionary.Remove(item.Key);
                return true;
            }

            return false;
        }

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            return new DictionaryEnumeratorWrapper<TKey, TValue>(InnerDictionary.GetEnumerator());
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

#if !NETFX_CORE
    [Serializable]
#endif
    internal sealed class DictionaryEnumeratorWrapper<TKey, TValue> : IEnumerator<KeyValuePair<TKey, TValue>>
    {
        public DictionaryEnumeratorWrapper(IDictionaryEnumerator enumerator)
        {
            this.enumerator = enumerator;
        }

        public KeyValuePair<TKey, TValue> Current
        {
            get
            {
                DictionaryEntry current = (DictionaryEntry)enumerator.Current;

                return new KeyValuePair<TKey, TValue>((TKey)current.Key, (TValue)current.Value);
            }
        }

        public void Dispose()
        {
        }

        object IEnumerator.Current
        {
            get { return Current; }
        }

        public bool MoveNext()
        {
            return enumerator.MoveNext();
        }

        public void Reset()
        {
            enumerator.Reset();
        }

        private IDictionaryEnumerator enumerator;
    }

#if !NETFX_CORE
    [Serializable]
#endif
    internal sealed class GenericDictionaryWrapper<TKey, TValue> : IDictionary<TKey, TValue>
    {
        public GenericDictionaryWrapper(IDictionary<object, object> innerDictionary)
        {
            this.InnerDictionary = innerDictionary;
        }

        public IDictionary<object, object> InnerDictionary { get; set; }


        public void Add(TKey key, TValue value)
        {
            InnerDictionary.Add(key, value);
        }

        public bool ContainsKey(TKey key)
        {
            return InnerDictionary.ContainsKey(key);
        }

        public ICollection<TKey> Keys
        {
            get { return InnerDictionary.Keys.Cast<TKey>().ToArray(); }
        }

        public bool Remove(TKey key)
        {
            if (!InnerDictionary.ContainsKey(key))
            {
                return false;
            }

            InnerDictionary.Remove(key);
            return true;
        }

        public bool TryGetValue(TKey key, out TValue value)
        {
            value = default(TValue);

            if (!InnerDictionary.ContainsKey(key))
            {
                return false;
            }

            value = (TValue)InnerDictionary[key];
            return true;
        }

        public ICollection<TValue> Values
        {
            get { return InnerDictionary.Values.Cast<TValue>().ToArray(); }
        }

        public TValue this[TKey key]
        {
            get
            {
                TValue value = (TValue)InnerDictionary[key];

                if (object.ReferenceEquals(value, null) && !InnerDictionary.ContainsKey(key))
                {
                    throw new KeyNotFoundException();
                }

                return value;
            }
            set
            {
                InnerDictionary[key] = value;
            }
        }

        public void Add(KeyValuePair<TKey, TValue> item)
        {
            InnerDictionary.Add(item.Key, item.Value);
        }

        public void Clear()
        {
            InnerDictionary.Clear();
        }

        public bool Contains(KeyValuePair<TKey, TValue> item)
        {
            if (!InnerDictionary.ContainsKey(item.Key))
            {
                return false;
            }

            var item2 = new KeyValuePair<TKey, TValue>(item.Key, (TValue)InnerDictionary[item.Key]);

            return item2.Equals(item);
        }

        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            this.ToArray().CopyTo(array, arrayIndex);
        }

        public int Count
        {
            get { return InnerDictionary.Count; }
        }

        public bool IsReadOnly
        {
            get { return InnerDictionary.IsReadOnly; }
        }

        public bool Remove(KeyValuePair<TKey, TValue> item)
        {
            if (!InnerDictionary.ContainsKey(item.Key))
            {
                return false;
            }

            var item2 = new KeyValuePair<TKey, TValue>(item.Key, (TValue)InnerDictionary[item.Key]);

            if (item2.Equals(item))
            {
                InnerDictionary.Remove(item.Key);
                return true;
            }

            return false;
        }

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            return new GenericDictionaryEnumeratorWrapper<TKey, TValue>(InnerDictionary.GetEnumerator());
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

#if !NETFX_CORE
    [Serializable]
#endif
    internal sealed class GenericDictionaryEnumeratorWrapper<TKey, TValue> : IEnumerator<KeyValuePair<TKey, TValue>>
    {
        public GenericDictionaryEnumeratorWrapper(IEnumerator<KeyValuePair<object, object>> enumerator)
        {
            this.enumerator = enumerator;
        }

        public KeyValuePair<TKey, TValue> Current
        {
            get
            {
                return new KeyValuePair<TKey, TValue>((TKey)enumerator.Current.Key, (TValue)enumerator.Current.Value);
            }
        }

        public void Dispose()
        {
        }

        object IEnumerator.Current
        {
            get { return Current; }
        }

        public bool MoveNext()
        {
            return enumerator.MoveNext();
        }

        public void Reset()
        {
            enumerator.Reset();
        }

        private IEnumerator<KeyValuePair<object, object>> enumerator;
    }

    internal sealed class EnumerableWrapper<T> : IEnumerable<T>
    {
        public EnumerableWrapper(IEnumerable<T> inner)
        {
            this.inner = inner;
        }

        public IEnumerator<T> GetEnumerator()
        {
            foreach (T t in inner)
            {
                yield return t;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        private IEnumerable<T> inner;
    }
}

