using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using Lw.ApplicationMessages;

namespace Lw.ComponentModel.DataAnnotations
{
    /// <summary>
    ///     Base class for all custom validation attributes.
    /// </summary>
    public abstract class DataValidationAttributeBase : ValidationAttribute
    {
        #region Protected Constructors
        /// <summary>
        ///     Initializes a new instance of the <see cref="DataValidationAttributeBase"/>
        ///     class.
        /// </summary>
        protected DataValidationAttributeBase()
            : base()
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="DataValidationAttributeBase"/>
        ///     class by using the error message to associate with a validation control.
        /// </summary>
        /// <param name="errorMessage">
        ///     The error message to associate with a validation control.
        /// </param>
        protected DataValidationAttributeBase(string errorMessage)
            : base(errorMessage)
        {
            
        }
        #endregion Protected Constructors

        #region Protected Methods
        protected sealed override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            return this.IsValidOverride(value, validationContext);
        }

        protected abstract ValidationResult IsValidCore(object value, ValidationContext validationContext);


        protected virtual ValidationResult IsValidOverride(object value, ValidationContext validationContext)
        {
            return this.IsValidCore(value, validationContext);
        }
        #endregion Protected Methods
    }
}
