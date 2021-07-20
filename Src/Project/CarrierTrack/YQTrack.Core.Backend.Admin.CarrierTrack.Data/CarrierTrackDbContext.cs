using Microsoft.EntityFrameworkCore;
using YQTrack.Core.Backend.Admin.CarrierTrack.Data.Models;

namespace YQTrack.Core.Backend.Admin.CarrierTrack.Data
{
    public partial class CarrierTrackDbContext : DbContext
    {
        public CarrierTrackDbContext()
        {
        }

        public CarrierTrackDbContext(DbContextOptions<CarrierTrackDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<TBusinessCtrl> TBusinessCtrl { get; set; }
        public virtual DbSet<TOrderMark> TOrderMark { get; set; }
        public virtual DbSet<TTrackBatchTaskControl> TTrackBatchTaskControl { get; set; }
        public virtual DbSet<TTrackInfo> TTrackInfo { get; set; }
        public virtual DbSet<TTrackNumConsumeRecord> TTrackNumConsumeRecord { get; set; }
        public virtual DbSet<TTrackUploadRecord> TTrackUploadRecord { get; set; }
        public virtual DbSet<TUserMark> TUserMark { get; set; }
        public virtual DbSet<TUserMarkLog> TUserMarkLog { get; set; }
        public virtual DbSet<TControl> TControl { get; set; }
        public virtual DbSet<TReport> TReport { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("ProductVersion", "2.2.4-servicing-10062");

            modelBuilder.Entity<TBusinessCtrl>(entity =>
            {
                entity.HasKey(e => e.FCtrlId);

                entity.HasIndex(e => new { e.FUserId, e.FBusinessCtrlType })
                    .HasName("UX_FUserId_FBusinessCtrlType");

                entity.HasIndex(e => new { e.FUserId, e.FPurchaseOrderId, e.FProductSkuId, e.FStopTime })
                    .HasName("UX_FUserId_FPurchaseOrderId_FProductSkuId_FStopTime")
                    .IsUnique();

                entity.Property(e => e.FCtrlId).ValueGeneratedNever();

                entity.Property(e => e.FCreateTime).HasColumnType("datetime");

                entity.Property(e => e.FExtra)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.FProductSkuId)
                    .HasMaxLength(32)
                    .IsUnicode(false);

                entity.Property(e => e.FProviderId).HasDefaultValueSql("((0))");

                entity.Property(e => e.FRemainCount).HasComputedColumnSql("([FServiceCount]-[FUsedCount])");

                entity.Property(e => e.FRemark)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.FStartTime).HasColumnType("datetime");

                entity.Property(e => e.FStopTime).HasColumnType("datetime");

                entity.Property(e => e.FUpdateTime).HasColumnType("datetime");
            });

            modelBuilder.Entity<TOrderMark>(entity =>
            {
                entity.HasKey(e => e.FOrderMarkId);

                entity.HasIndex(e => new { e.FMarkId, e.FTrackInfoId })
                    .HasName("UX_FMarkId_FTrackInfoId")
                    .IsUnique();

                entity.Property(e => e.FOrderMarkId).ValueGeneratedNever();

                entity.Property(e => e.FCreateTime).HasColumnType("datetime");
            });

            modelBuilder.Entity<TTrackBatchTaskControl>(entity =>
            {
                entity.HasKey(e => e.FBatchId);

                entity.Property(e => e.FBatchId).ValueGeneratedNever();

                entity.Property(e => e.FCreateTime).HasColumnType("datetime");

                entity.Property(e => e.FTaskEndTime).HasColumnType("datetime");

                entity.Property(e => e.FTaskStartTime).HasColumnType("datetime");

                entity.Property(e => e.FUpdateTime).HasColumnType("datetime");
            });

