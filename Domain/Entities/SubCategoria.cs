using kendo_londrina.Domain.Entities.BaseClasses;

namespace kendo_londrina.Domain.Entities;

public class SubCategoria : Entity
{
    public Guid CategoriaId { get; private set; }    
    public string Nome { get; private set; } = string.Empty;
    public string? Codigo { get; private set; } = string.Empty;
    virtual public Categoria? Categoria { get; private set; }    

    // virtual public IEnumerable<ContaPagar>? ContasPagar { get; private set; }    
    // virtual public IEnumerable<ContaReceber>? ContasReceber { get; private set; }    

    public SubCategoria(Guid userId,
        Guid categoriaId,
        string nome,
        string? codigo = null)
    {
        UserId = userId;
        CategoriaId = categoriaId;
        Nome = nome;
        Codigo = codigo;
    }

    // Construtor vazio para EF Core
    private SubCategoria() { }

    public void Atualizar(
        string nome,
        string? codigo = null)
    {
        Nome = nome;
        Codigo = codigo;
    }
}
