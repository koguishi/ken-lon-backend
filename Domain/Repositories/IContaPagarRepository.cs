using kendo_londrina.Domain.Entities;

namespace kendo_londrina.Domain.Repositories
{
    public interface IContaPagarRepository
    {
        Task<ContaPagar?> GetByIdAsync(Guid empresaId, Guid id);
        Task AddAsync(ContaPagar contaPagar);
        Task DeleteAsync(ContaPagar contaPagar);
        Task SaveChangesAsync();
        Task<List<ContaPagar>> GetAllAsync(Guid empresaId);
        // DDD paginado
        IQueryable<ContaPagar> Query(Guid empresaId);        
    }
}
