using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace Lw
{
    /// <summary>
    ///     Provides an <see cref="EqualityComparer{T}"/> implementation that relies on a 
    ///     <see cref="Delegate"/> method to provide a value to compare.
    /// </summary>
    /// <typeparam name="T">
    ///     The type to compare.
    /// </typeparam>
    /// <typeparam name="U">
    ///     The type used to provide the comparison.
    /// </typeparam>
    public class DelegateEqualityComparer<T, U> : EqualityComparer<T>
    {
        /// <summary>
        ///     Initializes the <see cref="DelegateEqualityComparer{T, U}"/> with the specified comparison 
        ///     value selector.
        /// </summary>
        /// <param name="compareValueSelector">
        ///     Returns the value to compare.
        /// </param>
        public DelegateEqualityComparer(Func<T, U> compareValueSelector)
        {
            Contract.Requires<ArgumentNullException>(compareValueSelector != null, "compareValueSelector");

            this.compareValueSelector = compareValueSelector;
        }

        /// <summary>
        ///     Determines whether the specified objects are equal.
        /// </summary>
        /// <param name="x">
        ///     The first object of type <typeparamref name="T"/> to compare.
        /// </param>
        /// <param name="y">
        ///     The second object of type <typeparamref name="T"/> to compare.
        /// </param>
        /// <returns>
        ///     <see langword="true"/> if the specified objects are equal; otherwise, <see langword="false"/>.
        /// </returns>
        [ExceptionContract]
        public override bool Equals(T x, T y)
        {
            return EqualityComparer<U>.Default.Equals(compareValueSelector(x), compareValueSelector(y));
        }

        /// <summary>
        ///     Returns a hash code for the specified object.
        /// </summary>
        /// <param name="obj">
        ///     The <see cref="Object"/> for which a hash code is to be returned.
        /// </param>
        /// <returns>
        ///     A hash code for the specified object.
        /// </returns>
        [ExceptionContract(typeof(ArgumentNullException))]
        public override int GetHashCode(T obj)
        {
            Contract.Requires<ArgumentNullException>(obj != null, "obj");

            return EqualityComparer<U>.Default.GetHashCode(compareValueSelector(obj));
        }

        private readonly Func<T, U> compareValueSelector;
    }

    /// <summary>
    ///     Provides an <see cref="EqualityComparer{T}"/> implementation that relies on a 
    ///     <see cref="Delegate"/> method to provide the equality comparison.
    /// </summary>
    /// <typeparam name="T">
    ///     The type to compare.
    /// </typeparam>
    public class DelegateEqualityComparer<T> : EqualityComparer<T>
    {
        /// <summary>
        ///     Initializes the <see cref="DelegateEqualityComparer{T}"/> with the specified comparison 
        ///     value selector.
        /// </summary>
        /// <param name="equalityComparison">
        ///     Evaluates equality between two objects.
        /// </param>
        /// <param name="hashCodeComputation">
        ///     Determines immutable hash value for object.
        /// </param>
        public DelegateEqualityComparer(Func<T, T, bool> equalityComparison, Func<T, int> hashCodeComputation)
        {
            this.equalityComparison = equalityComparison;
            this.hashCodeComputation = hashCodeComputation;
        }

        /// <summary>
        ///     Determines whether the specified objects are equal.
        /// </summary>
        /// <param name="x">
        ///     The first object of type <typeparamref name="T"/> to compare.
        /// </param>
        /// <param name="y">
        ///     The second object of type <typeparamref name="T"/> to compare.
        /// </param>
        /// <returns>
        ///     <see langword="true"/> if the specified objects are equal; otherwise, <see langword="false"/>.
        /// </returns>
        public override bool Equals(T x, T y)
        {
            return equalityComparison(x, y);
        }

        /// <summary>
        ///     Returns a hash code for the specified object.
        /// </summary>
        /// <param name="obj">
        ///     The <see cref="Object"/> for which a hash code is to be returned.
        /// </param>
        /// <returns>
        ///     A hash code for the specified object.
        /// </returns>
        [ExceptionContract(typeof(ArgumentNullException))]
        public override int GetHashCode(T obj)
        {
            ExceptionOperations.VerifyNonNull(obj, () => obj);

            return this.hashCodeComputation(obj);
        }

        private readonly Func<T, T, bool> equalityComparison;
        private readonly Func<T, int> hashCodeComputation;
    }
}
