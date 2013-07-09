using System;
using System.Collections.Generic;
using System.Linq;

namespace Lw.Build.T4.ApplicationMessages
{
    public class MessageSourceData
    {
        public string AuthorityName { get; set; }
        public string ClassName { get; set; }
        public string Description { get; set; }
        public string DomainCodeType { get; set; }
        public string DomainCodeValue { get; set; }
        public string LibraryCodeType { get; set; }
        public string LibraryCodeValue { get; set; }
        public IList<MessageData> Messages { get; set; }
        public string NamespaceName { get; set; }
        public string MessageCodePrefix { get; set; }
        public long MessageCodePrefixValue { get; set; }
        public bool UseLocalSeverityCodes { get; set; }
        public MessageSourceType Xml { get; set; }
    }
}

