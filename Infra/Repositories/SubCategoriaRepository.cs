using Microsoft.EntityFrameworkCore;
using kendo_londrina.Domain.Entities;
using kendo_londrina.Domain.Repositories;
using kendo_londrina.Infra.Data;

namespace kendo_londrina.Infrastructure.Repositories
{
    public class SubCategoriaRepository : ISubCategoriaRepository
    {
        private readonly KendoLondrinaContext _context;

        public SubCategoriaRepository(KendoLondrinaContext context)
        {
            _context = context;
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<SubCategoria?> GetByIdAsync(Guid userId, Guid id)
        {
            var subCategoria = await _context.SubCategorias
                .Where(p => p.UserId == userId && p.Id == id)
                .FirstOrDefaultAsync();
            return subCategoria;
        }

        public async Task AddAsync(SubCategoria subCategoria)
        {
            await _context.SubCategorias.AddAsync(subCategoria);
        }

        public Task DeleteAsync(SubCategoria subCategoria)
        {
            _context.SubCategorias.Remove(subCategoria);
            return _context.SaveChangesAsync();
        }

        Task<List<SubCategoria>> ISubCategoriaRepository.GetAllAsync(Guid userId)
        {
            return _context.SubCategorias.Where(p => p.UserId == userId).ToListAsync();
        }

        IQueryable<SubCategoria> ISubCategoriaRepository.Query(Guid userId)
        {
            return _context.SubCategorias.Where(p => p.UserId == userId).AsQueryable();
        }
    }
}
