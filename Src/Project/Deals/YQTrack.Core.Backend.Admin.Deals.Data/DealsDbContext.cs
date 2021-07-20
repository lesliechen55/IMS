using Microsoft.EntityFrameworkCore;
using YQTrack.Core.Backend.Admin.Deals.Data.Models;

namespace YQTrack.Core.Backend.Admin.Deals.Data
{
    public partial class DealsDbContext : DbContext
    {
        public DealsDbContext()
        {
        }

        public DealsDbContext(DbContextOptions<DealsDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<TYQMerchantLibraryLang> TYQMerchantLibraryLang { get; set; }
        public virtual DbSet<TYQStatisticsAll> TYQStatisticsAll { get; set; }
        public virtual DbSet<TYQStatisticsMer> TYQStatisticsMer { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("ProductVersion", "2.2.4-servicing-10062");

            modelBuilder.Entity<TYQMerchantLibraryLang>(entity =>
            {
                entity.HasKey(e => e.FYQMerchantLibraryLangId)
                    .HasName("PK_FYQMerchantLibraryLangId");

                entity.HasIndex(e => e.FYQMerchantLibraryId)
                    .HasName("FK_FYQMerchantLibraryId");

                entity.Property(e => e.FYQMerchantLibraryLangId).ValueGeneratedNever();

                entity.Property(e => e.FAlsoName).HasMaxLength(100);

                entity.Property(e => e.FCreateDate).HasColumnType("datetime");

                entity.Property(e => e.FLangCode)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.FModifyDate).HasColumnType("datetime");

                entity.Property(e => e.FName).HasMaxLength(100);
            });

            modelBuilder.Entity<TYQStatisticsAll>(entity =>
            {
                entity.HasKey(e => e.FId)
                    .HasName("PK__TYQStati__C1BEAA426B38F4F1");

                entity.Property(e => e.FId).ValueGeneratedNever();

                entity.Property(e => e.FClickRate).HasColumnType("decimal(5, 2)");

                entity.Property(e => e.FConversion).HasColumnType("decimal(5, 2)");

                entity.Property(e => e.FConversionRate).HasColumnType("decimal(5, 2)");

                entity.Property(e => e.FCreateDate).HasColumnType("datetime");

                entity.Property(e => e.FECPC).HasColumnType("decimal(5, 2)");

                entity.Property(e => e.FECPCRate).HasColumnType("decimal(5, 2)");

                entity.Property(e => e.FModifyDate).HasColumnType("datetime");

                entity.Property(e => e.FPaymentCount).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.FPaymentRate).HasColumnType("decimal(5, 2)");

                entity.Property(e => e.FStatisticsDate)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.FTransactionRate).HasColumnType("decimal(5, 2)");
            });

            modelBuilder.Entity<TYQStatisticsMer>(entity =>
            {
                entity.HasKey(e => e.FId)
                    .HasName("PK__TYQStati__C1BEAA42DDBAE340");

                entity.Property(e => e.FId).ValueGeneratedNever();

                entity.Property(e => e.FConversion).HasColumnType("decimal(5, 2)");

                entity.Property(e => e.FCreateDate).HasColumnType("datetime");

                entity.Property(e => e.FECPC).HasColumnType("decimal(5, 2)");

                entity.Property(e => e.FModifyDate).HasColumnType("datetime");

                entity.Property(e => e.FPaymentCount).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.FStatisticsDate)
                    .HasMaxLength(10)
                    .IsUnicode(false);
            });
        }
    }
}
