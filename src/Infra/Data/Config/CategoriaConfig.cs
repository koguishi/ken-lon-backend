using kendo_londrina.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace kendo_londrina.Infra.Data.Config;

public class CategoriaConfig : IEntityTypeConfiguration<Categoria>
{
    public void Configure(EntityTypeBuilder<Categoria> builder)
    {
        builder.HasKey(c => c.Id);

        builder.Property(c => c.Nome)
            .HasColumnType("varchar")
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(c => c.Codigo)
            .HasColumnType("varchar")
            .HasMaxLength(10)
            .IsRequired(false);

        builder.ToTable("Categorias");

        builder.HasOne(p => p.Empresa)
            .WithMany(e => e.Categorias)
            .HasForeignKey(p => p.EmpresaId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}