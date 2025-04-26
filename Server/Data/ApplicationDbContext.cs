using Microsoft.EntityFrameworkCore;
using stratzclone.Server.Models;

namespace stratzclone.Server.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        public DbSet<Player>          Players          => Set<Player>();
        public DbSet<Match>           Matches          => Set<Match>();
        public DbSet<PlayerMatch>     PlayerMatches    => Set<PlayerMatch>();
        public DbSet<PlayerMatchItem> PlayerMatchItems => Set<PlayerMatchItem>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Player (external key)
            modelBuilder.Entity<Player>()
                        .HasKey(p => p.SteamId);
            modelBuilder.Entity<Player>()
                        .Property(p => p.SteamId)
                        .ValueGeneratedNever();

            // Match (external ID)
            modelBuilder.Entity<Match>()
                        .HasKey(m => m.MatchId);
            modelBuilder.Entity<Match>()
                        .Property(m => m.MatchId)
                        .ValueGeneratedNever();

            // PlayerMatch (composite key)
            modelBuilder.Entity<PlayerMatch>()
                        .HasKey(pm => new { pm.MatchId, pm.SteamId });
            modelBuilder.Entity<PlayerMatch>()
                        .HasOne(pm => pm.Match)
                        .WithMany(m  => m.PlayerMatches)
                        .HasForeignKey(pm => pm.MatchId);
            modelBuilder.Entity<PlayerMatch>()
                        .HasOne(pm => pm.Player)
                        .WithMany(p  => p.PlayerMatches)
                        .HasForeignKey(pm => pm.SteamId);

            // PlayerMatchItem (composite key)
            modelBuilder.Entity<PlayerMatchItem>()
                        .HasKey(i => new { i.MatchId, i.SteamId, i.ItemSeq });
            modelBuilder.Entity<PlayerMatchItem>()
                        .HasOne(i => i.PlayerMatch)
                        .WithMany(pm => pm.Items)
                        .HasForeignKey(i => new { i.MatchId, i.SteamId });
            modelBuilder.Entity<PlayerMatchItem>()
                        .HasIndex(i => i.ItemId);
        }
    }
}
