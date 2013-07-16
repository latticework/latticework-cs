using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace Lw.ComponentModel.DataAnnotations
{
    /// <summary>
    ///     Specifies the numeric range constraints for the value of a data field.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
    public sealed class DataRangeAttribute : DataValidationWrapperAttributeBase
    {
        #region Public Constructors
        /// <summary>
        ///     Initializes a new instance of the <see cref="T:Lw.ComponentModel.DataAnnotations.DataRangeAttribute" /> class by using the specified minimum and maximum values.
        /// </summary>
        /// <param name="minimum">
        ///     Specifies the minimum value allowed for the data field value.
        /// </param>
        /// <param name="maximum">
        ///     Specifies the maximum value allowed for the data field value.
        /// </param>
        public DataRangeAttribute(int minimum, int maximum) 
            : base(new RangeAttribute(minimum, maximum))
        {
            
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="T:Lw.ComponentModel.DataAnnotations.DataRangeAttribute" /> class by using the specified minimum and maximum values. 
        /// </summary>
        /// <param name="minimum">
        ///     Specifies the minimum value allowed for the data field value.
        /// </param>
        /// <param name="maximum">
        ///     Specifies the maximum value allowed for the data field value.
        /// </param>
        public DataRangeAttribute(double minimum, double maximum)
            : base(new RangeAttribute(minimum, maximum))
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="T:Lw.ComponentModel.DataAnnotations.DataRangeAttribute" /> class by using the specified minimum and maximum values and the specific type.
        /// </summary>
        /// <param name="type">
        ///     Specifies the type of the object to test.
        /// </param>
        /// <param name="minimum">
        ///     Specifies the minimum value allowed for the data field value.
        /// </param>
        /// <param name="maximum">
        ///     Specifies the maximum value allowed for the data field value.
        /// </param>
        /// <exception cref="T:System.ArgumentNullException">
        ///     <paramref name="type" /> is null.
        /// </exception>
        public DataRangeAttribute(Type type, string minimum, string maximum)
            : base(new RangeAttribute(type, minimum, maximum))
        {
        }
        #endregion Public Constructors

        #region Public Properties
        /// <summary>Gets the maximum allowed field value.</summary>
        /// <value>The maximum value that is allowed for the data field.</value>
        public object Maximum
        {
            get { return ((RangeAttribute)this.InnerAttribute).Maximum; }
        }

        /// <summary>Gets the minimum allowed field value.</summary>
        /// <value>The minimu value that is allowed for the data field.</value>
        public object Minimum
        {
            get { return ((RangeAttribute)this.InnerAttribute).Minimum; }
        }

        /// <summary>Gets the type of the data field whose value must be validated.</summary>
        /// <value>The type of the data field whose value must be validated.</value>
        public Type OperandType
        {
            get { return ((RangeAttribute)this.InnerAttribute).OperandType; }
        }
        #endregion Public Properties


        #region Internal Methods
        protected internal override object[] GetMessageArgs(
            ValidationContext validationContext, object value, ValidationResult validationResult)
        {
            var propertyName = validationResult.MemberNames.First();

            return new object[]
            {
                validationContext.ObjectType.GetProperty(propertyName).PropertyType.Name,
                ((RangeAttribute)this.InnerAttribute).Minimum,
                ((RangeAttribute)this.InnerAttribute).Maximum,
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
