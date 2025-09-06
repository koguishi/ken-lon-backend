using Microsoft.EntityFrameworkCore;
using kendo_londrina.Domain.Entities;
using kendo_londrina.Domain.Repositories;
using kendo_londrina.Infra.Data;

namespace kendo_londrina.Infrastructure.Repositories
{
    public class PessoaRepository : IPessoaRepository
    {
        private readonly KendoLondrinaContext _context;

        public PessoaRepository(KendoLondrinaContext context)
        {
            _context = context;
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        async Task<Pessoa?> IPessoaRepository.GetByIdAsync(Guid userId, Guid id)
        {
            var pessoa = await _context.Pessoas.FindAsync(id);
            if (pessoa == null || pessoa.UserId != userId)
                return null;
            if (pessoa.UserId != userId)
                throw new Exception("Erro de pertencimento");            
            return pessoa;
        }

        public async Task AddAsync(Pessoa pessoa)
        {
            await _context.Pessoas.AddAsync(pessoa);
        }

        public Task DeleteAsync(Guid userId, Pessoa pessoa)
        {
            _context.Pessoas.Remove(pessoa);
            return _context.SaveChangesAsync();
        }

        Task<List<Pessoa>> IPessoaRepository.GetAllAsync(Guid userId)
        {
            return _context.Pessoas.Where(p => p.UserId == userId).ToListAsync();
        }

        IQueryable<Pessoa> IPessoaRepository.Query(Guid userId)
        {
            return _context.Pessoas.Where(p => p.UserId == userId).AsQueryable();
        }
    }
}
