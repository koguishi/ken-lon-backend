namespace kendo_londrina.Application.DTOs.FichaFinanceira;

public class FichaFinanceiraRequestDto
{
    public Guid PessoaId { get; set; }
    public DateTime VencimentoInicial { get; set; }
    public DateTime VencimentoFinal { get; set; }
    public bool? Recebido { get; set; }
}
