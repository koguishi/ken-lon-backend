using kendo_londrina.Models;
using Microsoft.EntityFrameworkCore;

namespace kendo_londrina.Data;

public class AcademiaContext : DbContext
{
    public AcademiaContext(DbContextOptions<AcademiaContext> options) : base(options) { }

    public DbSet<Aluno_OLD> Alunos => Set<Aluno_OLD>();

}
