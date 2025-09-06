using kendo_londrina.Domain.Entities.AbstractClasses;

namespace kendo_londrina.Domain.Entities;

public class Pessoa : Entity
{
    public string Nome { get; private set; } = string.Empty;
    public string? Codigo { get; private set; } = string.Empty;
    public string? Cpf { get; private set; }
    public string? Cnpj { get; private set; }
    // virtual public IEnumerable<ContaPagar>? ContasPagar { get; private set; }    
    // virtual public IEnumerable<ContaReceber>? ContasReceber { get; private set; }    

    public Pessoa(Guid userId, string nome,
        string? codigo = null,
        string? cpf = null,
        string? cnpj = null)
    {
        UserId = userId;
        Nome = nome;
        Codigo = codigo;
        Cpf = cpf;
        Cnpj = cnpj;
    }

    // Construtor vazio para EF Core
    private Pessoa() { }

    public void Atualizar(
        string nome,
        string? codigo = null,
        string? cpf = null,
        string? cnpj = null)
    {
        Nome = nome;
        Codigo = codigo;
        Cpf = cpf;
        Cnpj = cnpj;
    }
}
