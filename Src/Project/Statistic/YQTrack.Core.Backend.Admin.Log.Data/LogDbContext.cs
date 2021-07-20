using Microsoft.EntityFrameworkCore;
using YQTrack.Core.Backend.Admin.Log.Data.Models;

namespace YQTrack.Core.Backend.Admin.Log.Data
{
    public partial class LogDbContext : DbContext
    {
        public LogDbContext()
        {
        }

        public LogDbContext(DbContextOptions<LogDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<TStatisticDay> TStatisticDay { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("ProductVersion", "2.2.4-servicing-10062");

            modelBuilder.Entity<TStatisticDay>(entity =>
            {
                entity.HasKey(e => e.FStatisticId)
                    .HasName("PK_TStatisticUserDay");

                entity.HasIndex(e => new { e.FStatisticType, e.FType, e.FStatisticDate })
                    .HasName("UniqueYype")
                    .IsUnique();

                entity.Property(e => e.FStatisticId).ValueGeneratedNever();

                entity.Property(e => e.FCreateTime).HasColumnType("datetime");

                entity.Property(e => e.FStatisticDate).HasColumnType("date");

                entity.Property(e => e.FUpdateTime).HasColumnType("datetime");
            });
        }
    }
}
