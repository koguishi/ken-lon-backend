namespace kendo_londrina.Application.DTOs;

public class ContaReceberDto
{
    public string? Descricao { get; set; }
    public Guid? Id { get; set; }
    public decimal Valor { get; set; }
    public DateTime Vencimento { get; set; }
    public string? Observacao { get; set; }

    public bool Recebido { get; set; }
    public DateTime? DataRecebimento { get; set; }
    public string? MeioRecebimento { get; set; }
    public string? ObsRecebimento { get; set; }

    public bool Excluido { get; set; }
    public DateTime? DataExclusao { get; set; }
    public string? MotivoExclusao { get; set; }

    // Chaves Estrangeiras (IDs) - Tornadas "nullable" para o seu requisito
    public Guid? PessoaId { get; set; }
    public Guid? CategoriaId { get; set; }
    public Guid? SubCategoriaId { get; set; }
}
