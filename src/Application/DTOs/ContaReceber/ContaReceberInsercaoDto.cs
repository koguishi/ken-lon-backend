namespace kendo_londrina.Application.DTOs.ContaReceber;

public class ContaReceberInsercaoDto
{
    public string? Descricao { get; set; }
    public Guid? Id { get; set; }
    public decimal Valor { get; set; }
    public DateTime[]? Vencimentos { get; set; }
    public string? Observacao { get; set; }

    // Chaves Estrangeiras (IDs) - Tornadas "nullable" para o seu requisito
    public Guid? PessoaId { get; set; }
    public Guid? CategoriaId { get; set; }
    public Guid? SubCategoriaId { get; set; }
}
