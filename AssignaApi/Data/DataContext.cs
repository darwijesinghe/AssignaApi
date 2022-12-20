using System.Collections.Generic;
using AssignaApi.Models;
using Microsoft.EntityFrameworkCore;

namespace AssignaApi.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        // tables
        public DbSet<Category> category { get; set; } = null!;
        public DbSet<Users> users { get; set; } = null!;
        public DbSet<Task> task { get; set; } = null!;
        public DbSet<Priority> priority { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // default values
            modelBuilder.Entity<Category>()
            .Property(b => b.insertdate).HasDefaultValueSql("getdate()");

            modelBuilder.Entity<Users>()
            .Property(b => b.insertdate).HasDefaultValueSql("getdate()");

            modelBuilder.Entity<Task>()
            .Property(b => b.insertdate).HasDefaultValueSql("getdate()");

            modelBuilder.Entity<Priority>()
            .Property(b => b.insertdate).HasDefaultValueSql("getdate()");

            #region init data seed

            // category
            var cats = new List<Category>()
            {
               new(){cat_id = 1, cat_name = "Team Task"},
               new(){cat_id = 2, cat_name = "Individual Task"},
               new(){cat_id = 3, cat_name = "Home Task"},
               new(){cat_id = 4, cat_name = "Finance Task"},
               new(){cat_id = 5, cat_name = "Client Task"},
               new(){cat_id = 6, cat_name = "Reasearch Task"},
            };

            modelBuilder.Entity<Category>().HasData(cats);

            // priority
            var pri = new List<Priority>()
            {
               new(){pri_id = 1, pri_name = "High"},
               new(){pri_id = 2, pri_name = "Medium"},
               new(){pri_id = 3, pri_name = "Low"},
            };

            modelBuilder.Entity<Priority>().HasData(pri);

            #endregion init data seed

            base.OnModelCreating(modelBuilder);
        }
    }
}