using Microsoft.EntityFrameworkCore;
using kendo_londrina.Domain.Entities;
using kendo_londrina.Domain.Repositories;
using kendo_londrina.Infra.Data;

namespace kendo_londrina.Infrastructure.Repositories
{
    public class ContaPagarRepository : IContaPagarRepository
    {
        private readonly KendoLondrinaContext _context;

        public ContaPagarRepository(KendoLondrinaContext context)
        {
            _context = context;
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<ContaPagar?> GetByIdAsync(Guid empresaId, Guid id)
        {
            var contasPagar = await _context.ContasPagar
                .Where(p => p.EmpresaId == empresaId && p.Id == id)
                .FirstOrDefaultAsync();
            return contasPagar;
        }

        public async Task AddAsync(ContaPagar contaPagar)
        {
            await _context.ContasPagar.AddAsync(contaPagar);
        }

        public Task DeleteAsync(ContaPagar contaPagar)
        {
            _context.ContasPagar.Remove(contaPagar);
            return _context.SaveChangesAsync();
        }

        Task<List<ContaPagar>> IContaPagarRepository.GetAllAsync(Guid empresaId)
        {
            return _context.ContasPagar
                .Where(p => p.EmpresaId == empresaId).ToListAsync();
        }

        IQueryable<ContaPagar> IContaPagarRepository.Query(Guid empresaId)
        {
            return _context.ContasPagar
                .Where(p => p.EmpresaId == empresaId && p.Excluido == false)
                .AsQueryable();
        }

        public async Task<ContaPagar?> GetBySubCategoriaAsync(Guid empresaId, Guid subCategoriaId)
        {
            var contaPagar = await _context.ContasPagar
                .Where(p => p.EmpresaId == empresaId && p.SubCategoriaId == subCategoriaId)
                .FirstOrDefaultAsync();
            return contaPagar;
        }

    }
}
