using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lw.ComponentModel.DataAnnotations
{
    /// <summary>
    ///     Specifies the Minimum length of array or string data allowed in a member.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
    public class DataMinLengthAttribute : DataValidationWrapperAttributeBase
    {
        #region Public Constructors
        /// <summary>
        ///     Initializes a new instance of the <see cref="DataMinLengthAttribute"/> class 
        ///     based on the length parameter.
        /// </summary>
        /// <param name="length">
        ///     The Minimum allowable length of array or string data.
        /// </param>
        public DataMinLengthAttribute(int length)
            : base(new MinLengthAttribute(length))
        {
        }
        #endregion Public Constructors

        #region Public Properties
        public int Length
        {
            get { return ((MinLengthAttribute)this.InnerAttribute).Length; }
        }
        #endregion Public Properties


        #region Internal Methods
        protected internal override object[] GetMessageArgs(
            ValidationContext validationContext, object value, ValidationResult validationResult)
        {
            return new object[]
            {
                ((MinLengthAttribute)this.InnerAttribute).Length,
                value
            };
        }

        protected internal override long GetMessageCode(
            ValidationContext validationContext, object value, ValidationResult validationResult)
        {
            return LwComponentModelDataAnnotationsMessages.ErrorMessageCodeMinimumLengthError;
        }
        #endregion Internal Methods
    }
}
