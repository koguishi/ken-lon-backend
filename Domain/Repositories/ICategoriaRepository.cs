using kendo_londrina.Domain.Entities;

namespace kendo_londrina.Domain.Repositories
{
    public interface ICategoriaRepository
    {
        Task<Categoria?> GetByIdAsync(Guid userId, Guid id);
        Task AddAsync(Categoria categoria);
        Task DeleteAsync(Categoria categoria);
        Task SaveChangesAsync();
        Task<List<Categoria>> GetAllAsync(Guid userId);
        // DDD paginado
        IQueryable<Categoria> Query(Guid userId);        
    }
}
