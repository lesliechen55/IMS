using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace YQTrack.Core.Backend.Admin.Data.Entity.Mapping
{
    public class ManagerRoleMap : IEntityTypeConfiguration<ManagerRole>
    {
        public void Configure(EntityTypeBuilder<ManagerRole> builder)
        {
            builder.ToTable("TManagerRole");
            builder.HasKey(x => new { ManagerId = x.FManagerId, RoleId = x.FRoleId });
            builder.Property(x => x.FCreatedTime).IsRequired().HasDefaultValueSql("getutcdate()");
            builder.Property(x => x.FCreatedBy).IsRequired();
        }
    }
}