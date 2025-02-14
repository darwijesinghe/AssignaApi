using Domain.Classes;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace Infrastructure.Data
{
    /// <summary>
    /// Data context class.
    /// </summary>
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        // Tables ----------------------------------
        public DbSet<Category> Category { get; set; }
        public DbSet<User> User         { get; set; }
        public DbSet<Task> Task         { get; set; }
        public DbSet<Priority> Priority { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Default values
            modelBuilder.Entity<Category>()
            .Property(b => b.InsertDate).HasDefaultValueSql("getdate()");

            modelBuilder.Entity<User>()
            .Property(b => b.InsertDate).HasDefaultValueSql("getdate()");

            modelBuilder.Entity<Task>()
            .Property(b => b.InsertDate).HasDefaultValueSql("getdate()");

            modelBuilder.Entity<Priority>()
            .Property(b => b.InsertDate).HasDefaultValueSql("getdate()");

            // Init data seeding

            // Categories
            var cats = new List<Category>()
            {
               new(){CatId = 1, CatName = "Team Task"},
               new(){CatId = 2, CatName = "Individual Task"},
               new(){CatId = 3, CatName = "Home Task"},
               new(){CatId = 4, CatName = "Finance Task"},
               new(){CatId = 5, CatName = "Client Task"},
               new(){CatId = 6, CatName = "Reasearch Task"},
            };

            // Priorities
            var pri = new List<Priority>()
            {
               new(){PriorityId = 1, PriorityName = "High"},
               new(){PriorityId = 2, PriorityName = "Medium"},
               new(){PriorityId = 3, PriorityName = "Low"},
            };

            // Add seed data
            modelBuilder.Entity<Category>().HasData(cats);
            modelBuilder.Entity<Priority>().HasData(pri);

            base.OnModelCreating(modelBuilder);
        }
    }
}
