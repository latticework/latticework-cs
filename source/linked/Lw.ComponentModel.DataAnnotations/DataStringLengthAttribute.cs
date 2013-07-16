using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace Lw.ComponentModel.DataAnnotations
{
    /// <summary>
    ///     Specifies the minimum and maximum length of characters that are allowed in a data field.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
    public sealed class DataStringLengthAttribute : DataValidationWrapperAttributeBase
    {
        #region Public Constructors
        /// <summary>
        ///     Initializes a new instance of the <see cref="Lw.ComponentModel.DataAnnotations.DataStringLengthAttribute"/> class.
        /// </summary>
        /// <param name="maximumLength">
        ///     The maximum length a string.
        /// </param>
        public DataStringLengthAttribute(int maximumLength)
            : base(new StringLengthAttribute(maximumLength))
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="Lw.ComponentModel.DataAnnotations.DataStringLengthAttribute"/> class.
        /// </summary>
        /// <param name="maximumLength">
        ///     The maximum length a string.
        /// </param>
        /// <param name="errorMessage">
        ///     The error message to associate with a validation control.
        /// </param>
        public DataStringLengthAttribute(int maximumLength, string errorMessage)
            : base(errorMessage, new StringLengthAttribute(maximumLength))
        {
        }
        #endregion Public Constructors

        #region Public Properties
        /// <summary>
        ///     Gets or sets the maximum length of a string.
        /// </summary>
        /// <value>
        ///     The maximum length a string.
        /// </value>
        public int MaximumLength
        {
            get { return ((StringLengthAttribute)this.InnerAttribute).MaximumLength; }
        }


        /// <summary>
        ///     Gets or sets the minimum length of a string.
        /// </summary>
        /// <returns>
        ///     The minimum length of a string.
        /// </returns>
        public int MinimumLength { get; set; }
        #endregion Public Properties

        #region Protected Methods
        protected override void AssignOptionalPropertiesToInnerAttribute()
        {
            base.AssignOptionalPropertiesToInnerAttribute();
            ((StringLengthAttribute)this.InnerAttribute).MinimumLength = this.MinimumLength;
        }
        #endregion Protected Methods


        #region Internal Methods
        protected internal override object[] GetMessageArgs(
            ValidationContext validationContext, object value, ValidationResult validationResult)
        {
            return new object[]
            {
                ((StringLengthAttribute)this.InnerAttribute).MinimumLength,
                ((StringLengthAttribute)this.InnerAttribute).MaximumLength,
                value
            };
        }

        protected internal override long GetMessageCode(
            ValidationContext validationContext, object value, ValidationResult validationResult)
        {
            return LwComponentModelDataAnnotationsMessages.ErrorMessageCodeValueRequiredError;
        }
        #endregion Internal Methods
    }
}
