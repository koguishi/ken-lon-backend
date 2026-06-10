using kendo_londrina.Domain.Entities.BaseClasses;

namespace kendo_londrina.Domain.Entities;

public class Responsavel : Entity
{
    public string Nome { get; private set; } = string.Empty;
    public string? Telefone { get; private set; }
    public string? Email { get; private set; }

    virtual public ICollection<Aluno>? Alunos { get; private set; }
    virtual public ICollection<Mensalidade>? Mensalidades { get; private set; }

    private Responsavel() { }

    public Responsavel(Guid empresaId, string nome, string? telefone = null, string? email = null)
    {
        if (string.IsNullOrEmpty(nome))
            throw new DomainException("Nome não pode ser vazio.");
        EmpresaId = empresaId;
        Nome = nome;
        Telefone = telefone;
        Email = email;
    }

    public void Atualizar(string nome, string? telefone = null, string? email = null)
    {
        if (string.IsNullOrEmpty(nome))
            throw new DomainException("Nome não pode ser vazio.");
        Nome = nome;
        Telefone = telefone;
        Email = email;
    }
}
