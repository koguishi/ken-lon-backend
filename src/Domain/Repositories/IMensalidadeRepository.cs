using kendo_londrina.Domain.Entities;

namespace kendo_londrina.Domain.Repositories
{
    public interface IMensalidadeRepository
    {
        Task<Mensalidade?> GetByIdAsync(Guid id);
        Task AddAsync(Mensalidade mensalidade);
        Task SaveChangesAsync();
    }
}
