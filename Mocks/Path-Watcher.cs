using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace path_watcher.Mocks
{
    public class Path_Watcher
    {
        private List<FileSystemWatcher> Watchers;
        public Path_Watcher()
        {
            Watchers = new List<FileSystemWatcher>();
        }
        public void AddWatcher(string FullPath) => Watchers.Add(new FileSystemWatcher(FullPath));
        public void DeleteWatcher(string FullPath) => Watchers.Remove(new FileSystemWatcher(FullPath));
        public FileSystemWatcher GetWatcher(string FullPath) => Watchers.FirstOrDefault(x => x.Path == FullPath);
        public List<FileSystemWatcher> GetAllWatchers() => Watchers;
    }
}
