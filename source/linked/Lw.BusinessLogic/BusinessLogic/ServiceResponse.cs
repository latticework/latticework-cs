using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using Lw.BusinessLogic;
using Lw.ComponentModel.DataAnnotations;
using Lw.ApplicationMessages;

namespace Lw.SeniorLoans.Compliance.BusinessLogic
{
    public class ServiceResponse
    {
        #region Public Constructors
        public ServiceResponse(IEnumerable<ApplicationMessage> messages = null)
        {
            if (messages is ApplicationMessageCollection)
            {
                this.Messages = (ApplicationMessageCollection)messages;
            }
            else
            {
                this.Messages = new ApplicationMessageCollection(messages);
            }
        }
        #endregion Public Constructors

        #region Public Properties
        public ApplicationMessageCollection Messages { get; private set; }
        #endregion Public Properties
    }

    public class ServiceResponse<T> : ServiceResponse
    {
        #region Public Constructors
        public ServiceResponse(T result, IEnumerable<ApplicationMessage> messages = null)
            : base(messages)
        {
            this.Result = result;
        }
        #endregion Public Constructors

        #region Public Properties
        public T Result { get; private set; }
        #endregion Public Properties
    }
}
