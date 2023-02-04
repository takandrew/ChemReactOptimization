using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using ChemReactOptimization.Model;

namespace ChemReactOptimization
{
    public partial class ChemReactContext : DbContext
    {
        public ChemReactContext()
        {
        }

        public ChemReactContext(DbContextOptions<ChemReactContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Method> Methods { get; set; } = null!;
        public virtual DbSet<Task> Tasks { get; set; } = null!;
        public virtual DbSet<User> Users { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlite("DataSource=ChemReact.db");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Method>().ToTable("Methods");

            modelBuilder.Entity<Task>().ToTable("Tasks");

            modelBuilder.Entity<User>().ToTable("Users");
            
        }
    }
}
