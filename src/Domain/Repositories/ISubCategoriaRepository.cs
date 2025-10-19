using kendo_londrina.Domain.Entities;

namespace kendo_londrina.Domain.Repositories
{
    public interface ISubCategoriaRepository
    {
        Task<SubCategoria?> GetByIdAsync(Guid empresaId, Guid id);
        Task AddAsync(SubCategoria subCategoria);
        Task DeleteAsync(SubCategoria subCategoria);
        Task SaveChangesAsync();
        Task<List<SubCategoria>> GetAllAsync(Guid empresaId);
        // DDD paginado
        IQueryable<SubCategoria> Query(Guid empresaId);        
    }
}
