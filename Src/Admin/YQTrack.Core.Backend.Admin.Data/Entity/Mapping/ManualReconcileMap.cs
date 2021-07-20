using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace YQTrack.Core.Backend.Admin.Data.Entity.Mapping
{
    public class ManualReconcileMap : IEntityTypeConfiguration<ManualReconcile>
    {
        public void Configure(EntityTypeBuilder<ManualReconcile> builder)
        {
            builder.ToTable("TManualReconcile");

            builder.HasKey(x => x.FId);
            builder.Property(x => x.FId).UseSqlServerIdentityColumn();

            builder.Property(x => x.FFileName).HasMaxLength(50).IsRequired();
            builder.Property(x => x.FFileMd5).HasMaxLength(32).IsRequired();
            builder.Property(x => x.FFilePath).HasMaxLength(250).IsRequired();

            builder.Property(x => x.FYear).IsRequired();
            builder.Property(x => x.FMonth).IsRequired();
            builder.Property(x => x.FOrderCount).IsRequired();

            builder.Property(x => x.FRemark).IsRequired().HasMaxLength(50);

            builder.Property(x => x.FCreatedTime).IsRequired().HasDefaultValueSql("getutcdate()");
            builder.Property(x => x.FCreatedBy).IsRequired();
        }
    }
}