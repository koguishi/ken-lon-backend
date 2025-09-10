using Microsoft.EntityFrameworkCore;
using kendo_londrina.Domain.Entities;
using kendo_londrina.Domain.Repositories;
using kendo_londrina.Infra.Data;

namespace kendo_londrina.Infrastructure.Repositories
{
    public class CategoriaRepository : ICategoriaRepository
    {
        private readonly KendoLondrinaContext _context;

        public CategoriaRepository(KendoLondrinaContext context)
        {
            _context = context;
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        async Task<Categoria?> ICategoriaRepository.GetByIdAsync(Guid empresaId, Guid id)
        {
            var categoria = await _context.Categorias
                .Include(c => c.SubCategorias) // Inclui as SubCategorias na consulta
                .Where(p => p.EmpresaId == empresaId && p.Id == id)
                .FirstOrDefaultAsync();
            return categoria;
        }

        public async Task AddAsync(Categoria categoria)
        {
            await _context.Categorias.AddAsync(categoria);
        }

        public Task DeleteAsync(Categoria categoria)
        {
            _context.Categorias.Remove(categoria);
            return _context.SaveChangesAsync();
        }

        Task<List<Categoria>> ICategoriaRepository.GetAllAsync(Guid empresaId)
        {
            return _context.Categorias
                .Include(c => c.SubCategorias) // Inclui as SubCategorias na consulta
                .Where(p => p.EmpresaId == empresaId).ToListAsync();
        }

        IQueryable<Categoria> ICategoriaRepository.Query(Guid empresaId)
        {
            return _context.Categorias
                .Include(c => c.SubCategorias) // Inclui as SubCategorias na consulta
                .Where(p => p.EmpresaId == empresaId).AsQueryable();
        }
    }
}
