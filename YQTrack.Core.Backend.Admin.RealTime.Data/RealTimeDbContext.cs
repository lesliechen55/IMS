using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.Collections.Generic;
using YQTrack.Core.Backend.Admin.Core.Sharding;
using YQTrack.Core.Backend.Admin.RealTime.Data.Models;

namespace YQTrack.Core.Backend.Admin.RealTime.Data
{
    public partial class RealTimeDbContext : DbContext
    {
        public RealTimeDbContext()
        {
        }

        public RealTimeDbContext(DbContextOptions<RealTimeDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<TUserShop> TUserShop { get; set; }
        public virtual DbSet<TBusinessCtrl> TBusinessCtrl { get; set; }
        public virtual DbSet<TTrackInfo> TTrackInfo { get; set; }


        private ICollection<TableMappingRule> m_TableMappingRule;
        public RealTimeDbContext(ICollection<TableMappingRule> rules)
        {
            this.m_TableMappingRule = rules;
        }

        public static string ConnectString { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(ConnectString).ReplaceService<IModelCacheKeyFactory, DynamicModelCacheKeyFactory>();
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("ProductVersion", "2.2.6-servicing-10079");

            //参考 CarrierTrackDbContext.cs
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

            modelBuilder.Entity<TBusinessCtrl>(entity =>
            {
                entity.HasKey(e => e.FCtrlId);

                entity.HasIndex(e => new { e.FUserId, e.FBusinessCtrlType })
                    .HasName("IX_BusinessCtrl_UserId01");

                entity.HasIndex(e => new { e.FUserId, e.FPurchaseOrderId, e.FProductSkuId, e.FStopTime })
                    .HasName("IX_BusinessCtrl_Unique_UserId01")
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

            modelBuilder.Entity<TUserShop>(entity =>
            {
                entity.HasKey(e => e.FShopId);

                entity.Property(e => e.FShopId).ValueGeneratedNever();

                entity.Property(e => e.FAccessToken).IsUnicode(false);

                entity.Property(e => e.FCreateTime).HasColumnType("datetime");

                entity.Property(e => e.FHasOrder).HasDefaultValueSql("((0))");

                entity.Property(e => e.FImportTime).HasColumnType("datetime");

                entity.Property(e => e.FIsDefault).HasDefaultValueSql("((0))");

                entity.Property(e => e.FIsImporting).HasDefaultValueSql("((0))");

                entity.Property(e => e.FIsSendEmail).HasDefaultValueSql("((0))");

                entity.Property(e => e.FLastImportedTime).HasColumnType("datetime");

                entity.Property(e => e.FLastSyncNum).HasDefaultValueSql("((0))");

                entity.Property(e => e.FLastSyncTime).HasColumnType("datetime");

                entity.Property(e => e.FLastSyncedTime).HasColumnType("datetime");

                entity.Property(e => e.FNextMessageSyncingTime).HasColumnType("datetime");

                entity.Property(e => e.FNextSyncTime).HasColumnType("smalldatetime");

                entity.Property(e => e.FPlatformArgs).IsUnicode(false);

                entity.Property(e => e.FPlatformUID)
                    .HasMaxLength(320)
                    .IsUnicode(false);

                entity.Property(e => e.FShopAlias).HasMaxLength(50);

                entity.Property(e => e.FShopEmail).HasMaxLength(256);

                entity.Property(e => e.FShopName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.FSyncTime).HasColumnType("datetime");

                entity.Property(e => e.FTimeStamp).IsRowVersion();

                entity.Property(e => e.FTokenExpire).HasColumnType("datetime");

                entity.Property(e => e.FUpdateTime).HasColumnType("datetime");
            });

            modelBuilder.ChangeTableMapping(m_TableMappingRule);
        }
    }
}
