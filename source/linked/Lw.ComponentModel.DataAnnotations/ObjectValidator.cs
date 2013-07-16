using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Lw.Reflection;
using Lw.ApplicationMessages;

namespace Lw.ComponentModel.DataAnnotations
{
    public static class ObjectValidator
    {
        #region Public Methods
        public static bool TryValidate<T>(T obj, out ApplicationMessageCollection messages)
        {
            LwComponentModelDataAnnotationExtensions.EnsureCashedType(typeof(T));

            messages = null;

            var validationResults = new List<ValidationResult>();

            var context = new ValidationContext(obj, null, null);
            var isValid = Validator.TryValidateObject(obj, context, validationResults, validateAllProperties: true);

            var messagesToReturn = (
                    from vr in validationResults
                    let dvr = vr as DataValidationResult
                    from am in (dvr == null) ? Enumerable.Empty<ApplicationMessage>() : dvr.Messages
                    select (dvr == null) ? vr.ToApplicationMessage(obj) : am
                );


            messages = new ApplicationMessageCollection(messagesToReturn);

            return messages.HasErrors;
        }

        public static ApplicationMessageCollection Validate<T>(T obj)
        {
            ApplicationMessageCollection messages;

            if (!ObjectValidator.TryValidate(obj, out messages))
            {
                throw new ApplicationMessageException(messages);
            }

            return messages;
        }
        #endregion Public Methods
    }
}
