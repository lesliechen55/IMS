using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace YQTrack.Core.Backend.Admin.Data.Entity.Mapping
{
    public class OperationLogMap : IEntityTypeConfiguration<OperationLog>
    {
        public void Configure(EntityTypeBuilder<OperationLog> builder)
        {
            builder.ToTable("TOperationLog");

            builder.HasIndex(x => x.FCreatedTime);
            builder.HasIndex(x => x.FAccount);
            builder.HasIndex(x => x.FMethod);

            builder.HasKey(x => x.FId);
            builder.Property(x => x.FId).UseSqlServerIdentityColumn();

            builder.Property(x => x.FOperatorId).IsRequired();
            builder.Property(x => x.FNickName).HasMaxLength(16).IsRequired();
            builder.Property(x => x.FAccount).HasMaxLength(16).IsRequired();
            builder.Property(x => x.FIp).HasMaxLength(16).IsRequired();
            builder.Property(x => x.FMethod).HasMaxLength(128).IsRequired();
            builder.Property(x => x.FParameter).IsRequired().HasColumnType("nvarchar(max)");
            builder.Property(x => x.FDesc).HasMaxLength(16).IsRequired();
            builder.Property(x => x.FOperationType).IsRequired().HasConversion<int>();

            builder.Property(x => x.FCreatedTime).IsRequired().HasDefaultValueSql("getutcdate()");
            builder.Property(x => x.FCreatedBy).IsRequired();
        }
    }
}