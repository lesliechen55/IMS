using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace YQTrack.Core.Backend.Admin.Data.Entity.Mapping
{
    public class ESFieldMap : IEntityTypeConfiguration<ESField>
    {
        public void Configure(EntityTypeBuilder<ESField> builder)
        {
            builder.ToTable("TESField");
            builder.HasKey(e => e.FId);
            builder.HasIndex(e => new { e.FName, e.FCategory })
                .HasName("IX_TESField");

            builder.Property(e => e.FName)
                .IsRequired()
                .HasMaxLength(50);;

            builder.Property(e => e.FCategory)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(e => e.FValue).HasMaxLength(50);
        }
    }
}