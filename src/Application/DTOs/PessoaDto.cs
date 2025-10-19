namespace kendo_londrina.Application.DTOs;
public class PessoaDto
{
    public Guid? Id { get; set; }
    public required string Nome { get; set; }
    public string? Codigo { get; set; }
    public string? Cpf { get; set; }
    public string? Cnpj { get; set; }
}
