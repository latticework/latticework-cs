using Lw.ApplicationMessages;
using System.Collections.Generic;

namespace Lw.Diagnostics
{
    public class LogMessages : IEnumerable<ApplicationMessage>
    {
        public virtual void Add(ApplicationMessage item)
        {
            messages.Add(item);
        }

        public IEnumerator<ApplicationMessage> GetEnumerator()
        {
            return messages.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        protected ApplicationMessageCollection Messages
        {
            get { return messages; }
        }

        private ApplicationMessageCollection messages = new ApplicationMessageCollection { };
    }
}
