using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace path_watcher.Models
{
    public class File: BaseModel
    {
        public string FileName { get; set; }
        public string FullPath { get; set; }
        public string Extension { get; set; }
        public string ByteSize { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateLastChanged { get; set; }
        public DateTime DateLastOpened { get; set; }
        public DateTime DateLastRenamed { get; set; }
        public bool IsDeleted { get; set; } = false;
        public Guid DirectoryId { get; set; }
        public virtual Directory Directory { get; set; }
        public virtual List<Log> Logs { get; set; } = new List<Log>();
    }
}
