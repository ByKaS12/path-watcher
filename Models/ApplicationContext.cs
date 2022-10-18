using Microsoft.EntityFrameworkCore;
using System;
using System.IO;


namespace path_watcher.Models
{
    public class ApplicationContext : DbContext
    {
        public DbSet<Directory> Directories { get; set; }
        public DbSet<File> Files { get; set; }
        public DbSet<Log> Logs { get; set; }
        public ApplicationContext()
        {
            Database.EnsureCreated();
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=PathWatcher;Trusted_Connection=True;");
            var folder = Environment.SpecialFolder.LocalApplicationData;
            var path = Environment.GetFolderPath(folder);

            optionsBuilder
                .UseLazyLoadingProxies()
                .UseSqlite($"Data Source={path}{Path.DirectorySeparatorChar}path-watcher.db");
        }
    }
}
