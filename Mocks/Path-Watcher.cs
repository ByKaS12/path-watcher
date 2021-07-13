using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace path_watcher.Mocks
{
    public class SuperVisor
    {
        private List<PathWatcher> Watchers;
        public SuperVisor()
        {
            Watchers = new List<PathWatcher>();
        }
        public void AddWatcher(string FullPath) => Watchers.Add(new PathWatcher(FullPath));
        public void DeleteWatcher(string FullPath) => Watchers.Remove(new PathWatcher(FullPath));
        public FileSystemWatcher GetWatcher(string FullPath) => Watchers.FirstOrDefault(x => x.Path == FullPath);
        public List<PathWatcher> GetAllWatchers() => Watchers;
    }
}
