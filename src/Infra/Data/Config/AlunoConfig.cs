using kendo_londrina.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace kendo_londrina.Infra.Data.Config;

public class AlunoConfig : IEntityTypeConfiguration<Aluno>
{
    public void Configure(EntityTypeBuilder<Aluno> builder)
    {
        builder.HasKey(c => c.Id);

        //builder.HasIndex(c => new { c.EscolaId, c.Codigo })
        //    .IsUnique(true);

        builder.Property(c => c.Codigo)
            .HasColumnType("varchar")
            .HasMaxLength(10)
            .IsRequired(false);

        builder.Property(c => c.Nome)
            .HasColumnType("varchar")
            .HasMaxLength(100)
            .IsRequired();
        builder.Property(c => c.DataNascimento)
            .HasColumnType("date")
            .IsRequired();
        builder.Property(c => c.Nacionalidade)
            .HasColumnType("varchar")
            .HasMaxLength(100)
            .IsRequired(false);
        builder.Property(c => c.UfNascimento)
            .HasColumnType("char")
            .HasMaxLength(2)
            .IsRequired(false);
        builder.Property(c => c.CidadeNascimento)
            .HasColumnType("varchar")
            .HasMaxLength(100)
            .IsRequired(false);
        builder.Property(c => c.Sexo)
            .HasColumnType("char")
            .HasMaxLength(1)
            .IsRequired(false);
        builder.Property(c => c.Rg)
            .HasColumnType("varchar")
            .HasMaxLength(50)
            .IsRequired(false);
        builder.Property(c => c.Cpf)
            .HasColumnType("varchar")
            .HasMaxLength(15)
            .IsRequired(false);
        builder.Property(c => c.Email)
            .HasColumnType("varchar")
            .HasMaxLength(200)
            .IsRequired(false);
        builder.Property(c => c.TelCelular)
            .HasColumnType("varchar")
            .HasMaxLength(20)
            .IsRequired(false);
        builder.Property(c => c.Religiao)
            .HasColumnType("varchar")
            .HasMaxLength(50)
            .IsRequired(false);

        // builder.Property(c => c.EnderecoId)
        //     .IsRequired(false);

        builder.ToTable("Alunos");

        // builder.HasOne(c => c.Escola)
        //     .WithMany(c => c.Alunos)
        //     .OnDelete(DeleteBehavior.Restrict);
    }
}