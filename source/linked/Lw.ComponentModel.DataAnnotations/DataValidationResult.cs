using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using Lw.ApplicationMessages;
using System.Diagnostics.Contracts;

namespace Lw.ComponentModel.DataAnnotations
{
    public class DataValidationResult : ValidationResult
    {
        #region Public Constructors
        public DataValidationResult(ApplicationMessage message)
            : this(new[] { message })
        {
        }

        public DataValidationResult(params ApplicationMessage[] messages)
            : this((IEnumerable<ApplicationMessage>)messages)
        {
        }

        public DataValidationResult(IEnumerable<ApplicationMessage> messages)
            : base(messages.First().Message, messages.First().MemberNames)
        {
            Contract.Requires(messages != null);

            if (!messages.Any())
            {
                ExceptionOperations.ThrowArgumentException(() => messages);
            }

            var first = messages.First();

            if (messages.Min(am => am.Severity) != first.Severity)
            {
                ExceptionOperations.ThrowArgumentException(() => messages);
            }

            this.Messages = new ApplicationMessageCollection(messages);
        }
        #endregion Public Constructors

        #region Public Properties
        public ApplicationMessageCollection Messages { get; private set; }
        #endregion Public Properties
    }
}
