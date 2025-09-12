namespace kendo_londrina.Application.DTOs;

public class ContaPagarDto
{
    public Guid? Id { get; set; }
    public decimal Valor { get; set; }
    public DateTime Vencimento { get; set; }
    public string? Observacao { get; set; }

    public bool Pago { get; set; }
    public DateTime? DataPagamento { get; set; }
    public string? MeioPagamento { get; set; }
    public string? ObsPagamento { get; set; }

    public bool Excluido { get; set; }
    public DateTime? DataExclusao { get; set; }
    public string? MotivoExclusao { get; set; }

    // Chaves Estrangeiras (IDs) - Tornadas "nullable" para o seu requisito
    public Guid? PessoaId { get; set; }
    public Guid? CategoriaId { get; set; }
    public Guid? SubCategoriaId { get; set; }
}
