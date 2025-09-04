using Microsoft.EntityFrameworkCore;
using kendo_londrina.Domain.Entities;
using kendo_londrina.Domain.Repositories;
using kendo_londrina.Infra;

namespace kendo_londrina.Infrastructure.Repositories
{
    public class AlunoRepository : IAlunoRepository
    {
        private readonly KendoLondrinaContext _context;

        public AlunoRepository(KendoLondrinaContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Aluno aluno)
        {
            await _context.Alunos.AddAsync(aluno);
        }

        public async Task<Aluno?> GetByIdAsync(Guid id)
        {
            return await _context.Alunos.FindAsync(id);
        }

        public async Task<List<Aluno>> GetAllAsync()
        {
            return await _context.Alunos.ToListAsync();
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public IQueryable<Aluno> Query()
        {
            return _context.Alunos.AsQueryable();
        }

        public Task DeleteAsync(Aluno aluno)
        {
            _context.Alunos.Remove(aluno);
            return _context.SaveChangesAsync();
        }
    }
}
