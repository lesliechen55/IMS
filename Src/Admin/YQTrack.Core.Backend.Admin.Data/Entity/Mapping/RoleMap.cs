using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace YQTrack.Core.Backend.Admin.Data.Entity.Mapping
{
    public class RoleMap : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            builder.ToTable("TRole");
            builder.HasIndex(x => x.FName).IsUnique();
            builder.HasKey(x => x.FId);
            builder.Property(x => x.FId).UseSqlServerIdentityColumn();

            builder.Property(x => x.FCreatedTime).IsRequired().HasDefaultValueSql("getutcdate()");
            builder.Property(x => x.FCreatedBy).IsRequired();
            builder.Property(x => x.FIsDeleted).IsRequired().HasDefaultValue(false);

            builder.Property(x => x.FName).IsRequired().HasMaxLength(16);
            builder.Property(x => x.FIsActive).IsRequired().HasDefaultValue(false);
            builder.Property(x => x.FRemark).IsRequired().HasMaxLength(32);
        }
    }
}