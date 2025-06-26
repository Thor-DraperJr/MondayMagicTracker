using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace MondayMagicTracker.Models;

public partial class DatabaseContext : IdentityDbContext<User>
{
    // Change this if you want to have a different database name in development
    private static string DEVELOPMENT_DATABASE_NAME = "MondayMagicTracker";

    // Change this to true if you want to have logging of SQL statements in development
    private static bool LOG_SQL_STATEMENTS_IN_DEVELOPMENT = false;

    public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
    {
    }

    // Add database tables here
    public DbSet<Playgroup> Playgroups { get; set; } = null!;
    public DbSet<PlaygroupMember> PlaygroupMembers { get; set; } = null!;
    public DbSet<Game> Games { get; set; } = null!;
    public DbSet<GamePlayer> GamePlayers { get; set; } = null!;
    public DbSet<Commander> Commanders { get; set; } = null!;

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (LOG_SQL_STATEMENTS_IN_DEVELOPMENT && Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development")
        {
            var loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());
            optionsBuilder.UseLoggerFactory(loggerFactory);
        }

        if (!optionsBuilder.IsConfigured)
        {
            // Use Azure SQL Database connection string or local SQL Server for development
            var connectionString = Environment.GetEnvironmentVariable("AZURE_SQL_CONNECTIONSTRING") 
                ?? $"Server=(localdb)\\mssqllocaldb;Database={DEVELOPMENT_DATABASE_NAME};Trusted_Connection=true;MultipleActiveResultSets=true";

            optionsBuilder.UseSqlServer(connectionString);
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configure relationships
        modelBuilder.Entity<PlaygroupMember>()
            .HasOne(pm => pm.Playgroup)
            .WithMany(p => p.Members)
            .HasForeignKey(pm => pm.PlaygroupId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<PlaygroupMember>()
            .HasOne(pm => pm.User)
            .WithMany(u => u.PlaygroupMemberships)
            .HasForeignKey(pm => pm.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Playgroup>()
            .HasOne(p => p.Owner)
            .WithMany(u => u.OwnedPlaygroups)
            .HasForeignKey(p => p.OwnerId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Game>()
            .HasOne(g => g.Playgroup)
            .WithMany(p => p.Games)
            .HasForeignKey(g => g.PlaygroupId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<GamePlayer>()
            .HasOne(gp => gp.Game)
            .WithMany(g => g.GamePlayers)
            .HasForeignKey(gp => gp.GameId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<GamePlayer>()
            .HasOne(gp => gp.User)
            .WithMany(u => u.GamePlayers)
            .HasForeignKey(gp => gp.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<GamePlayer>()
            .HasOne(gp => gp.Commander)
            .WithMany(c => c.GamePlayers)
            .HasForeignKey(gp => gp.CommanderId)
            .OnDelete(DeleteBehavior.SetNull);

        // Configure indexes for better performance
        modelBuilder.Entity<PlaygroupMember>()
            .HasIndex(pm => new { pm.PlaygroupId, pm.UserId })
            .IsUnique();

        modelBuilder.Entity<GamePlayer>()
            .HasIndex(gp => new { gp.GameId, gp.UserId })
            .IsUnique();

        // Seed data for some popular commanders
        modelBuilder.Entity<Commander>().HasData(
            new Commander { Id = 1, Name = "Atraxa, Praetors' Voice", Colors = "WUBG", Description = "Legendary Artifact Creature — Phyrexian Angel Horror" },
            new Commander { Id = 2, Name = "Edgar Markov", Colors = "RWB", Description = "Legendary Creature — Vampire Knight" },
            new Commander { Id = 3, Name = "The Ur-Dragon", Colors = "WUBRG", Description = "Legendary Creature — Dragon Avatar" },
            new Commander { Id = 4, Name = "Korvold, Fae-Cursed King", Colors = "BRG", Description = "Legendary Creature — Dragon Noble" },
            new Commander { Id = 5, Name = "Meren of Clan Nel Toth", Colors = "BG", Description = "Legendary Creature — Human Shaman" }
        );
    }
}
