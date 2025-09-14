namespace kendo_londrina.Infra.Auditoria;

public interface IAuditoriaRepository
{
    Task AdicionarAsync(AuditoriaEntry entry, CancellationToken cancellationToken = default);
    // Task<List<AuditoriaEntry>> ObterPorEntidadeAsync(string entidade, Guid entidadeId, CancellationToken cancellationToken = default);
    // Task<List<AuditoriaEntry>> ObterTodosAsync(CancellationToken cancellationToken = default);
}
