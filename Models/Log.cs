using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace path_watcher.Models
{
    public class Log: BaseModel
    {
        public string NameEvent { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateChanged { get; set; }
        public DateTime DateOpened { get; set; }
        public DateTime DateRenamed { get; set; }
        public Guid FileId { get; set; }
        public File File { get; set; }
    }
}
