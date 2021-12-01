using Microsoft.EntityFrameworkCore;
using NiceNumber.Domain.Entities;

namespace NiceNumber.Domain
{
    public class NumberDataContext: DbContext
    {
        public NumberDataContext(DbContextOptions<NumberDataContext> options) : base(options)
        {
        }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Game>(entity =>
            {
                entity.HasKey(x => x.Id);
                entity.HasOne(x => x.Number)
                    .WithMany(x => x.Games)
                    .HasForeignKey(x => x.NumberId);
            });

            modelBuilder.Entity<Regularity>(entity =>
            {
                entity.HasKey(x => x.Id);
                entity.HasOne(x => x.Number)
                    .WithMany(x => x.Regularities)
                    .HasForeignKey(x => x.NumberId);
            });

            modelBuilder.Entity<Number>(entity =>
            {
                entity.HasKey(x => x.Id);
                entity.HasIndex(x => x.Value);
                entity.HasOne(x => x.TutorialLevel)
                    .WithOne(x => x.Number)
                    .HasForeignKey<TutorialLevel>(x => x.NumberId);
            });

            modelBuilder.Entity<Check>(entity =>
            {
                entity.HasKey(x => x.Id);
                entity.HasOne(x => x.Game)
                    .WithMany(x => x.Checks)
                    .HasForeignKey(x => x.GameId);
                entity.HasOne(x => x.Regularity)
                    .WithMany(x => x.Checks)
                    .HasForeignKey(x => x.RegularityId);
                entity.HasOne(x => x.ClosestRegularity)
                    .WithMany(x => x.ClosestChecks)
                    .HasForeignKey(x => x.ClosestRegularityId);
            });
            
            modelBuilder.Entity<TutorialLevel>(entity =>
            {
                entity.HasKey(x => x.Id);
                entity.HasOne(x => x.Number)
                    .WithOne(x => x.TutorialLevel)
                    .HasForeignKey<TutorialLevel>(x => x.NumberId);
            });
        }
    }
}