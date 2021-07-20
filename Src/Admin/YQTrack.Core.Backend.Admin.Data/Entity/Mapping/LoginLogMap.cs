using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace YQTrack.Core.Backend.Admin.Data.Entity.Mapping
{
    public class LoginLogMap : IEntityTypeConfiguration<LoginLog>
    {
        public void Configure(EntityTypeBuilder<LoginLog> builder)
        {
            builder.ToTable("TLoginLog");

            builder.HasIndex(x => x.FLoginTime);
            builder.HasIndex(x => x.FAccount);
            builder.HasIndex(x => x.FNickName);
            builder.HasIndex(x => x.FPlatform);

            builder.HasKey(x => x.FId);
            builder.Property(x => x.FId).UseSqlServerIdentityColumn();

            builder.Property(x => x.FManagerId).IsRequired();
            builder.Property(x => x.FNickName).HasMaxLength(16).IsRequired();
            builder.Property(x => x.FAccount).HasMaxLength(16).IsRequired();
            builder.Property(x => x.FIp).HasMaxLength(16).IsRequired();
            builder.Property(x => x.FPlatform).HasMaxLength(16).IsRequired();
            builder.Property(x => x.FUserAgent).HasMaxLength(128).IsRequired();
            builder.Property(x => x.FLoginTime).IsRequired().HasDefaultValueSql("getutcdate()");

            builder.Property(x => x.FCreatedTime).IsRequired().HasDefaultValueSql("getutcdate()");
            builder.Property(x => x.FCreatedBy).IsRequired();
        }
    }
}