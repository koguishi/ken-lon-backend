using kendo_londrina.Domain.Entities.BaseClasses;

namespace kendo_londrina.Domain.Entities;

public class Categoria : Entity
{
    public string Nome { get; private set; } = string.Empty;
    public string? Codigo { get; private set; } = string.Empty;
    virtual public ICollection<SubCategoria>? SubCategorias { get; private set; } = [];
    virtual public ICollection<ContaPagar>? ContasPagar { get; private set; }    
    virtual public ICollection<ContaReceber>? ContasReceber { get; private set; }    

    public Categoria(Guid empresaId, string nome,
        ICollection<SubCategoria>? subCategorias = null,
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
        string? codigo = null)
    {
        Nome = nome;
        Codigo = codigo;
    }

    public SubCategoria AdicionarSubcategoria(string nome)
    {
        if (string.IsNullOrWhiteSpace(nome))
            throw new DomainException("Nome da subcategoria inválido");
        SubCategorias ??= [];
        if (SubCategorias.Any(s => s.Nome == nome))
            throw new DomainException("Subcategoria já existe");
        var subCategoria = new SubCategoria(EmpresaId, Id, nome);
        SubCategorias.Add(subCategoria);
        return subCategoria;
    }

    public void ExcluirSubcategoria(Guid subId)
    {
        SubCategorias ??= [];
        var sub = SubCategorias.FirstOrDefault(s => s.Id == subId);
        if (sub != null) SubCategorias.Remove(sub);
    }

    public void AlterarSubcategoria(Guid subId, string nome)
    {
        if (string.IsNullOrWhiteSpace(nome))
            throw new DomainException("Nome da subcategoria inválido");
        SubCategorias ??= [];
        var sub = SubCategorias.FirstOrDefault(s => s.Id == subId)
            ?? throw new DomainException("Subcategoria não encontrada");
        sub.Atualizar(nome);
    }

}
