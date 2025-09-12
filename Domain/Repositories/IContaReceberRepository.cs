using kendo_londrina.Domain.Entities;

namespace kendo_londrina.Domain.Repositories
{
    public interface IContaReceberRepository
    {
        Task<ContaReceber?> GetByIdAsync(Guid empresaId, Guid id);
        Task AddAsync(ContaReceber contaReceber);
        Task DeleteAsync(ContaReceber contaReceber);
        Task SaveChangesAsync();
        Task<List<ContaReceber>> GetAllAsync(Guid empresaId);
        // DDD paginado
        IQueryable<ContaReceber> Query(Guid empresaId);        
    }
}
