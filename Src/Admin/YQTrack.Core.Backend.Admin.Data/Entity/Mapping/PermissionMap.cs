using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace YQTrack.Core.Backend.Admin.Data.Entity.Mapping
{
    public class PermissionMap : IEntityTypeConfiguration<Permission>
    {
        public void Configure(EntityTypeBuilder<Permission> builder)
        {
            builder.ToTable("TPermission");
            builder.HasIndex(x => x.FName).IsUnique();
            builder.HasIndex(x => x.FFullName);
            builder.HasIndex(x => x.FUrl);
            builder.HasKey(x => x.FId);
            builder.Property(x => x.FId).UseSqlServerIdentityColumn();

            builder.Property(x => x.FCreatedTime).IsRequired().HasDefaultValueSql("getutcdate()");
            builder.Property(x => x.FCreatedBy).IsRequired();
            builder.Property(x => x.FIsDeleted).IsRequired().HasDefaultValue(false);

            builder.Property(x => x.FName).IsRequired().HasMaxLength(32);

            builder.Property(x => x.FAreaName).HasMaxLength(16);
            builder.Property(x => x.FControllerName).HasMaxLength(32);
            builder.Property(x => x.FActionName).HasMaxLength(32);
            builder.Property(x => x.FFullName).HasMaxLength(64);
            builder.Property(x => x.FUrl).HasMaxLength(128);

            builder.Property(x => x.FRemark).IsRequired().HasMaxLength(64);
            builder.Property(x => x.FIcon).HasMaxLength(64);
            builder.Property(x => x.FMenuType).IsRequired().HasConversion<int>();
            builder.Property(x => x.FTopMenuKey).HasMaxLength(32);
        }
    }
}