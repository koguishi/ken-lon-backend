using AcademiaApi.Models;
using Microsoft.EntityFrameworkCore;

namespace AcademiaApi.Data;

public class AcademiaContext : DbContext
{
    public AcademiaContext(DbContextOptions<AcademiaContext> options) : base(options) { }

    public DbSet<Aluno> Alunos => Set<Aluno>();

}
