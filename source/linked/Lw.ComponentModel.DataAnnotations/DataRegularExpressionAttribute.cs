using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace Lw.ComponentModel.DataAnnotations
{
    /// <summary>
    ///     Specifies that a data field value in ASP.NET Dynamic Data must match the specified regular expression.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
    public sealed class DataRegularExpressionAttribute : DataValidationWrapperAttributeBase
    {
        #region Public Constructors
        /// <summary>
        ///     Initializes a new instance of the <see cref="DataRegularExpressionAttribute" /> class.
        /// </summary>
        /// <param name="pattern">
        ///     The regular expression that is used to validate the data field value. 
        /// </param>
        /// <exception cref="ArgumentNullException">
        ///   <paramref name="pattern" /> is null.
        /// </exception>
        public DataRegularExpressionAttribute(string pattern)
            : base(new RegularExpressionAttribute(pattern))
        {
        }
        #endregion Public Constructors

        #region Public Properties
        /// <summary>Gets the regular expression pattern.</summary>
        /// <property>The pattern to match.</property>
        public string Pattern 
        {
            get { return ((RegularExpressionAttribute)this.InnerAttribute).Pattern; }
        }
        #endregion Public Properties


        #region Internal Methods
        protected internal override object[] GetMessageArgs(
            ValidationContext validationContext, object value, ValidationResult validationResult)
        {
            var propertyName = validationResult.MemberNames.First();

            return new object[]
            {
                ((RegularExpressionAttribute)this.InnerAttribute).Pattern,
                value
            };
        }

        protected internal override long GetMessageCode(
            ValidationContext validationContext, object value, ValidationResult validationResult)
        {
            return LwComponentModelDataAnnotationsMessages.ErrorMessageCodeValueRangeError;
        }
        #endregion Internal Methods
    }
}
