using kendo_londrina.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace kendo_londrina.Infra.Data.Config;

public class MensalidadeConfig : IEntityTypeConfiguration<Mensalidade>
{
    public void Configure(EntityTypeBuilder<Mensalidade> builder)
    {
        builder.HasKey(c => c.Id);

        builder.Property(c => c.AlunoNome)
            .HasColumnType("varchar")
            .HasMaxLength(100)
            .IsRequired();
        builder.Property(m => m.Valor)
            .HasColumnType("decimal(10,2)");
        builder.Property(c => c.DataVencimento)
            .HasColumnType("date")
            .IsRequired();
        builder.Property(c => c.DataPagamento)
            .HasColumnType("datetime")
            .IsRequired(false);
        builder.Property(c => c.MeioPagamento)
            .HasColumnType("varchar")
            .HasMaxLength(50)
            .IsRequired(false);
        builder.Property(c => c.Observacao)
            .HasColumnType("nvarchar")
            .HasMaxLength(2000)
            .IsRequired(false);
        // builder.Property(c => c.Excluida)
        //     .HasColumnType("bit")
        //     .HasDefaultValue(false)
        //     .IsRequired(false);
        builder.Property(c => c.MotivoExclusao)
            .HasColumnType("nvarchar")
            .HasMaxLength(2000)
            .IsRequired(false);
        builder.Property(c => c.DataExclusao)
            .HasColumnType("datetime")
            .IsRequired(false);
 
        builder.ToTable("Mensalidades");

        builder.HasOne(c => c.Aluno)
            .WithMany(c => c.Mensalidades)
            .HasForeignKey(m => m.AlunoId)
            .OnDelete(DeleteBehavior.Restrict);        
    }
}