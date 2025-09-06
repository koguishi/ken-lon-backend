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

        async Task<Categoria?> ICategoriaRepository.GetByIdAsync(Guid userId, Guid id)
        {
            var categoria = await _context.Categorias
                .Where(p => p.UserId == userId && p.Id == id)
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

        Task<List<Categoria>> ICategoriaRepository.GetAllAsync(Guid userId)
        {
            return _context.Categorias.Where(p => p.UserId == userId).ToListAsync();
        }

        IQueryable<Categoria> ICategoriaRepository.Query(Guid userId)
        {
            return _context.Categorias.Where(p => p.UserId == userId).AsQueryable();
        }
    }
}
