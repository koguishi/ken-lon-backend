using kendo_londrina.Domain.Entities;

namespace kendo_londrina.Domain.Repositories
{
    public interface ICategoriaRepository
    {
        Task<Categoria?> GetByIdAsync(Guid empresaId, Guid id);
        Task AddAsync(Categoria categoria);
        Task DeleteAsync(Categoria categoria);
        Task SaveChangesAsync();
        Task<List<Categoria>> GetAllAsync(Guid empresaId);
        // DDD paginado
        IQueryable<Categoria> Query(Guid empresaId);        
        Task LoadContasPagarAsync(Categoria categoria);
        Task LoadContasReceberAsync(Categoria categoria);
    }
}
