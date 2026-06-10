using kendo_londrina.Domain.Entities;
using kendo_londrina.Domain.Repositories;
using kendo_londrina.Infra.Data;
using Microsoft.EntityFrameworkCore;

namespace kendo_londrina.Infrastructure.Repositories;

public class ResponsavelRepository : IResponsavelRepository
{
    private readonly KendoLondrinaContext _context;

    public ResponsavelRepository(KendoLondrinaContext context)
    {
        _context = context;
    }

    public async Task<Responsavel?> GetByIdAsync(Guid empresaId, Guid id) =>
        await _context.Responsaveis
            .FirstOrDefaultAsync(r => r.EmpresaId == empresaId && r.Id == id);

    public async Task<IEnumerable<Responsavel>> GetAllAsync(Guid empresaId) =>
        await _context.Responsaveis
            .Where(r => r.EmpresaId == empresaId)
            .OrderBy(r => r.Nome)
            .ToListAsync();

    public async Task<Responsavel?> GetByIdComAlunosAsync(Guid empresaId, Guid id) =>
        await _context.Responsaveis
            .Include(r => r.Alunos)
            .FirstOrDefaultAsync(r => r.EmpresaId == empresaId && r.Id == id);

    public async Task AddAsync(Responsavel responsavel) =>
        await _context.Responsaveis.AddAsync(responsavel);

    public async Task SaveChangesAsync() =>
        await _context.SaveChangesAsync();
}
