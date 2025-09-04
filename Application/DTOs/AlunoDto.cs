namespace kendo_londrina.Application.DTOs;
public class AlunoDto
{
    public Guid? Id { get; set; }
    public required string Nome { get; set; }
    public DateTime DataNascimento { get; set; }
    public string? Cpf { get; set; }
    public string? TelCelular { get; set; }
    public string? Email { get; set; }
    public string? Nacionalidade { get; set; }
    public string? UfNascimento { get; set; }
    public string? CidadeNascimento { get; set; }
    public string? Sexo { get; set; }
    public string? Rg { get; set; }
    public string? Religiao { get; set; }
}
