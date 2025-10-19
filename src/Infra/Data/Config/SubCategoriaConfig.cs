using kendo_londrina.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace kendo_londrina.Infra.Data.Config;

public class SubCategoriaConfig : IEntityTypeConfiguration<SubCategoria>
{
    public void Configure(EntityTypeBuilder<SubCategoria> builder)
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

        builder.ToTable("SubCategorias");

        builder.HasOne(p => p.Empresa)
            .WithMany(e => e.SubCategorias)
            .HasForeignKey(p => p.EmpresaId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(c => c.Categoria)
            .WithMany(c => c.SubCategorias)
            .HasForeignKey(m => m.CategoriaId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}