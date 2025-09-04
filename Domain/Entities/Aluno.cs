using kendo_londrina.Domain.Entities.AbstractClasses;

namespace kendo_londrina.Domain.Entities;

public class Aluno : Entity
{
    public string Codigo { get; set; } = string.Empty;
    public string Nome { get; set; } = string.Empty;
    public string? Cpf { get; set; }
    public string? TelCelular { get; set; }
    public string? Email { get; set; }
    public DateTime DataNascimento { get; set; }
    public string? Nacionalidade { get; set; }
    public string? UfNascimento { get; set; }
    public string? CidadeNascimento { get; set; }
    public string? Sexo { get; set; }
    public string? Rg { get; set; }
    public string? Religiao { get; set; }
    public Guid? EnderecoId { get; private set; }
    virtual public IEnumerable<Mensalidade>? Mensalidades { get; private set; }    

    // Construtor para criar novo aluno
    public Aluno(string nome, DateTime dataNascimento,
        string? cpf = null, string? telCelular = null, string? email = null)
    {
        Nome = nome;
        DataNascimento = dataNascimento;
        Cpf = cpf;
        TelCelular = telCelular;
        Email = email;
    }

    // Construtor vazio para EF Core
    private Aluno() { }

    public void Atualizar(
        string nome,
        DateTime dataNascimento,
        string? cpf = null,
        string? telCelular = null,
        string? email = null,
        string? nacionalidade = null,
        string? ufNascimento = null,
        string? cidadeNascimento = null,
        string? sexo = null,
        string? rg = null,
        string? religiao = null)
    {
        Nome = nome;
        Cpf = cpf;
        TelCelular = telCelular;
        Email = email;
        DataNascimento = dataNascimento;
        Nacionalidade = nacionalidade;
        UfNascimento = ufNascimento;
        CidadeNascimento = cidadeNascimento;
        Sexo = sexo;
        Rg = rg;
        Religiao = religiao;
    }
}
