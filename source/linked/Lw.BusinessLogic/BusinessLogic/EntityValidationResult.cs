using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace Lw.BusinessLogic
{
    public class EntityValidationResult
    {
        #region Public Constructors
        public EntityValidationResult(IList<ValidationResult> violations = null)
        {
            Violations = violations ?? new List<ValidationResult>();
        }
        #endregion Public Constructors

        #region Public Properties
        public bool HasViolations
        {
            get { return Violations.Count > 0; }
        }

        public IList<ValidationResult> Violations { get; private set; }
        #endregion Public Properties
    }
}
