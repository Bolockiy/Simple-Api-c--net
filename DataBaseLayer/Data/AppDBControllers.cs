using ApiToDo.Domain.Entities;
using Helper.Security;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Data;
using toDoList.Entities.UserAccount;

namespace ApiToDo.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<ToDoTask> Tasks { get; set; }
        public DbSet<UserAccount> UserAccounts { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<ToDoTask>().ToTable("tasks");
            modelBuilder.Entity<UserAccount>().ToTable("UserAccount");
        }

    }

    public static class DbInitializer
    {
        public static void SeedAdminUser(AppDbContext dbContext)
        {
            if (!dbContext.UserAccounts.Any(u => u.Role == 1))
            {
                var adminUser = new UserAccount
                {
                    UserName = "admin",
                    FullName = "Administrator",
                    Role = 1,
                    Password = PasswordHasher.HashPassword("admin123")
                };

                dbContext.UserAccounts.Add(adminUser);
                dbContext.SaveChanges();
            }
        }
    }

}
