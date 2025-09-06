using kendo_londrina.Domain.Entities.AbstractClasses;

namespace kendo_londrina.Domain.Entities;

public class Categoria : Entity
{
    public string Nome { get; private set; } = string.Empty;
    public string? Codigo { get; private set; } = string.Empty;
    // virtual public IEnumerable<SubCategoria>? SubCategorias { get; private set; }    

    public Categoria(Guid userId, string nome,
        string? codigo = null)
    {
        UserId = userId;
        Nome = nome;
        Codigo = codigo;
    }

    // Construtor vazio para EF Core
    private Categoria() { }

    public void Atualizar(
        string nome,
        string? codigo = null)
    {
        Nome = nome;
        Codigo = codigo;
    }
}
