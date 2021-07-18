using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace path_watcher.Static
{
    public static class Config
    {
        public static object Get(string key)
        {
            return Properties.Settings.Default[key];
        }

        public static void Set(string key, object value)
        {
            Properties.Settings.Default[key] = value;
            Properties.Settings.Default.Save();
        }

        public static string[] GetStringArray(string key)
        {
            return (Get(key) as string).Split(Get("separator") as string, StringSplitOptions.RemoveEmptyEntries);
        }

        public static void SetStringArray(string key, string[] value)
        {
            Set(key, String.Join(Get("separator") as string, value));
        }
    }
}
