using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace kendo_londrina.Infra.Data.Config;

public class AuditoriaEntryConfig : IEntityTypeConfiguration<AuditoriaEntry>
{
    public void Configure(EntityTypeBuilder<AuditoriaEntry> builder)
    {
        builder.HasKey(c => c.Id);

        builder.Property(c => c.DadosAntes)
            .HasColumnType("nvarchar")
            .HasMaxLength(-1)
            .IsRequired(false);

        builder.Property(c => c.DadosDepois)
            .HasColumnType("nvarchar")
            .HasMaxLength(-1)
            .IsRequired(false);

        builder.Property(c => c.Data)
            .HasColumnType("datetime")
            .IsRequired();

        builder.ToTable("AuditoriaEntries");
    }
}
