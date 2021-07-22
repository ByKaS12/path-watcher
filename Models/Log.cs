using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace path_watcher.Models
{
    public class Log: BaseModel
    {
        public string NameEvent { get; set; }
        public DateTime DateEvent { get; set; }      
        public Guid FileId { get; set; }
        public virtual File File { get; set; }
    }
}
