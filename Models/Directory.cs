using System.Collections.Generic;

namespace path_watcher.Models
{
    public class Directory : BaseModel
    {
        public string DirectoryName { get; set; }
        public string FullPath { get; set; }
        public virtual List<File> Files { get; set; } = new List<File>();
    }
}
