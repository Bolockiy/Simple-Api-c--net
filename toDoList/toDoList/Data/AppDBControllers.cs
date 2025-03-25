using ApiToDo.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ApiToDo.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<ToDoTask> Tasks { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ToDoTask>().ToTable("tasks");
        }
    }
}
