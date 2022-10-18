using System;

namespace path_watcher.Models
{
    public class Log : BaseModel
    {
        public string NameEvent { get; set; }
        public DateTime DateEvent { get; set; }
        public Guid FileId { get; set; }
        public virtual File File { get; set; }
    }
}
