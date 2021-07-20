using Microsoft.EntityFrameworkCore;
using YQTrack.Core.Backend.Admin.User.Data.Models;

namespace YQTrack.Core.Backend.Admin.User.Data
{
    public partial class UserDbContext : DbContext
    {
        public UserDbContext()
        {
        }

        public UserDbContext(DbContextOptions<UserDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<TuserInfo> TuserInfo { get; set; }

        public virtual DbSet<TuserProfile> TuserProfile { get; set; }

        public virtual DbSet<TuserFeedback> TuserFeedback { get; set; }

        public virtual DbSet<TuserDevice> TuserDevice { get; set; }
        public virtual DbSet<TuserMemberInfo> TuserMemberInfo { get; set; }
        public virtual DbSet<TUserUnRegisterInfo> TUserUnRegisterInfo { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("ProductVersion", "2.2.2-servicing-10034");

            modelBuilder.Entity<TuserInfo>(entity =>
            {
                entity.HasKey(e => e.FuserId);

                entity.ToTable("TUserInfo");

                entity.HasIndex(e => e.Femail)
                    .IsUnique();

                entity.HasIndex(e => new { e.FnodeId, e.FdbNo, e.FuserRole, e.FuserId })
                    .HasName("IX_TUserInfo_FUserRole");

                entity.Property(e => e.FuserId)
                    .HasColumnName("FUserId")
                    .ValueGeneratedNever();

                entity.Property(e => e.FcreateBy).HasColumnName("FCreateBy");

                entity.Property(e => e.FcreateTime)
                    .HasColumnName("FCreateTime")
                    .HasColumnType("datetime");

                entity.Property(e => e.FdbNo).HasColumnName("FDbNo");

                entity.Property(e => e.Femail)
                    .IsRequired()
                    .HasColumnName("FEmail")
                    .HasMaxLength(256)
                    .IsUnicode(false);

                entity.Property(e => e.FlastSignIn)
                    .HasColumnName("FLastSignIn")
                    .HasColumnType("datetime");

                entity.Property(e => e.FnickName)
                    .HasColumnName("FNickName")
                    .HasMaxLength(64);

                entity.Property(e => e.FnodeId).HasColumnName("FNodeId");

                entity.Property(e => e.FpasswordHash)
                    .HasColumnName("FPasswordHash")
                    .HasMaxLength(64);

                entity.Property(e => e.FpasswordLevel).HasColumnName("FPasswordLevel");

                entity.Property(e => e.FpasswordSalt)
                    .HasColumnName("FPasswordSalt")
                    .HasMaxLength(64);

                entity.Property(e => e.Fstate).HasColumnName("FState");

                entity.Property(e => e.FtableNo).HasColumnName("FTableNo");

                entity.Property(e => e.FtimeStamp)
                    .HasColumnName("FTimeStamp")
                    .IsRowVersion();

                entity.Property(e => e.FupdateBy).HasColumnName("FUpdateBy");

                entity.Property(e => e.FupdateTime)
                    .HasColumnName("FUpdateTime")
                    .HasColumnType("datetime");

                entity.Property(e => e.FuserRole).HasColumnName("FUserRole");
            });

            modelBuilder.Entity<TuserProfile>(entity =>
            {
                entity.HasKey(e => e.FuserId);

                entity.ToTable("TUserProfile");

                entity.Property(e => e.FuserId)
                    .HasColumnName("FUserId")
                    .ValueGeneratedNever();

                entity.Property(e => e.Favator)
                    .HasColumnName("FAvator")
                    .HasMaxLength(36)
                    .IsUnicode(false);

                entity.Property(e => e.Fcountry).HasColumnName("FCountry");

                entity.Property(e => e.FcreateBy).HasColumnName("FCreateBy");

                entity.Property(e => e.FcreateTime)
                    .HasColumnName("FCreateTime")
                    .HasColumnType("datetime");

                entity.Property(e => e.FisPay)
                    .HasColumnName("FIsPay")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.Flanguage)
                    .IsRequired()
                    .HasColumnName("FLanguage")
                    .HasMaxLength(16)
                    .IsUnicode(false);

                entity.Property(e => e.FmodifyEmailTimeData)
                    .HasColumnName("FModifyEmailTimeData")
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.Fphoto).HasColumnName("FPhoto");

                entity.Property(e => e.FroleCharacterData)
                    .HasColumnName("FRoleCharacterData")
                    .IsUnicode(false);

                entity.Property(e => e.Fsource)
                    .HasColumnName("FSource")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.FtimeStamp)
                    .IsRequired()
                    .HasColumnName("FTimeStamp")
                    .IsRowVersion();

                entity.Property(e => e.FupdateBy).HasColumnName("FUpdateBy");

                entity.Property(e => e.FupdateTime)
                    .HasColumnName("FUpdateTime")
                    .HasColumnType("datetime");
            });

