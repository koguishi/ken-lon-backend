using kendo_londrina.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace kendo_londrina.Infra.Data.Config;

public class ContaPagarConfig : IEntityTypeConfiguration<ContaPagar>
{
    public void Configure(EntityTypeBuilder<ContaPagar> builder)
    {
        builder.HasKey(c => c.Id);

        builder.Property(m => m.Valor)
            .HasColumnType("decimal(10,2)");
        builder.Property(c => c.Vencimento)
            .HasColumnType("date")
            .IsRequired();
        builder.Property(c => c.Descricao)
            .HasColumnType("nvarchar")
            .HasMaxLength(100)
            .IsRequired(false);
        builder.Property(c => c.Observacao)
            .HasColumnType("nvarchar")
            .HasMaxLength(2000)
            .IsRequired(false);

        builder.Property(c => c.DataPagamento)
            .HasColumnType("date")
            .IsRequired(false);
        builder.Property(c => c.MeioPagamento)
            .HasColumnType("varchar")
            .HasMaxLength(50)
            .IsRequired(false);
        builder.Property(c => c.ObsPagamento)
            .HasColumnType("nvarchar")
            .HasMaxLength(2000)
            .IsRequired(false);

        // builder.Property(c => c.Excluida)
        //     .HasColumnType("bit")
        //     .HasDefaultValue(false)
        //     .IsRequired(false);
        builder.Property(c => c.DataExclusao)
            .HasColumnType("date")
            .IsRequired(false);
        builder.Property(c => c.MotivoExclusao)
            .HasColumnType("nvarchar")
            .HasMaxLength(2000)
            .IsRequired(false);

        builder.ToTable("ContasPagar");

        builder.HasOne(t => t.Empresa)
            .WithMany(t => t.ContasPagar)
            .HasForeignKey(t => t.EmpresaId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(t => t.Pessoa)
            .WithMany(t => t.ContasPagar)
            .HasForeignKey(t => t.PessoaId)
            .OnDelete(DeleteBehavior.Restrict);

       builder.HasOne(t => t.Categoria)
            .WithMany(t => t.ContasPagar)
            .HasForeignKey(t => t.CategoriaId)
            .OnDelete(DeleteBehavior.Restrict);

       builder.HasOne(t => t.SubCategoria)
            .WithMany(t => t.ContasPagar)
            .HasForeignKey(t => t.SubCategoriaId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}