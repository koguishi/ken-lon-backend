using kendo_londrina.Domain.Entities;
using kendo_londrina.Infra.Data.Config;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace kendo_londrina.Infra.Data;

public class KendoLondrinaContext : IdentityDbContext<ApplicationUser>
{
    public KendoLondrinaContext(DbContextOptions<KendoLondrinaContext> options) : base(options) { }
    public DbSet<Empresa> Empresas => Set<Empresa>();
    public DbSet<Pessoa> Pessoas => Set<Pessoa>();
    public DbSet<Categoria> Categorias => Set<Categoria>();
    public DbSet<SubCategoria> SubCategorias => Set<SubCategoria>();
    public DbSet<Aluno> Alunos => Set<Aluno>();
    public DbSet<Mensalidade> Mensalidades => Set<Mensalidade>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.ApplyConfiguration(new EmpresaConfig());
        builder.ApplyConfiguration(new PessoaConfig());
        builder.ApplyConfiguration(new CategoriaConfig());
        builder.ApplyConfiguration(new SubCategoriaConfig());
        builder.ApplyConfiguration(new PessoaConfig());
        builder.ApplyConfiguration(new AlunoConfig());
        builder.ApplyConfiguration(new MensalidadeConfig());
    }

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        configurationBuilder.Properties<string>()
            .HaveMaxLength(100)
            .HaveColumnType("varchar");
    }    

}
