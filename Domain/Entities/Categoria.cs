using kendo_londrina.Domain.Entities.BaseClasses;

namespace kendo_londrina.Domain.Entities;

public class Categoria : Entity
{
    public string Nome { get; private set; } = string.Empty;
    public string? Codigo { get; private set; } = string.Empty;
    //virtual public IEnumerable<SubCategoria>? SubCategorias { get; private set; }
    public List<SubCategoria> SubCategorias { get; private set; } = [];

    public Categoria(Guid empresaId, string nome,
        List<SubCategoria>? subCategorias = null,
        string? codigo = null)
    {
        EmpresaId = empresaId;
        Nome = nome;
        SubCategorias = subCategorias ?? [];
        Codigo = codigo;
    }

    // Construtor vazio para EF Core
    private Categoria() { }

    public void Atualizar(
        string nome,
        List<SubCategoria>? subCategorias,
        string? codigo = null)
    {
        Nome = nome;
        SubCategorias = subCategorias ?? [];
        Codigo = codigo;
    }

    public void AdicionarSubcategoria(string nome)
    {
        if (SubCategorias.Any(s => s.Nome == nome))
            throw new InvalidOperationException("Subcategoria já existe");
        SubCategorias.Add(new SubCategoria(EmpresaId, Id, nome));
    }

    public void RemoverSubcategoria(Guid subId)
    {
        var sub = SubCategorias.FirstOrDefault(s => s.Id == subId);
        if (sub != null) SubCategorias.Remove(sub);
    }    
}
