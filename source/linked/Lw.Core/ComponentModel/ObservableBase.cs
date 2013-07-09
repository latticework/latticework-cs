using Lw.Linq.Expressions;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;

namespace Lw.ComponentModel
{
    public abstract class ObservableBase : INotifyPropertyChanged
    {
        #region Public Events
        /// <summary>
        ///     Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion Public Events

        #region Protected Constructors
        /// <summary>
        ///     Initializes a new <see cref="ObservableBase"/> instance with default properties.
        /// </summary>
        /// <remarks>
        ///     <note type="implementnotes">
        ///     The protected default constructor helps communicate the base class is abstract.
        ///     </note>
        /// </remarks>
        protected ObservableBase() { }
        #endregion Protected Constructors

        #region Protected Methods
        /// <summary>
        ///     Raises the <see cref="PropertyChanged"/> event.
        /// </summary>
        /// <param name="propertyName">
        ///     The property whose value has changed.
        /// </param>
        /// <remarks>
        ///     <para>
        ///     This method is less safe for implementers to use. Plese use <see cref="RaisePropertyChanged{T}"/>.
        ///     </para><para>
        ///         <note type="implementnotes">
        ///             If changes are made this this method, you must also make the same changes in 
        ///             <c>ObservableUIComponentBase.RaisePropertyChanged(string)</c>.
        ///         </note>
        ///     </para>
        /// </remarks>
        /// <seealso cref="RaisePropertyChanged{T}"/>
        [DebuggerStepThrough]
        protected virtual void OnPropertyChanged(string propertyName)
        {
            ExceptionOperations.VerifyNonNull(propertyName, () => propertyName);

            if (propertyName.IsNullOrEmpty())
            {
                ExceptionOperations.ThrowArgumentException(() => propertyName);
            }

            PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if (propertyChanged != null)
            {
                propertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }


        /// <overloads>
        ///     <summary>
        ///         Raises the <see cref="PropertyChanged"/> event.
        ///     </summary>
        ///     <remarks>
        ///         <see cref="RaisePropertyChanged(string)"/> should be called only from the changed property using 
        ///         the default parameter (i.e., do not use any arguments). <see cref="RaisePropertyChanged{T}"/> is 
        ///         the safe, compiler-verified form that is recommended in other circumstances. 
        ///     </remarks>
        ///     <example>
        ///         Called from the setter of a property:
        ///         this.<see cref="RaisePropertyChanged{t}">RaisePropertyChanged</see>();
        ///     </example>
        ///     <example>
        ///         Called from another method than the <c>Amount</c> property setter:
        ///         this.<see cref="RaisePropertyChanged{t}">RaisePropertyChanged</see>(() => this.Amount);
        ///     </example>
        /// </overloads>
        /// <summary>
        ///     Raises the <see cref="PropertyChanged"/> event.
        /// </summary>
        /// <typeparam name="T">The property type.</typeparam>
        /// <param name="lambdaExpression">
        ///     A lambda expression that resolves to the property whose value has changed.
        /// </param>
        /// <remarks>
        ///     <para>
        ///     The expression used is usually of the form  () => this.PropertyName.
        ///     </para><para>
        ///         <note type="implementnotes">
        ///             If changes are made this this method, you must also make the same changes in 
        ///             <c>ObservableUIComponentBase.RaisePropertyChanged&lt;T&gt;</c>.
        ///         </note>
        ///     </para>
        /// </remarks>
        /// <seealso cref="RaisePropertyChanged(string)"/>
        /// <seealso cref="PropertyChanged"/>
        [DebuggerStepThrough]
        protected void RaisePropertyChanged<T>(Expression<Func<T>> lambdaExpression)
        {
            ExceptionOperations.VerifyNonNull(lambdaExpression, () => lambdaExpression);

            RaisePropertyChanged(LambdaExpressionOperations.FindMemberName(lambdaExpression));
        }

        /// <summary>
        ///     Raises the <see cref="PropertyChanged"/> event.
        /// </summary>
        /// <param name="propertyName">
        ///     The property whose value has changed. Defaults to the calling method.
        /// </param>
        /// <remarks>
        ///     <para>
        ///     This method should only be called only from the changed property using the default parameter (i.e., do 
        ///     not use any arguments). Please use <see cref="RaisePropertyChanged{T}"/> in all other circumstances.
        ///     </para><para>
        ///         <note type="implementnotes">
        ///             If changes are made this this method, you must also make the same changes in 
        ///             <c>ObservableUIComponentBase.RaisePropertyChanged(string)</c>.
        ///         </note>
        ///     </para>
        /// </remarks>
        /// <seealso cref="RaisePropertyChanged{T}"/>
        /// <seealso cref="PropertyChanged"/>
        [DebuggerStepThrough]
        protected void RaisePropertyChanged([CallerMemberName] string propertyName = null)
        {
            OnPropertyChanged(propertyName);
        }
        #endregion Protected Methods
    }
}
