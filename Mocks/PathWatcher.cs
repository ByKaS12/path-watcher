using Microsoft.Toolkit.Uwp.Notifications;
using path_watcher.Models;
using System;
using System.IO;

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

            Watcher.Filter = "";
            Watcher.InternalBufferSize = 65536;
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

        private void OnChanged(object sender, FileSystemEventArgs e)
        {
            FileInfo file = new FileInfo(e.FullPath);
            if (file.Attributes == FileAttributes.Directory)
                new ToastContentBuilder()
                  .AddText($"Директория была изменена!")
                  .AddText($"Директория {file.FullName} была изменена {DateTime.Now}").Show();

            else if (file.Exists == true)
            {
                db.AddToLog(file, e.ChangeType, e.FullPath);
                if (file.Attributes != FileAttributes.Archive && file.Exists == true)
                    new ToastContentBuilder()
                      .AddText($"Файл был изменён!")
                      .AddText($"Файл {file.Name} в папке {file.DirectoryName} был изменён {DateTime.Now}").Show();

            }



        }





        private void OnCreated(object sender, FileSystemEventArgs e)
        {

            FileInfo file = new FileInfo(e.FullPath);
            if (file.Attributes == FileAttributes.Directory)
                new ToastContentBuilder()
                          .AddText($"Директория была создана!")
                          .AddText($"Директория {file.FullName} была создана {DateTime.Now}").Show();


            else if (file.Exists == true)
            {
                db.AddToLog(file, e.ChangeType, e.FullPath, Watcher.Path);

                new ToastContentBuilder()
                    .AddText($"Файл был создан!")
                    .AddText($"Файл {file.Name} в папке {file.DirectoryName} был создан {DateTime.Now}").Show();
            }


        }



        private void OnDeleted(object sender, FileSystemEventArgs e)
        {

            FileInfo file = new FileInfo(e.FullPath);
            if (file.Attributes == FileAttributes.Directory)

                new ToastContentBuilder()
                  .AddText($"Директория была удалена!")
                      .AddText($"Директория {e.FullPath} была удалена {DateTime.Now}").Show();




            else
            {
                db.AddToLog(file, e.ChangeType, e.FullPath);
                new ToastContentBuilder()
                    .AddText($"Файл был удалён!")
                    .AddText($"Файл {file.Name} в папке {file.DirectoryName} был удалён {DateTime.Now}").Show();
            }
        }






        private void OnRenamed(object sender, RenamedEventArgs e)
        {

            FileInfo file = new FileInfo(e.FullPath);
            if (file.Attributes == FileAttributes.Directory)
                new ToastContentBuilder()
                  .AddText($"Директория была переименована!")
                          .AddText($"Директория {e.OldFullPath} была переименована   {DateTime.Now} на {e.FullPath}").Show();

            else if (file.Exists == true)
            {
                db.AddToLog(file, e.ChangeType, e.OldFullPath);
                const char val = '\\';
                int lastIndex = e.OldFullPath.LastIndexOf(val) + 1;
                int count = e.OldFullPath.Length - lastIndex;
                new ToastContentBuilder()
                    .AddText($"Файл был переименован!")
                    .AddText($"Файл {e.OldFullPath.Substring(lastIndex, count)} в папке {file.DirectoryName} был переименован {DateTime.Now} на {e.FullPath.Substring(e.FullPath.LastIndexOf(val) + 1, e.FullPath.Length - e.FullPath.LastIndexOf(val) - 1)}").Show();


            }
        }
    }
}


