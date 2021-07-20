using Microsoft.EntityFrameworkCore;
using YQTrack.Core.Backend.Admin.Freight.Data.Models;

namespace YQTrack.Core.Backend.Admin.Freight.Data
{
    public partial class CarrierContext : DbContext
    {
        public CarrierContext()
        {
        }

        public CarrierContext(DbContextOptions<CarrierContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Tchannel> Tchannel { get; set; }
        public virtual DbSet<TchannelDraftBox> TchannelDraftBox { get; set; }
        public virtual DbSet<TchannelExtra> TchannelExtra { get; set; }
        public virtual DbSet<TchannelFreight> TchannelFreight { get; set; }
        public virtual DbSet<TchannelReportRecord> TchannelReportRecord { get; set; }
        public virtual DbSet<TchannelStatistics> TchannelStatistics { get; set; }
        public virtual DbSet<Tcompany> Tcompany { get; set; }
        public virtual DbSet<TcompanyBusiness> TcompanyBusiness { get; set; }
        public virtual DbSet<TcompanyConfig> TcompanyConfig { get; set; }
        public virtual DbSet<TcompanyStatistics> TcompanyStatistics { get; set; }
        public virtual DbSet<TinquiryOrder> TinquiryOrder { get; set; }
        public virtual DbSet<TinquiryOrderView> TinquiryOrderView { get; set; }
        public virtual DbSet<TorderNoGenerate> TorderNoGenerate { get; set; }
        public virtual DbSet<TquoteOrder> TquoteOrder { get; set; }
        public virtual DbSet<TInquiryOrderStatusLog> TInquiryOrderStatusLog { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("ProductVersion", "2.2.0-rtm-35687");

            modelBuilder.Entity<Tchannel>(entity =>
            {
                entity.HasKey(e => e.FchannelId);

                entity.ToTable("TChannel");

                entity.Property(e => e.FchannelId)
                    .HasColumnName("FChannelId")
                    .ValueGeneratedNever();

                entity.Property(e => e.FavgDay)
                    .HasColumnName("FAvgDay")
                    .HasColumnType("decimal(9, 4)");

                entity.Property(e => e.FchannelDesc)
                    .HasColumnName("FChannelDesc")
                    .HasMaxLength(200);

                entity.Property(e => e.FchannelSubTypeId).HasColumnName("FChannelSubTypeId");

                entity.Property(e => e.FchannelTitle)
                    .IsRequired()
                    .HasColumnName("FChannelTitle")
                    .HasMaxLength(50);

                entity.Property(e => e.FcheckState).HasColumnName("FCheckState");

                entity.Property(e => e.FcompanyId).HasColumnName("FCompanyId");

                entity.Property(e => e.Fcountrys)
                    .HasColumnName("FCountrys")
                    .HasMaxLength(1500)
                    .IsUnicode(false);

                entity.Property(e => e.FcreateBy).HasColumnName("FCreateBy");

                entity.Property(e => e.FcreateTime)
                    .HasColumnName("FCreateTime")
                    .HasColumnType("smalldatetime");

                entity.Property(e => e.FexpireTime)
                    .HasColumnName("FExpireTime")
                    .HasColumnType("datetime");

                entity.Property(e => e.FisProcessed).HasColumnName("FIsProcessed");

                entity.Property(e => e.FmaxDay).HasColumnName("FMaxDay");

                entity.Property(e => e.FminDay).HasColumnName("FMinDay");

                entity.Property(e => e.FoneKgprice)
                    .HasColumnName("FOneKGPrice")
                    .HasColumnType("decimal(9, 4)");

                entity.Property(e => e.FproductType).HasColumnName("FProductType");

                entity.Property(e => e.FpublishTime)
                    .HasColumnName("FPublishTime")
                    .HasColumnType("smalldatetime");

                entity.Property(e => e.Fstate).HasColumnName("FState");

                entity.Property(e => e.FtransTypes)
                    .HasColumnName("FTransTypes")
                    .HasMaxLength(128)
                    .IsUnicode(false);

                entity.Property(e => e.FupdateBy).HasColumnName("FUpdateBy");

                entity.Property(e => e.FupdateTime)
                    .HasColumnName("FUpdateTime")
                    .HasColumnType("smalldatetime");

                entity.Property(e => e.FupdateTimestamp)
                    .IsRequired()
                    .HasColumnName("FUpdateTimestamp")
                    .IsRowVersion();

                entity.Property(e => e.FvalidReportTimes).HasColumnName("FValidReportTimes");
            });

            modelBuilder.Entity<TchannelDraftBox>(entity =>
            {
                entity.HasKey(e => e.FchannelId);

                entity.ToTable("TChannelDraftBox");

                entity.Property(e => e.FchannelId)
                    .HasColumnName("FChannelId")
                    .ValueGeneratedNever();

                entity.Property(e => e.FchannelTitle)
                    .IsRequired()
                    .HasColumnName("FChannelTitle")
                    .HasMaxLength(50);

                entity.Property(e => e.FcompanyId).HasColumnName("FCompanyId");

                entity.Property(e => e.FcreateBy).HasColumnName("FCreateBy");

                entity.Property(e => e.FcreateTime)
                    .HasColumnName("FCreateTime")
                    .HasColumnType("smalldatetime");

                entity.Property(e => e.Fjson).HasColumnName("FJson");

                entity.Property(e => e.FupdateBy).HasColumnName("FUpdateBy");

                entity.Property(e => e.FupdateTime)
                    .HasColumnName("FUpdateTime")
                    .HasColumnType("smalldatetime");
            });

            modelBuilder.Entity<TchannelExtra>(entity =>
            {
                entity.HasKey(e => e.FchannelId);

                entity.ToTable("TChannelExtra");

                entity.Property(e => e.FchannelId)
                    .HasColumnName("FChannelId")
                    .ValueGeneratedNever();

                entity.Property(e => e.FadditionalFees)
                    .HasColumnName("FAdditionalFees")
                    .HasMaxLength(300);

                entity.Property(e => e.Fclaim)
                    .HasColumnName("FClaim")
                    .HasMaxLength(300);

                entity.Property(e => e.FcombineRemark).HasColumnName("FCombineRemark");

                entity.Property(e => e.FcreateBy).HasColumnName("FCreateBy");

                entity.Property(e => e.FcreateTime)
                    .HasColumnName("FCreateTime")
                    .HasColumnType("smalldatetime");

                entity.Property(e => e.Finvoice)
                    .HasColumnName("FInvoice")
                    .HasMaxLength(300);

                entity.Property(e => e.FlongOverweight)
                    .HasColumnName("FLongOverweight")
                    .HasMaxLength(300);

                entity.Property(e => e.Fother)
                    .HasColumnName("FOther")
                    .HasMaxLength(300);

                entity.Property(e => e.Fpackage)
                    .HasColumnName("FPackage")
                    .HasMaxLength(300);

                entity.Property(e => e.Fpiling)
                    .HasColumnName("FPiling")
                    .HasMaxLength(300);

                entity.Property(e => e.Freturn)
                    .HasColumnName("FReturn")
                    .HasMaxLength(300);

                entity.Property(e => e.FupdateBy).HasColumnName("FUpdateBy");

                entity.Property(e => e.FupdateTime)
                    .HasColumnName("FUpdateTime")
                    .HasColumnType("smalldatetime");
            });

            modelBuilder.Entity<TchannelFreight>(entity =>
            {
                entity.HasKey(e => e.FchannelId);

                entity.ToTable("TChannelFreight");

                entity.Property(e => e.FchannelId)
                    .HasColumnName("FChannelId")
                    .ValueGeneratedNever();

                entity.Property(e => e.FcreateBy).HasColumnName("FCreateBy");

                entity.Property(e => e.FcreateTime)
                    .HasColumnName("FCreateTime")
                    .HasColumnType("smalldatetime");

                entity.Property(e => e.FfirstPrice)
                    .HasColumnName("FFirstPrice")
                    .HasColumnType("decimal(9, 4)");

                entity.Property(e => e.FfirstWeight).HasColumnName("FFirstWeight");

                entity.Property(e => e.FfreightIntervals)
                    .HasColumnName("FFreightIntervals")
                    .HasMaxLength(4000);

                entity.Property(e => e.FfreightType).HasColumnName("FFreightType");

                entity.Property(e => e.FlimitWeight).HasColumnName("FLimitWeight");

                entity.Property(e => e.FoperateCost)
                    .HasColumnName("FOperateCost")
                    .HasColumnType("decimal(9, 4)");

                entity.Property(e => e.FupdateBy).HasColumnName("FUpdateBy");

                entity.Property(e => e.FupdateTime)
                    .HasColumnName("FUpdateTime")
                    .HasColumnType("smalldatetime");
            });

            modelBuilder.Entity<TchannelReportRecord>(entity =>
            {
                entity.HasKey(e => e.Fid)
                    .HasName("PK_ChannelReportRecord");

                entity.ToTable("TChannelReportRecord");

                entity.Property(e => e.Fid)
                    .HasColumnName("FId")
                    .ValueGeneratedNever();

                entity.Property(e => e.FchannelId).HasColumnName("FChannelId");

                entity.Property(e => e.FcompanyId).HasColumnName("FCompanyId");

                entity.Property(e => e.FcreateBy).HasColumnName("FCreateBy");

                entity.Property(e => e.FcreateTime)
                    .HasColumnName("FCreateTime")
                    .HasColumnType("datetime");

                entity.Property(e => e.Fdetail)
                    .IsRequired()
                    .HasColumnName("FDetail");

                entity.Property(e => e.FisJobProcessed).HasColumnName("FIsJobProcessed");

                entity.Property(e => e.FprocessDescription).HasColumnName("FProcessDescription");

                entity.Property(e => e.FprocessRemark).HasColumnName("FProcessRemark");

                entity.Property(e => e.FprocessStatus).HasColumnName("FProcessStatus");

                entity.Property(e => e.FprocessTime)
                    .HasColumnName("FProcessTime")
                    .HasColumnType("datetime");

                entity.Property(e => e.FreasonType).HasColumnName("FReasonType");

                entity.Property(e => e.FreportEmail)
                    .IsRequired()
                    .HasColumnName("FReportEmail")
                    .HasMaxLength(128)
                    .IsUnicode(false);

                entity.Property(e => e.FreportTime)
                    .HasColumnName("FReportTime")
                    .HasColumnType("datetime");

                entity.Property(e => e.FreportUserId).HasColumnName("FReportUserId");

                entity.Property(e => e.FreportUserLanguage)
                    .HasColumnName("FReportUserLanguage")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.FreportUserNickName)
                    .HasColumnName("FReportUserNickName")
                    .HasMaxLength(50);

                entity.Property(e => e.FreportUserRole).HasColumnName("FReportUserRole");

                entity.Property(e => e.FupdateBy).HasColumnName("FUpdateBy");

                entity.Property(e => e.FupdateTime)
                    .HasColumnName("FUpdateTime")
                    .HasColumnType("datetime");
            });

            modelBuilder.Entity<TchannelStatistics>(entity =>
            {
                entity.HasKey(e => e.FchannelId);

                entity.ToTable("TChannelStatistics");

                entity.Property(e => e.FchannelId)
                    .HasColumnName("FChannelId")
                    .ValueGeneratedNever();

                entity.Property(e => e.FcreateBy).HasColumnName("FCreateBy");

                entity.Property(e => e.FcreateTime)
                    .HasColumnName("FCreateTime")
                    .HasColumnType("datetime");

                entity.Property(e => e.FupdateBy).HasColumnName("FUpdateBy");

                entity.Property(e => e.FupdateTime)
                    .HasColumnName("FUpdateTime")
                    .HasColumnType("datetime");
            });

            modelBuilder.Entity<Tcompany>(entity =>
            {
                entity.HasKey(e => e.FcompanyId);

                entity.ToTable("TCompany");

                entity.Property(e => e.FcompanyId)
                    .HasColumnName("FCompanyId")
                    .ValueGeneratedNever();

                entity.Property(e => e.Faddress)
                    .IsRequired()
                    .HasColumnName("FAddress")
                    .HasMaxLength(100);

                entity.Property(e => e.Farea)
                    .IsRequired()
                    .HasColumnName("FArea")
                    .HasMaxLength(8)
                    .IsUnicode(false);

                entity.Property(e => e.FchannelValidReportTimes).HasColumnName("FChannelValidReportTimes");

                entity.Property(e => e.FcheckDesc).HasColumnName("FCheckDesc");

                entity.Property(e => e.FcheckDescHistory).HasColumnName("FCheckDescHistory");

                entity.Property(e => e.FcheckState).HasColumnName("FCheckState");

                entity.Property(e => e.FcheckTime)
                    .HasColumnName("FCheckTime")
                    .HasColumnType("datetime");

                entity.Property(e => e.Fcode)
                    .HasColumnName("FCode")
                    .HasMaxLength(32)
                    .IsUnicode(false);

                entity.Property(e => e.FcompanyName)
                    .IsRequired()
                    .HasColumnName("FCompanyName")
                    .HasMaxLength(50);

                entity.Property(e => e.Fcontact)
                    .IsRequired()
                    .HasColumnName("FContact")
                    .HasMaxLength(10);

                entity.Property(e => e.FcreateBy).HasColumnName("FCreateBy");

                entity.Property(e => e.FcreateTime)
                    .HasColumnName("FCreateTime")
                    .HasColumnType("datetime");

                entity.Property(e => e.Femail)
                    .IsRequired()
                    .HasColumnName("FEmail")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Fimg)
                    .IsRequired()
                    .HasColumnName("FImg")
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.Finfo)
                    .IsRequired()
                    .HasColumnName("FInfo")
                    .HasMaxLength(50);

                entity.Property(e => e.Flogo)
                    .IsRequired()
                    .HasColumnName("FLogo")
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.Fmobile)
                    .IsRequired()
                    .HasColumnName("FMobile")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Fqq)
                    .IsRequired()
                    .HasColumnName("FQQ")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Fremark)
                    .IsRequired()
                    .HasColumnName("FRemark")
                    .HasMaxLength(600);

                entity.Property(e => e.Fscale).HasColumnName("FScale");

                entity.Property(e => e.Ftelphone)
                    .IsRequired()
                    .HasColumnName("FTelphone")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.FupdateBy).HasColumnName("FUpdateBy");

                entity.Property(e => e.FupdateTime)
                    .HasColumnName("FUpdateTime")
                    .HasColumnType("smalldatetime");

                entity.Property(e => e.FupdateTimestamp)
                    .IsRequired()
                    .HasColumnName("FUpdateTimestamp")
                    .IsRowVersion();

                entity.Property(e => e.Furl)
                    .IsRequired()
                    .HasColumnName("FUrl")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.FuserId).HasColumnName("FUserId");
            });

            modelBuilder.Entity<TcompanyBusiness>(entity =>
            {
                entity.HasKey(e => e.FcompanyId)
                    .HasName("PK_TCommanyBusiness");

                entity.ToTable("TCompanyBusiness");

                entity.Property(e => e.FcompanyId)
                    .HasColumnName("FCompanyId")
                    .ValueGeneratedNever();

                entity.Property(e => e.Fcitys).HasColumnName("FCitys");

                entity.Property(e => e.FcreateBy).HasColumnName("FCreateBy");

                entity.Property(e => e.FcreateTime)
                    .HasColumnName("FCreateTime")
                    .HasColumnType("smalldatetime");

                entity.Property(e => e.FmainBusiness)
                    .HasColumnName("FMainBusiness")
                    .HasMaxLength(100);

                entity.Property(e => e.Fprovinces)
                    .HasColumnName("FProvinces")
                    .HasMaxLength(128);

                entity.Property(e => e.FupdateBy).HasColumnName("FUpdateBy");

                entity.Property(e => e.FupdateTime)
                    .HasColumnName("FUpdateTime")
                    .HasColumnType("smalldatetime");

                entity.Property(e => e.FupdateTimestamp)
                    .IsRequired()
                    .HasColumnName("FUpdateTimestamp")
                    .IsRowVersion();
            });

            modelBuilder.Entity<TcompanyConfig>(entity =>
            {
                entity.HasKey(e => e.FcompanyId);

                entity.ToTable("TCompanyConfig");

                entity.Property(e => e.FcompanyId)
                    .HasColumnName("FCompanyId")
                    .ValueGeneratedNever();

                entity.Property(e => e.FcreateBy).HasColumnName("FCreateBy");

                entity.Property(e => e.FcreateTime)
                    .HasColumnName("FCreateTime")
                    .HasColumnType("smalldatetime");

                entity.Property(e => e.FmaxChannel).HasColumnName("FMaxChannel");

                entity.Property(e => e.FupdateBy).HasColumnName("FUpdateBy");

                entity.Property(e => e.FupdateTime).HasColumnName("FUpdateTime");
            });

            modelBuilder.Entity<TcompanyStatistics>(entity =>
            {
                entity.HasKey(e => e.FcompanyId);

                entity.ToTable("TCompanyStatistics");

                entity.Property(e => e.FcompanyId)
                    .HasColumnName("FCompanyId")
                    .ValueGeneratedNever();

                entity.Property(e => e.FcreateBy).HasColumnName("FCreateBy");

                entity.Property(e => e.FcreateTime)
                    .HasColumnName("FCreateTime")
                    .HasColumnType("datetime");

                entity.Property(e => e.FupdateBy).HasColumnName("FUpdateBy");

                entity.Property(e => e.FupdateTime)
                    .HasColumnName("FUpdateTime")
                    .HasColumnType("datetime");
            });

            modelBuilder.Entity<TinquiryOrder>(entity =>
            {
                entity.HasKey(e => e.ForderId)
                    .HasName("PK_TOrderId");

                entity.ToTable("TInquiryOrder");

                entity.Property(e => e.ForderId)
                    .HasColumnName("FOrderId")
                    .ValueGeneratedNever();

                entity.Property(e => e.FautoStopTime)
                    .HasColumnName("FAutoStopTime")
                    .HasColumnType("datetime");

                entity.Property(e => e.FcontactInfo)
                    .HasColumnName("FContactInfo")
                    .HasMaxLength(1024);

                entity.Property(e => e.FcreateBy).HasColumnName("FCreateBy");

                entity.Property(e => e.FcreateTime)
                    .HasColumnName("FCreateTime")
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.FdeleteFlag).HasColumnName("FDeleteFlag");

                entity.Property(e => e.FdeliveryCountry).HasColumnName("FDeliveryCountry");

                entity.Property(e => e.FdeliveryDate)
                    .HasColumnName("FDeliveryDate")
                    .HasColumnType("datetime");

                entity.Property(e => e.FexpireDate)
                    .HasColumnName("FExpireDate")
                    .HasColumnType("datetime");

                entity.Property(e => e.FstatusTime)
                    .HasColumnName("FStatusTime")
                    .HasColumnType("datetime");

                entity.Property(e => e.FinquiryOrderNo)
                    .IsRequired()
                    .HasColumnName("FInquiryOrderNo")
                    .HasMaxLength(32)
                    .IsUnicode(false);

                entity.Property(e => e.FisNewQuote)
                    .HasColumnName("FIsNewQuote")
                    .HasColumnType("bit");

                entity.Property(e => e.FlogisticsRequire)
                    .IsRequired()
                    .HasColumnName("FLogisticsRequire");

                entity.Property(e => e.FloveCarrierId).HasColumnName("FLoveCarrierId");

                entity.Property(e => e.FloveQuoteId).HasColumnName("FLoveQuoteId");

                entity.Property(e => e.Fother)
                    .HasColumnName("FOther")
                    .HasMaxLength(1024);

                entity.Property(e => e.FpackageCity).HasColumnName("FPackageCity");

                entity.Property(e => e.FprocessTime)
                    .HasColumnName("FProcessTime")
                    .HasColumnType("datetime");

                entity.Property(e => e.FquoterCount).HasColumnName("FQuoterCount");

                entity.Property(e => e.FrejectReason)
                    .HasColumnName("FRejectReason")
                    .HasMaxLength(512);

                entity.Property(e => e.Fstatus).HasColumnName("FStatus");

                entity.Property(e => e.FstopSelfEnum).HasColumnName("FStopSelfEnum");

                entity.Property(e => e.FstopSelfReason)
                    .HasColumnName("FStopSelfReason")
                    .HasMaxLength(512);

                entity.Property(e => e.Ftitle)
                    .IsRequired()
                    .HasColumnName("FTitle")
                    .HasMaxLength(256);

                entity.Property(e => e.FupdateBy).HasColumnName("FUpdateBy");

                entity.Property(e => e.FupdateTime)
                    .HasColumnName("FUpdateTime")
                    .HasColumnType("datetime");

                entity.Property(e => e.FuserId).HasColumnName("FUserId");

                entity.Property(e => e.FuserUniqueId)
                    .IsRequired()
                    .HasColumnName("FUserUniqueId")
                    .HasMaxLength(32)
                    .IsUnicode(false);

                entity.Property(e => e.FviewerCount).HasColumnName("FViewerCount");
            });

            modelBuilder.Entity<TinquiryOrderView>(entity =>
            {
                entity.HasKey(e => e.FviewId)
                    .HasName("PK_TViewId");

                entity.ToTable("TInquiryOrderView");

                entity.Property(e => e.FviewId)
                    .HasColumnName("FViewId")
                    .ValueGeneratedNever();

                entity.Property(e => e.FcreateBy).HasColumnName("FCreateBy");

                entity.Property(e => e.FcreateTime)
                    .HasColumnName("FCreateTime")
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.Femail)
                    .HasColumnName("FEmail")
                    .HasMaxLength(256)
                    .IsUnicode(false);

                entity.Property(e => e.ForderId).HasColumnName("FOrderId");

                entity.Property(e => e.FupdateBy).HasColumnName("FUpdateBy");

                entity.Property(e => e.FupdateTime)
                    .HasColumnName("FUpdateTime")
                    .HasColumnType("datetime");

                entity.Property(e => e.FuserId).HasColumnName("FUserId");

                entity.Property(e => e.FuserRole).HasColumnName("FUserRole");
            });

            modelBuilder.Entity<TorderNoGenerate>(entity =>
            {
                entity.HasKey(e => e.FgenerateId)
                    .HasName("PK_TGenerateId");

                entity.ToTable("TOrderNoGenerate");

                entity.Property(e => e.FgenerateId)
                    .HasColumnName("FGenerateId")
                    .ValueGeneratedNever();

                entity.Property(e => e.FcreateBy).HasColumnName("FCreateBy");

                entity.Property(e => e.FcreateTime)
                    .HasColumnName("FCreateTime")
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.Fprefix)
                    .IsRequired()
                    .HasColumnName("FPrefix")
                    .HasMaxLength(16)
                    .IsUnicode(false);

                entity.Property(e => e.Fsequence).HasColumnName("FSequence");

                entity.Property(e => e.Ftype)
                    .IsRequired()
                    .HasColumnName("FType")
                    .HasMaxLength(2)
                    .IsUnicode(false);

                entity.Property(e => e.FupdateBy).HasColumnName("FUpdateBy");

                entity.Property(e => e.FupdateTime)
                    .HasColumnName("FUpdateTime")
                    .HasColumnType("datetime");
            });

            modelBuilder.Entity<TquoteOrder>(entity =>
            {
                entity.HasKey(e => e.FquoteId)
                    .HasName("PK_TQuoteId");

                entity.ToTable("TQuoteOrder");

                entity.Property(e => e.FquoteId)
                    .HasColumnName("FQuoteId")
                    .ValueGeneratedNever();

                entity.Property(e => e.Fcancel)
                    .HasColumnName("FCancel")
                    .HasColumnType("bit");

                entity.Property(e => e.Fviewed)
                    .HasColumnName("FViewed")
                    .HasColumnType("bit");

                entity.Property(e => e.FupdateTime)
                    .HasColumnName("FCancelTime")
                    .HasColumnType("datetime");

                entity.Property(e => e.FcancelReason)
                    .HasColumnName("FCancelReason")
                    .HasMaxLength(256);

                entity.Property(e => e.FcompanyId).HasColumnName("FCompanyId");

                entity.Property(e => e.FcompanyName)
                    .IsRequired()
                    .HasColumnName("FCompanyName")
                    .HasMaxLength(50);

                entity.Property(e => e.Fcontent)
                    .IsRequired()
                    .HasColumnName("FContent");

                entity.Property(e => e.FcreateBy).HasColumnName("FCreateBy");

                entity.Property(e => e.FcreateTime)
                    .HasColumnName("FCreateTime")
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.FinquiryOrderNo)
                    .IsRequired()
                    .HasColumnName("FInquiryOrderNo")
                    .HasMaxLength(32)
                    .IsUnicode(false);

                entity.Property(e => e.ForderId).HasColumnName("FOrderId");

                entity.Property(e => e.FquoteOrderNo)
                    .IsRequired()
                    .HasColumnName("FQuoteOrderNo")
                    .HasMaxLength(32)
                    .IsUnicode(false);

                entity.Property(e => e.Fremark)
                    .HasColumnName("FRemark")
                    .HasMaxLength(500);

                entity.Property(e => e.FupdateBy).HasColumnName("FUpdateBy");

                entity.Property(e => e.FupdateTime)
                    .HasColumnName("FUpdateTime")
                    .HasColumnType("datetime");

                entity.Property(e => e.FuserId).HasColumnName("FUserId");
            });

            // 添加对询价单状态变更记录表数据库映射关系
            modelBuilder.Entity<TInquiryOrderStatusLog>(entity =>
            {
                entity.ToTable("TInquiryOrderStatusLog");

                entity.HasKey(x => x.FId);

                entity.Property(x => x.FId)
                    .IsRequired()
                    .UseSqlServerIdentityColumn()
                    .HasColumnName("FId")
                    .HasColumnType("bigint");

                entity.Property(x => x.FOrderId)
                    .IsRequired()
                    .HasColumnName("FOrderId")
                    .HasColumnType("bigint");

                entity.Property(x => x.FFrom)
                    .IsRequired()
                    .HasColumnName("FFrom")
                    .HasColumnType("tinyint")
                    .HasConversion<byte>();

                entity.Property(x => x.FTo)
                    .IsRequired()
                    .HasColumnName("FTo")
                    .HasColumnType("tinyint")
                    .HasConversion<byte>();

                entity.Property(x => x.FDesc)
                    .IsRequired()
                    .HasColumnName("FDesc")
                    .HasMaxLength(128);

                entity.Property(e => e.FCreateTime)
                    .IsRequired()
                    .HasColumnName("FCreateTime")
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.FCreateBy)
                    .IsRequired()
                    .HasColumnType("bigint")
                    .HasColumnName("FCreateBy");
            });

        }
    }
}
