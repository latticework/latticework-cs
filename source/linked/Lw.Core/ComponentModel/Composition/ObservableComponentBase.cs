using Lw.Linq.Expressions;
using Lw.Services;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq.Expressions;

#if !NETFX_CORE
namespace Lw.ComponentModel.Composition
#else
namespace Lw.Composition
#endif
{
    public class ObservableComponentBase : ComponentBase, INotifyPropertyChanged
    {
        #region Public Events
        /// <summary>
        ///     Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion Public Events

        #region Protected Constructors
        /// <summary>
        ///     Initializes a new <see cref="ObservableComponentBase"/> instance. <see cref="CoreServices"/> will be 
        ///     initialized from <see cref="Components.Current"/> on first access.
        /// </summary>
        protected ObservableComponentBase()
            : base()
        {
        }

        
        /// <summary>
        ///     Initializes a new <see cref="ObservableComponentBase"/> instance. <see cref="CoreServices"/> will be 
        ///     initialized from <see cref="Components.Current"/> on first access.
        /// </summary>
        /// <param name="coreServices">
        ///     An <see cref="ICoreServices"/> implementation.
        /// </param>
        protected ObservableComponentBase(ICoreServices coreServices)
            : base(coreServices)
        {
        }
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
        ///             <c>ObservableBase.RaisePropertyChanged(string)</c>.
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

        ///// <summary>
        /////     Raises the <see cref="PropertyChanged"/> event.
        ///// </summary>
        ///// <remarks>
        /////     <para>
        /////     <see cref="RaisePropertyChanged()"/> can only be used directly within the property setter method.
        /////     </para><para>
        /////         <note type="implementnotes">
        /////             If changes are made this this method, you must also make the same changes in 
        /////             <c>ObservableBase.RaisePropertyChanged()</c>.
        /////         </note>
        /////     </para>
        ///// </remarks>
        ///// <seealso cref="RaisePropertyChanged{T}"/>
        ///// <seealso href="http://www.wintellect.com/cs/blogs/jlikness/archive/2010/12/17/jounce-part-8-raising-property-changed.aspx">Jounce Part 8: Raising Version Changed</seealso>
        //[DebuggerStepThrough]
        //protected void RaisePropertyChanged()
        //{
        //    var frames = new System.Diagnostics.StackTrace();

        //    bool hasError = true;
        //    do
        //    {
        //        if (frames.FrameCount < 2) { break; }


        //        var method = frames.GetFrame(2).GetMethod() as MethodInfo;

        //        if (!method.IsSpecialName) { break; }


        //        var name = method.Name;

        //        if (!name.StartsWith("set_")) { break; }

        //        RaisePropertyChanged(name.Substring(4));
        //    } while (false);

        //    if (hasError)
        //    {
        //        throw new InternalErrorException(
        //            "Method NotifyPropertyChanged()' can only by invoked directly within a property setter.");
        //    }
        //}

        /// <overloads>
        ///     <summary>
        ///         Raises the <see cref="PropertyChanged"/> event.
        ///     </summary>
        ///     <remarks>
        ///         <see cref="RaisePropertyChanged{T}"/> is the safe, compiler-verified form that is recommended. 
        ///         <see cref="RaisePropertyChanged(string)"/> is less safe and should not be used.
        ///     </remarks>
        ///     <example>
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
        ///             <c>ObservableBase.RaisePropertyChanged&lt;T&gt;</c>.
        ///         </note>
        ///     </para>
        /// </remarks>
        /// <seealso cref="RaisePropertyChanged(string)"/>
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
        ///     The property whose value has changed.
        /// </param>
        /// <remarks>
        ///     <para>
        ///     This method is less safe for implementers to use. Plese use <see cref="RaisePropertyChanged{T}"/>.
        ///     </para><para>
        ///         <note type="implementnotes">
        ///             If changes are made this this method, you must also make the same changes in 
        ///             <c>ObservableBase.RaisePropertyChanged(string)</c>.
        ///         </note>
        ///     </para>
        /// </remarks>
        /// <seealso cref="RaisePropertyChanged{T}"/>
        [DebuggerStepThrough]
        protected void RaisePropertyChanged(string propertyName)
        {
            OnPropertyChanged(propertyName);
        }

        #endregion Protected Methods
    }
}
