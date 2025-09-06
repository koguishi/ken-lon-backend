using kendo_londrina.Domain.Entities;

namespace kendo_londrina.Domain.Repositories
{
    public interface IPessoaRepository
    {
        Task<Pessoa?> GetByIdAsync(Guid userId, Guid id);
        Task AddAsync(Pessoa pessoa);
        Task DeleteAsync(Guid userId, Pessoa pessoa);
        Task SaveChangesAsync();
        Task<List<Pessoa>> GetAllAsync(Guid userId);
        // DDD paginado
        IQueryable<Pessoa> Query(Guid userId);        
    }
}
