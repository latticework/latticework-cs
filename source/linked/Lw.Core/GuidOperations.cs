using System;
using System.Text.RegularExpressions;

namespace Lw
{
    public static class GuidOperations
    {
        /// <summary>
        ///     Converts the string representation of a Guid to its Guid 
        ///     equivalent. A return value indicates whether the operation 
        ///     succeeded. 
        /// </summary>
        /// <param name="s">
        ///     A string containing a Guid to convert.
        /// </param>
        /// <returns>
        ///     The Guid value equivalent to 
        ///     the Guid contained in <paramref name="s"/>, if the conversion 
        ///     succeeded, or <see langword="null"/> if the conversion failed. 
        /// </returns>
        public static Guid? TryParse(string s)
        {
            Guid result;
            bool succeeded = TryParse(s, out result);

            return (succeeded) ? result : (Guid?)null;
        }

        /// <summary>
        ///     Converts the string representation of a Guid to its Guid 
        ///     equivalent. A return value indicates whether the operation 
        ///     succeeded. 
        /// </summary>
        /// <param name="s">
        ///     A string containing a Guid to convert.
        /// </param>
        /// <param name="result">
        ///     When this method returns, contains the Guid value equivalent to 
        ///     the Guid contained in <paramref name="s"/>, if the conversion 
        ///     succeeded, or <see cref="Guid.Empty"/> if the conversion failed. 
        ///     The conversion fails if the <paramref name="s"/> parameter is 
        ///     <see langword="null" />. 
        /// </param>
        /// <value>
        ///     <see langword="true" /> if <paramref name="s"/> was converted successfully; otherwise, 
        ///     <see langword="false" />.
        /// </value>
        public static bool TryParse(string s, out Guid result)
        {
            result = Guid.Empty;

            ExceptionOperations.VerifyNonNull(s, ()=>s);

            if (!s.IsNullOrEmpty() & guidRegex.Match(s).Success)
            {
                result = new Guid(s);
                return true;
            }

            return false;
        }

        private const string guidPattern = 
                "^[A-Fa-f0-9]{32}$|" +
                "^({|\\()?[A-Fa-f0-9]{8}-([A-Fa-f0-9]{4}-){3}[A-Fa-f0-9]{12}(}|\\))?$|" +
                "^({)?[0xA-Fa-f0-9]{3,10}(, {0,1}[0xA-Fa-f0-9]{3,6}){2}, {0,1}({)([0xA-Fa-f0-9]{3,4}, {0,1}){7}[0xA-Fa-f0-9]{3,4}(}})$";

        private static Regex guidRegex = new Regex(guidPattern);
    }
}
