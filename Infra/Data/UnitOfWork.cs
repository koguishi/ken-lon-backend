using kendo_londrina.Infra.Data;

public interface IUnitOfWork
{
    Task CommitAsync();
}
public class UnitOfWork : IUnitOfWork
{
    private readonly KendoLondrinaContext _context;
    public UnitOfWork(KendoLondrinaContext context)
    {
        _context = context;
    }

    public async Task CommitAsync()
    {
        await _context.SaveChangesAsync();
    }
}