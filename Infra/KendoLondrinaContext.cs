using kendo_londrina.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace kendo_londrina.Infra;

public class KendoLondrinaContext : DbContext
{
    public KendoLondrinaContext(DbContextOptions<KendoLondrinaContext> options) : base(options) { }

    public DbSet<Aluno> Alunos => Set<Aluno>();
    public DbSet<Mensalidade> Mensalidades => Set<Mensalidade>();

}
