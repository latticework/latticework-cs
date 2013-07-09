using Lw.Collections.Generic;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;

namespace Lw.Configuration
{
    // TODO: SettingsPath -- Test for valid characters in paths!
    public class SettingsPath
    {
        public SettingsPath()
            : this(default(SettingsRoot), (IEnumerable<string>)null, null)
        {
        }

        public SettingsPath(SettingsRoot root, params string[] pathsAndName)
        {
            Contract.Requires<ArgumentOutOfRangeException>(root.IsDefined(), "root");
            Contract.Requires<ArgumentNullException>(pathsAndName != null, "pathsAndName");
            Contract.Requires<ArgumentException>(pathsAndName.Any(), "pathsAndName");


            this.Root = root;
            this.Paths = new List<string>(pathsAndName.Take(pathsAndName.Count() - 1));
            this.Name = pathsAndName.Last();
        }

        public SettingsPath(SettingsRoot root, IEnumerable<string> paths, string name)
        {
            Contract.Requires<ArgumentOutOfRangeException>(root.IsDefined(), "root");

            this.Root = root;
            this.Paths = new List<string>(paths ?? Enumerable.Empty<string>());
            this.Name = name;
        }

        public const char PathSeparatorChar = '/';
        public const string PathStart = "//";
        public const string RootApp = "App";
        public const string RootLocal = "Local";
        public const string RootRoaming = "Roaming";
        public const string RootSystem = "System";
        public const string RootTemp = "Temp";

        public IList<string> Paths { get; private set; }

        public SettingsRoot Root { get; set; }

        public string QualifiedPath
        {
            get { return SettingsPath.JoinPath(this.Root, this.Paths.Append(this.Name)); }
        }

        public string Name { get; set; }


        public static string Combine(params string[] paths)
        {
            return SettingsPath.Combine((IEnumerable<string>)paths);
        }

        public static string Combine(IEnumerable<string> paths)
        {
            var trimmedPaths = paths.Select(p => p.Trim(SettingsPath.PathSeparatorChar));
            return string.Join(new string(SettingsPath.PathSeparatorChar, 1), trimmedPaths);
        }

        public static SettingsPath Parse(string qualifiedPath)
        {
            Contract.Requires<ArgumentNullException>(qualifiedPath != null, "qualifiedPath");
            Contract.Requires<ArgumentException>(qualifiedPath.Length > 0, "qualifiedPath");

            SettingsPath path;
            if (!SettingsPath.TryParse(qualifiedPath, out path))
            {
                // TODO: -- Robust error messages
                throw new FormatException();
            }
            return path;
        }
        public static bool TryParse(string qualifiedPath, out SettingsPath path)
        {
            path = null;
            if (qualifiedPath.IsNullOrEmpty()) { return false; }

            var isRooted = qualifiedPath.StartsWith(SettingsPath.PathStart);

            var stringToSplit = (isRooted)
                ? qualifiedPath
                : qualifiedPath.Substring(SettingsPath.PathStart.Length);

            var splitPaths = stringToSplit.Split(SettingsPath.PathSeparatorChar);

            var paths = splitPaths.Take(splitPaths.Count() - 1);

            paths = (isRooted)
                ? paths.Skip(1)
                : paths;

            SettingsRoot root;
            if (isRooted)
            {
                if (!EnumOperations.TryParse<SettingsRoot>(splitPaths.First(), out root))
                { 
                    return false;
                }
            }
            else
            {
                root = default(SettingsRoot);
            }

            var name = splitPaths.Last();

            path = new SettingsPath(root, paths, name);

            return true;
        }

        public static string JoinPath(SettingsRoot root, params string[] paths)
        {
            return JoinPath(root, (IEnumerable<string>)paths);
        }

        public static string JoinPath(SettingsRoot root, IEnumerable<string> paths)
        {
            var rootPath = string.Concat(SettingsPath.PathStart, root.GetName());
            return SettingsPath.Combine((new[] { rootPath }).Concat(paths));
        }

        public override string ToString()
        {
            return this.QualifiedPath;
        }

        public bool IsValid
        {
            get { return this.Root.IsDefined() && !this.Name.IsNullOrEmpty(); }
        }
    }
}
