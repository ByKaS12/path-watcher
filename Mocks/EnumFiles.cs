using path_watcher.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace path_watcher.Mocks
{
   public  class EnumFiles
    {
        private IBaseRepository<Models.Directory> Directories { get; set; }
        private IBaseRepository<Models.File> Files { get; set; }
        private IBaseRepository<Models.Log> Logs { get; set; }
        public EnumFiles(IBaseRepository<Models.Directory> directories, IBaseRepository<Models.File> files, IBaseRepository<Models.Log> logs)
        {
            Directories = directories;
            Files = files;
            Logs = logs;

        }
        public EnumFiles() { }

        public void AddToDbFile(FileInfo file,string PathRoot)
        {
            Models.File model = new Models.File();
            model.Id = Guid.NewGuid();
            model.ByteSize = file.Length.ToString();
            model.DateCreated = file.CreationTimeUtc;
            model.DateLastChanged = file.LastWriteTimeUtc;
            model.DateLastOpened = file.LastAccessTimeUtc;
            




        }
        public void AddToDbDir(DirectoryInfo dir)
        {
            Models.Directory directory = new();
            directory.DateCreated = dir.CreationTimeUtc;
            directory.DirectoryName = dir.Name;
            directory.FullPath = dir.FullName;
            directory.Id = Guid.NewGuid();
            _ = Directories.Create(directory);

        }
    }
}
