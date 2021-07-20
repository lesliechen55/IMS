using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using YQTrack.Core.Backend.Admin.Core.Sharding;
using YQTrack.Core.Backend.Admin.Seller.Data.Models;

namespace YQTrack.Core.Backend.Admin.Seller.Data
{
    public partial class SellerOrderDBContext : DbContext
    {
        public SellerOrderDBContext()
        {
        }

        public SellerOrderDBContext(DbContextOptions<SellerOrderDBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<TBusinessCtrl> TBusinessCtrl { get; set; }
        public virtual DbSet<TShopOperateLog> TShopOperateLog { get; set; }
        public virtual DbSet<TTrackBatchTaskControl> TTrackBatchTaskControl { get; set; }
        public virtual DbSet<TUserShop> TUserShop { get; set; }
        public virtual DbSet<TTrackUploadRecord> TTrackUploadRecord { get; set; }

        private ICollection<TableMappingRule> m_TableMappingRule;

        public SellerOrderDBContext(ICollection<TableMappingRule> rules)
        {
            this.m_TableMappingRule = rules;
        }
        public static string ConnectString { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                // setp 0 : Replace Service
                optionsBuilder.UseSqlServer(ConnectString).ReplaceService<IModelCacheKeyFactory, DynamicModelCacheKeyFactory>();
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("ProductVersion", "2.2.6-servicing-10079");

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

            modelBuilder.Entity<TShopOperateLog>(entity =>
            {
                entity.HasKey(e => e.FLogId);

                entity.HasIndex(e => new { e.FUserId, e.FShopId, e.FCreateTime })
                    .HasName("IX_TShopOperateLog_FUserId_FShopId02");

                entity.Property(e => e.FLogId).ValueGeneratedNever();

                entity.Property(e => e.FCreateTime).HasColumnType("datetime");
            });

            modelBuilder.Entity<TTrackBatchTaskControl>(entity =>
            {
                entity.HasKey(e => e.FBatchId);

                entity.HasIndex(e => new { e.FTaskStatus, e.FUserId, e.FIsRead, e.FCreateTime })
                    .HasName("TTrackBatchTaskControl01_UserId");

                entity.Property(e => e.FBatchId).ValueGeneratedNever();

                entity.Property(e => e.FCreateTime).HasColumnType("datetime");

                entity.Property(e => e.FSessionId)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.FTaskEndTime).HasColumnType("datetime");

                entity.Property(e => e.FTaskExpireTime).HasColumnType("datetime");

                entity.Property(e => e.FTaskStartTime).HasColumnType("datetime");

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

            modelBuilder.Entity<TTrackUploadRecord>(entity =>
            {
                entity.HasKey(e => e.FUploadRecordId);

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

            // step 3 Call Extension method
            modelBuilder.ChangeTableMapping(m_TableMappingRule);
        }
    }
}
