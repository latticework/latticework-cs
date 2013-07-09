using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text.RegularExpressions;


namespace Lw.Text.RegularExpressions
{
    public static class Extensions
    {
        #region RegEx Extensions
        [DebuggerStepThrough]
        public static IDictionary<string, Group> MatchNamed(this Regex reference, string input)
        {
            Contract.Requires<ArgumentNullException>(input != null, "input");

            var match = reference.Match(input);

            var namedCaptures = (
                        from s in reference.GetGroupNames()
                        let g = match.Groups[s]
                        where g != null && !g.Value.IsNullOrEmpty() && !s.EqualsOrdinal("0")
                        select new {Name=s, Group=g})
                .ToDictionary(ng=>ng.Name, ng=>ng.Group);

            return namedCaptures;
        }
        #endregion RegEx Extensions

        #region GroupCollection Extensions
        [DebuggerStepThrough]
        public static IList<Group> ToList(this GroupCollection reference)
        {
            List<Group> groups = new List<Group>();

            for (int groupIndex = 0; groupIndex < reference.Count; ++groupIndex)
            {
                groups.Add(reference[groupIndex]);
            }

            return groups;
        }
        #endregion GroupCollection Extensions
    }
}
