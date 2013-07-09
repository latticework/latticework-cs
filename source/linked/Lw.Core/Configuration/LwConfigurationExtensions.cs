using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Lw.Configuration
{
    // TODO: Lw.Configuration.Extensions -- Rebust error messages
    public static class Extensions
    {
        #region ISettingsDb Extensions
        public static bool? GetBoolean(this ISettingsDb db, string qualifiedPath)
        {
            Contract.Requires<ArgumentNullException>(qualifiedPath != null, "qualifiedPath");

            return Extensions.GetValueCore(db, qualifiedPath, Extensions.GetBoolean);
        }

        public static bool? GetBoolean(this ISettingsDb db, SettingsPath path)
        {
            Contract.Requires<ArgumentNullException>(path != null, "path");

            return Extensions.GetValueCore(db, path, Extensions.GetBoolean);
        }

        [Pure]
        public static bool? GetBoolean(this ISettingsDb db,
            SettingsRoot root, IEnumerable<string> paths, string name)
        {
            Contract.Requires<ArgumentOutOfRangeException>(root.IsDefined(), "root");
            Contract.Requires<ArgumentNullException>(name != null, "name");

            return (bool?)db.GetValue(root, paths, name, SettingDataType.Boolean);
        }


        public static DateTime? GetDateTime(this ISettingsDb db, string qualifiedPath)
        {
            Contract.Requires<ArgumentNullException>(qualifiedPath != null, "qualifiedPath");

            var path = Extensions.ParsePath(qualifiedPath);


            return Extensions.GetDateTime(db, path.Root, path.Paths, path.Name);
        }

        public static DateTime? GetDateTime(this ISettingsDb db, SettingsPath path)
        {
            Contract.Requires<ArgumentNullException>(path != null, "path");

            if (!path.IsValid) { throw new ArgumentException("Invalid path", "path"); }


            return Extensions.GetDateTime(db, path.Root, path.Paths, path.Name);
        }

        public static DateTime? GetDateTime(this ISettingsDb db,
            SettingsRoot root, IEnumerable<string> path, string name)
        {
            return (DateTime?)db.GetValue(root, path, name, SettingDataType.DateTime);
        }


        public static decimal? GetDecimal(this ISettingsDb db, string qualifiedPath)
        {
            return (decimal?)Extensions.GetValueCore(db, qualifiedPath, SettingDataType.Decimal);
        }

        public static TimeSpan? GetDuration(this ISettingsDb db, string qualifiedPath)
        {
            return (TimeSpan?)Extensions.GetValueCore(db, qualifiedPath, SettingDataType.Duration);
        }

        public static int? GetInteger(this ISettingsDb db, string qualifiedPath)
        {
            return (int?)Extensions.GetValueCore(db, qualifiedPath, SettingDataType.Integer);
        }

        public static Task<Stream> GetStreamAsync(this ISettingsDb db, string qualifiedPath)
        {
            Contract.Requires<ArgumentNullException>(qualifiedPath != null, "qualifiedPath");

            var builder = Extensions.ParsePath(qualifiedPath);

            Func<Task<Stream>> func = async () =>
            {
                return await GetValueCoreAsync(db, builder, SettingDataType.Stream);
            };

            return func();
        }

        public static string GetString(this ISettingsDb db, string qualifiedPath)
        {
            return (string)Extensions.GetValueCore(db, qualifiedPath, SettingDataType.String);
        }

        public static Task<String> GetStringAsync(this ISettingsDb db, string qualifiedPath)
        {
            Contract.Requires<ArgumentNullException>(qualifiedPath != null, "qualifiedPath");
            
            var builder = Extensions.ParsePath(qualifiedPath);

            Func<Task<String>> func = async () => 
            {
                var stream = await GetValueCoreAsync(db, builder, SettingDataType.Stream);
                return await new StreamReader(stream, Encoding.UTF8).ReadToEndAsync();
            };

            return func();
        }

        public static Task<StreamReader> GetStringReaderAsync(this ISettingsDb db, string qualifiedPath)
        {
            Contract.Requires<ArgumentNullException>(qualifiedPath != null, "qualifiedPath");

            var builder = Extensions.ParsePath(qualifiedPath);

            Func<Task<StreamReader>> func = async () =>
            {
                var stream = await GetValueCoreAsync(db, builder, SettingDataType.Stream);
                return new StreamReader(stream);
            };

            return func();
        }


        public static object GetValue(this ISettingsDb db, string qualifiedPath)
        {
            Contract.Requires<ArgumentNullException>(qualifiedPath != null, "qualifiedPath");
            Contract.Requires<ArgumentException>(qualifiedPath.Length > 0, "qualifiedPath");

            return Extensions.GetValueCore(db, qualifiedPath, Extensions.GetValue);
        }

        public static bool? GetValue(this ISettingsDb db, SettingsPath path)
        {
            Contract.Requires<ArgumentNullException>(path != null, "path");
            Contract.Requires<ArgumentException>(path.IsValid, "path");

            return Extensions.GetValueCore(db, path, Extensions.GetValue);
        }

        public static bool? GetValue(this ISettingsDb db,
            SettingsRoot root, IEnumerable<string> paths, string name)
        {
            Contract.Requires<ArgumentOutOfRangeException>(root.IsDefined(), "root");
            Contract.Requires<ArgumentNullException>(name != null, "name");
            Contract.Requires<ArgumentNullException>(name.Length > 0, "name");

            return (bool?)db.GetValue(root, paths, name, SettingDataType.Any);
        }

        public static void SetBoolean(this ISettingsDb db, string qualifiedPath, bool? value)
        {
            Extensions.SetValueCore(db, qualifiedPath, SettingDataType.DateTime, value);
        }

        public static void SetDateTime(this ISettingsDb db, string qualifiedPath, DateTime? value)
        {
            Extensions.SetValueCore(db, qualifiedPath, SettingDataType.DateTime, value);
        }

        public static void SetDuration(this ISettingsDb db, string qualifiedPath, TimeSpan? value)
        {
            Extensions.SetValueCore(db, qualifiedPath, SettingDataType.Duration, value);
        }

        public static void SetDecimal(this ISettingsDb db, string qualifiedPath, decimal? value)
        {
            Extensions.SetValueCore(db, qualifiedPath, SettingDataType.Decimal, value);
        }

        public static void SetInteger(this ISettingsDb db, string qualifiedPath, int? value)
        {
            Extensions.SetValueCore(db, qualifiedPath, SettingDataType.Integer, value);
        }

        public static void SetString(this ISettingsDb db, string qualifiedPath, string value)
        {
            Extensions.SetValueCore(db, qualifiedPath, SettingDataType.String, value);
        }

        public static Task SetValueAsync(this ISettingsDb db, string qualifiedPath, Stream value)
        {
            return Extensions.SetValueCoreAsync(db, qualifiedPath, SettingDataType.String, value);
        }

        public static Task SetValueAsync(this ISettingsDb db, string qualifiedPath, string value)
        {
            var encoding = new UTF8Encoding(encoderShouldEmitUTF8Identifier: false, throwOnInvalidBytes: true);
            var stream = new MemoryStream(encoding.GetBytes(value), writable: false);

            return Extensions.SetValueCoreAsync(db, qualifiedPath, SettingDataType.String, stream);
        }
        #endregion ISettingsDb Extensions

        #region ISettingsDbProvider Extensions
        public static ISettingsDb GetDb(this ISettingsDbProvider reference)
        {
            return reference.GetDb(reference.GetDescription().GetNewestVersion());
        }
        public static Task UpgradeAsync(this ISettingsDbProvider reference, int installedVersion)
        {
            return reference.UpgradeAsync(installedVersion, reference.GetDescription().GetNewestVersion());
        }
        #endregion ISettingsDbProvider Extensions


        #region Private Methods
        private static SettingsPath ParsePath(string qualifiedPath)
        {
            var builder = SettingsPath.Parse(qualifiedPath);

            if (builder.Root == default(SettingsRoot))
            {
                // TODO: Lw.Configuration.Extensions.GetBuilder - robust error messages
                throw new ArgumentException();
            }

            return builder;
        }

        private static async Task<Stream> GetValueCoreAsync(
            ISettingsDb db, SettingsPath builder, SettingDataType valueType)
        {
            return await db.GetValueAsync(builder.Root, builder.Paths, builder.Name, valueType);
        }

        //////////////////////////////////////////////////
        // Created this method to make methods that depend on the signature to compile. Not sure of the intended logic...
        //////////////////////////////////////////////////
        private static object GetValueCore(
            ISettingsDb db, 
            string qualifiedPath,
            SettingDataType valueType)
        {
            var path = Extensions.ParsePath(qualifiedPath);

            return db.GetValue(path.Root, path.Paths, path.Name, valueType);
        }


        private static T GetValueCore<T>(
            ISettingsDb db, 
            string qualifiedPath, 
            Func<ISettingsDb, SettingsRoot, IEnumerable<string>, string, T> getValueType)
        {
            var path = Extensions.ParsePath(qualifiedPath);

            return (T)db.GetValue(path.Root, path.Paths, path.Name, SettingDataType.Boolean);
            //return getValueType(db, path.Root, path.Paths, path.Name);
        }

        private static T GetValueCore<T>(
            ISettingsDb db,
            SettingsPath path,
            Func<ISettingsDb, SettingsRoot, IEnumerable<string>, string, T> getValueType)
        {
            if (!path.IsValid) { throw new ArgumentException("Invalid path", "path"); }


            return getValueType(db, path.Root, path.Paths, path.Name);
        }



        private static void SetValueCore(ISettingsDb db, string qualifiedPath, SettingDataType valueType, object value)
        {
            var builder = Extensions.ParsePath(qualifiedPath);
            db.SetValue(builder.Root, builder.Paths, builder.Name, valueType, value);
        }

        private static async Task SetValueCoreAsync(
            ISettingsDb db, string qualifiedPath, SettingDataType valueType, Stream value)
        {
            var builder = Extensions.ParsePath(qualifiedPath);
            await db.SetValueAsync(builder.Root, builder.Paths, builder.Name, valueType, value);
        }
        #endregion Private Methods
    }
}
