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

 
    }
}