            modelBuilder.Entity<TTrackInfo>(entity =>
            {
                entity.HasKey(e => e.FTrackInfoId);

                entity.HasIndex(e => e.FNextScheduleTime)
                    .HasName("IX_FNextScheduleTime");

                entity.HasIndex(e => new { e.FUserId, e.FIsCompleted, e.FTrackNo, e.FCreateTime })
                    .HasName("UX_FUserId_FIsCompleted_FTrackNo_FCreateTime")
                    .IsUnique();

                entity.Property(e => e.FTrackInfoId).ValueGeneratedNever();

                entity.Property(e => e.FCompletedTime).HasColumnType("datetime");

                entity.Property(e => e.FCreateTime).HasColumnType("datetime");

                entity.Property(e => e.FExcelCountry).HasMaxLength(32);

                entity.Property(e => e.FExcelCustomerCode).HasMaxLength(50);

                entity.Property(e => e.FExcelPostCode).HasMaxLength(50);

                entity.Property(e => e.FExcelProduct).HasMaxLength(32);

                entity.Property(e => e.FFirstScheduleTime).HasColumnType("datetime");

                entity.Property(e => e.FLastEventUpdate).HasColumnType("datetime");

                entity.Property(e => e.FNextScheduleTime)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.FTrackNo)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.FTrackStateType).HasDefaultValueSql("((-1))");

                entity.Property(e => e.FUpdateTime).HasColumnType("datetime");
            });

            modelBuilder.Entity<TTrackNumConsumeRecord>(entity =>
            {
                entity.HasKey(e => e.FRecordId);

                entity.HasIndex(e => new { e.FUserId, e.FTrackInfoId })
                    .HasName("IX_FUserId_FTrackInfoId");

                entity.Property(e => e.FCreateTime).HasColumnType("datetime");

                entity.Property(e => e.FIsConsume)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.FTrackNo)
                    .HasMaxLength(64)
                    .IsUnicode(false);

                entity.Property(e => e.FUpdateTime).HasColumnType("datetime");
            });

            modelBuilder.Entity<TTrackUploadRecord>(entity =>
            {
                entity.HasKey(e => e.FUploadRecordId);

                entity.HasIndex(e => new { e.FUserId, e.FFileMD5 })
                    .HasName("IX_FUserId_FFileMD5");

                entity.Property(e => e.FUploadRecordId).ValueGeneratedNever();

                entity.Property(e => e.FCreateTime).HasColumnType("datetime");

                entity.Property(e => e.FCurrentFileName)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.FErrorDetail).IsUnicode(false);

                entity.Property(e => e.FFileMD5)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.FFilePath)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.FSourceFileName).HasMaxLength(200);

                entity.Property(e => e.FUpdateTime).HasColumnType("datetime");
            });

            modelBuilder.Entity<TUserMark>(entity =>
            {
                entity.HasKey(e => e.FMarkId);

                entity.HasIndex(e => e.FUserId)
                    .HasName("IX_FUserId");

                entity.Property(e => e.FMarkId).ValueGeneratedNever();

                entity.Property(e => e.FCreateTime)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.FMarkName)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.FSortNo).HasDefaultValueSql("((0))");

                entity.Property(e => e.FUpdateTime).HasColumnType("datetime");
            });

            modelBuilder.Entity<TUserMarkLog>(entity =>
            {
                entity.HasKey(e => e.FId);

                entity.HasIndex(e => e.FUserId)
                    .HasName("IX_FUserId");

                entity.Property(e => e.FId).ValueGeneratedNever();

                entity.Property(e => e.FCreateTime)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.FDescription)
                    .IsRequired()
                    .HasMaxLength(128);

                entity.Property(e => e.FDetail)
                    .IsRequired()
                    .IsUnicode(false);
            });

            modelBuilder.Entity<TControl>(entity =>
            {
                entity.HasKey(e => e.FControlId);

                entity.HasIndex(e => e.FUserId)
                    .HasName("UX_FUserId")
                    .IsUnique();

                entity.Property(e => e.FControlId).ValueGeneratedNever();

                entity.Property(e => e.FCreateTime)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.FEmail)
                    .IsRequired()
                    .HasMaxLength(256)
                    .IsUnicode(false);

                entity.Property(e => e.FUpdateTime).HasColumnType("datetime");
                entity.Property(e => e.FLastAccessTime).HasColumnType("datetime");
            });

            modelBuilder.Entity<TReport>(entity =>
            {
                entity.HasKey(e => e.FId);

                entity.Property(e => e.FId).ValueGeneratedNever();

                entity.Property(e => e.FCreateTime)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.FDate).HasColumnType("datetime");

                entity.Property(e => e.FEmail)
                    .IsRequired()
                    .HasMaxLength(256)
                    .IsUnicode(false);

                entity.Property(e => e.FUpdateTime).HasColumnType("datetime");
            });
        }
    }
}
