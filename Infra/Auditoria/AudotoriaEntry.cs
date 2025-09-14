public class AuditoriaEntry
{
    public Guid Id { get; set; }

    public string Entidade { get; set; } = null!;      // Ex: "TituloFinanceiro"
    public Guid EntidadeId { get; set; }               // Id da entidade alterada
    public string Acao { get; set; } = null!;           // Ex: "Criar", "Atualizar", "Excluir", "PagamentoEfetuado"
    
    public string? DadosAntes { get; set; }             // JSON opcional
    public string? DadosDepois { get; set; }            // JSON opcional

    public DateTime Data { get; set; }
    public Guid UsuarioId { get; set; }                 // Quem fez a ação
}
