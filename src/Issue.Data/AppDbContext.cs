using Issue.Data.Entity;
using Microsoft.EntityFrameworkCore;

namespace Issue.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Project> Projects { get; set; }
        public DbSet<Task> Tasks { get; set; }
        public DbSet<TaskFile> TaskFiles { get; set; }
        public DbSet<File> Files { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<TaskFile>(entity =>
            {
                entity.HasKey(x => new { x.TaskId, x.FileId });
            });

            builder.Entity<Project>(entity =>
            {
                entity.HasQueryFilter(x => x.Active);
            });

            builder.Entity<Task>(entity =>
            {
                entity.HasQueryFilter(x => x.Active);
            });

            builder.Entity<File>(entity =>
            {
                entity.HasQueryFilter(x => x.Active);
            });
        }
    }
}
