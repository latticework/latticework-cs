using System;
using System.Collections.Generic;
using System.Linq;

namespace Lw.Build.T4.ApplicationMessages
{
    public class MessageData
    {
        public IList<MessageArgumentData> Arguments { get; set; }
        public string BaseCode { get; set; }
        public string BaseCodeComment { get; set; }
        public string FieldName { get { return string.Concat(this.SeverityPrefix, "MessageCode", this.Name); } }
        public string FormatString { get; set; }
        public string LocalSeverityExpression { get; set; }
        public string MessageCodeValue { get; set; }
        public string Name { get; set; }
        public string PriorityExpression { get; set; }
        public string PriorityName { get; set; }
        public string ResourceName { get { return string.Concat(this.SeverityPrefix, "Message", this.Name); } }
        public string SeverityExpression { get; set; }
        public string SeverityName { get; set; }
        public string SeverityPrefix { get; set; }
        public MessageType Xml { get; set; }
    }
}

