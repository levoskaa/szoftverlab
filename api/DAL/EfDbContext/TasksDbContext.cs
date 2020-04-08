using Microsoft.EntityFrameworkCore;

namespace api.DAL.EfDbContext
{
    public class TasksDbContext : DbContext
    {
        public DbSet<DbStatus> Statuses { get; set; }
        public DbSet<DbTask> Tasks { get; set; }

        // DO NOT CHANGE THE CONSTRUCTOR - NE VALTOZTASD MEG A KONSTRUKTORT
        public TasksDbContext(DbContextOptions options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<DbStatus>()
                .ToTable("statuses");
            modelBuilder.Entity<DbStatus>()
                .HasKey(s => s.Id);
            modelBuilder.Entity<DbStatus>()
                .Property(s => s.Name).HasMaxLength(50).IsRequired(required: true).IsUnicode(unicode: true);

            modelBuilder.Entity<DbTask>()
                .ToTable("tasks");
            modelBuilder.Entity<DbTask>()
                .HasKey(t => t.Id);
            modelBuilder.Entity<DbTask>()
                .Property(t => t.Title).HasMaxLength(50).IsRequired(required: true).IsUnicode(unicode: true);
            modelBuilder.Entity<DbTask>()
                .Property(t => t.Done).IsRequired(required: true);
            modelBuilder.Entity<DbTask>()
                .HasOne(t => t.Status)
                .WithMany()
                .HasForeignKey(s => s.StatusId)
                .IsRequired(required: true)
                .Metadata.DependentToPrincipal.SetPropertyAccessMode(PropertyAccessMode.Field);

            DbStatus[] dataStatus = new[]
                {
                    new DbStatus() { Id = 1, Name = "new" },
                    new DbStatus() { Id = 2, Name = "in progress" }
                };

            modelBuilder.Entity<DbStatus>()
                .HasData(dataStatus);
        }
    }
}
