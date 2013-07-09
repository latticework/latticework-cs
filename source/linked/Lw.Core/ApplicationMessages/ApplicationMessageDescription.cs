using System.Collections.Generic;

namespace Lw.ApplicationMessages
{
    public class ApplicationMessageDescription
    {
        public string MessageFormat { get; set; }
        public IList<MessageArgumentDescription> Arguments { get; set; }
    }
}
