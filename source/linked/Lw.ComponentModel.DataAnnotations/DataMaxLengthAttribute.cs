using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lw.ComponentModel.DataAnnotations
{
    /// <summary>
    ///     Specifies the maximum length of array or string data allowed in a member.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
    public class DataMaxLengthAttribute : DataValidationWrapperAttributeBase
    {
        #region Public Constructors
        /// <summary>
        ///     Initializes a new instance of the <see cref="DataMaxLengthAttribute"/> class.
        /// </summary>
        public DataMaxLengthAttribute()
            :base(new MaxLengthAttribute())
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="DataMaxLengthAttribute"/> class 
        ///     based on the length parameter.
        /// </summary>
        /// <param name="length">
        ///     The maximum allowable length of array or string data.
        /// </param>
        public DataMaxLengthAttribute(int length)
            : base(new MaxLengthAttribute(length))
        {
        }
        #endregion Public Constructors

        #region Public Properties
        public int Length
        {
            get { return ((MaxLengthAttribute)this.InnerAttribute).Length; }
        }
        #endregion Public Properties


        #region Internal Methods
        protected internal override object[] GetMessageArgs(
            ValidationContext validationContext, object value, ValidationResult validationResult)
        {
            return new object[]
            {
                ((MaxLengthAttribute)this.InnerAttribute).Length,
                value
            };
        }

        protected internal override long GetMessageCode(
            ValidationContext validationContext, object value, ValidationResult validationResult)
        {
            return LwComponentModelDataAnnotationsMessages.ErrorMessageCodeMaximumLengthError;
        }
        #endregion Internal Methods
    }
}
