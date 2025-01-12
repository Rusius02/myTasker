using Domain;
using Microsoft.EntityFrameworkCore;

public class AppDbContext : DbContext
{
    public DbSet<Project> Projects { get; set; }
    public DbSet<TaskItem> TaskItems { get; set; }
    public DbSet<Member> Members { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        // Connexion à une base SQLite
        optionsBuilder.UseSqlite("Data Source=ProjectManager.db");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Relations entre les entités
        modelBuilder.Entity<Project>()
            .HasMany(p => p.Tasks)
            .WithOne()
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<TaskItem>()
            .HasOne(t => t.AssignedMember)
            .WithMany()
            .HasForeignKey(t => t.AssignedMemberId)
            .OnDelete(DeleteBehavior.SetNull);

        // Configuration des clés primaires et autres contraintes
        modelBuilder.Entity<Project>(entity =>
        {
            entity.HasKey(p => p.Id);
            entity.Property(p => p.Name).IsRequired().HasMaxLength(100);
        });


        modelBuilder.Entity<Member>(entity =>
        {
            entity.HasKey(m => m.Id);
            entity.Property(m => m.Name).IsRequired().HasMaxLength(50);
        });
    }
}
