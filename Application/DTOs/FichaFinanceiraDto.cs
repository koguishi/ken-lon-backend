namespace kendo_londrina.Application.DTOs;

public class FichaFinanceiraDto
{
    public Guid? JobId { get; set; }
    public int Ano { get; set; }
    public required string NomePessoa { get; set; }
    public ResumoFinanceiroDto? Resumo { get; set; }
    public required List<TituloDto> Titulos { get; set; }
}

public class ResumoFinanceiroDto
{
    public decimal Total { get; set; }
    public decimal TotalLiquidado { get; set; }
    public decimal TotalEmAberto { get; set; }
    public DateTime UltimaAtualizacao { get; set; }
}

public class TituloDto
{
    public DateTime Vencimento { get; set; }
    public decimal Valor { get; set; }
    public DateTime? DataLiquidacao { get; set; }
}
