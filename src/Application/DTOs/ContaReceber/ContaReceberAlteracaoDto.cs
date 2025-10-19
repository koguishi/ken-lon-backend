namespace kendo_londrina.Application.DTOs.ContaReceber;

public class ContaReceberAlteracaoDto
{
    public string? Descricao { get; set; }
    public Guid? Id { get; set; }
    public decimal Valor { get; set; }
    public DateTime Vencimento { get; set; }
    public string? Observacao { get; set; }
    public Guid? PessoaId { get; set; }
    public Guid? CategoriaId { get; set; }
    public Guid? SubCategoriaId { get; set; }
}
