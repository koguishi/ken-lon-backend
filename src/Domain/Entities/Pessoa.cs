using kendo_londrina.Domain.Entities.BaseClasses;

namespace kendo_londrina.Domain.Entities;

public class Pessoa : Entity
{
    public string Nome { get; private set; } = string.Empty;
    public string? Codigo { get; private set; } = string.Empty;
    public string? Cpf { get; private set; }
    public string? Cnpj { get; private set; }
    virtual public ICollection<ContaPagar>? ContasPagar { get; private set; }    
    virtual public ICollection<ContaReceber>? ContasReceber { get; private set; }    

    private void Validar()
    {
        if (EmpresaId == Guid.Empty)
            throw new DomainException("EmpresaId não pode ser vazio.");
        if (string.IsNullOrEmpty(Nome))
            throw new DomainException("Nome não pode ser nulo ou vazio.");
        if (!string.IsNullOrEmpty(Cpf) && !string.IsNullOrEmpty(Cnpj))
            throw new DomainException("CPF e CNPJ não podem ser informados simultaneamente.");
    }

    public Pessoa(Guid empresaId, string nome,
        string? codigo = null,
        string? cpf = null,
        string? cnpj = null)
    {
        EmpresaId = empresaId;
        Nome = nome;
        Codigo = codigo;
        Cpf = cpf;
        Cnpj = cnpj;
        Validar();
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