            modelBuilder.Entity<TuserFeedback>(entity =>
            {
                entity.HasKey(e => e.FfeedbackId);

                entity.ToTable("TUserFeedback");

                entity.Property(e => e.FfeedbackId)
                    .HasColumnName("FFeedbackId")
                    .ValueGeneratedNever();

                entity.Property(e => e.FcreateBy).HasColumnName("FCreateBy");

                entity.Property(e => e.FcreateTime)
                    .HasColumnName("FCreateTime")
                    .HasColumnType("datetime");

                entity.Property(e => e.Femail)
                    .HasColumnName("FEmail")
                    .HasMaxLength(256)
                    .IsUnicode(false);

                entity.Property(e => e.Ffeedback)
                    .HasColumnName("FFeedback")
                    .HasColumnType("text");

                entity.Property(e => e.Fmobile)
                    .HasColumnName("FMobile")
                    .HasMaxLength(256)
                    .IsUnicode(false);

                entity.Property(e => e.FreplyContent)
                    .HasColumnName("FReplyContent")
                    .HasColumnType("text");

                entity.Property(e => e.FreplyTime)
                    .HasColumnName("FReplyTime")
                    .HasColumnType("datetime");

                entity.Property(e => e.FreplyUserId).HasColumnName("FReplyUserId");

                entity.Property(e => e.Fstate).HasColumnName("FState");

                entity.Property(e => e.Ftitle)
                    .HasColumnName("FTitle")
                    .HasMaxLength(500);

                entity.Property(e => e.FupdateBy).HasColumnName("FUpdateBy");

                entity.Property(e => e.FupdateTime)
                    .HasColumnName("FUpdateTime")
                    .HasColumnType("datetime");

                entity.Property(e => e.FuserId).HasColumnName("FUserId");
            });
            modelBuilder.Entity<TuserDevice>(entity =>
            {
                entity.HasKey(e => e.FuserDeviceId);

                entity.ToTable("TUserDevice");

                entity.HasIndex(e => e.FsessionId)
                    .HasName("FuserDevice_SessionId");

                entity.HasIndex(e => e.FuserId);

                entity.HasIndex(e => new { e.FuserId, e.FpushToken, e.FisPush, e.FisValid })
                    .HasName("<Name of Missing Index, sysname,>");

                entity.Property(e => e.FuserDeviceId)
                    .HasColumnName("FUserDeviceId")
                    .ValueGeneratedNever();

                entity.Property(e => e.FappVersion)
                    .HasColumnName("FAppVersion")
                    .HasMaxLength(16)
                    .IsUnicode(false);

                entity.Property(e => e.FcreateBy).HasColumnName("FCreateBy");

                entity.Property(e => e.FcreateTime)
                    .HasColumnName("FCreateTime")
                    .HasColumnType("datetime");

                entity.Property(e => e.FdeviceId)
                    .HasColumnName("FDeviceId")
                    .HasMaxLength(128)
                    .IsUnicode(false);

                entity.Property(e => e.FdeviceModel)
                    .HasColumnName("FDeviceModel")
                    .HasMaxLength(128)
                    .IsUnicode(false);

                entity.Property(e => e.FisPush)
                    .IsRequired()
                    .HasColumnName("FIsPush")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.FisValid)
                    .IsRequired()
                    .HasColumnName("FIsValid")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.Flanguage)
                    .HasColumnName("FLanguage")
                    .HasMaxLength(16)
                    .IsUnicode(false);

                entity.Property(e => e.FlastVisitTime)
                    .HasColumnName("FLastVisitTime")
                    .HasColumnType("datetime");

                entity.Property(e => e.Fostype)
                    .HasColumnName("FOSType")
                    .HasMaxLength(16)
                    .IsUnicode(false);

                entity.Property(e => e.Fosversion)
                    .HasColumnName("FOSVersion")
                    .HasMaxLength(16)
                    .IsUnicode(false);

                entity.Property(e => e.FpushProvider)
                    .HasColumnName("FPushProvider")
                    .HasMaxLength(8)
                    .IsUnicode(false);

                entity.Property(e => e.FpushToken)
                    .HasColumnName("FPushToken")
                    .HasMaxLength(400)
                    .IsUnicode(false);

                entity.Property(e => e.FsearchTime)
                    .HasColumnName("FSearchTime")
                    .HasColumnType("datetime");

                entity.Property(e => e.FsessionId)
                    .HasColumnName("FSessionId")
                    .HasMaxLength(100);

                entity.Property(e => e.FtimeStamp)
                    .IsRequired()
                    .HasColumnName("FTimeStamp")
                    .IsRowVersion();

                entity.Property(e => e.FupdateBy).HasColumnName("FUpdateBy");

                entity.Property(e => e.FupdateTime)
                    .HasColumnName("FUpdateTime")
                    .HasColumnType("datetime");

                entity.Property(e => e.FuserId).HasColumnName("FUserId");
            });

            modelBuilder.Entity<TuserMemberInfo>(entity =>
            {
                entity.HasKey(e => new { e.FuserId, e.FmemberType, e.FmemberLevel });

                entity.ToTable("TUserMemberInfo");

                entity.Property(e => e.FuserId).HasColumnName("FUserId");

                entity.Property(e => e.FmemberType).HasColumnName("FMemberType");

                entity.Property(e => e.FmemberLevel).HasColumnName("FMemberLevel");

                entity.Property(e => e.FcreateBy).HasColumnName("FCreateBy");

                entity.Property(e => e.FcreateTime)
                    .HasColumnName("FCreateTime")
                    .HasColumnType("datetime");

                entity.Property(e => e.FexpiresTime)
                    .HasColumnName("FExpiresTime")
                    .HasColumnType("datetime");

                entity.Property(e => e.FstartTime)
                    .HasColumnName("FStartTime")
                    .HasColumnType("datetime");

                entity.Property(e => e.FupdateBy).HasColumnName("FUpdateBy");

                entity.Property(e => e.FupdateTime)
                    .HasColumnName("FUpdateTime")
                    .HasColumnType("datetime");
            });

            modelBuilder.Entity<TUserUnRegisterInfo>(entity =>
            {
                entity.HasKey(e => e.FId);

                entity.Property(e => e.FCompletedTime)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("('1970-01-01')");

                entity.Property(e => e.FEmail)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.FUnRegisterTime)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getutcdate())");
            });
        }
    }
}
