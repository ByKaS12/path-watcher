using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using path_watcher.Mocks;
using path_watcher.Models;
using System.Diagnostics;

namespace path_watcher.Static
{
    public static class SuperVisor
    {
        private static List<PathWatcher> Watchers;
        
        static SuperVisor()
        {
            Watchers = new List<PathWatcher>();

            // TODO: compare all files to db history
            foreach (string path in Config.GetStringArray("paths"))
            {
                AddWatcher(path);
            }
        }

        public static void AddWatcher(string FullPath) => Watchers.Add(new PathWatcher(FullPath));
        public static void DeleteWatcher(string FullPath)
        {
            int index = Watchers.FindIndex(w => w.Watcher.Path == FullPath);
            Watchers[index].Watcher.Dispose();
            Watchers.RemoveAt(index);
        }
        public static PathWatcher GetWatcher(string FullPath) => Watchers.FirstOrDefault(x => x.Watcher.Path == FullPath);
        public static List<PathWatcher> GetAllWatchers() => Watchers;
    }
}
