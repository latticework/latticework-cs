using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using Lw.BusinessLogic.EF.Validation.Net40;
using System.ComponentModel.DataAnnotations;
using Lw.BusinessLogic;
using Lw.ComponentModel.DataAnnotations;

namespace Lw.SeniorLoans.Compliance.BusinessLogic
{
    public class ServiceResponse
    {
        #region Public Ctor
        public ServiceResponse(ApplicationValidationResult validationResult = null)
        {
            IList<ValidationResult> validationResults = null;
            this.ValidationResult = validationResult ?? new ApplicationValidationResult(validationResults);

            this.Status = !ValidationResult.Succeeded ?
                ServiceResponseStatus.Failed :
                ServiceResponseStatus.Succeeded;
        }
        #endregion Public Ctor

        #region Public Properties
        public ServiceResponseStatus Status { get; private set; }
        public ApplicationValidationResult ValidationResult { get; private set; }
        #endregion Public Properties
    }

    public class ServiceResponse<T>
    {
        #region Public Ctor
        public ServiceResponse(T result, ApplicationValidationResult validationResult = null)
        {
            this.Result = result;
            IList<ValidationResult> validationResults = null;
            this.ValidationResult = validationResult ?? new ApplicationValidationResult(validationResults);

            this.Status = !ValidationResult.Succeeded ? 
                ServiceResponseStatus.Failed : 
                ServiceResponseStatus.Succeeded;
        }
        #endregion Public Ctor

        #region Public Properties
        public T Result { get; private set; }
        public ServiceResponseStatus Status { get; private set; }
        public ApplicationValidationResult ValidationResult { get; private set; }
        #endregion Public Properties
    }
}
