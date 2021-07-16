using path_watcher.Interfaces;
using path_watcher.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace path_watcher.Mocks
{
    public class BaseRepository
    {
        private ApplicationContext Context { get; set; }
        public BaseRepository(ApplicationContext context)
        {
            Context = context;
        }

        public void Create(Models.Directory model)
        {
            if(Context.Directories.FirstOrDefault(x => x.Id == model.Id) == null)
            {
                Context.Directories.Add(model);
                Context.SaveChanges();
            }
         

            
        }
        public void Create(Models.File model)
        {
            if (Context.Files.FirstOrDefault(x => x.Id == model.Id) == null)
            {
                Context.Files.Add(model);
                Context.SaveChanges();
            }
        }
        public void Create(Models.Log model)
        {
            if (Context.Logs.FirstOrDefault(x => x.Id == model.Id) == null)
            {
                Context.Logs.Add(model);
                Context.SaveChanges();
            }

        }
        public void AddToDbFile(FileInfo file, string PathRoot)
        {
            Models.File model = new Models.File();
            model.Id = Guid.NewGuid();
            model.ByteSize = file.Length.ToString();
            model.DateCreated = file.CreationTimeUtc;
            model.DateLastChanged = file.LastWriteTimeUtc;
            model.DateLastOpened = file.LastAccessTimeUtc;
            model.DateLastRenamed = file.CreationTimeUtc;
            model.FileName = file.Name;
            model.FullPath = file.FullName;
            model.Extension = file.FullName.Split('.')[1];
            Models.Directory dir = Context.Directories.FirstOrDefault(x => x.FullPath == PathRoot);
            if (dir != null)
            {
                model.DirectoryId = dir.Id;
                model.Directory = dir;
            }
            Create(model);





        }
        public void AddToDbDir(DirectoryInfo dir)
        {
            Models.Directory directory = new();
            directory.DirectoryName = dir.Name;
            directory.FullPath = dir.FullName;
            directory.Id = Guid.NewGuid();
            Create(directory);

        }
        public void AddToLog(FileInfo file, WatcherChangeTypes watcherChange, string PathRoot)
        {
            Models.File toChange;
            if (watcherChange == WatcherChangeTypes.Created)
            {
                AddToDbFile(file, PathRoot);
            }
            if (watcherChange == WatcherChangeTypes.Renamed)
            {
                toChange = Context.Files.FirstOrDefault(x => x.FullPath == PathRoot);
            } else
                toChange = Context.Files.FirstOrDefault(x => x.FullPath == file.FullName);
            if (watcherChange == WatcherChangeTypes.Deleted)
            {
                toChange.IsDeleted = true;
                Update(toChange);
                //DeleteFile(toChange.Id);
                Log log = new();
                log.Id = Guid.NewGuid();
                log.DateEvent = DateTime.UtcNow;
                log.NameEvent = watcherChange.ToString();
                log.FileId = toChange.Id;
                log.File = toChange;
                Create(log);
            }
            else
            {
                toChange.IsDeleted = false;
                string NameEvent = watcherChange.ToString();
                toChange.ByteSize = file.Length.ToString();
                toChange.DateCreated = file.CreationTimeUtc;
                toChange.DateLastChanged = file.LastWriteTimeUtc;
                toChange.FileName = file.Name;
                toChange.FullPath = file.FullName;
                var exp = file.FullName.Split('.');
                toChange.Extension = exp[^1];
                if (watcherChange == WatcherChangeTypes.Renamed)
                    toChange.DateLastRenamed = DateTime.UtcNow;
                Update(toChange);
                Log log = new();
                log.Id = Guid.NewGuid();
                log.DateEvent = DateTime.UtcNow;
                log.NameEvent = NameEvent;
                log.FileId = toChange.Id;
                log.File = toChange;
                Create(log);

            }
        }
        public void DeleteDir(Guid id)
        {
            var toDelete = Context.Directories.FirstOrDefault(x => x.Id == id);
            if (toDelete != null)
            {
                Context.Directories.Remove(toDelete);
                Context.SaveChanges();

            }

        }
        public void DeleteFile(Guid id)
        {
            var toDelete = Context.Files.FirstOrDefault(x => x.Id == id);
            if (toDelete != null)
            {
                Context.Files.Remove(toDelete);
                Context.SaveChanges();

            }

        }
        public void DeleteLog(Guid id)
        {
            var toDelete = Context.Logs.FirstOrDefault(x => x.Id == id);
            if (toDelete != null)
            {
                Context.Logs.Remove(toDelete);
                Context.SaveChanges();

            }

        }


        public List<Models.Directory> GetDirectories() => Context.Directories.ToList();
        public List<Models.File> GetFiles() => Context.Files.ToList();
        public List<Models.Log> GetLogs() => Context.Logs.ToList();


        public void Update(Models.Directory model)
        {
            var toUpdate = Context.Directories.FirstOrDefault(x => x.Id == model.Id);
            if (toUpdate != null)
            {
                toUpdate = model;
            }
            Context.Directories.Update(toUpdate);
            Context.SaveChanges();
        }
        public void Update(Models.File model)
        {
            var toUpdate = Context.Files.FirstOrDefault(x => x.Id == model.Id);
            if (toUpdate != null)
            {
                toUpdate = model;
            }
            Context.Files.Update(toUpdate);
            Context.SaveChanges();
        }
        public void Update(Models.Log model)
        {
            var toUpdate = Context.Logs.FirstOrDefault(x => x.Id == model.Id);
            if (toUpdate != null)
            {
                toUpdate = model;
            }
            Context.Logs.Update(toUpdate);
            Context.SaveChanges();
        }


    }
}
