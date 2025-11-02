using kendo_londrina.Application.Services;
using kendo_londrina.Domain.Repositories;
using kendo_londrina.Infra.Data;
using Microsoft.EntityFrameworkCore.Storage;

namespace kendo_londrina.Infra.Data;

public interface IUnitOfWork
{
    AuditoriaService Auditoria { get; }
    IPessoaRepository Pessoas { get; }
    ICategoriaRepository Categorias { get; }
    IContaReceberRepository ContasReceber { get; }
    IContaPagarRepository ContasPagar { get; }
    Task BeginTransactionAsync();
    Task CommitAsync(CancellationToken cancellationToken = default);
    Task RollbackAsync();
}
public class UnitOfWork(KendoLondrinaContext context
    , AuditoriaService auditoria
    , IPessoaRepository pessoas
    , ICategoriaRepository categorias
    , IContaReceberRepository contasReceber
    , IContaPagarRepository contasPagar
) : IUnitOfWork
{
    private readonly KendoLondrinaContext _context = context;
    private IDbContextTransaction? _currentTransaction;
    public AuditoriaService Auditoria { get; } = auditoria;
    public IContaReceberRepository ContasReceber { get; } = contasReceber;
    public IContaPagarRepository ContasPagar { get; } = contasPagar;
    public IPessoaRepository Pessoas { get; } = pessoas;
    public ICategoriaRepository Categorias { get; } = categorias;

    public async Task BeginTransactionAsync()
    {
        _currentTransaction = await _context.Database.BeginTransactionAsync();
    }

    public async Task CommitAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            await _context.SaveChangesAsync(cancellationToken);

            if (_currentTransaction != null)
                await _currentTransaction.CommitAsync(cancellationToken);
        }
        catch (OperationCanceledException)
        {
            // cancelamento solicitado → rollback
            if (_currentTransaction != null)
                await _currentTransaction.RollbackAsync();

            throw; // relança a exceção para o pipeline tratar
        }
        catch
        {
            // qualquer outro erro → rollback
            if (_currentTransaction != null)
                await _currentTransaction.RollbackAsync();

            throw;
        }
        finally
        {
            if (_currentTransaction != null)
            {
                await _currentTransaction.DisposeAsync();
                _currentTransaction = null;
            }
        }
    }

    public async Task RollbackAsync()
    {
        if (_currentTransaction != null)
        {
            await _currentTransaction.RollbackAsync();
            await _currentTransaction.DisposeAsync();
            _currentTransaction = null;
        }
    }
}