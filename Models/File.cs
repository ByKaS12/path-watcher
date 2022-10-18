using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace path_watcher.Models
{
    public class File : BaseModel
    {
        public string FileName { get; set; }
        public string FullPath { get; set; }
        public string Extension { get; set; }
        public string ByteSize { get; set; }
        public BitmapSource GetIcon
        {
            get
            {
                FileInfo info = new(FullPath);
                if (info.Exists == true)
                {
                    Icon extractedIcon = System.Drawing.Icon.ExtractAssociatedIcon(FullPath);
                    Bitmap bitmap = extractedIcon.ToBitmap();
                    BitmapData bitmapData = bitmap.LockBits(
                    new Rectangle(0, 0, bitmap.Width, bitmap.Height),
                    ImageLockMode.ReadOnly, bitmap.PixelFormat);

                    BitmapSource bitmapSource = BitmapSource.Create(
                        bitmapData.Width, bitmapData.Height, 32, 32, PixelFormats.Bgra32, null,
                        bitmapData.Scan0, bitmapData.Stride * bitmapData.Height, bitmapData.Stride);

                    bitmap.UnlockBits(bitmapData);
                    return bitmapSource;
                }
                else
                {
                    string path = Environment.CurrentDirectory + "\\pngFuck.png";
                    Bitmap bitmap = new(path);
                    BitmapData bitmapData = bitmap.LockBits(
                    new Rectangle(0, 0, bitmap.Width, bitmap.Height),
                    ImageLockMode.ReadOnly, bitmap.PixelFormat);

                    BitmapSource bitmapSource = BitmapSource.Create(
                        bitmapData.Width, bitmapData.Height, 64, 64, PixelFormats.Bgra32, null,
                        bitmapData.Scan0, bitmapData.Stride * bitmapData.Height, bitmapData.Stride);

                    bitmap.UnlockBits(bitmapData);
                    return bitmapSource;

                }

            }
        }
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
