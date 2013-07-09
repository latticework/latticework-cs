using System;
using System.Diagnostics;

namespace Lw.Diagnostics
{
    /// <summary>
    ///     Utility class that verifies that a class's static constructor has been called. Should be 
    ///     referenced by the static constructor, instance constructors and all static properties.
    /// </summary>
    /// <typeparam name="T">
    ///     The type whose static constructor is being monitored.
    /// </typeparam>
    public sealed class StaticConstructionValidator<T>
    {
        #region Public Constructors
        [DebuggerStepThrough]
        public StaticConstructionValidator()
        {
            this.Exception = null;
        }
        #endregion Public Constructors

        #region Public Methods
        /// <summary>
        ///     Throws an <see cref="InvalidOperationException"/> if the static constructor did not succeed.
        /// </summary>
        [DebuggerStepThrough]
        public void Validate()
        {
            if (!Valid)
            {
                throw new InvalidOperationException(
                    ExceptionOperations.CreateStaticConstructorExceptionMessage(Type, Exception));
            }
        }
        #endregion Public Methods

        #region Public Properties
        public Exception Exception { get; set; }

        public Type Type 
        {
            get { return typeof(T); }
        }

        public bool Valid
        {
            get { return Exception == null; }
        }
        #endregion Public Properties
    }
}
