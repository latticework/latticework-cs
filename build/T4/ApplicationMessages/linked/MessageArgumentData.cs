using System;
using System.Linq;

namespace Lw.Build.T4.ApplicationMessages
{
    public class MessageArgumentData
    {
        public string ArgumentType { get; set; }
        public string Description { get; set; }
        public int Index { get; set; }
        public string Name { get; set; }
        public MessageArgumentType Xml { get; set; }
    }
}

