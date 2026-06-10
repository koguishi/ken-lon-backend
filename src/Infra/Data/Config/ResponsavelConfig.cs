
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class ResponsavelConfig : IEntityTypeConfiguration<Responsavel>
{
    public void Configure(EntityTypeBuilder<Responsavel> builder)
    {
        builder.ToTable("Responsaveis");
        builder.HasKey(r => r.Id);
        builder.Property(r => r.Nome).IsRequired().HasMaxLength(200);
        builder.Property(r => r.Telefone).HasMaxLength(20);
        builder.Property(r => r.Email).HasMaxLength(200);

        builder.HasMany(r => r.Alunos)
               .WithOne(a => a.Responsavel)
               .HasForeignKey(a => a.ResponsavelId)
               .IsRequired(false);

        builder.HasMany(r => r.Mensalidades)
               .WithOne(m => m.Responsavel)
               .HasForeignKey(m => m.ResponsavelId)
               .IsRequired(false);
    }
}
