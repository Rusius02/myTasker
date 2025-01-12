using Domain;
using Microsoft.EntityFrameworkCore;
using System.IO;
using System;

public class AppDbContext : DbContext
{
    public DbSet<Project> Projects { get; set; }
    public DbSet<TaskItem> TaskItems { get; set; }
    public DbSet<Member> Members { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        string connectionString = "Data Source=D:\\GitProjects\\WPF\\Tasker\\src\\ProjectManager.db";

        if (!System.IO.File.Exists("D:\\GitProjects\\WPF\\Tasker\\src\\ProjectManager.db"))
        {
            throw new FileNotFoundException($"Database file not found: {connectionString}");
        }

        Console.WriteLine($"Using database file: {connectionString}");
        optionsBuilder.UseSqlite(connectionString);
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
       
        modelBuilder.Entity<TaskItem>()
          .HasOne(t => t.Project) // Une tâche est associée à un projet
          .WithMany(p => p.Tasks) // Un projet peut avoir plusieurs tâches
          .HasForeignKey(t => t.ProjectId) // Clé étrangère
          .OnDelete(DeleteBehavior.Cascade); // Comportement de suppression (optionnel)

     

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
