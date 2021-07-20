using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using YQTrack.Core.Backend.Admin.Pay.Data.Models;

namespace YQTrack.Core.Backend.Admin.Pay.Data
{
    public partial class PayDbContext : DbContext
    {
        public PayDbContext()
        {
        }

        public PayDbContext(DbContextOptions<PayDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<TActivity> Activities { get; set; }
        public virtual DbSet<TActivityCoupon> ActivityCoupons { get; set; }
        public virtual DbSet<TBusinessType> TBusinessType { get; set; }
        public virtual DbSet<TCurrency> TCurrency { get; set; }
        public virtual DbSet<TExchangeRate> TExchangeRate { get; set; }
        public virtual DbSet<TPayment> TPayment { get; set; }
        public virtual DbSet<TPaymentLog> TPaymentLog { get; set; }
        public virtual DbSet<TProduct> TProduct { get; set; }
        public virtual DbSet<TProductCategory> TProductCategory { get; set; }
        public virtual DbSet<TProductSku> TProductSku { get; set; }
        public virtual DbSet<TProductSkuPrice> TProductSkuPrice { get; set; }
        public virtual DbSet<TProvider> TProvider { get; set; }
        public virtual DbSet<TProviderType> TProviderType { get; set; }
        public virtual DbSet<TPurchaseOrder> TPurchaseOrder { get; set; }
        public virtual DbSet<TPurchaseOrderItem> TPurchaseOrderItem { get; set; }
        public virtual DbSet<TReconcile> TReconcile { get; set; }
        public virtual DbSet<TReconcileItem> TReconcileItem { get; set; }
        public virtual DbSet<TSequence> TSequence { get; set; }
        public virtual DbSet<TSerialNo> TSerialNo { get; set; }
        public virtual DbSet<TInvoice> TInvoice { get; set; }
        public virtual DbSet<TInvoiceApply> TInvoiceApply { get; set; }
        public virtual DbSet<TInvoiceApplyExtra> TInvoiceApplyExtra { get; set; }
        public virtual DbSet<TInvoiceApplyPayment> TInvoiceApplyPayment { get; set; }
        public virtual DbSet<TOfflinePayment> TOfflinePayment { get; set; }
        public virtual DbSet<TOfflinePaymentOrder> TOfflinePaymentOrder { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("ProductVersion", "2.2.4-servicing-10062");

            modelBuilder.Entity<TActivity>(entity =>
            {
                entity.HasKey(e => e.FActivityId);

                entity.Property(e => e.FActivityId).UseSqlServerIdentityColumn();

                entity.Property(e => e.FDescription).HasMaxLength(200);

                entity.Property(e => e.FCnName)
                    .IsRequired()
                    .HasMaxLength(100);
                entity.Property(e => e.FEnName)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.FCreateAt).HasDefaultValueSql("(getutcdate())");
                entity.Property(e => e.FUpdateAt).HasDefaultValueSql("(getutcdate())");
            });

            modelBuilder.Entity<TActivityCoupon>(entity =>
            {
                entity.HasKey(e => e.FActivityCouponId);
                entity.Property(e => e.FActivityCouponId).UseSqlServerIdentityColumn();

                entity.Property(e => e.FActivityId)
                    .IsRequired();

                entity.HasOne(d => d.FActivity)
                    .WithMany(p => p.FActivityCoupons)
                    .HasForeignKey(d => d.FActivityId)
                    .HasConstraintName("FK_TActivityCoupon_FActivityId");

                entity.Property(e => e.FCreateAt).HasDefaultValueSql("(getutcdate())");
                entity.Property(e => e.FUpdateAt).HasDefaultValueSql("(getutcdate())");
            });

            modelBuilder.Entity<TBusinessType>(entity =>
            {
                entity.HasKey(e => e.FBusinessTypeId)
                    .HasName("pk_TBusinessType");

                entity.Property(e => e.FBusinessTypeId).ValueGeneratedNever();

                entity.Property(e => e.FBackUrl)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.FCode)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.FCreateAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.FDescription).HasMaxLength(255);

                entity.Property(e => e.FErrorUrl)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.FName)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.FSuccessUrl)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.FUpdateAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getutcdate())");
            });

            modelBuilder.Entity<TCurrency>(entity =>
            {
                entity.HasKey(e => e.FCurrencyId)
                    .HasName("pk_TCurrency");

                entity.Property(e => e.FCurrencyId).ValueGeneratedNever();

                entity.Property(e => e.FCode)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.FCreateAt).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.FDescription).HasMaxLength(255);

                entity.Property(e => e.FName)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.FSign)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.FUpdateAt).HasDefaultValueSql("(getutcdate())");
            });

            modelBuilder.Entity<TExchangeRate>(entity =>
            {
                entity.HasKey(e => e.FExchangeRateId)
                    .HasName("pk_TExchangeRate");

                entity.Property(e => e.FExchangeRateId).ValueGeneratedNever();

                entity.Property(e => e.FCreateAt).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.FRate).HasColumnType("decimal(18, 6)");

                entity.Property(e => e.FUpdateAt).HasDefaultValueSql("(getutcdate())");

                entity.HasOne(d => d.FFromCurrency)
                    .WithMany(p => p.TExchangeRateFFromCurrency)
                    .HasForeignKey(d => d.FFromCurrencyId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_TExchangeRate_FFromCurrencyId");

                entity.HasOne(d => d.FToCurrency)
                    .WithMany(p => p.TExchangeRateFToCurrency)
                    .HasForeignKey(d => d.FToCurrencyId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_TExchangeRate_FToCurrencyId");
            });

            modelBuilder.Entity<TPayment>(entity =>
            {
                entity.HasKey(e => e.FPaymentId);

                entity.HasIndex(e => e.FProviderTradeNo);

                entity.HasIndex(e => new { e.FOrderId, e.FPaymentStatus })
                    .HasName("IX_TPayment_FOrderId");

                entity.Property(e => e.FCreateAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.FOrderName).HasMaxLength(255);

                entity.Property(e => e.FPaymentAmount).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.FProviderRefundTime).HasColumnType("datetime");

                entity.Property(e => e.FProviderRefundUrl)
                    .HasMaxLength(128)
                    .IsUnicode(false);

                entity.Property(e => e.FProviderTradeNo)
                    .HasMaxLength(64)
                    .IsUnicode(false);

                entity.Property(e => e.FProviderTradeStatus)
                    .HasMaxLength(32)
                    .IsUnicode(false);

                entity.Property(e => e.FProviderTradeTime).HasColumnType("datetime");

                entity.Property(e => e.FProviderTradeToken).IsUnicode(false);

                entity.Property(e => e.FProviderTradeUrl).IsUnicode(false);

                entity.Property(e => e.FUpdateAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getutcdate())");
            });

            modelBuilder.Entity<TPaymentLog>(entity =>
            {
                entity.HasKey(e => e.FPaymentLogId);

                entity.HasIndex(e => e.FOrderId);

                entity.Property(e => e.FAction)
                    .IsRequired()
                    .HasMaxLength(32)
                    .IsUnicode(false);

                entity.Property(e => e.FClientIP)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.FCreateAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.FOrderId)
                    .IsRequired()
                    .HasMaxLength(32)
                    .IsUnicode(false);

                entity.Property(e => e.FRequest).IsUnicode(false);

                entity.Property(e => e.FResponse).IsUnicode(false);

                entity.Property(e => e.FUpdateAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getutcdate())");
            });

            modelBuilder.Entity<TProduct>(entity =>
            {
                entity.HasKey(e => e.FProductId);

                entity.Property(e => e.FProductId).UseSqlServerIdentityColumn();

                entity.HasIndex(x => x.FCode).HasName("UX_TCode").IsUnique();

                entity.Property(e => e.FCode)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.FCreateAt).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.FDescription).HasMaxLength(255);

                //entity.Property(e => e.FRole).HasConversion<byte>();

                //entity.Property(e => e.FServiceType).HasConversion<int>();

                entity.Property(e => e.FName)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.FUpdateAt).HasDefaultValueSql("(getutcdate())");

                entity.HasOne(d => d.FProductCategory)
                    .WithMany(p => p.TProduct)
                    .HasForeignKey(d => d.FProductCategoryId)
                    .HasConstraintName("FK_TProduct_FProductCategoryId");
            });

            modelBuilder.Entity<TProductCategory>(entity =>
            {
                entity.HasKey(e => e.FProductCategoryId);

                entity.Property(e => e.FProductCategoryId).UseSqlServerIdentityColumn();

                entity.HasIndex(x => x.FCode).HasName("UX_TCategoryCode").IsUnique();

                entity.Property(e => e.FCode)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.FCreateAt).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.FDescription).HasMaxLength(255);

                entity.Property(e => e.FName)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.FUpdateAt).HasDefaultValueSql("(getutcdate())");
            });

            modelBuilder.Entity<TProductSku>(entity =>
            {
                entity.HasKey(e => e.FProductSkuId);

                entity.HasIndex(e => e.FCode)
                    .HasName("UN_TProductSku_FCode")
                    .IsUnique();

                entity.Property(e => e.FBusiness).IsUnicode(false);

                entity.Property(e => e.FCode)
                    .IsRequired()
                    .HasMaxLength(32)
                    .IsUnicode(false);

                entity.Property(e => e.FCreateAt).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.FDescription).HasMaxLength(255);

                entity.Property(e => e.FName)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.FUpdateAt).HasDefaultValueSql("(getutcdate())");

                entity.HasOne(d => d.FProduct)
                    .WithMany(p => p.TProductSku)
                    .HasForeignKey(d => d.FProductId)
                    .HasConstraintName("FK_TProductSku_FProductId");
            });

            modelBuilder.Entity<TProductSkuPrice>(entity =>
            {
                entity.HasKey(e => e.FProductSkuPriceId);

                entity.Property(e => e.FAmount).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.FCreateAt).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.FDescription).HasMaxLength(255);

                entity.Property(e => e.FSaleUnitPrice).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.FUpdateAt).HasDefaultValueSql("(getutcdate())");

                entity.HasOne(d => d.FProductSku)
                    .WithMany(p => p.TProductSkuPrice)
                    .HasForeignKey(d => d.FProductSkuId)
                    .HasConstraintName("FK_TProductSkuPrice_FProductSkuId");
            });

            modelBuilder.Entity<TProvider>(entity =>
            {
                entity.HasKey(e => e.FProviderId);

                entity.Property(e => e.FProviderId).ValueGeneratedNever();

                entity.Property(e => e.FCreateAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.FDescription).HasMaxLength(255);

                entity.Property(e => e.FIconUrl)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.FName)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.FPrefix)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.FSetting).IsUnicode(false);

                entity.Property(e => e.FUpdateAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getutcdate())");
            });

            modelBuilder.Entity<TProviderType>(entity =>
            {
                entity.HasKey(e => e.FProviderTypeId);

                entity.Property(e => e.FProviderTypeId).ValueGeneratedNever();

                entity.Property(e => e.FCode)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.FCreateAt).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.FDescription).HasMaxLength(255);

                entity.Property(e => e.FName)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.FUpdateAt).HasDefaultValueSql("(getutcdate())");
            });

            modelBuilder.Entity<TPurchaseOrder>(entity =>
            {
                entity.HasKey(e => e.FPurchaseOrderId);

                entity.HasIndex(e => e.FUserId);

                entity.HasIndex(e => new { e.FConfirmTime, e.FStatus })
                    .HasName("IX_TPurchaseOrder_FConfirmTime");

                entity.Property(e => e.FPurchaseOrderId).ValueGeneratedNever();

                entity.Property(e => e.FConfirmTime).HasColumnType("datetime");

                entity.Property(e => e.FCreateAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.FEmail)
                    .HasMaxLength(256)
                    .IsUnicode(false);

                entity.Property(e => e.FName).HasMaxLength(255);

                entity.Property(e => e.FPaymentAmount).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.FRefundConfirmTime).HasColumnType("datetime");

                entity.Property(e => e.FSalePrice).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.FUpdateAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getutcdate())");
            });

            modelBuilder.Entity<TPurchaseOrderItem>(entity =>
            {
                entity.HasKey(e => e.FPurchaseOrderItemId);

                entity.HasIndex(e => e.FPurchaseOrderId)
                    .IsUnique();

                entity.Property(e => e.FBusiness).IsUnicode(false);

                entity.Property(e => e.FCreateAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.FPaymentAmount).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.FProductSkuCode)
                    .IsRequired()
                    .HasMaxLength(32)
                    .IsUnicode(false);

                entity.Property(e => e.FProductSkuMappingCode)
                    .HasMaxLength(32)
                    .IsUnicode(false);

                entity.Property(e => e.FProductSkuName)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.FSaleUnitPrice).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.FStartTime)
                    .HasColumnType("date")
                    .HasDefaultValueSql("('1970-01-01')");

                entity.Property(e => e.FStopTime)
                    .HasColumnType("date")
                    .HasDefaultValueSql("('1970-01-01')");

                entity.Property(e => e.FUnitPrice).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.FUpdateAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getutcdate())");

                entity.HasOne(d => d.FPurchaseOrder)
                    .WithMany(p => p.TPurchaseOrderItem)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TPurchaseOrderItem_TPurchaseOrder");
            });

            modelBuilder.Entity<TReconcile>(entity =>
            {
                entity.HasKey(e => e.FReconcileId);

                entity.Property(e => e.FBeginTime).HasColumnType("datetime");

                entity.Property(e => e.FCreateAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.FEndTime).HasColumnType("datetime");

                entity.Property(e => e.FUpdateAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getutcdate())");
            });

            modelBuilder.Entity<TReconcileItem>(entity =>
            {
                entity.HasKey(e => e.FReconcileItemId);

                entity.HasIndex(e => e.FOrderId);

                entity.HasIndex(e => e.FProviderNo);

                entity.Property(e => e.FCreateAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.FDetail)
                    .IsRequired()
                    .IsUnicode(false);

                entity.Property(e => e.FOrderId)
                    .IsRequired()
                    .HasMaxLength(32)
                    .IsUnicode(false);

                entity.Property(e => e.FProviderAmount)
                    .IsRequired()
                    .HasMaxLength(32)
                    .IsUnicode(false);

                entity.Property(e => e.FProviderCurrency)
                    .IsRequired()
                    .HasMaxLength(4)
                    .IsUnicode(false);

                entity.Property(e => e.FProviderNo)
                    .HasMaxLength(64)
                    .IsUnicode(false);

                entity.Property(e => e.FProviderStatus)
                    .HasMaxLength(32)
                    .IsUnicode(false);

                entity.Property(e => e.FProviderType)
                    .IsRequired()
                    .HasMaxLength(32)
                    .IsUnicode(false);

                entity.Property(e => e.FUpdateAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getutcdate())");

                entity.HasOne(d => d.FReconcile)
                    .WithMany(p => p.TReconcileItem)
                    .HasForeignKey(d => d.FReconcileId)
                    .HasConstraintName("fk_TReconcileItem_FReconcileId");
            });

            modelBuilder.Entity<TSequence>(entity =>
            {
                entity.HasKey(e => e.FSequenceId)
                    .HasName("pk_TSequence");

                entity.Property(e => e.FSequenceId).ValueGeneratedNever();

                entity.Property(e => e.FCreateAt).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.FPrefix)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.FRowVersion)
                    .IsRequired()
                    .IsRowVersion();

                entity.Property(e => e.FUpdateAt).HasDefaultValueSql("(getutcdate())");
            });

            modelBuilder.Entity<TSerialNo>(entity =>
            {
                entity.HasKey(e => e.FType);

                entity.Property(e => e.FType).ValueGeneratedNever();

                entity.Property(e => e.FCreateAt).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.FUpdateAt).HasDefaultValueSql("(getutcdate())");
            });
            modelBuilder.Entity<TInvoice>(entity =>
            {
                entity.HasKey(e => e.FInvoiceId);

                entity.Property(e => e.FAddress).HasMaxLength(256);

                entity.Property(e => e.FBank).HasMaxLength(32);

                entity.Property(e => e.FBankAccount)
                    .HasMaxLength(32)
                    .IsUnicode(false);

                entity.Property(e => e.FCompanyName)
                    .IsRequired()
                    .HasMaxLength(64);

                entity.Property(e => e.FContact)
                    .IsRequired()
                    .HasMaxLength(16);

                entity.Property(e => e.FCreateAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.FExpressAddress)
                    .IsRequired()
                    .HasMaxLength(256);

                entity.Property(e => e.FInvoiceEmail)
                    .IsRequired()
                    .HasMaxLength(256)
                    .IsUnicode(false);

                entity.Property(e => e.FPhone)
                    .IsRequired()
                    .HasMaxLength(32)
                    .IsUnicode(false);

                entity.Property(e => e.FTaxNo)
                    .IsRequired()
                    .HasMaxLength(32)
                    .IsUnicode(false);

                entity.Property(e => e.FTaxPayerCertificateUrl)
                    .HasMaxLength(256)
                    .IsUnicode(false);

                entity.Property(e => e.FTelephone)
                    .HasMaxLength(32)
                    .IsUnicode(false);

                entity.Property(e => e.FUpdateAt).HasColumnType("datetime");
            });

            modelBuilder.Entity<TInvoiceApply>(entity =>
            {
                entity.HasKey(e => e.FInvoiceApplyId);

                entity.Property(e => e.FInvoiceApplyId).ValueGeneratedNever();

                entity.Property(e => e.FAmount).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.FCreateAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.FHandleTime).HasColumnType("datetime");

                entity.Property(e => e.FRejectReason).HasMaxLength(64);

                entity.Property(e => e.FRemark).HasMaxLength(50);

                entity.Property(e => e.FSendInfo).HasMaxLength(128);

                entity.Property(e => e.FUpdateAt).HasColumnType("datetime");
            });

            modelBuilder.Entity<TInvoiceApplyExtra>(entity =>
            {
                entity.HasKey(e => e.FInvoiceApplyId);

                entity.Property(e => e.FInvoiceApplyId).ValueGeneratedNever();

                entity.Property(e => e.FAddress).HasMaxLength(256);

                entity.Property(e => e.FBank).HasMaxLength(32);

                entity.Property(e => e.FBankAccount)
                    .HasMaxLength(32)
                    .IsUnicode(false);

                entity.Property(e => e.FCompanyName)
                    .IsRequired()
                    .HasMaxLength(64);

                entity.Property(e => e.FContact)
                    .IsRequired()
                    .HasMaxLength(16);

                entity.Property(e => e.FCreateAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.FExpressAddress)
                    .IsRequired()
                    .HasMaxLength(256);

                entity.Property(e => e.FInvoiceEmail)
                    .IsRequired()
                    .HasMaxLength(256)
                    .IsUnicode(false);

                entity.Property(e => e.FPhone)
                    .IsRequired()
                    .HasMaxLength(32)
                    .IsUnicode(false);

                entity.Property(e => e.FTaxNo)
                    .IsRequired()
                    .HasMaxLength(32)
                    .IsUnicode(false);

                entity.Property(e => e.FTaxPayerCertificateUrl)
                    .HasMaxLength(256)
                    .IsUnicode(false);
                entity.Property(e => e.FTelephone)

                    .HasMaxLength(32)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<TInvoiceApplyPayment>(entity =>
            {
                entity.HasKey(e => e.FId);

                entity.Property(e => e.FId).ValueGeneratedNever();

                entity.Property(e => e.FPaymentCreateTime).HasColumnType("datetime");

                entity.Property(e => e.FOrderName)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.FPaymentAmount).HasColumnType("decimal(18, 2)");
            });

            modelBuilder.Entity<TOfflinePayment>(entity =>
            {
                entity.HasKey(e => e.FOfflinePaymentId);

                entity.Property(e => e.FOfflinePaymentId).ValueGeneratedNever();

                entity.Property(e => e.FAmount).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.FCreateAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.FHandleTime).HasColumnType("datetime");

                entity.Property(e => e.FRejectReason).HasMaxLength(1024);

                entity.Property(e => e.FRemark).HasMaxLength(50);

                entity.Property(e => e.FTransferNo)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.FTransferPhotoUrl)
                    .IsRequired()
                    .HasMaxLength(256)
                    .IsUnicode(false);

                entity.Property(e => e.FUpdateAt).HasColumnType("datetime");
            });

            modelBuilder.Entity<TOfflinePaymentOrder>(entity =>
            {
                entity.HasKey(e => e.FId);

                entity.Property(e => e.FId).ValueGeneratedNever();

                entity.Property(e => e.FEffectiveTime).HasColumnType("datetime");

                entity.Property(e => e.FProductSkuName)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.FUnitPrice).HasColumnType("decimal(18, 2)");
            });
        }
        public override int SaveChanges()
        {
            OnBeforeSaving();
            return base.SaveChanges();
        }

        /// <summary>
        /// 保存携带操作人/操作日期
        /// </summary>
        /// <param name="operatorId"></param>
        /// <returns></returns>
        public int SaveChanges(int operatorId)
        {
            OnBeforeSaving(operatorId);
            return base.SaveChanges();
        }

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            OnBeforeSaving();
            return base.SaveChanges(acceptAllChangesOnSuccess);
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            OnBeforeSaving();
            return await base.SaveChangesAsync(cancellationToken);
        }

        /// <summary>
        /// 保存携带操作人/操作日期
        /// </summary>
        /// <param name="operatorId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<int> SaveChangesAsync(int operatorId, CancellationToken cancellationToken = new CancellationToken())
        {
            OnBeforeSaving(operatorId);
            return await base.SaveChangesAsync(cancellationToken);
        }


        public override async Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = new CancellationToken())
        {
            OnBeforeSaving();
            return await base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        private void OnBeforeSaving(long? operatorId = null)
        {
            var entries = ChangeTracker.Entries();
            foreach (var entry in entries)
            {

                var now = DateTime.UtcNow;

                switch (entry.State)
                {
                    case EntityState.Modified:
                        if (entry.Properties.Any(a => a.Metadata.Name == "FUpdateAt"))
                        {
                            entry.Property("FUpdateAt").CurrentValue = now;
                        }
                        if (operatorId.HasValue && entry.Properties.Any(a => a.Metadata.Name == "FUpdateBy"))
                        {
                            entry.Property("FUpdateBy").CurrentValue = operatorId.Value;
                        }
                        break;
                    case EntityState.Added:
                        if (entry.Properties.Any(a => a.Metadata.Name == "FCreateAt"))
                        {
                            entry.Property("FCreateAt").CurrentValue = now;
                        }
                        if (entry.Properties.Any(a => a.Metadata.Name == "FUpdateAt"))
                        {
                            entry.Property("FUpdateAt").CurrentValue = now;
                        }

                        if (operatorId.HasValue)
                        {
                            if (entry.Properties.Any(a => a.Metadata.Name == "FCreateBy"))
                            {
                                entry.Property("FCreateBy").CurrentValue = operatorId.Value;
                            }
                            if (entry.Properties.Any(a => a.Metadata.Name == "FUpdateBy"))
                            {
                                entry.Property("FUpdateBy").CurrentValue = operatorId.Value;
                            }
                        }

                        break;
                }

            }
        }
    }
}
