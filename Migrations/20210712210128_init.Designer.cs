﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using path_watcher.Models;

namespace path_watcher.Migrations
{
    [DbContext(typeof(ApplicationContext))]
    [Migration("20210712210128_init")]
    partial class init
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "5.0.7");

            modelBuilder.Entity("path_watcher.Models.Directory", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("ByteSize")
                        .HasColumnType("TEXT");

                    b.Property<int>("CountFileContain")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("DateCreated")
                        .HasColumnType("TEXT");

                    b.Property<string>("DirectoryName")
                        .HasColumnType("TEXT");

                    b.Property<string>("FullPath")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Directories");
                });

            modelBuilder.Entity("path_watcher.Models.File", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("ByteSize")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("DateCreated")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("DateLastChanged")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("DateLastOpened")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("DateLastRenamed")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("DirectoryId")
                        .HasColumnType("TEXT");

                    b.Property<string>("Expansion")
                        .HasColumnType("TEXT");

                    b.Property<string>("FileName")
                        .HasColumnType("TEXT");

                    b.Property<string>("FullPath")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("DirectoryId");

                    b.ToTable("Files");
                });

            modelBuilder.Entity("path_watcher.Models.Log", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("DateChanged")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("DateCreated")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("DateOpened")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("DateRenamed")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("FileId")
                        .HasColumnType("TEXT");

                    b.Property<string>("NameEvent")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("FileId");

                    b.ToTable("Logs");
                });

            modelBuilder.Entity("path_watcher.Models.File", b =>
                {
                    b.HasOne("path_watcher.Models.Directory", "Directory")
                        .WithMany("Files")
                        .HasForeignKey("DirectoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Directory");
                });

            modelBuilder.Entity("path_watcher.Models.Log", b =>
                {
                    b.HasOne("path_watcher.Models.File", "File")
                        .WithMany("Logs")
                        .HasForeignKey("FileId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("File");
                });

            modelBuilder.Entity("path_watcher.Models.Directory", b =>
                {
                    b.Navigation("Files");
                });

            modelBuilder.Entity("path_watcher.Models.File", b =>
                {
                    b.Navigation("Logs");
                });
#pragma warning restore 612, 618
        }
    }
}
