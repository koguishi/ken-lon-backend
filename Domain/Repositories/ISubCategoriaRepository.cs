using kendo_londrina.Domain.Entities;

namespace kendo_londrina.Domain.Repositories
{
    public interface ISubCategoriaRepository
    {
        Task<SubCategoria?> GetByIdAsync(Guid userId, Guid id);
        Task AddAsync(SubCategoria subCategoria);
        Task DeleteAsync(Guid userId, SubCategoria subCategoria);
        Task SaveChangesAsync();
        Task<List<SubCategoria>> GetAllAsync(Guid userId);
        // DDD paginado
        IQueryable<SubCategoria> Query(Guid userId);        
    }
}
