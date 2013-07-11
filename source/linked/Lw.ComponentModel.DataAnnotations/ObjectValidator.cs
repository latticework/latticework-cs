using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Lw.Reflection;

namespace Lw.ComponentModel.DataAnnotations
{
    public static class ObjectValidator
    {
        #region Public Methods
        public static bool TryValidate<T>(T obj, out ApplicationValidationResult result)
        {
            LwComponentModelDataAnnotationExtensions.EnsureCashedType(typeof(T));

            result = null;

            var validationResults = new List<ValidationResult>();

            var context = new ValidationContext(obj, null, null);
            var isValid = Validator.TryValidateObject(obj, context, validationResults, validateAllProperties: true);

            var validationsToReturn = (
                    from vr in validationResults
                    let dvrs = vr as DataValidationResult
                    from dvr in (dvrs == null) ? Enumerable.Empty<DataValidationResult>() : dvrs.GetResults()
                    select (dvrs == null) ? vr : dvr
                );


            result = new ApplicationValidationResult(validationsToReturn);

            return result.Succeeded;
        }

        public static ApplicationValidationResult Validate<T>(T obj)
        {
            ApplicationValidationResult result;

            if (!ObjectValidator.TryValidate(obj, out result))
            {
                throw new ApplicationValidationException(result);
            }

            return result;
        }
        #endregion Public Methods
    }
}
