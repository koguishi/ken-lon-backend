using kendo_londrina.Domain.Entities;

namespace kendo_londrina.Domain.Repositories;

public interface IResponsavelRepository
{
    Task<Responsavel?> GetByIdAsync(Guid empresaId, Guid id);
    Task<IEnumerable<Responsavel>> GetAllAsync(Guid empresaId);
    Task<Responsavel?> GetByIdComAlunosAsync(Guid empresaId, Guid id);
    Task AddAsync(Responsavel responsavel);
    Task SaveChangesAsync();
}
