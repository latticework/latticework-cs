using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;


namespace Lw.ApplicationMessages
{
#if !NETFX_CORE
    [Serializable]
#endif
    public class ApplicationMessageCollection : ObservableCollection<ApplicationMessage>
    {
        public ApplicationMessageCollection() : base() { }

        public ApplicationMessageCollection(IEnumerable<ApplicationMessage> messages) : base(messages.ToList()) { }

        public override string ToString()
        {
            return this
                .Select(am => "{0} {1X12}:{3}{4}".DoFormat(
                    am.Severity, am.MessageCode, Environment.NewLine, am.Message))
                .Join(Environment.NewLine + Environment.NewLine);
        }

        public IEnumerable<ApplicationMessage> Errors
        {
            get { return this.Where(am => am.Severity == ApplicationMessageSeverity.Error); }
        }

        public bool HasErrors
        {
            get { return MeetsThreshold(ApplicationMessageSeverity.Error); }
        }

        public bool MeetsThreshold(ApplicationMessageSeverity severity)
        {
            if (severity < ApplicationMessageSeverity.Error)
            {
                throw new ArgumentOutOfRangeException("severity");
            }

            return this.Any(am => am.MeetsThreshold(severity));
        }

        public IEnumerable<ApplicationMessage> Warnings
        {
            get { return this.Where(am => am.Severity == ApplicationMessageSeverity.Warning); }
        }
    }
}
