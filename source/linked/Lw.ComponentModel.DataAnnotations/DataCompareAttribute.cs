using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Lw.ComponentModel.DataAnnotations
{
    /// <summary>
    ///     Provides an attribute that compares two properties.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple=false)]
    public class DataCompareAttribute : DataValidationWrapperAttributeBase
    {
        /// <summary>
        ///     Initializes a new instance of the System.ComponentModel.DataAnnotations.CompareAttribute
        ///     class.
        /// </summary>
        /// <param name="otherProperty">
        ///     The property to compare with the current property.
        /// </param>
        public DataCompareAttribute(string otherProperty)
            : base(new CompareAttribute(otherProperty))
        {
        }

        /// <summary>
        ///     Gets the property to compare with the current property.
        /// </summary>
        /// <value>
        ///     The other property.
        /// </value>
        public string OtherProperty
        {
            get { return ((CompareAttribute)this.InnerAttribute).OtherProperty; }
        }

        /// <summary>
        ///     Gets the display name of the other property.
        /// </summary>
        /// <value>
        ///     The display name of the other property.
        /// </value>
        public string OtherPropertyDisplayName
        {
            get { return ((CompareAttribute)this.InnerAttribute).OtherPropertyDisplayName; }
        }

        //
        // Summary:
        //     Gets a value that indicates whether the attribute requires validation context.
        //
        // Returns:
        //     true if the attribute requires validation context; otherwise, false.
        
        /// <summary>
        ///     Gets a value that indicates whether the attribute requires validation context.
        /// </summary>
        /// <value>
        ///     <see langword="true"/> if the attribute requires validation context; otherwise, <see langword="false"/>.
        /// </value>
        public override bool RequiresValidationContext
        {
            get
            {
                return true;
            }
        }

        protected internal override object[] GetMessageArgs(
            ValidationContext validationContext, object value, ValidationResult validationResult)
        {
            var propertyName = validationResult.MemberNames.First();
            var otherProperty = validationContext.ObjectType.GetProperty(this.OtherProperty);

            return new object[]
            {
                validationContext.ObjectType.GetProperty(propertyName).PropertyType.Name,
                value,
                this.OtherProperty,
                otherProperty.PropertyType.Name,
                otherProperty.GetValue(validationContext.ObjectInstance)
            };
        }

        protected internal override long GetMessageCode(
            ValidationContext validationContext, object value, ValidationResult validationResult)
        {
            return LwComponentModelDataAnnotationsMessages.ErrorMessageCodePropertyCompareError;
        }

     }
}
