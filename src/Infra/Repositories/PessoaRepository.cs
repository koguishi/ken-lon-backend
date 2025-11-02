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

        async Task<Pessoa?> IPessoaRepository.GetByIdAsync(Guid empresaId, Guid id)
        {
            var pessoa = await _context.Pessoas
                .Where(p => p.EmpresaId == empresaId && p.Id == id)
                .FirstOrDefaultAsync();
            return pessoa;
        }

        public async Task AddAsync(Pessoa pessoa)
        {
            await _context.Pessoas.AddAsync(pessoa);
        }

        public Task DeleteAsync(Pessoa pessoa)
        {
            _context.Pessoas.Remove(pessoa);
            return _context.SaveChangesAsync();
        }

        Task<List<Pessoa>> IPessoaRepository.GetAllAsync(Guid empresaId)
        {
            return _context.Pessoas.Where(p => p.EmpresaId == empresaId).ToListAsync();
        }

        IQueryable<Pessoa> IPessoaRepository.Query(Guid empresaId)
        {
            return _context.Pessoas.Where(p => p.EmpresaId == empresaId).AsQueryable();
        }

        public async Task LoadContasPagarAsync(Pessoa pessoa)
        {
            await _context.Entry(pessoa)
                .Collection(t => t.ContasPagar!)
                .LoadAsync();
        }

        public async Task LoadContasReceberAsync(Pessoa pessoa)
        {
            await _context.Entry(pessoa)
                .Collection(t => t.ContasReceber!)
                .LoadAsync();
        }
    }
}
