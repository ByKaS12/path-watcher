using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace path_watcher.Models
{
    public class Directory: BaseModel
    {
        public string DirectoryName { get; set; }
        public string FullPath { get; set; }
        public int CountFileContain { get; set; }
        public string ByteSize { get; set; }
        public DateTime DateCreated { get; set; }
        public List<File> Files { get; set; } = new List<File>(); // files



    }
}
