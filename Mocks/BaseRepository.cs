using Microsoft.EntityFrameworkCore;
using path_watcher.Interfaces;
using path_watcher.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Excel = Microsoft.Office.Interop.Excel;
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
        public void CreateNaNSave(Models.File model)
        {
            if (Context.Files.FirstOrDefault(x => x.Id == model.Id) == null)
            {
                Context.Files.Add(model);
                //Context.SaveChanges();
            }
        }
        public void Save()
        {
            Context.SaveChanges();
        }
        public void Create(Models.Log model)
        {
            if (Context.Logs.FirstOrDefault(x => x.Id == model.Id) == null)
            {
                Context.Logs.Add(model);
                Context.SaveChanges();
            }

        }
        public List<Models.File> getListFileModel(List<FileInfo> fileInfos, string PathRoot)
        {
            List<Models.File> files = new();
            
            Models.Directory dir = Context.Directories.FirstOrDefault(x => x.FullPath == PathRoot);
            if (dir != null)
            {
                foreach (var file in fileInfos)
                {
                    var model = new Models.File();
                    model.Id = Guid.NewGuid();
                    model.ByteSize = file.Length.ToString();
                    model.DateCreated = file.CreationTimeUtc;
                    model.DateLastChanged = file.LastWriteTimeUtc;
                    model.DateLastOpened = file.LastAccessTimeUtc;
                    model.DateLastRenamed = file.CreationTimeUtc;
                    model.FileName = file.Name;
                    model.FullPath = file.FullName;
                    model.Extension = file.FullName.Split('.')[^1];
                    model.DirectoryId = dir.Id;
                    model.Directory = dir;
                    files.Add(model);
                }

            }
            return files;
        }
        public  void Inserts<TEntity>(IEnumerable<TEntity> entities) where TEntity : class
        {
            foreach (TEntity entity in entities)
                Context.Entry(entity).State = EntityState.Added;
            Context.SaveChanges();

        }
        public void AddToDbFile(FileInfo file, string PathRoot)
        {
            if (file.Exists == true)
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
                model.Extension = file.FullName.Split('.')[^1];
                Models.Directory dir = Context.Directories.FirstOrDefault(x => x.FullPath == PathRoot);
                if (dir != null)
                {
                    model.DirectoryId = dir.Id;
                    model.Directory = dir;
                }
                Create(model);
            }

            //CreateNaNSave(model);





        }
        public void AddToDbDir(DirectoryInfo dir)
        {
            Models.Directory directory = new();
            directory.DirectoryName = dir.Name;
            directory.FullPath = dir.FullName;
            directory.Id = Guid.NewGuid();
            Create(directory);

        }
        public void AddToLog(FileInfo file, WatcherChangeTypes watcherChange, string PathRoot,string DirPath = null)
        {
            Models.File toChange;
            switch (watcherChange)
            {
                case WatcherChangeTypes.Created:
                    {
                        AddToDbFile(file, DirPath);
                        toChange = Context.Files.FirstOrDefault(x => x.FullPath == PathRoot);
                        Log log = new();
                        log.Id = Guid.NewGuid();
                        log.DateEvent = DateTime.Now;
                        log.NameEvent = watcherChange.ToString();
                        log.FileId = toChange.Id;
                        log.File = toChange;
                        Create(log);
                    }
                    break;
                case WatcherChangeTypes.Deleted:
                    {
                        toChange = Context.Files.FirstOrDefault(x => x.FullPath == PathRoot);
                        toChange.IsDeleted = true;
                        Update(toChange);
                        Log log = new();
                        log.Id = Guid.NewGuid();
                        log.DateEvent = DateTime.Now;
                        log.NameEvent = watcherChange.ToString();
                        log.FileId = toChange.Id;
                        log.File = toChange;
                        Create(log);
                    }
                    break;
                case WatcherChangeTypes.Changed:
                    {
                        toChange = Context.Files.FirstOrDefault(x => x.FullPath == PathRoot);
                        toChange.IsDeleted = false;
                        string NameEvent = watcherChange.ToString();
                        toChange.ByteSize = file.Length.ToString();
                        toChange.DateCreated = file.CreationTimeUtc;
                        toChange.DateLastChanged = file.LastWriteTimeUtc;
                        toChange.FileName = file.Name;
                        toChange.FullPath = file.FullName;
                        var exp = file.FullName.Split('.');
                        toChange.Extension = exp[^1];
                        Update(toChange);
                        Log log = new();
                        log.Id = Guid.NewGuid();
                        log.DateEvent = DateTime.Now;
                        log.NameEvent = NameEvent;
                        log.FileId = toChange.Id;
                        log.File = toChange;
                        Create(log);
                    }
                    break;
                case WatcherChangeTypes.Renamed:
                    {
                        toChange = Context.Files.FirstOrDefault(x => x.FullPath == PathRoot);
                        toChange.IsDeleted = false;
                        toChange.ByteSize = file.Length.ToString();
                        toChange.DateCreated = file.CreationTimeUtc;
                        toChange.DateLastChanged = file.LastWriteTimeUtc;
                        toChange.FileName = file.Name;
                        toChange.FullPath = file.FullName;
                        var exp = file.FullName.Split('.');
                        toChange.Extension = exp[^1];
                        toChange.DateLastRenamed = DateTime.Now;
                        //Update(toChange);
                        Log log = new();
                        log.Id = Guid.NewGuid();
                        log.DateEvent = DateTime.Now;
                        log.NameEvent = watcherChange.ToString(); 
                        log.FileId = toChange.Id;
                        log.File = toChange;
                        Create(log);
                        
                    }
                    break;
                default:
                    break;
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
        public void DeleteLogs()
        {


            foreach (var item in Context.Logs.ToList())
            {
                Context.Logs.Remove(item);

            }
                Context.SaveChanges();


        }

        public void ExportToExcel()
        {
            Excel.Application ex = new();
            ex.Visible = true;
            ex.SheetsInNewWorkbook = 1;
            Excel.Workbook workBook = ex.Workbooks.Add(Type.Missing);
            ex.DisplayAlerts = false;
            Excel.Worksheet sheet = (Excel.Worksheet)ex.Worksheets.get_Item(1);
            sheet.Name = $"Logs for {DateTime.UtcNow.ToShortDateString()}";
            sheet.Columns.ColumnWidth = 20;
            sheet.Cells[1, 1] = "Date";
            sheet.Cells[1, 2] = "Event";
            sheet.Cells[1, 3] = "Path";
            var logs = GetLogs().ToArray();
            for (int i = 0; i < logs.Length; i++)
            {
                sheet.Cells[i + 2,1] = logs[i].DateEvent;
                sheet.Cells[i + 2,2] = logs[i].NameEvent;
                sheet.Cells[i + 2,3] = GetFile(logs[i].FileId).FullPath;

            }
            

        }
        public List<Models.Directory> GetDirectories() => Context.Directories.ToList();
        public List<Models.File> GetFiles() => Context.Files.ToList();
        public List<Models.Log> GetLogs() => Context.Logs.ToList();




        //public List<Models.File> GetFilesByFilename(string name) => Context.Files.Where(f => EF.Functions.FreeText(f.FileName, name)).ToList();
        public List<Models.File> GetFilesByFilename(string name) => Context.Files.Where(f => f.FileName.Contains(name)).ToList();

        public Models.File GetFile(Guid id)
        {
            var model = Context.Files.FirstOrDefault(x => x.Id == id);
            if (model != null) return model;
            else return null;
        }
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


