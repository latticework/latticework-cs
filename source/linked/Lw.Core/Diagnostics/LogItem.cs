using Lw.ApplicationMessages;
using Lw.Collections.Generic;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Lw.Diagnostics
{
#if !NETFX_CORE
    [Serializable]
#endif
    public class LogItem
    {
        #region Public Constructors
        public LogItem()
        {
        }

        public LogItem(ApplicationMessage message)
            : this(new ApplicationMessageCollection { message })
        {
        }

        public LogItem(IEnumerable<ApplicationMessage> messages)
            : this()
        {
            this.Messages = 
                messages as ApplicationMessageCollection ?? new ApplicationMessageCollection(messages);
        }
        #endregion Public Constructors

        #region Public Operators
        public static implicit operator LogItem(ApplicationMessage message)
        {
            return new LogItem(message);
        }
        #endregion Public Operators

        #region Public Properties
        public Guid? ActivityId { get; set; }

        public LifetimeEventType LifetimeEventType { get; set; }

        public IList<string> LogCategories
        {
            get
            {
                var logCategories = new List<string>(
                    Operations.InitializeIfNull(ref explicitLogCategories));

                logCategories.AddRange(StaticLogCategories);

                return logCategories;
            }
            set
            {
                explicitLogCategories = (value == null)
                    ? new List<string>()
                    : new List<string>(value);
            }
        }

        public ApplicationMessageCollection Messages
        {
            get
            {
                return Operations.InitializeIfNull(ref messages);
            }
            set
            {
                if (value.IsNullOrEmpty())
                {
                    severity = explicitSeverity;
                    messages = new ApplicationMessageCollection { };
                }
                else
                {
                    UpdatePriorityAndSeverity(value);
                    messages = new ApplicationMessageCollection(value);
                }


                messages.CollectionChanged += (s, e) => UpdatePriorityAndSeverity(messages);
            }
        }

        public string Message
        {
            get
            {
                return string.Concat(
                    message ?? string.Empty,
                    (!message.IsNullOrEmpty() && !messages.IsNullOrEmpty()) 
                        ? Environment.NewLine + 
                            Properties.Resources.LogItem_Message_ApplicationMessages + 
                            Environment.NewLine
                        : string.Empty,
                    messages.GetValueOrDefault(m => m.ToString(), string.Empty));
            }
            set
            {
                message = value;
            }
        }

        public ApplicationMessagePriority Priority
        {
            get { return priority; }
            set
            {
                if (value == ApplicationMessagePriority.Default)
                {
                    throw new ArgumentOutOfRangeException("value");
                }

                explicitPriority = value;

                UpdatePriorityAndSeverity(Messages);
            }
        }

        public ApplicationMessageSeverity Severity
        {
            get { return severity; }
            set
            {
                if (value == ApplicationMessageSeverity.None)
                {
                    throw new ArgumentOutOfRangeException("value");
                }

                explicitSeverity = value;

                UpdatePriorityAndSeverity(Messages);
            }
        }

        public DateTime Timestamp
        {
            get
            {
                return (DateTime)Operations.InitializeIfNull(ref timestamp, () => DateTime.UtcNow);
            }
            set
            {
                timestamp = value;
            }
        }

        public string Title { get; set; }
        #endregion Public Properties

        #region Internal Properties
        internal List<string> StaticLogCategories
        {
            get
            {
                return Operations.InitializeIfNull(ref staticLogCategories);
            }
            set
            {
                staticLogCategories = value;
            }
        }
        #endregion Internal Properties

        #region Private Methods
        private void Initialize()
        {
            this.LogCategories = new List<string> { };
        }

        private void UpdatePriorityAndSeverity(IEnumerable<ApplicationMessage> value)
        {
            if (value.Count() == 0) 
            {
                priority = explicitPriority;
                severity = explicitSeverity;
                return; 
            }

            ApplicationMessageSeverity maxSeverity = value.Min(am => am.Severity);

            if (explicitSeverity == ApplicationMessageSeverity.None)
            {
                severity = maxSeverity;
            }
            else if (maxSeverity == ApplicationMessageSeverity.None)
            {
                severity = explicitSeverity;
            }
            else
            {
                severity = (ApplicationMessageSeverity)Math.Min((int)explicitSeverity, (int)maxSeverity);
            }


            ApplicationMessagePriority maxPriority = value.Min(am => am.Priority);

            if (explicitPriority == ApplicationMessagePriority.Default)
            {
                priority = maxPriority;
            }
            else if (maxPriority == ApplicationMessagePriority.Default)
            {
                priority = explicitPriority;
            }
            else
            {
                priority = (ApplicationMessagePriority)Math.Min((int)explicitPriority, (int)maxPriority);
            }
        }
        #endregion Private Methods

        #region Private Fields
        private ApplicationMessageCollection messages = null;
        private string message = null;
        private DateTime? timestamp;

        private ApplicationMessagePriority priority = ApplicationMessagePriority.Default;
        private ApplicationMessagePriority explicitPriority = ApplicationMessagePriority.Default;

        private ApplicationMessageSeverity severity = ApplicationMessageSeverity.None;
        private ApplicationMessageSeverity explicitSeverity = ApplicationMessageSeverity.None;

        private List<string> staticLogCategories;
        private List<string> explicitLogCategories;
        #endregion Private Fields
    }
}
