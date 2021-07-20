using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using YQTrack.Core.Backend.Admin.Core.Sharding;
using YQTrack.Core.Backend.Admin.Seller.Data.Models;

namespace YQTrack.Core.Backend.Admin.Seller.Data
{
    public partial class SellerMessageDBContext : DbContext
    {
        public SellerMessageDBContext()
        {
        }

        public SellerMessageDBContext(DbContextOptions<SellerMessageDBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<TMsgEmailBusinessCtrl> TMsgEmailBusinessCtrl { get; set; }

        private ICollection<TableMappingRule> m_TableMappingRule;

        public SellerMessageDBContext(ICollection<TableMappingRule> rules)
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

            modelBuilder.Entity<TMsgEmailBusinessCtrl>(entity =>
            {
                entity.HasKey(e => e.FCtrlId);

                entity.HasIndex(e => new { e.FUserId, e.FBusinessCtrlType, e.FAvailable })
                    .HasName("IX_EmailBusinessCtrl_UserId01");

                entity.Property(e => e.FCtrlId).ValueGeneratedNever();

                entity.Property(e => e.FCreateTime).HasColumnType("datetime");

                entity.Property(e => e.FExtra)
                    .HasMaxLength(5000)
                    .IsUnicode(false);

                entity.Property(e => e.FProductSkuId)
                    .HasMaxLength(32)
                    .IsUnicode(false);

                entity.Property(e => e.FProviderId).HasDefaultValueSql("((0))");

                entity.Property(e => e.FRemainCount).HasComputedColumnSql("(([FServiceCount]-[FUsedCount])-[FlockedCount])");

                entity.Property(e => e.FRemark)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.FStartTime).HasColumnType("datetime");

                entity.Property(e => e.FStopTime).HasColumnType("datetime");

                entity.Property(e => e.FUpdateTime).HasColumnType("datetime");
            });
            // step 3 Call Extension method
            modelBuilder.ChangeTableMapping(m_TableMappingRule);
        }
    }
}
