using BoardGamesTrackerApi.Models;
using Microsoft.EntityFrameworkCore;

namespace BoardGamesTrackerApi;

public class DatabaseContext : DbContext
{
    public DatabaseContext(DbContextOptions options) : base(options)
    {
    }
    
    public virtual DbSet<BoardGame> BoardGames { get; set; }
    public virtual DbSet<Category> Categories { get; set; }
    public virtual DbSet<BoardGameCategory> BoardGameCategories { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<BoardGame>()
            .Property(e => e.Id)
            .ValueGeneratedNever();

        modelBuilder.Entity<BoardGameCategory>()
            .HasOne(e => e.BoardGame)
            .WithMany(e => e.BoardGameCategories)
            .HasForeignKey(e => e.BoardGameId);

        modelBuilder.Entity<BoardGameCategory>()
            .HasOne(e => e.Category)
            .WithMany(e => e.BoardGameCategories)
            .HasForeignKey(e => e.CategoryId);
    }
}