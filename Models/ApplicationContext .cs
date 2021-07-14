﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


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
            optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=PathWatcher;Trusted_Connection=True;");
        }
    }
}
