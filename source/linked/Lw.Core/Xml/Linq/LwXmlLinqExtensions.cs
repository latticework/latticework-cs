using System.Xml;
using System.Xml.Linq;

namespace Lw.Xml.Linq
{
    public static class Extensions
    {
        #region XObject Extensions
        public static bool HasLineInfo(this XObject reference)
        {
            return ((IXmlLineInfo)reference).HasLineInfo();
        }

        public static int LineNumber(this XObject reference)
        {
            return ((IXmlLineInfo)reference).LineNumber;
        }

        public static int LinePosition(this XObject reference)
        {
            return ((IXmlLineInfo)reference).LinePosition;
        }
        #endregion XObject Extensions
    }
}
