using Lw.Diagnostics;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Lw
{
    /// <summary>
    ///     Provides a sequence of <see cref="IUniqueObject"/> objects that specify an object graph traversal
    ///     to a specified unique object.
    /// </summary>
    /// <remarks>
    ///     Compare the return value of <see cref="IUniqueObject.Find"/> with <see cref="Empty"/> to 
    ///     determine whether the object was found. The matched object can be accessed by calling 
    ///     <c><see cref="Path"/>.<see cref="Enumerable.Last{TSource}(IEnumerable{TSource})"/></c>.
    /// </remarks>
    public sealed class UniqueObjectPath : IEquatable<UniqueObjectPath>
    {
        #region Public Constructors
        /// <summary>
        ///     Initializes an empty <see cref="UniqueObjectPath"/>.
        /// </summary>
        public UniqueObjectPath()
        {
            staticConstructionValidator.Validate();

            this.Path = new List<IUniqueObject>();
        }

        /// <summary>
        ///     Initializes a new <see cref="UniqueObjectPath"/> with a matched <see cref="IUniqueObject"/>.
        /// </summary>
        /// <param name="value">
        ///     A matched <see cref="IUniqueObject"/> or <see langword="null"/>.
        /// </param>
        public UniqueObjectPath(IUniqueObject value)
        {
            staticConstructionValidator.Validate();

            this.Path = (value == null)
                ? new List<IUniqueObject>()
                : new List<IUniqueObject> { value };
        }

        /// <summary>
        ///     Initializes a <see cref="UniqueObjectPath"/> that prepends a new root object.
        /// </summary>
        /// <param name="root">
        ///     The <see cref="IUniqueObject"/> to set as the root of the graph.
        /// </param>
        /// <param name="innerPath">
        ///     A sequence of <see cref="IUniqueObject"/> instances that represent the rest of the path.
        /// </param>
        /// <exception cref="ArgumentNullException">
        ///     <para>
        ///     <paramref name="root"/> is a <see langword="null"/>.
        ///     </para><para>
        ///     -or-
        ///     </para><para>
        ///     <paramref name="innerPath"/> is a <see langword="null"/>.
        ///     </para>
        /// </exception>
        [ExceptionContract(typeof(ArgumentNullException))]
        public UniqueObjectPath(IUniqueObject root, IEnumerable<IUniqueObject> innerPath)
        {
            staticConstructionValidator.Validate();

            ExceptionOperations.VerifyNonNull(root, () => root);
            ExceptionOperations.VerifyNonNull(innerPath, () => innerPath);

            var path = new List<IUniqueObject> { root };
            path.AddRange(innerPath);

            this.Path = path;
        }
        #endregion Public Constructors

        #region Private Constructors
        static UniqueObjectPath()
        {
            staticConstructionValidator = new StaticConstructionValidator<UniqueObjectPath>();
            try
            {
                Empty = new UniqueObjectPath();
            }
            catch (Exception exception)
            {
                staticConstructionValidator.Exception = exception;
            }
        }
        #endregion Private Constructors

        #region Public Methods
        /// <summary>
        ///     Determines whether the specified <see cref="UniqueObjectPath"/> is equal to the current 
        ///     <see cref="UniqueObjectPath"/>.
        /// </summary>
        /// <param name="other">
        ///     The <see cref="UniqueObjectPath"/> to compare with the current <see cref="UniqueObjectPath"/>.
        /// </param>
        /// <returns>
        ///     <see langword="true"/> if the specified <see cref="UniqueObjectPath"/> is equal to the 
        ///     current <see cref="UniqueObjectPath"/>; otherwise, <see langword="false"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="other"/> is <see langword="null"/>.
        /// </exception>
        public bool Equals(UniqueObjectPath other)
        {
            // Object.Equals may throw an ArgumentNullException.
            if (base.Equals(other)) { return true; }

            return this.Path.SequenceEqual(other.Path);
        }

        /// <summary>
        ///     Determines whether the specified <see cref="Object"/> is equal to the current 
        ///     <see cref="UniqueObjectPath"/>.
        /// </summary>
        /// <param name="obj">
        ///     The <see cref="Object"/> to compare with the current <see cref="UniqueObjectPath"/>.
        /// </param>
        /// <returns>
        ///     <see langword="true"/> if the specified <see cref="Object"/> is equal to the current 
        ///     <see cref="UniqueObjectPath"/>; otherwise, <see langword="false"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="obj"/> is <see langword="null"/>.
        /// </exception>
        public override bool Equals(object obj)
        {
            // Object.Equals may throw an ArgumentNullException.
            if (base.Equals(obj)) { return true; }


            if (!(obj is UniqueObjectPath)) { return false; }


            return Equals((UniqueObjectPath)obj);
        }

        /// <summary>
        ///     Serves as a hash function for <see cref="UniqueObjectPath"/>.
        /// </summary>
        /// <returns>
        ///     A hash code for the current <see cref="UniqueObjectPath"/>.
        /// </returns>
        public override int GetHashCode()
        {
            return Path.GetHashCode();
        }
        #endregion Public Methods

        #region Public Properties
        public static UniqueObjectPath Empty { get; private set; }

        /// <summary>
        ///     Gets the traversal sequence to a <see cref="IUniqueObject"/> within a unique object graph.
        /// </summary>
        /// <remarks>
        ///     The retrieved object can be accessed by calling 
        ///     <c><see cref="Path"/>.<see cref="Enumerable.Last{TSource}(IEnumerable{TSource})"/></c>.
        /// </remarks>
        public IList<IUniqueObject> Path { get; private set; }
        #endregion Public Properties

        #region Private Fields
        private static StaticConstructionValidator<UniqueObjectPath> staticConstructionValidator;
        #endregion Private Fields
    }
}
