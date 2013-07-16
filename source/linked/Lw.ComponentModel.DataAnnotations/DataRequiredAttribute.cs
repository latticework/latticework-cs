using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Resources;
using System.Reflection;

namespace Lw.ComponentModel.DataAnnotations
{
    /// <summary>
    ///     Specifies that a data field value is required.
    ///     Based on <see cref="System.ComponentModel.DataAnnotations.RequiredAttribute"/>.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
    public sealed class DataRequiredAttribute : DataValidationWrapperAttributeBase
    {
        #region Public Constructors
        /// <summary>
        ///     Initializes a new instance of the <see cref="Lw.ComponentModel.DataAnnotations.DataRequiredAttribute"/> class.
        ///     This would find the property name with Transientkey attribute.
        /// </summary>
        public DataRequiredAttribute()
            : base(new RequiredAttribute())
        {
        }
        #endregion Public Constructors

        #region Public Properties
        /// <summary>
        ///     Gets or sets a value that indicates whether an empty string is allowed.
        /// </summary>
        /// <returns>
        ///     <see langword="true"/> if an empty string is allowed; otherwise, <see langword="false"/>. The default 
        ///     value is <see langword="false"/>.
        /// </returns>
        public bool AllowEmptyStrings { get; set; }
        
        #endregion Public Properties


        #region Protected Methods
        protected override void AssignOptionalPropertiesToInnerAttribute()
        {
            base.AssignOptionalPropertiesToInnerAttribute();
            ((RequiredAttribute)this.InnerAttribute).AllowEmptyStrings = this.AllowEmptyStrings;
        }
        #endregion Protected Methods


        #region Internal Methods
        protected internal override object[] GetMessageArgs(
            ValidationContext validationContext, object value, ValidationResult validationResult)
        {
            return new object[]
            {
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
