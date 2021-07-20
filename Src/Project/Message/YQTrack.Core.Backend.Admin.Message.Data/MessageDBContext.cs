using Microsoft.EntityFrameworkCore;
using YQTrack.Core.Backend.Admin.Message.Data.Models;

namespace YQTrack.Core.Backend.Admin.Message.Data
{
    public partial class MessageDbContext : DbContext
    {
        public MessageDbContext()
        {
        }

        public MessageDbContext(DbContextOptions<MessageDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Tproject> Tproject { get; set; }
        public virtual DbSet<Ttemplate> Ttemplate { get; set; }
        public virtual DbSet<TtemplateType> TtemplateType { get; set; }
        public virtual DbSet<TsendTask> TsendTask { get; set; }
        public virtual DbSet<TsendTaskObj> TsendTaskObj { get; set; }
        public virtual DbSet<TsiteMessageDetails> TsiteMessageDetails { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("ProductVersion", "2.2.3-servicing-35854");

            modelBuilder.Entity<Tproject>(entity =>
            {
                entity.HasKey(e => e.FprojectId);

                entity.ToTable("TProject");

                entity.Property(e => e.FprojectId)
                    .HasColumnName("FProjectId")
                    .ValueGeneratedNever();

                entity.Property(e => e.FparentId).HasColumnName("FParentId");

                entity.Property(e => e.Fprefix)
                    .HasColumnName("FPrefix")
                    .HasMaxLength(50);

                entity.Property(e => e.FprojectName)
                    .IsRequired()
                    .HasColumnName("FProjectName")
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<Ttemplate>(entity =>
            {
                entity.HasKey(e => e.FtemplateId);

                entity.ToTable("TTemplate");

                entity.HasIndex(e => e.FtemplateTypeId)
                    .HasName("IXFK_TTemplate_TTemplateType");

                entity.HasIndex(e => new { e.FtemplateTypeId, e.FisDel })
                    .HasName("FTemplateTypeIdFIsDel");

                entity.Property(e => e.FtemplateId)
                    .HasColumnName("FTemplateId")
                    .ValueGeneratedNever();

                entity.Property(e => e.FisDel).HasColumnName("FIsDel");

                entity.Property(e => e.Flanguage)
                    .IsRequired()
                    .HasColumnName("FLanguage")
                    .HasMaxLength(16)
                    .IsUnicode(false);

                entity.Property(e => e.FtemplateBody).HasColumnName("FTemplateBody");

                entity.Property(e => e.FtemplateTitle)
                    .HasColumnName("FTemplateTitle")
                    .HasMaxLength(1000);

                entity.Property(e => e.FtemplateTypeId).HasColumnName("FTemplateTypeId");

                entity.HasOne(d => d.FtemplateType)
                    .WithMany(p => p.Ttemplate)
                    .HasForeignKey(d => d.FtemplateTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TTemplate_TTemplateType");
            });

            modelBuilder.Entity<TtemplateType>(entity =>
            {
                entity.HasKey(e => e.FtemplateTypeId);

                entity.ToTable("TTemplateType");

                entity.HasIndex(e => e.FprojectId)
                    .HasName("IXFK_TTemplateType_TProject");

                entity.Property(e => e.FtemplateTypeId)
                    .HasColumnName("FTemplateTypeId")
                    .ValueGeneratedNever();

                entity.Property(e => e.Fchannel).HasColumnName("FChannel");

                entity.Property(e => e.FcreateTime)
                    .HasColumnName("FCreateTime")
                    .HasColumnType("datetime");

                entity.Property(e => e.FdataJson)
                    .HasColumnName("FDataJson")
                    .HasMaxLength(4000);

                entity.Property(e => e.Fenable)
                    .HasColumnName("FEnable")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.FisRendering).HasColumnName("FIsRendering");

                entity.Property(e => e.FprojectId).HasColumnName("FProjectId");

                entity.Property(e => e.FtemplateCode).HasColumnName("FTemplateCode");

                entity.Property(e => e.FtemplateDescribe)
                    .HasColumnName("FTemplateDescribe")
                    .HasMaxLength(200);

                entity.Property(e => e.FtemplateName)
                    .IsRequired()
                    .HasColumnName("FTemplateName")
                    .HasMaxLength(50);

                entity.Property(e => e.FupdateTime)
                    .HasColumnName("FUpdateTime")
                    .HasColumnType("datetime");

                entity.HasOne(d => d.Fproject)
                    .WithMany(p => p.TtemplateType)
                    .HasForeignKey(d => d.FprojectId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TTemplateType_TProject");
            });
            modelBuilder.Entity<TsendTask>(entity =>
            {
                entity.HasKey(e => e.FtaskId);

                entity.ToTable("TSendTask");

                entity.HasIndex(e => e.FtemplateTypeId)
                    .HasName("IXFK_TSendTask_TTemplateType");

                entity.Property(e => e.FtaskId)
                    .HasColumnName("FTaskId")
                    .ValueGeneratedNever();

                entity.Property(e => e.FcreateTime)
                    .HasColumnName("FCreateTime")
                    .HasColumnType("datetime");

                entity.Property(e => e.FdataJson)
                    .HasColumnName("FDataJson")
                    .HasMaxLength(4000);

                entity.Property(e => e.FpushFail).HasColumnName("FPushFail");

                entity.Property(e => e.FpushSucess).HasColumnName("FPushSucess");

                entity.Property(e => e.FpushTime)
                    .HasColumnName("FPushTime")
                    .HasColumnType("datetime");

                entity.Property(e => e.Fremarks)
                    .HasColumnName("FRemarks")
                    .HasMaxLength(200);

                entity.Property(e => e.FretryCount)
                    .HasColumnName("FRetryCount")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.Fstate).HasColumnName("FState");

                entity.Property(e => e.FtemplateTypeId).HasColumnName("FTemplateTypeId");

                entity.Property(e => e.FupdateTime)
                    .HasColumnName("FUpdateTime")
                    .HasColumnType("datetime");
            });

            modelBuilder.Entity<TsendTaskObj>(entity =>
            {
                entity.HasKey(e => e.FtaskObjId);

                entity.ToTable("TSendTaskObj");

                entity.HasIndex(e => e.FtaskId)
                    .HasName("IXFK_TSendTaskObj_TSendTask");

                entity.Property(e => e.FtaskObjId)
                    .HasColumnName("FTaskObjId")
                    .ValueGeneratedNever();

                entity.Property(e => e.FobjDetails).HasColumnName("FObjDetails");

                entity.Property(e => e.FobjType).HasColumnName("FObjType");

                entity.Property(e => e.FtaskId).HasColumnName("FTaskId");

                entity.HasOne(d => d.Ftask)
                    .WithMany(p => p.TsendTaskObj)
                    .HasForeignKey(d => d.FtaskId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TSendTaskObj_TSendTask");
            });

            modelBuilder.Entity<TsiteMessageDetails>(entity =>
            {
                entity.HasKey(e => e.FsiteMessageDetailsId)
                    .HasName("PK_FSiteMessageDetailsId")
                    .ForSqlServerIsClustered(false);

                entity.ToTable("TSiteMessageDetails");

                entity.HasIndex(e => new { e.FuserId, e.FcreateTime, e.FisRead, e.FisDel, e.FsiteMessageId })
                    .HasName("PK_TSiteMessageDetails")
                    .ForSqlServerIsClustered();

                entity.Property(e => e.FsiteMessageDetailsId).HasColumnName("FSiteMessageDetailsId").UseSqlServerIdentityColumn();

                entity.Property(e => e.FcreateTime)
                    .HasColumnName("FCreateTime")
                    .HasColumnType("datetime");

                entity.Property(e => e.FdataJson).HasColumnName("FDataJson");

                entity.Property(e => e.FisDel).HasColumnName("FIsDel");

                entity.Property(e => e.FisRead)
                    .HasColumnName("FIsRead")
                    .HasDefaultValueSql("((0))").ValueGeneratedNever();

                entity.Property(e => e.Foverdue)
                    .HasColumnName("FOverdue")
                    .HasColumnType("datetime");

                entity.Property(e => e.FsiteMessageId).HasColumnName("FSiteMessageId");

                entity.Property(e => e.FtemplateTypeId).HasColumnName("FTemplateTypeId");

                entity.Property(e => e.FupdateTime)
                    .HasColumnName("FUpdateTime")
                    .HasColumnType("datetime");

                entity.Property(e => e.FuserId).HasColumnName("FUserId");
            });
        }
    }
}
