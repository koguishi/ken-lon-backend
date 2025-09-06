using kendo_londrina.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace kendo_londrina.Infra.Data.Config;

public class PessoaConfig : IEntityTypeConfiguration<Pessoa>
{
    public void Configure(EntityTypeBuilder<Pessoa> builder)
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

        builder.Property(c => c.Cpf)
            .HasColumnType("varchar")
            .HasMaxLength(15)
            .IsRequired(false);

        builder.Property(c => c.Cnpj)
            .HasColumnType("varchar")
            .HasMaxLength(15)
            .IsRequired(false);

        builder.ToTable("Pessoas");
    }
}