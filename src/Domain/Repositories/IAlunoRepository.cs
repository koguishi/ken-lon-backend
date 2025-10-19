using kendo_londrina.Domain.Entities;

namespace kendo_londrina.Domain.Repositories
{
    public interface IAlunoRepository
    {
        Task<Aluno?> GetByIdAsync(Guid id);
        Task AddAsync(Aluno aluno);
        Task DeleteAsync(Aluno aluno);
        Task SaveChangesAsync();
        Task<List<Aluno>> GetAllAsync();
        // DDD paginado
        IQueryable<Aluno> Query();        
    }
}
