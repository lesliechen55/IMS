using Microsoft.EntityFrameworkCore;
using YQTrack.Core.Backend.Admin.TrackApi.Data.Models;

namespace YQTrack.Core.Backend.Admin.TrackApi.Data
{
    public partial class ApiUserDbContext : DbContext
    {
        public ApiUserDbContext()
        {
        }

        public ApiUserDbContext(DbContextOptions<ApiUserDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<TApiTrackCount> TApiTrackCount { get; set; }
        public virtual DbSet<TApiUserConfig> TApiUserConfig { get; set; }
        public virtual DbSet<TApiUserInfo> TApiUserInfo { get; set; }
        public virtual DbSet<TApiUserInvoice> TApiUserInvoice { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("ProductVersion", "2.2.4-servicing-10062");

            modelBuilder.Entity<TApiTrackCount>(entity =>
            {
                entity.HasKey(e => new { e.FUserId, e.FDate, e.FHour, e.FMinute });

                entity.Property(e => e.FDate).HasColumnType("date");
            });

            modelBuilder.Entity<TApiUserConfig>(entity =>
            {
                entity.HasKey(e => e.FUserId);

                entity.Property(e => e.FUserId).ValueGeneratedNever();

                entity.Property(e => e.FCreatedTime).HasColumnType("datetime");

                entity.Property(e => e.FIPWhiteList).IsUnicode(false);

                entity.Property(e => e.FSecretSeed).HasColumnType("datetime");

                entity.Property(e => e.FUpdateTime).HasColumnType("datetime");

                entity.Property(e => e.FWebHook)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(x => x.FScheduleFrequency).HasDefaultValue(100);
                entity.Property(x => x.FGiftQuota).HasDefaultValue(0);
                entity.Property(x => x.FApiNotify).HasDefaultValue("{\"push\":{\"d\":[10,20,30,35,40,50],\"e\":0},\"quota\":{\"w\":0}}");
            });

            modelBuilder.Entity<TApiUserInfo>(entity =>
            {
                entity.HasKey(e => e.FUserNo);

                entity.HasIndex(e => e.FUserId);

                entity.Property(e => e.FUserNo).ValueGeneratedNever();

                entity.Property(e => e.FCompanyName)
                    .HasMaxLength(128);
                entity.Property(e => e.FVATNo)
                    .HasMaxLength(50);
                entity.Property(e => e.FAddress)
                    .HasMaxLength(256);
                entity.Property(e => e.FCountry)
                    .HasMaxLength(50);

                entity.Property(e => e.FContactEmail)
                    .IsRequired()
                    .HasMaxLength(256)
                    .IsUnicode(false);

                entity.Property(e => e.FContactName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.FContactPhone)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.FCreatedTime).HasColumnType("datetime");

                entity.Property(e => e.FEmail)
                    .IsRequired()
                    .HasMaxLength(256)
                    .IsUnicode(false);

                entity.Property(e => e.FRemark)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.FTrackFrequency)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.FUpdateTime).HasColumnType("datetime");

                entity.Property(e => e.FUserId).HasDefaultValueSql("((0))");

                entity.Property(e => e.FUserName)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<TApiUserInvoice>(entity =>
            {
                entity.HasKey(e => e.FInvoiceId);

                entity.HasIndex(e => new { e.FUserId, e.FStartDate })
                    .HasName("IX_TApiUserInvoice_FUserId");

                entity.Property(e => e.FInvoiceId).ValueGeneratedNever();

                entity.Property(e => e.FCNYAmount).HasColumnType("numeric(18, 2)");

                entity.Property(e => e.FUSDAmount).HasColumnType("numeric(18, 2)");

                entity.Property(e => e.FData)
                    .IsRequired()
                    .HasColumnType("text");

                entity.Property(e => e.FEndDate).HasColumnType("date");

                entity.Property(e => e.FIssueTime).HasColumnType("datetime");

                entity.Property(e => e.FStartDate).HasColumnType("date");
            });
        }
    }
}
