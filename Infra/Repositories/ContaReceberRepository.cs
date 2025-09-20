using Microsoft.EntityFrameworkCore;
using kendo_londrina.Domain.Entities;
using kendo_londrina.Domain.Repositories;
using kendo_londrina.Infra.Data;

namespace kendo_londrina.Infrastructure.Repositories
{
    public class ContaReceberRepository : IContaReceberRepository
    {
        private readonly KendoLondrinaContext _context;

        public ContaReceberRepository(KendoLondrinaContext context)
        {
            _context = context;
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<ContaReceber?> GetByIdAsync(Guid empresaId, Guid id)
        {
            var contaReceber = await _context.ContasReceber
                .Include(c => c.Pessoa)
                .Include(c => c.Categoria)
                .Include(c => c.SubCategoria)
                .Where(p => p.EmpresaId == empresaId && p.Id == id)
                .FirstOrDefaultAsync();
            return contaReceber;
        }

        public async Task AddAsync(ContaReceber contaReceber)
        {
            await _context.ContasReceber.AddAsync(contaReceber);
        }

        public Task DeleteAsync(ContaReceber contaReceber)
        {
            _context.ContasReceber.Remove(contaReceber);
            return _context.SaveChangesAsync();
        }

        Task<List<ContaReceber>> IContaReceberRepository.GetAllAsync(Guid empresaId)
        {
            return _context.ContasReceber
                .Where(p => p.EmpresaId == empresaId).ToListAsync();
        }

        IQueryable<ContaReceber> IContaReceberRepository.Query(Guid empresaId)
        {
            return _context.ContasReceber
                .Where(p => p.EmpresaId == empresaId).AsQueryable();
        }

        public async Task<ContaReceber?> GetBySubCategoriaAsync(Guid empresaId, Guid subCategoriaId)
        {
            var contaReceber = await _context.ContasReceber
                .Where(p => p.EmpresaId == empresaId && p.SubCategoriaId == subCategoriaId)
                .FirstOrDefaultAsync();
            return contaReceber;
        }

    }
}
