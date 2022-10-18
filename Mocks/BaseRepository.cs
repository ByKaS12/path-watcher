using Microsoft.EntityFrameworkCore;
using path_watcher.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
            if (Context.Directories.FirstOrDefault(x => x.Id == model.Id) == null)
            {
                _ = Context.Directories.Add(model);
                _ = Context.SaveChanges();
            }



        }
        public void Create(Models.File model)
        {
            if (Context.Files.FirstOrDefault(x => x.Id == model.Id) == null)
            {
                _ = Context.Files.Add(model);
                _ = Context.SaveChanges();
            }
        }
        public void CreateNaNSave(Models.File model)
        {
            if (Context.Files.FirstOrDefault(x => x.Id == model.Id) == null)
            {
                _ = Context.Files.Add(model);
                _ = Context.SaveChanges();
            }
        }
        public void Save()
        {
            _ = Context.SaveChanges();
        }
        public void Create(Models.Log model)
        {
            if (Context.Logs.FirstOrDefault(x => x.Id == model.Id) == null)
            {
                _ = Context.Logs.Add(model);
                _ = Context.SaveChanges();
            }

        }
        public List<Models.File> getListFileModel(List<FileInfo> fileInfos, string PathRoot)
        {
            List<Models.File> files = new();

            Models.Directory dir = Context.Directories.FirstOrDefault(x => x.FullPath == PathRoot);
            if (dir != null)
            {
                foreach (FileInfo file in fileInfos)
                {
                    Models.File model = new()
                    {
                        Id = Guid.NewGuid(),
                        ByteSize = file.Length.ToString(),
                        DateCreated = file.CreationTimeUtc,
                        DateLastChanged = file.LastWriteTimeUtc,
                        DateLastOpened = file.LastAccessTimeUtc,
                        DateLastRenamed = file.CreationTimeUtc,
                        FileName = file.Name,
                        FullPath = file.FullName,
                        Extension = file.FullName.Split('.')[^1],
                        DirectoryId = dir.Id,
                        Directory = dir
                    };
                    files.Add(model);
                }

            }
            return files;
        }
        public void Inserts<TEntity>(IEnumerable<TEntity> entities) where TEntity : class
        {

            foreach (TEntity entity in entities)
            {
                Context.Entry(entity).State = EntityState.Added;
            }

            _ = Context.SaveChanges();

        }
        public void AddToDbFile(FileInfo file, string PathRoot)
        {
            if (file.Exists == true)
            {
                Models.File model = new()
                {
                    Id = Guid.NewGuid(),
                    ByteSize = file.Length.ToString(),
                    DateCreated = file.CreationTimeUtc,
                    DateLastChanged = file.LastWriteTimeUtc,
                    DateLastOpened = file.LastAccessTimeUtc,
                    DateLastRenamed = file.CreationTimeUtc,
                    FileName = file.Name,
                    FullPath = file.FullName,
                    Extension = file.FullName.Split('.')[^1]
                };
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
            Models.Directory directory = new()
            {
                DirectoryName = dir.Name,
                FullPath = dir.FullName,
                Id = Guid.NewGuid()
            };
            Create(directory);

        }
        public void AddToLog(FileInfo file, WatcherChangeTypes watcherChange, string PathRoot, string DirPath = null)
        {
            Models.File toChange;
            switch (watcherChange)
            {
                case WatcherChangeTypes.Created:
                    {
                        try
                        {
                            AddToDbFile(file, DirPath);
                            toChange = Context.Files.FirstOrDefault(x => x.FullPath == PathRoot);
                            if (toChange == null)
                            {
                                return;
                            }

                            Log log = new()
                            {
                                Id = Guid.NewGuid(),
                                DateEvent = DateTime.Now,
                                NameEvent = watcherChange.ToString(),
                                FileId = toChange.Id,
                                File = toChange
                            };
                            Create(log);
                        }
                        catch (Exception) { }

                    }
                    break;
                case WatcherChangeTypes.Deleted:
                    {
                        try
                        {
                            toChange = Context.Files.FirstOrDefault(x => x.FullPath == PathRoot);
                            if (toChange == null)
                            {
                                return;
                            }

                            toChange.IsDeleted = true;
                            Update(toChange);
                            Log log = new()
                            {
                                Id = Guid.NewGuid(),
                                DateEvent = DateTime.Now,
                                NameEvent = watcherChange.ToString(),
                                FileId = toChange.Id,
                                File = toChange
                            };
                            Create(log);
                        }
                        catch (Exception) { }

                    }
                    break;
                case WatcherChangeTypes.Changed:
                    {
                        try
                        {
                            toChange = Context.Files.FirstOrDefault(x => x.FullPath == PathRoot);
                            if (toChange == null)
                            {
                                return;
                            }

                            toChange.IsDeleted = false;
                            string NameEvent = watcherChange.ToString();
                            toChange.ByteSize = file.Length.ToString();
                            toChange.DateCreated = file.CreationTimeUtc;
                            toChange.DateLastChanged = file.LastWriteTimeUtc;
                            toChange.FileName = file.Name;
                            toChange.FullPath = file.FullName;
                            string[] exp = file.FullName.Split('.');
                            toChange.Extension = exp[^1];
                            Update(toChange);
                            Log log = new()
                            {
                                Id = Guid.NewGuid(),
                                DateEvent = DateTime.Now,
                                NameEvent = NameEvent,
                                FileId = toChange.Id,
                                File = toChange
                            };
                            Create(log);
                        }
                        catch (Exception) { }

                    }
                    break;
                case WatcherChangeTypes.Renamed:
                    {
                        try
                        {
                            toChange = Context.Files.FirstOrDefault(x => x.FullPath == PathRoot);
                            toChange ??= Context.Files.FirstOrDefault(x => x.FullPath == file.FullName);
                            if (toChange == null)
                            {
                                return;
                            }

                            toChange.IsDeleted = false;
                            toChange.ByteSize = file.Length.ToString();
                            toChange.DateCreated = file.CreationTimeUtc;
                            toChange.DateLastChanged = file.LastWriteTimeUtc;
                            toChange.FileName = file.Name;
                            toChange.FullPath = file.FullName;
                            string[] exp = file.FullName.Split('.');
                            toChange.Extension = exp[^1];
                            toChange.DateLastRenamed = DateTime.Now;
                            //Update(toChange);
                            Log log = new()
                            {
                                Id = Guid.NewGuid(),
                                DateEvent = DateTime.Now,
                                NameEvent = watcherChange.ToString(),
                                FileId = toChange.Id,
                                File = toChange
                            };
                            Create(log);
                        }
                        catch (Exception) { }


                    }
                    break;
                default:
                    break;
            }
        }
        public void DeleteDir(Guid id)
        {
            Models.Directory toDelete = Context.Directories.FirstOrDefault(x => x.Id == id);
            if (toDelete != null)
            {
                _ = Context.Directories.Remove(toDelete);
                _ = Context.SaveChanges();

            }

        }
        public void DeleteFile(Guid id)
        {
            Models.File toDelete = Context.Files.FirstOrDefault(x => x.Id == id);
            if (toDelete != null)
            {
                _ = Context.Files.Remove(toDelete);
                _ = Context.SaveChanges();

            }

        }
        public void DeleteLog(Guid id)
        {
            Log toDelete = Context.Logs.FirstOrDefault(x => x.Id == id);
            if (toDelete != null)
            {
                _ = Context.Logs.Remove(toDelete);
                _ = Context.SaveChanges();

            }

        }
        public void DeleteLogs()
        {


            foreach (Log item in Context.Logs.ToList())
            {
                _ = Context.Logs.Remove(item);

            }
            _ = Context.SaveChanges();


        }

        public void ExportToExcel()
        {
            Excel.Application ex = new()
            {
                Visible = true,
                SheetsInNewWorkbook = 1
            };
            Excel.Workbook workBook = ex.Workbooks.Add(Type.Missing);
            ex.DisplayAlerts = false;
            Excel.Worksheet sheet = (Excel.Worksheet)ex.Worksheets.get_Item(1);
            sheet.Name = $"Logs for {DateTime.UtcNow.ToShortDateString()}";
            sheet.Columns.ColumnWidth = 20;
            sheet.Cells[1, 1] = "Date";
            sheet.Cells[1, 2] = "Event";
            sheet.Cells[1, 3] = "Path";
            Log[] logs = GetLogs().ToArray();
            for (int i = 0; i < logs.Length; i++)
            {
                sheet.Cells[i + 2, 1] = logs[i].DateEvent;
                sheet.Cells[i + 2, 2] = logs[i].NameEvent;
                sheet.Cells[i + 2, 3] = GetFile(logs[i].FileId).FullPath;

            }


        }
        public List<Models.Directory> GetDirectories()
        {
            return Context.Directories.ToList();
        }

        public List<Models.File> GetFiles()
        {
            return Context.Files.ToList();
        }

        public List<Models.Log> GetLogs()
        {
            return Context.Logs.ToList();
        }




        //public List<Models.File> GetFilesByFilename(string name) => Context.Files.Where(f => EF.Functions.FreeText(f.FileName, name)).ToList();
        public List<Models.File> GetFilesByFilename(string name)
        {
            return Context.Files.Where(f => f.FileName.Contains(name)).ToList();
        }

        public Models.File GetFile(Guid id)
        {
            Models.File model = Context.Files.FirstOrDefault(x => x.Id == id);
            return model ?? null;
        }
        public void Update(Models.Directory model)
        {
            Models.Directory toUpdate = Context.Directories.FirstOrDefault(x => x.Id == model.Id);
            if (toUpdate != null)
            {
                toUpdate = model;
            }
            _ = Context.Directories.Update(toUpdate);
            _ = Context.SaveChanges();
        }
        public void Update(Models.File model)
        {
            Models.File toUpdate = Context.Files.FirstOrDefault(x => x.Id == model.Id);
            if (toUpdate != null)
            {
                toUpdate = model;
            }
            _ = Context.Files.Update(toUpdate);
            _ = Context.SaveChanges();
        }
        public void Update(Models.Log model)
        {
            Log toUpdate = Context.Logs.FirstOrDefault(x => x.Id == model.Id);
            if (toUpdate != null)
            {
                toUpdate = model;
            }
            _ = Context.Logs.Update(toUpdate);
            _ = Context.SaveChanges();
        }

    }
}


