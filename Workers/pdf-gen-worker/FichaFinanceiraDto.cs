
namespace pdf_gen_worker;

public class FichaFinanceiraDto
{
    public Guid JobId { get; set; }
    public int? Ano { get; set; }
    public DateTime VencimentoInicial { get; set; }
    public DateTime VencimentoFinal { get; set; }
    public string NomePessoa { get; set; } = "";
    public ResumoFinanceiroDto? Resumo { get; set; }
    public List<TituloDto> Titulos { get; set; } = [];
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
