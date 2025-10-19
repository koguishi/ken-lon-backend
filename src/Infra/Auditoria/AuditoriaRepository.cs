using kendo_londrina.Infra.Data;

namespace kendo_londrina.Infra.Auditoria;
public class AuditoriaRepository(KendoLondrinaContext context) : IAuditoriaRepository
{
    private readonly KendoLondrinaContext _context = context;

    public async Task AdicionarAsync(AuditoriaEntry entry, CancellationToken cancellationToken = default)
    {
        _context.AuditoriaEntries.Add(entry);
        await _context.SaveChangesAsync(cancellationToken);
    }

    // public async Task<List<AuditoriaEntry>> ObterPorEntidadeAsync(string entidade, Guid entidadeId, CancellationToken cancellationToken = default)
    // {
    //     return await _context.AuditoriaEntries
    //         .Where(a => a.Entidade == entidade && a.EntidadeId == entidadeId)
    //         .OrderBy(a => a.Data)
    //         .ToListAsync(cancellationToken);
    // }

    // public async Task<List<AuditoriaEntry>> ObterTodosAsync(CancellationToken cancellationToken = default)
    // {
    //     return await _context.AuditoriaEntries
    //         .OrderBy(a => a.Data)
    //         .ToListAsync(cancellationToken);
    // }
}
