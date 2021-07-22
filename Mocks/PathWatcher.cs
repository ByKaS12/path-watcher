using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Hardcodet.Wpf.TaskbarNotification;
using Microsoft.Toolkit.Uwp.Notifications;
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
                if(file.Attributes == FileAttributes.Directory)
                {
                    ToastAudio audio = new();
                  //  audio.Src = new Uri(@"C:\Users\SEEGa\source\repos\theDimZone\path-watcher\Audio\1.mp3");
                    audio.Silent = true;
                    var notify = new ToastContentBuilder();

                    notify
                      .AddArgument("action", "viewConversation")
                      .AddArgument("conversationId", 9813)
                      .AddText($"Директория была изменена!")
                      .AddText($"Директория {file.FullName} была изменена {DateTime.Now}");

                    notify.AddAudio(audio);
                    notify.Show();
                }
                else 
                {
                    db.AddToLog(file, e.ChangeType, e.FullPath);
                    ToastAudio audio = new();
                  //  audio.Src = new Uri(@"C:\Users\SEEGa\source\repos\theDimZone\path-watcher\Audio\1.mp3");
                    audio.Silent = true;
                    var notify = new ToastContentBuilder();

                    notify
                      .AddArgument("action", "viewConversation")
                      .AddArgument("conversationId", 9813)
                      .AddText($"Файл был изменён!")
                      .AddText($"Файл {file.Name} в папке {file.DirectoryName} был изменён {DateTime.Now}");

                    notify.AddAudio(audio);
                    notify.Show();
                }



            }

                
            
        }

        private  void OnCreated(object sender, FileSystemEventArgs e)
        {
            if (e.Name.Split('.')[^1] != "tmp")
            {
                FileInfo file = new FileInfo(e.FullPath);
                if (file.Attributes == FileAttributes.Directory)
                {
                    ToastAudio audio = new();
                   // audio.Src = new Uri(@"C:\Users\SEEGa\source\repos\theDimZone\path-watcher\Audio\1.mp3");
                    audio.Silent = true;
                    var notify = new ToastContentBuilder();

                    notify
                      .AddArgument("action", "viewConversation")
                      .AddArgument("conversationId", 9813)
                      .AddText($"Директория была создана!")
                      .AddText($"Директория {file.FullName} была создана {DateTime.Now}");

                    notify.AddAudio(audio);
                    notify.Show();
                }
                else
                {
                    db.AddToLog(file, e.ChangeType, e.FullPath,Watcher.Path);
                    ToastAudio audio = new();
                    //audio.Src = new Uri(@"C:\Users\SEEGa\source\repos\theDimZone\path-watcher\Audio\1.mp3");
                    audio.Silent = true;
                    new ToastContentBuilder()
       .AddAudio(audio)
    .AddArgument("action", "viewConversation")
    .AddArgument("conversationId", 9813)
    .AddText($"Файл был создан!")
    .AddText($"Файл {file.Name} в папке {file.DirectoryName} был создан {DateTime.Now}")
    .Show();
                }
            }
        }

        private  void OnDeleted(object sender, FileSystemEventArgs e)
        {
            if (e.Name.Split('.')[^1] != "tmp")
            {
                FileInfo file = new FileInfo(e.FullPath);
                
                if (e.Name.Contains('.')==false)
                {
                    ToastAudio audio = new();
                    //audio.Src = new Uri(@"C:\Users\SEEGa\source\repos\theDimZone\path-watcher\Audio\1.mp3");
                    audio.Silent = true;
                    var notify = new ToastContentBuilder();

                    notify
                      .AddArgument("action", "viewConversation")
                      .AddArgument("conversationId", 9813)
                      .AddText($"Директория была удалена!")
                      .AddText($"Директория {e.FullPath} была удалена {DateTime.Now}");
                    
                    notify.AddAudio(audio);
                    notify.Show();
                }
                else
                {
                    db.AddToLog(file, e.ChangeType, e.FullPath);
                    ToastAudio audio = new();
                   // audio.Src = new Uri(@"C:\Users\SEEGa\source\repos\theDimZone\path-watcher\Audio\1.mp3");
                    audio.Silent = true;
                    new ToastContentBuilder()
       .AddAudio(audio)
    .AddArgument("action", "viewConversation")
    .AddArgument("conversationId", 9813)
    .AddText($"Файл был удалён!")
    .AddText($"Файл {file.Name} в папке {file.DirectoryName} был удалён {DateTime.Now}")
    .Show();
                }
            }
        }
            

        private  void OnRenamed(object sender, RenamedEventArgs e)
        {
            if (e.Name.Split('.')[^1] != "tmp")
            {
                FileInfo file = new FileInfo(e.FullPath);
                if (file.Attributes == FileAttributes.Directory)
                {
                    ToastAudio audio = new();
                  //  audio.Src = new Uri(@"C:\Users\SEEGa\source\repos\theDimZone\path-watcher\Audio\1.mp3");
                    audio.Silent = true;
                    var notify = new ToastContentBuilder();

                    notify
                      .AddArgument("action", "viewConversation")
                      .AddArgument("conversationId", 9813)
                      .AddText($"Директория была переименована!")
                      .AddText($"Директория {e.OldFullPath} была переименована   {DateTime.Now} на {e.FullPath}");

                    notify.AddAudio(audio);
                    notify.Show();
                }
                else
                {
                    db.AddToLog(file, e.ChangeType, e.OldFullPath);
                    const char val = '\\';

                    int lastIndex = e.OldFullPath.LastIndexOf(val) + 1;
                    int count = e.OldFullPath.Length - lastIndex - 1;
                    ToastAudio audio = new();
                   // audio.Src = new Uri(@"C:\Users\SEEGa\source\repos\theDimZone\path-watcher\Audio\1.mp3");
                    audio.Silent = true;
                    new ToastContentBuilder()
       .AddAudio(audio)
    .AddArgument("action", "viewConversation")
    .AddArgument("conversationId", 9813)

    .AddText($"Файл был переименован!")
    .AddText($"Файл {e.OldFullPath.Substring(lastIndex, count)} в папке {file.DirectoryName} был переименован {DateTime.Now} на {e.FullPath.Substring(e.FullPath.LastIndexOf(val) + 1, e.FullPath.Length - e.FullPath.LastIndexOf(val) - 1)}")
    .Show();
                }
            }
        }

    }
}

