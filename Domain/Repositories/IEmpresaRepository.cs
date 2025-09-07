using kendo_londrina.Domain.Entities;

namespace kendo_londrina.Domain.Repositories
{
    public interface IEmpresaRepository
    {
        Task<Empresa?> GetByIdAsync(Guid id);
        Task AddAsync(Empresa empresa);
        Task DeleteAsync(Empresa empresa);
        Task SaveChangesAsync();
        Task<List<Empresa>> GetAllAsync();
        // DDD paginado
        IQueryable<Empresa> Query();        
    }
}
