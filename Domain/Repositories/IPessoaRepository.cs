using kendo_londrina.Domain.Entities;

namespace kendo_londrina.Domain.Repositories
{
    public interface IPessoaRepository
    {
        Task<Pessoa?> GetByIdAsync(Guid empresaId, Guid id);
        Task AddAsync(Pessoa pessoa);
        Task DeleteAsync(Pessoa pessoa);
        Task SaveChangesAsync();
        Task<List<Pessoa>> GetAllAsync(Guid empresaId);
        // DDD paginado
        IQueryable<Pessoa> Query(Guid empresaId);        
    }
}
