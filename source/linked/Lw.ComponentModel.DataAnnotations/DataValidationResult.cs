using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using Lw.ApplicationMessages;

namespace Lw.ComponentModel.DataAnnotations
{
    public class DataValidationResult : ValidationResult
    {
        #region Public Constructors
        public DataValidationResult(string message, ApplicationMessageSeverity severity, object obj)
            : base(message)
        {
            Initialize(severity, obj);
        }

        public DataValidationResult(
            string message, IEnumerable<string> memberNames, ApplicationMessageSeverity severity, object obj)
            : base(message, memberNames)
        {
            Initialize(severity, obj);
        }

        public DataValidationResult(ValidationResult validationResult)
            : this(
                validationResult,
                DataValidationResult.GetResultSeverity(validationResult),
                DataValidationResult.GetResultObject(validationResult))
        {

        }

        public DataValidationResult(
                ValidationResult validationResult, ApplicationMessageSeverity severity, object obj)
            : base(validationResult)
        {
            Initialize(severity, obj);
        }

        private void Initialize(ApplicationMessageSeverity severity, object obj)
        {
            this.results = new List<ValidationResult>();

            this.Severity = severity;
            this.Object = obj;
            this.UId = obj.GetValueOrDefault(o => o.GetTransientKey());
        }
        #endregion Public Constructors

        #region Public Properties
        public object Object { get; set; }

        public ApplicationMessageSeverity Severity { get; set; }

        public Guid? UId { get; set; }
        #endregion Public Properties

        #region Public Methods
        public void AddResult(ValidationResult result)
        {
            var resultToAdd = result is DataValidationResult ? result : new DataValidationResult(result);
            this.results.Add(resultToAdd);
        }

        public IEnumerable<ValidationResult> GetResults()
        {
            yield return this;

            foreach (var result in results)
            {
                yield return result;
            }
        }

        public bool HasError { get { return this.Severity <= ApplicationMessageSeverity.Error; } }
        #endregion Public Methods


        #region Private Fields
        private List<ValidationResult> results;
        #endregion Private Fields

        #region Private Methods
        private static ApplicationMessageSeverity GetResultSeverity(ValidationResult validationResult)
        {
            return (validationResult is DataValidationResult)
                ? ((DataValidationResult)validationResult).Severity
                : ApplicationMessageSeverity.Error;
        }

        private static object GetResultObject(ValidationResult validationResult)
        {
            return (validationResult is DataValidationResult)
                ? ((DataValidationResult)validationResult).Object
                : null;
        }
        #endregion Private Methods
    }
}
