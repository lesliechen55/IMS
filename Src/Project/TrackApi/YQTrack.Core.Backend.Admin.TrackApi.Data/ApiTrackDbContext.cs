using Microsoft.EntityFrameworkCore;
using YQTrack.Core.Backend.Admin.TrackApi.Data.Models;

namespace YQTrack.Core.Backend.Admin.TrackApi.Data
{
    public partial class ApiTrackDbContext : DbContext
    {
        public ApiTrackDbContext(string connectionString)
        {
            ConnectionString = connectionString;
        }
        public string ConnectionString;
        public ApiTrackDbContext(DbContextOptions<ApiTrackDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<TTrackInfo> TTrackInfo { get; set; }
        public virtual DbSet<TTrackQuota> TTrackQuota { get; set; }

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    if (!optionsBuilder.IsConfigured)
        //    {
        //        optionsBuilder.UseSqlServer(ConnectionString);
        //    }
        //}
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("ProductVersion", "2.2.4-servicing-10062");

            modelBuilder.Entity<TTrackInfo>(entity =>
            {
                entity.HasKey(e => e.FTrackInfoId);

                entity.HasIndex(e => e.FNextTrackingTime);

                entity.HasIndex(e => new { e.FTrackNo, e.FFirstCarrier, e.FUserId })
                    .HasName("UX_TTrackInfo_FUserNo_FTrackNo_FFirstCarrier")
                    .IsUnique();

                entity.HasIndex(e => new { e.FUserId, e.FTrackNo, e.FRegisterTime, e.FLastTrackTime, e.FLastPushTime })
                    .HasName("IX_TTrackInfo_FUserNo_FTrackNo");

                entity.Property(e => e.FTrackInfoId).ValueGeneratedNever();

                entity.Property(e => e.FLastPushTime).HasColumnType("datetime");

                entity.Property(e => e.FLastTrackTime).HasColumnType("datetime");

                entity.Property(e => e.FNextTrackingTime).HasColumnType("datetime");

                entity.Property(e => e.FRegisterTime).HasColumnType("datetime");

                entity.Property(e => e.FStopTrackingTime).HasColumnType("datetime");

                entity.Property(e => e.FTrackNo)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<TTrackQuota>(entity =>
            {
                entity.HasKey(e => e.FUserId);

                entity.HasIndex(e => new { e.FUserId, e.FUsed })
                    .HasName("IX_TBusinessCtrl");

                entity.Property(e => e.FUserId).ValueGeneratedNever();

                entity.Property(e => e.FRemain).HasComputedColumnSql("([FQuota]-[FUsed])");
            });
        }
    }
}
