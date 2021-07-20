using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace YQTrack.Core.Backend.Admin.Data.Entity.Mapping
{
    public class ManagerMap : IEntityTypeConfiguration<Manager>
    {
        public void Configure(EntityTypeBuilder<Manager> builder)
        {
            builder.ToTable("TManager");
            builder.HasIndex(x => x.FAccount).IsUnique();
            builder.HasKey(x => x.FId);
            builder.Property(x => x.FId).UseSqlServerIdentityColumn();

            builder.Property(x => x.FCreatedTime).IsRequired().HasDefaultValueSql("getutcdate()");
            builder.Property(x => x.FCreatedBy).IsRequired();
            builder.Property(x => x.FIsDeleted).IsRequired().HasDefaultValue(false);

            builder.Property(x => x.FNickName).HasMaxLength(16).IsRequired();
            builder.Property(x => x.FAccount).HasMaxLength(16).IsRequired();
            builder.Property(x => x.FPassword).IsRequired().HasMaxLength(32);
            builder.Property(x => x.FIsLock).IsRequired().HasDefaultValue(false);
            builder.Property(x => x.FRemark).IsRequired().HasMaxLength(32);
            builder.Property(x => x.FAvatar).HasMaxLength(128);
            builder.Property(x => x.FLastLoginTime).IsRequired().HasDefaultValueSql("getutcdate()");

            builder.Property(x => x.FEmail).HasMaxLength(128);

        }
    }
}