using System.ComponentModel.DataAnnotations;

namespace kendo_londrina.Domain.Entities;

public class Aluno
{
    public Guid EscolaId { get; set; } = Guid.Parse("b3e53df9-b128-4227-81fe-cc0b9ad9720b");
    public Guid Id { get; set; }

    [Required, MaxLength(100)]
    public string Nome { get; set; } = string.Empty;

    [MaxLength(15)]
    public string? Cpf { get; set; }

    [MaxLength(20)]
    public string? TelCelular { get; set; }

    [EmailAddress, MaxLength(200)]
    public string? Email { get; set; }

    public DateTime DataNascimento { get; set; }

    [MaxLength(100)]
    public string? Nacionalidade { get; set; }

    [MaxLength(2)]
    public string? UfNascimento { get; set; }

    [MaxLength(100)]
    public string? CidadeNascimento { get; set; }

    [MaxLength(1)]
    public string? Sexo { get; set; }

    [MaxLength(50)]
    public string? Rg { get; set; }

    [MaxLength(50)]
    public string? Religiao { get; set; }

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
