using NLog;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Linq;

namespace Web.Helpers
{
    public class AppSettings
    {
        private static readonly Logger _Logger = LogManager.GetCurrentClassLogger();

        public static HashSet<string> ValidProjectKeys => GetValidProjectKeys();
        public static string MediaPath => GetMediaPath();
        public static string MediaUrl => Get<string>("MediaUrl");
        public static string DefaultContentFolder => Get<string>("DefaultContentFolder");
        public static string DefaultSize => Get<string>("DefaultSize");
        public static string WebSite => Get<string>("WebSite");
        private static HashSet<string> GetValidProjectKeys()
        {
            var keys = Get<string>("ValidProjectKeys");
            return keys.Split(',').ToHashSet();
        }

        private static string GetMediaPath()
        {
            var path = Get<string>("MediaPath");
            if (!path.Contains(":"))
            {
                var folderPath = System.AppDomain.CurrentDomain.BaseDirectory;
                return $"{folderPath}{path}";
            }
            else
            {
                return path;
            }
        }

        private static T Get<T>(string key)
        {
            string setting = ConfigurationManager.AppSettings[key];
            if (setting == null)
            {
                _Logger.Error($"AppSettings key is missing:{key}");
            }
            TypeConverter converter = TypeDescriptor.GetConverter(typeof(T));
            return (T)converter.ConvertFromInvariantString(setting);
        }
    }
}