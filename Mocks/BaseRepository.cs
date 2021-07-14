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
            Context.Set<Models.Log>().Add(model);
            Context.SaveChanges();

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
            model.Expansion = file.FullName.Split('.')[1];
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
            directory.DateCreated = dir.CreationTimeUtc;
            directory.DirectoryName = dir.Name;
            directory.FullPath = dir.FullName;
            directory.Id = Guid.NewGuid();
            Create(directory);

        }

        //public void Delete(Guid id)
        //{
        //    var toDelete = Context.Set<TDbModel>().FirstOrDefault(m => m.Id == id);
        //    Context.Set<TDbModel>().Remove(toDelete);
        //    Context.SaveChanges();
        //}

        //public List<TDbModel> GetAll()
        //{
        //    return Context.Set<TDbModel>().ToList();
        //}

        //public TDbModel Update(TDbModel model)
        //{
        //    var toUpdate = Context.Set<TDbModel>().FirstOrDefault(m => m.Id == model.Id);
        //    if (toUpdate != null)
        //    {
        //        toUpdate = model;
        //    }
        //    Context.Update(toUpdate);
        //    Context.SaveChanges();
        //    return toUpdate;
        //}

        //public TDbModel Get(Guid id)
        //{
        //    return Context.Set<TDbModel>().FirstOrDefault(m => m.Id == id);
        //}

        //public Directory GetDir(string Path)
        //{
        //    return Context.Set<Directory>().FirstOrDefault(m => m.FullPath == Path);
        //}

    }
}
