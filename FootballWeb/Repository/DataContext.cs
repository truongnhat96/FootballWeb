using FootballWeb.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FootballWeb.Repository
{
    public class DataContext : IdentityDbContext<IdentityUser>
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }
        public DbSet<Team> Teams { get; set; }
        public DbSet<Player> Players { get; set; }
        public DbSet<PlayerRecord> PlayerRecords { get; set; }
        
        public DbSet<Advertisement> Advertisements { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            foreach (var entityType in builder.Model.GetEntityTypes())
            {
                var tableName = entityType.GetTableName();
                if (tableName != null && tableName.StartsWith("AspNet"))
                {
                    entityType.SetTableName(tableName.Substring(6));
                }
            }

            builder.Entity<Team>(entity =>
            {
                entity.Property(t => t.Id).UseIdentityColumn(seed: 1000, increment: 1);
            });

            builder.Entity<Player>(entity =>
            {
                entity.HasOne(p => p.Team)
                      .WithMany(t => t.Players)
                      .HasForeignKey(p => p.TeamId)
                      .OnDelete(DeleteBehavior.Cascade);
                entity.Property(p => p.Id).UseIdentityColumn(seed: 10000, increment: 1);
            });

            builder.Entity<PlayerRecord>(entity =>
            {
                entity.HasOne(pr => pr.Player)
                      .WithOne(p => p.PlayerRecord)
                      .HasForeignKey<PlayerRecord>(pr => pr.Id)
                      .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}
