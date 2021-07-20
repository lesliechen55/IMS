using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace YQTrack.Core.Backend.Admin.Data.Entity.Mapping
{
    public class ESDashboardMap : IEntityTypeConfiguration<ESDashboard>
    {
        public void Configure(EntityTypeBuilder<ESDashboard> builder)
        {
            builder.ToTable("TESDashboard");
            builder.HasKey(e => e.FPermissionId);

            builder.Property(e => e.FPermissionId).ValueGeneratedNever();

            builder.Property(e => e.FDashboardSrc).IsRequired();

            builder.Property(e => e.FPassword).HasMaxLength(50);

            builder.Property(e => e.FUsername).HasMaxLength(50);
        }
    }
}