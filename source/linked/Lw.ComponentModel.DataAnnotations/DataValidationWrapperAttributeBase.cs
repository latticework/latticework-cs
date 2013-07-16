using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using Lw.ApplicationMessages;

namespace Lw.ComponentModel.DataAnnotations
{
    public abstract class DataValidationWrapperAttributeBase : DataValidationAttributeBase
    {
        #region Protected Constructors
        /// <summary>
        ///     Initializes a new instance of the <see cref="DataValidationAttributeBase"/>
        ///     class.
        /// </summary>
        protected DataValidationWrapperAttributeBase(ValidationAttribute innerAttribute)
            : base()
        {
            this.InnerAttribute = innerAttribute;
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="DataValidationAttributeBase"/>
        ///     class by using the error message to associate with a validation control.
        /// </summary>
        /// <param name="errorMessage">
        ///     The error message to associate with a validation control.
        /// </param>
        protected DataValidationWrapperAttributeBase(string errorMessage, ValidationAttribute innerAttribute)
            : base(errorMessage)
        {
            this.InnerAttribute = innerAttribute;
            this.InnerAttribute.ErrorMessage = errorMessage; 
        }
        #endregion Protected Constructors

        #region Protected Properties
        protected ValidationAttribute InnerAttribute { get; private set; }
        #endregion Protected Properties


        #region Protected Methods
        protected virtual void AssignOptionalPropertiesToInnerAttribute()
        {

        }

        protected virtual ApplicationMessage CreateApplicationMessage(
            ValidationContext validationContext, object value, ValidationResult validationResult)
        {
            return LwComponentModelDataAnnotationsMessages.CreateMessage(
                validationContext.ObjectInstance,
                validationResult.MemberNames,
                this.GetMessageCode(validationContext, value, validationResult),
                this.GetMessageArgs(validationContext, value, validationResult));
        }
        
        protected internal virtual object[] GetMessageArgs(
            ValidationContext validationContext, object value, ValidationResult validationResult)
        {
            throw new NotImplementedException();
        }

        protected internal abstract long GetMessageCode(
            ValidationContext validationContext, object value, ValidationResult validationResult);

        protected override ValidationResult IsValidCore(object value, ValidationContext validationContext)
        {
            var validationResult = this.InnerAttribute.GetValidationResult(value, validationContext);


            if (validationResult != ValidationResult.Success)
            {
                validationResult = new DataValidationResult(CreateApplicationMessage(validationContext, value, validationResult));
            }

            return validationResult;
        }

        protected override ValidationResult IsValidOverride(object value, ValidationContext validationContext)
        {
            AssignOptionalPropertiesToInnerAttribute();
            return base.IsValidOverride(value, validationContext);
        }
        #endregion Protected Methods
    }
}
