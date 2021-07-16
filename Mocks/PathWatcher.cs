using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using path_watcher.Interfaces;
using path_watcher.Models;

namespace path_watcher.Mocks
{
    public class PathWatcher
    {
        public FileSystemWatcher Watcher;
        ApplicationContext Context;
        private BaseRepository db;

        public PathWatcher(string FullPath)
        {
            Context = new ApplicationContext();
            db = new BaseRepository(Context);
            Watcher = new FileSystemWatcher(FullPath);
            
            Watcher.Filter = "*.*";
            Watcher.InternalBufferSize = 16384;
            Watcher.NotifyFilter =
                                 NotifyFilters.DirectoryName
                                 | NotifyFilters.FileName
                                 | NotifyFilters.CreationTime
                                 | NotifyFilters.LastWrite;
            Watcher.Changed += OnChanged;
            Watcher.Created += OnCreated;
            Watcher.Deleted += OnDeleted;
            Watcher.Renamed += OnRenamed;
            Watcher.IncludeSubdirectories = true;
            Watcher.EnableRaisingEvents = true;

        }

        private  void OnChanged(object sender, FileSystemEventArgs e)
        {
            if (e.Name.Split('.')[^1] != "tmp")
            {
                FileInfo file = new FileInfo(e.FullPath);
                db.AddToLog(file, e.ChangeType, Watcher.Path);
            }

                
            
        }

        private  void OnCreated(object sender, FileSystemEventArgs e)
        {
            if (e.Name.Split('.')[^1] != "tmp")
            {
                FileInfo file = new FileInfo(e.FullPath);
                db.AddToLog(file, e.ChangeType, Watcher.Path);
            }
        }

        private  void OnDeleted(object sender, FileSystemEventArgs e)
        {
            if (e.Name.Split('.')[^1] != "tmp")
            {
                FileInfo file = new FileInfo(e.FullPath);
                db.AddToLog(file, e.ChangeType, Watcher.Path);
            }
        }
            

        private  void OnRenamed(object sender, RenamedEventArgs e)
        {
            if (e.Name.Split('.')[^1] != "tmp")
            {
                FileInfo file = new FileInfo(e.FullPath);
                db.AddToLog(file, e.ChangeType, e.OldFullPath);
            }

        }

    }
}

